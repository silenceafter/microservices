using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/crud")]
    [ApiController]
    public class CrudController : Controller
    {
        private readonly ValuesHolder _holder;
        public ValuesHolder holder { get { return _holder; } set { } }

        public CrudController(ValuesHolder holder)
        {
            _holder = holder;
        }

        [HttpPost("create")]
        public IActionResult Create([FromQuery] int temperature, [FromQuery] DateTime time)
        {
            holder.Values.Add(new WeatherForecast() { TemperatureC = temperature, Date = time });
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult Read()
        {
            return Ok(holder.Values);
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime time, [FromQuery] int newTemperature)
        {
            for (int i = 0; i < holder.Values.Count; i++)
            {
                if (holder.Values[i].Date == time)
                    holder.Values[i] = new WeatherForecast() { TemperatureC = newTemperature, Date = time };
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime timeFrom, [FromQuery] DateTime timeTo)
        {
            holder.Values = holder.Values.Where(w => w.Date < timeFrom || w.Date > timeTo).ToList();                  
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}