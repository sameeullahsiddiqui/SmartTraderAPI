using System;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorStockController : ControllerBase
    {
        private ISmartTraderContext _context;
        private ISectorStockViewRepository _repository;

        public SectorStockController(ISmartTraderContext context, ISectorStockViewRepository repository)
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

        [HttpGet("GetBySectorName/{date:DateTime}/{sectorName}/{gainer:int}")]
        public IActionResult GetBySectorName(DateTime date,string sectorName, int gainer)
        {
            var result = _repository.GetBySectorName(date, sectorName, gainer);
            return Ok(result);
        }


        [HttpGet("GetByIndustryName/{date:DateTime}/{industryName}/{gainer:int}")]
        public IActionResult GetByIndustryName(DateTime date,string industryName, int gainer)
        {
            var result = _repository.GetByIndustryName(date, industryName, gainer);
            return Ok(result);
        }

        [HttpPut("{SectorStock}")]
        public void Put(string SectorStock, [FromBody] string newSectorStock)
        {
            //_repository.Update();
        }

    }
}
