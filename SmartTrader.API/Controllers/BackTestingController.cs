using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackTestingController : ControllerBase
    {

        private readonly ISmartTraderContext _context;
        private readonly IStockPriceRepository _repository;
        private readonly IEarningReportRepository _earningRepository;
        private readonly ISymbolRepository _symbolRepository;
        private readonly ILogger<BackTestingController> _logger;

        public BackTestingController(ISmartTraderContext context, IStockPriceRepository repository,
            IEarningReportRepository earningRepository,
            ISymbolRepository symbolRepository,
            ILogger<BackTestingController> logger)
        {

            _context = context;
            _repository = repository;
            _earningRepository = earningRepository;
            _symbolRepository = symbolRepository;
            _logger = logger;

        }


        [HttpGet("BackTestStrategy/{strategyName}/{symbolName}/{date:DateTime}")]
        public IActionResult BackTestStrategy(string strategyName, string symbolName, DateTime date)
        {
            var stockPrices = _repository.Find(x => x.Date >= date && x.SymbolName == symbolName).OrderBy(x => x.Date).ToList();

            var shortlistedStockQuery = _context.ShortlistedStocks.Where(x => x.IsActive == true);

            if (!string.IsNullOrEmpty(symbolName))
            {
                shortlistedStockQuery = shortlistedStockQuery.Where(x => x.SymbolName == symbolName);
            }

            if (!string.IsNullOrEmpty(strategyName))
            {
                shortlistedStockQuery = shortlistedStockQuery.Where(x => x.StrategyName == strategyName);
            }

            var shortlistedStocks = shortlistedStockQuery.ToList();

            //Step 1 - find potential stocks
            foreach (var item in stockPrices)
            {
                var currentStock = shortlistedStocks.Where(x => x.SymbolName == item.SymbolName && x.IsActive).FirstOrDefault();
                if (item.Monthly < 0)
                {
                    if (currentStock == null)
                    {
                        _logger.LogInformation($"Shortlisted stock {item.SymbolName} on {item.Date}, Monthly: {item.Monthly} ");
                        shortlistedStocks.Add(new ShortlistedStock
                        {
                            SymbolName = item.SymbolName,
                            ShortlistDate = item.Date,
                            Percentage = item.Percentage,
                            Monthly = item.Monthly,
                            Weekly = item.Weekly,
                            DelRatio = item.DelRatio,
                            StrategyName = strategyName,
                            IsActive = true,
                            Price = item.Close
                        });

                    //} else if (isHighVolumeHammer) { 
                    //} else if (isNearBottom) { 
                    }
                    else if (currentStock.Price > item.Close)
                    {
                        currentStock.Price = item.Close;
                        currentStock.ClosedDate = item.Date;
                        currentStock.DelRatio = item.DelRatio;

                        _logger.LogInformation($"Close stock {item.SymbolName} on {item.Date} Del Ratio:{item.DelRatio}");
                    }
                    else if (currentStock != null && currentStock.IsActive && item.DelRatio >= 2)
                    {
                        currentStock.IsActive = false;
                        currentStock.ClosedDate = item.Date;

                        _context.WatchLists.Add(new WatchList
                        {
                            Symbol = item.SymbolName,
                            Date = item.Date,
                            Description = strategyName,
                            ReasonToWatch = strategyName,
                            Price = (double)item.Close,
                            Status = "Open",
                            UpdateTime = item.Date,
                            CurrentPrice = (double)item.Close,
                            ChangeSinceAdded = 0,
                            Days = 0
                        });

                        _logger.LogInformation($"Add stock {item.SymbolName} on {item.Date} to Watch list, Del Ratio:{item.DelRatio}");
                    }
                    else
                    {
                        _logger.LogInformation($"Skip stock {item.SymbolName} on {item.Date}, Monthly: {item.Monthly}");
                    }
                }
            }


            //foreach (var item in stockPrices)
            //{

            //    var currentStock = shortlistedStocks.Where(x => x.SymbolName == item.SymbolName && x.IsActive).FirstOrDefault();

            //    if (currentStock != null && item.DelRatio >= 2)
            //    {
            //        currentStock.IsActive = false;
            //        currentStock.ClosedDate = item.Date;

            //        _context.WatchLists.Add(new WatchList
            //        {
            //            Symbol = item.SymbolName,
            //            Date = item.Date,
            //            Description = strategyName,
            //            ReasonToWatch = strategyName,
            //            Price = (double)item.Close,
            //            Status = "Open",
            //            UpdateTime = item.Date,
            //            CurrentPrice = (double)item.Close,
            //            ChangeSinceAdded = 0,
            //            Days = 0
            //        });

            //        _logger.LogInformation($"Add stock {item.SymbolName} on {item.Date} to Watch list, Del Ratio:{item.DelRatio}");
            //    }
            //}

            _context.ShortlistedStocks.AddRange(shortlistedStocks.Where(x => x.ShortlistedStockId == 0).ToList());

            _context.SaveChanges();

            return Ok();
        }


    }
}
