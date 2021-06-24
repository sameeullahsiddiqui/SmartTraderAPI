using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymbolController : ControllerBase
    {
        private ISmartTraderContext _context;
        private ISymbolRepository _repository;

        public SymbolController(ISmartTraderContext context, ISymbolRepository repository)
        {

            _context = context;
            _repository = repository;

        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var result = _repository.GetAll();
            return Ok(result);
        }

        [HttpGet("search/{code}")]
        public IActionResult Search(string code)
        {
            var result = _repository.Find(x=>x.Code == code).Select(x=>x.Code);
            return Ok(result);
        }

        [HttpGet("GetCompanyCode/{name}")]
        public IActionResult GetCompanyCode(string name)
        {
            var result = _repository.Find(x => x.Name == name).FirstOrDefault();

            if (result==null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{symbolId}")]
        public IActionResult Put(int symbolId, Symbol model)
        {
            var existingSymbol = _repository.Get(model.SymbolId);

            if (existingSymbol != null && symbolId == existingSymbol.SymbolId)
            {
                existingSymbol.Sector = model.Sector;
                existingSymbol.Industry = model.Industry;
                existingSymbol.Flag = model.Flag;

                _repository.SaveChanges();

            }
            else
            {
                return NotFound();
            }

            return Ok();
        }


    }
}
