using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using WebApplication10.Controllers.Utils;
using WebApplication10.DataAccess.Base;
using WebApplication10.Models;


namespace WebApplication10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carSearchController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public carSearchController(CoreDbContext context)
        {
            _context = context;
        }

        //POST:api/carSearch/getSell
        [HttpPost("getSell")]
        public async Task<IActionResult> GetSell(string searchkey)
        {

        }

        //POST:api/carSearch/getBuy
        [HttpPost("getBuy")]
        public async Task<IActionResult> GetBuy(string searchkey)
        {

        }
    }
}