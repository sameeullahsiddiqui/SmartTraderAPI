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
    public class DeliveryController : ControllerBase
    {
        private ISmartTraderContext _context;
        private IDeliveryRepository _repository;

        public DeliveryController(ISmartTraderContext context, IDeliveryRepository repository)
        {

            _context = context;
            _repository = repository;

        }

        [HttpGet("{date:DateTime}/{withDelivery}")]
        public IActionResult GetByDate(DateTime date, bool withDelivery)
        {
            var result = _repository.GetByDate(date, withDelivery);
            return Ok(result);
        }

    }
}
