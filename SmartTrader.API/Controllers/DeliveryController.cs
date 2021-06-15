using System;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryRepository _repository;

        public DeliveryController(IDeliveryRepository repository)
        {

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
