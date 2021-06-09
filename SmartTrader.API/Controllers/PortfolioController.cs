using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private ISmartTraderContext _context;
        private IPortfolioRepository _repository;

        public PortfolioController(ISmartTraderContext context, IPortfolioRepository repository)
        {

            _context = context;
            _repository = repository;

        }

        [HttpGet("{status}")]
        public IActionResult GetByStatus(string status)
        {
            var result = _repository.GetByStatus(status);
            return Ok(result);
        }

    }
}
