using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShiftsLoggerAPI.Models;
using ShiftsLoggerUI.Vizualisation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Formatting;

namespace ShiftsLoggerUI.Controllers
{
    public class ShiftsUIController
    {
        
        public async Task GetShifts()
        {
            List<Shift> shifts = new List<Shift>();
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("http://localhost:5294/api/Shifts/GetShifts");
                var response = await result.Content.ReadAsStringAsync();
                
                if(response != null)
                    shifts = JsonConvert.DeserializeObject<List<Shift>>(response);

                if(shifts != null && shifts.Count > 0)
                {
                    TableVizualisationEngine.ShowTable(shifts, "Shifts ");
                }
            }

            return;
        }


        public async Task GetShift(int? id)
        {
            
            Shift shift = new Shift();
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"http://localhost:5294/api/Shifts/GetShift?Id={id}");
                var response = await result.Content.ReadAsStringAsync();
                shift = JsonConvert.DeserializeObject<Shift>(response);

                if (shift != null)
                {
                    TableVizualisationEngine.ShowTable(new List<Shift> { shift }, "Shifts");
                }
            }

            return;
        }

        public async Task UpdateShift(int? id, DateTime? newStartTime, DateTime? newEndTime)

        {
            
            var shift = new Shift();
            
            using (var context = new ShiftContext(new DbContextOptions<ShiftContext>()))
            {
                shift = await context.Shifts.FindAsync(id);
                if (shift == null)
                {
                    Console.WriteLine($"Cant find shift with id = {id}");
                    return;
                }
            }

            if(newStartTime != null)
                shift.StartTime = newStartTime;
            if(newEndTime != null)
                shift.EndTime = newEndTime;

            using (var client = new HttpClient())
            {
               
                var result = await client.PutAsJsonAsync($"http://localhost:5294/api/Shifts/UpdateShift/{id}", shift);

                var response = await result.Content.ReadAsStringAsync();
                shift = JsonConvert.DeserializeObject<Shift>(response);

                if (shift != null)
                {
                    TableVizualisationEngine.ShowTable(new List<Shift> { shift }, "Shifts");
                }
            }
            return;
        }

        public async Task StartNewShift()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("http://localhost:5294/api/Shifts/StartNewShift");
                var response = await result.Content.ReadAsStringAsync();
            }

            return;
        }

        public async Task EndShift(int? id)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"http://localhost:5294/api/Shifts/EndShift?Id={id}");
                var response = await result.Content.ReadAsStringAsync();
                var shift = JsonConvert.DeserializeObject<Shift>(response);

                if (shift != null)
                {
                    TableVizualisationEngine.ShowTable(new List<Shift> { shift }, "Shifts");
                }

                return;
            }
        }

        public async Task CreateNewShift(DateTime? startTime, DateTime? endTime)
        {
            var shift = new Shift();

            using (var context = new ShiftContext(new DbContextOptions<ShiftContext>()))
            {

                shift = new Shift()
                {
                    StartTime = startTime,
                    EndTime = endTime
                };
            }

            using (var client = new HttpClient())
            {
                var result = await client.PostAsJsonAsync($"http://localhost:5294/api/Shifts/CreateNewShift", shift);

                var response = await result.Content.ReadAsStringAsync();

                shift = JsonConvert.DeserializeObject<Shift>(response);

                if (shift != null)
                {
                    TableVizualisationEngine.ShowTable(new List<Shift> { shift }, "Shifts");
                }
            }

            return;
        }

       
    }
}
