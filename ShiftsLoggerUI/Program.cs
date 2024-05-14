using ShiftsLoggerAPI.Models;
using ShiftsLoggerUI.Input;

namespace ShiftsLoggerUI;

public class Program
{
    private readonly ShiftContext _context;

    public static async Task Main(string[] args)
    {
        var userInput = new UserInput();
        //aaksdkads
        await userInput.Menu();
    }
}
