namespace IncomeExpenseApp.Logging
{
    public class Logging:ILogging
    {
        public void Log(string message, string level)
        {
            Console.ResetColor(); // Reset color before applying new color

            switch (level.ToUpper())
            {
                case "ERROR":
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"ERROR: {message}");
                    break;

                case "WARNING":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"WARNING: {message}");
                    break;

                case "INFO":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"INFO: {message}");
                    break;

                case "DEBUG":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"DEBUG: {message}");
                    break;

                default:
                    Console.WriteLine(message);
                    break;
            }

            Console.ResetColor(); // Reset color after logging
        }
    }
}