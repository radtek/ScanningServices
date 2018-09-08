using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScanningServicesDataObjects;
using NLog;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Net;
using ScanningServices.Models;


namespace ScanningServices.Controllers
{
    /// <returns></returns>
    /// 
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ScanningServicesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        
    }
}
