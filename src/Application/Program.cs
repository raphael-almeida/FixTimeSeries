using Application.Extensibility;

namespace Application;

/// <summary>
/// The main program class.
/// Receives the input arguments and starts the application.
/// </summary>
public class Program
{
    /// <summary>
    /// The default path to the default FIX log file.
    /// </summary>
    const string defaultPath = "./fix.051.incr.log";

    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    public static void Main(string[] args)
    {
        var provider = new ExtensibilityProvider();
        Console.WriteLine("Fix Time Series Parser");
        Console.WriteLine(
            "Please enter the path to the FIX log file (or enter for default ./fix.051.incr.log):"
        );
        do
        {
            string path = defaultPath;
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                path = Console.ReadLine();
            }
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine(
                    "Please enter a valid path (or enter for default ./fix.051.incr.log):"
                );
            }
            if (!File.Exists(path))
            {
                Console.WriteLine(
                    "Please enter a valid file(or enter for default ./fix.051.incr.log):"
                );
            }
            else
            {
                try
                {
                    provider.TimeSeriesService.Run(path);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred parsing file {path}: {e.Message}");
                }
            }
        } while (true);
    }
}
