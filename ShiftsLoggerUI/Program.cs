using ShiftsLoggerAPI.Models;
using ShiftsLoggerUI.Input;

namespace ShiftsLoggerUI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var userInput = new UserInput();
        //test for version control
        await userInput.Menu();
    }
}
