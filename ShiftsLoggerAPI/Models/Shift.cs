using System.ComponentModel.DataAnnotations;

namespace ShiftsLoggerAPI.Models;

public class Shift
{
    public int Id { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [DataType(DataType.DateTime)]
    public DateTime? StartTime { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [DataType(DataType.DateTime)]
    public DateTime? EndTime { get; set; }
    //public string? Duration 
    //{ 
    //    get => _duration;
    //    protected set => (EndTime - StartTime).ToString("hh:mm:ss");
    //} 

    //private string? _duration;
    public string? Duration { get; set; }
}
