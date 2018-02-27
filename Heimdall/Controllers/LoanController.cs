using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Tavant.CL.Gateway.Retail.Controllers
{

    [Route("Retail/[controller]")]
    public class LoanController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Index()
        {
            return new string[] { "Hello World" };
        }
    }
}
