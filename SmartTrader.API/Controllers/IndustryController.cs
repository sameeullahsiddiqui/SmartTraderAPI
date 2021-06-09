using System;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustryController : ControllerBase
    {
        private ISmartTraderContext _context;
        private IIndustryViewRepository _repository;

        public IndustryController(ISmartTraderContext context, IIndustryViewRepository repository)
        {

            _context = context;
            _repository = repository;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _repository.GetAll(); 
            return Ok(result);
        }

        [HttpGet("{date:DateTime}")]
        public IActionResult GetByDate(DateTime date)
        {
            var result = _repository.GetByDate(date);
            return Ok(result);
        }

        [HttpGet("GetByIndustryName/{industryName}")]
        public IActionResult GetByIndustryName(string industryName)
        {
            var result = _repository.GetByIndustryName(industryName);
            return Ok(result);
        }

        [HttpPut("{sector}")]
        public void Put(string sector, [FromBody] string newSector)
        {
            //_repository.Update();
        }

    }
}
