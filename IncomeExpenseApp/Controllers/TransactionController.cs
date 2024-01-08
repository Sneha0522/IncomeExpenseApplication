using IncomeExpenseApp.Data;
using IncomeExpenseApp.Model;
using IncomeExpenseApp.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncomeExpenseApp.Controllers
{
    [Route("api/TransactionAPI")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(AppDbContext dbContext, ILogger<TransactionController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TransactionDto>> GetTransactions()
        {
            try
            {
                _logger.LogInformation("Getting all transactions");
                var transactions = _dbContext.Transactions.Include(t => t.Category).ToList();
                var transactionDtos = transactions.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    Amount = t.Amount,
                    Date = t.Date,
                    CategoryId = t.CategoryId
                });
                return Ok(transactionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTransactions: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult<object> CreateTransaction([FromBody] TransactionDto transactionDto)
        {
            try
            {
                if (transactionDto == null)
                {
                    return BadRequest("Invalid input");
                }

                var category = _dbContext.Category.Find(transactionDto.CategoryId);

                if (category == null)
                {
                    return NotFound($"Category with Id {transactionDto.CategoryId} not found");
                }

                var transaction = new Transaction
                {
                    Description = transactionDto.Description,
                    Amount = transactionDto.Amount,
                    Date = transactionDto.Date,
                    CategoryId = transactionDto.CategoryId
                };

                _dbContext.Transactions.Add(transaction);
                _dbContext.SaveChanges();

                // Return a custom response
                return new { Message = "Transaction created successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateTransaction: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult<object> UpdateTransaction(int id, [FromBody] TransactionDto transactionDto)
        {
            try
            {
                if (transactionDto == null || id != transactionDto.Id)
                {
                    return BadRequest();
                }

                var existingTransaction = _dbContext.Transactions.Include(t => t.Category).FirstOrDefault(t => t.Id == id);

                if (existingTransaction == null)
                {
                    return NotFound();
                }

                existingTransaction.Description = transactionDto.Description;
                existingTransaction.Amount = transactionDto.Amount;
                existingTransaction.Date = transactionDto.Date;
                existingTransaction.CategoryId = transactionDto.CategoryId;

                _dbContext.SaveChanges();

                return new { Message = "Transaction updated successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateTransaction: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<object> DeleteTransaction(int id)
        {
            try
            {
                var transaction = _dbContext.Transactions.Find(id);

                if (transaction == null)
                {
                    return NotFound();
                }

                _dbContext.Transactions.Remove(transaction);
                _dbContext.SaveChanges();

                return new { Message = "Transaction deleted successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteTransaction: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
