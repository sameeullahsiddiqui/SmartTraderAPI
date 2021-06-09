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
    public class TradeController : ControllerBase
    {
        private ISmartTraderContext _context;
        private ITradeRepository _repository;

        public TradeController(ISmartTraderContext context, ITradeRepository repository)
        {

            _context = context;
            _repository = repository;

        }

        [HttpGet("{date:DateTime}")]
        public IActionResult GetByDate(DateTime date)
        {
            var result = _repository.GetByDate(date);
            return Ok(result);
        }

        [HttpGet("{symbol}")]
        public IActionResult GetBySymbol(string symbol)
        {
            var result = _repository.GetBySymbol(symbol);
            return Ok(result);
        }

    }
}
