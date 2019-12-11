using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorLight;

namespace RazorLightTest.Controllers
{

    public class Test { 
    public string Name { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IRazorLightEngine _razorLightEngine;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IWebHostEnvironment webHostEnvironment )
        {
            _hostingEnvironment = webHostEnvironment; 
            _logger = logger;
        }

        [HttpGet]
        public  async Task<IActionResult> Get()
        {
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Views");
            var engine = new RazorLightEngineBuilder()
               .UseFileSystemProject(path)
               .UseMemoryCachingProvider()
               .Build();
            

            var result =await engine.CompileRenderAsync("jobs/config.cshtml",new Test { Name="World"});
            return Ok(result);
        }
        [HttpGet("1")]
        public async Task<IActionResult> Get2()
        {
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Views");
            var engine = new RazorLightEngineBuilder() 
                .UseFileSystemProject(path)
               .UseMemoryCachingProvider()
               .Build();

            var result = await engine.CompileRenderStringAsync("test", @"@using RazorLightTest.Controllers
@model Test

Hello @Model.Name", new Test { Name = "World" });
            
            return Ok(result);
        }
    }
}
