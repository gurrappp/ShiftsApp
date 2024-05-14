
using System.ComponentModel.DataAnnotations;
using ShiftsLoggerUI.Validation;
using ShiftsLoggerUI.Controllers;

namespace ShiftsLoggerUI.Input;

public class UserInput
{
    private Validate validate;
    private ShiftsUIController controller;
    public UserInput()
    {
        validate = new Validate();
        controller = new ShiftsUIController();
    }

    public async Task Menu()
    {
        bool closeApp = false;

        Console.Clear();
        while (!closeApp)
        {
            Console.WriteLine("----- SHIFT LOGGER MENU ---------");
            Console.WriteLine(" Options: ");
            Console.WriteLine("0 - exit");
            Console.WriteLine("1 - Get all shifts");
            Console.WriteLine("2 - Get shift by id");
            Console.WriteLine("3 - Update a shift");
            Console.WriteLine("4 - Start new shift");
            Console.WriteLine("5 - End a shift");
            Console.WriteLine("6 - Create a new shift with start and end times");

            var option = Console.ReadLine();

            var answer = validate.ValidateMenuOption(option);
            if (answer == -1)
            {
                Console.Clear();
                break;
            }

            switch (answer)
            {
                case 0:
                    closeApp = true;
                    break;
                case 1:
                    await controller.GetShifts();
                    break;
                case 2:
                    var id = GetId();
                    if (id != null)
                        await controller.GetShift(id);
                   
                    break;
                case 3:
                    var updateShiftId = GetId();
                    var updateStartTime = GetStartTime();
                    var updateEndTime = GetEndTime();
                    if(updateShiftId != null)
                    {
                        await controller.UpdateShift(updateShiftId, updateStartTime, updateEndTime);
                        await controller.GetShift(updateShiftId);
                    }
                        
                    break;
                case 4:
                    await controller.StartNewShift();
                    break;
                case 5:
                    var endShiftId = GetId();
                    if (endShiftId != null)
                        await controller.EndShift(endShiftId);
                    break;
                case 6:
                    
                    var createStartTime = GetStartTime();
                    var createEndTime = GetEndTime();
                    if (createStartTime != null && createEndTime != null)
                        await controller.CreateNewShift(createStartTime, createEndTime);
                    break;
                default:
                    break;
            }
        }
    }

    private int? GetId()
    {
        Console.WriteLine("Write Id of shift: ");
        var value = Console.ReadLine();
        var id = validate.ValidateId(value);
        return id;
    }

    private DateTime? GetStartTime()
    {
        Console.WriteLine("Write new start time of shift with format: yyyy-MM-dd HH:mm:ss, write 0 to keep value:");
        var value = Console.ReadLine();
        if (value != "0")
            return validate.ValidateTime(value);

        return null;
    }
    private DateTime? GetEndTime()
    {
        Console.WriteLine("Write new end time of shift with format: yyyy-MM-dd HH:mm:ss, write 0 to keep value:");
        var value = Console.ReadLine();
        if(value != "0")
            return validate.ValidateTime(value);

        return null;
    }

}
