using Business.Abstract;
using Core.Utilities.Result;
using IMandCRM.UI.Constants;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {         
            return View();
        }

    }
}
