using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RemoteOrganControl.Server.Controllers
{
    public class SmfController : Controller
    {
        private OrganController _organController;
        
        public SmfController(OrganController organController)
        {
            _organController = organController;
        }
        
        // GET
        [HttpGet]
        public IEnumerable<string> Index()
        {
            var strings = new List<string>();
            strings.Add("Testing 123");
            strings.Add("This is gonna work");
            return strings;
        }

        [HttpPost("smf/upload")]
        public IActionResult PostUpload(IFormFile file)
        {
            var memStream = new MemoryStream();
            file.CopyTo(memStream);
            this._organController.AddSmf(file.FileName, memStream);
            return StatusCode(200, "SMF uploaded successfully.");
        }
    }
}