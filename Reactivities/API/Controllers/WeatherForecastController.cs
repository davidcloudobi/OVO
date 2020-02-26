using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly DataContext context;


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext context)
        {
            _logger = logger;
            this.context = context;
        }

        [HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        public async  Task<ActionResult<IEnumerable<Value>>> Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            var value = await context.Values.ToArrayAsync();
            return Ok(value);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Value>> GetSingle(int id)
        {
            //var response = await context.Values.SingleOrDefaultAsync(res => res.Id == id);
            var response = await context.Values.FirstAsync(res => res.Id == id);
            return Ok(response);
        }
    }
}
