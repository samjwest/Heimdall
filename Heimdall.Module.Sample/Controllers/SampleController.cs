using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Heimdall.Module.Sample.Controllers
{

    [Route("Retail/[controller]")]
    public class SampleController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Index()
        {
            return new string[] { "Hello World" };
        }
    }
}
