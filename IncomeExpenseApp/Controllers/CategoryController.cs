using IncomeExpenseApp.Data;
using IncomeExpenseApp.Logging;
using IncomeExpenseApp.Model.Dto;
using IncomeExpenseApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 

namespace IncomeExpenseApp.Controllers
{
    [Route("api/CategoryAPI")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CategoryController> _logger;
        //private readonly ILogging _logging;

        public CategoryController(AppDbContext dbContext, ILogger<CategoryController> logger/* ILogging logging*/)
        {
            _dbContext = dbContext;
            _logger = logger;
           // _logging = logging;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Getting all categories");
                //_logging.Log("Getting all categories","INFO");
                return Ok(_dbContext.Category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCategories: {ex.Message}");
                //_logging.Log($"Error in GetCategories:","ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<CategoryDto> GetCategories(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogWarning("Invalid Id passed to GetCategories");
                    return BadRequest();
                }

                var category = _dbContext.Category.FirstOrDefault(u => u.Id == id);
                if (category == null)
                {
                    _logger.LogInformation($"Category with Id {id} not found");
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCategories: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult<CategoryDto> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest(categoryDto);
                }

                if (categoryDto.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Category model = new()
                {
                    Name = categoryDto.Name
                };

                _dbContext.Category.Add(model);
                _dbContext.SaveChanges();
                return CreatedAtRoute("GetCategory", new { id = categoryDto.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateCategory: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var category = _dbContext.Category.FirstOrDefault(u => u.Id == id);
                if (category == null)
                {
                    return NotFound();
                }

                _dbContext.Category.Remove(category);
                _dbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteCategory: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null || id != categoryDto.Id)
                {
                    return BadRequest();
                }

                var existingCategory = _dbContext.Category.FirstOrDefault(u => u.Id == id);
                if (existingCategory == null)
                {
                    return NotFound();
                }
                existingCategory.Name = categoryDto.Name;
                _dbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateCategory: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
