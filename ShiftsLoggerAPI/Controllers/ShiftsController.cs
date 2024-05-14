
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Specialized;
using System.Configuration;
using ShiftsLoggerAPI.Models;
using Newtonsoft.Json;


namespace ShiftsLoggerAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ShiftsController : ControllerBase
{
    private readonly ShiftContext _context;

    public ShiftsController(ShiftContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
    {
        return await _context.Shifts.ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<Shift>> GetShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if(shift == null)
            return NotFound();

        return shift;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateShift(int id, Shift shiftItemToUpdate)
    {
        //test for git
        if (id != shiftItemToUpdate?.Id)
        {
            return BadRequest();
        }

        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
        {
            return NotFound();
        }

        shift.StartTime = shiftItemToUpdate.StartTime;
        shift.EndTime = shiftItemToUpdate.EndTime;
        if(shift.EndTime != null && shift.StartTime != null)
        {
            TimeSpan value = shift.EndTime.Value.Subtract(shift.StartTime.Value);
            if (value > TimeSpan.FromHours(24))
                return BadRequest();
            DateTime date = DateTime.Parse(value.ToString());
            shift.Duration = date.ToString("HH:mm:ss");
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!ShiftItemExists(id))
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<Shift>> StartNewShift()
    {
        //if (newShift == null)
        //    return BadRequest();

        Shift newShift = new Shift()
        {
            StartTime = DateTime.Now,
            EndTime = null,
            Duration = null
        };

        _context.Shifts.Add(newShift);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetShift),
            new { id = newShift.Id },
            newShift);
    }

    [HttpGet]
    public async Task<ActionResult<Shift>> EndShift(int id)
    {
        
        var shift = _context.Shifts.Find(id);
        if (shift == null || shift.StartTime == null)
            return BadRequest();

        shift.EndTime = DateTime.Now;
        TimeSpan value = shift.EndTime.Value.Subtract(shift.StartTime.Value);
        if (value > TimeSpan.FromHours(24))
            return BadRequest();
        DateTime date = DateTime.Parse(value.ToString());
        shift.Duration = date.ToString("HH:mm:ss");

        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException) when(!ShiftItemExists(id))
        {
            return NotFound();
        }

        return shift;
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> CreateNewShift(Shift newShift)
    {
        if(newShift == null || newShift.StartTime == null || newShift.EndTime == null)
            return BadRequest();

       
        TimeSpan value = newShift.EndTime.Value.Subtract(newShift.StartTime.Value);
        if(value > TimeSpan.FromHours(24))
            return BadRequest();

        DateTime date = DateTime.Parse(value.ToString());
        newShift.Duration = date.ToString("HH:mm:ss");
        _context.Shifts.Add(newShift);
       
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return NotFound();
        }

        //return newShift;

        return CreatedAtAction(
            nameof(GetShift),
            new { id = newShift.Id },
            newShift);
    }
    

    private bool ShiftItemExists(int id)
    {
        return _context.Shifts.Any(x => x.Id == id);
    }
}
