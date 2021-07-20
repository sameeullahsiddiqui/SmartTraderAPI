using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Helpers;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly ISmartTraderContext _context;
        private readonly IPortfolioRepository _repository;
        private readonly IStockPriceRepository _stockPriceRepository;

        public PortfolioController(ISmartTraderContext context, IPortfolioRepository repository, IStockPriceRepository stockPriceRepository)
        {

            _context = context;
            _repository = repository;
            _stockPriceRepository = stockPriceRepository;

        }

        [HttpGet("{status}")]
        public IActionResult GetByStatus(string status)
        {
            var result = _repository.GetByStatus(status).OrderByDescending(x => x.BuyExecutionTime);
            return Ok(result);
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var result = _repository.GetAll().OrderByDescending(x => x.BuyExecutionTime);
            return Ok(result);
        }

        [HttpGet("GetAllByName/{stockName}")]
        public IActionResult GetAllByName(string stockName)
        {
            var result = _repository.Find(x => x.SymbolName == stockName).OrderBy(x => x.BuyExecutionTime);
            return Ok(result);
        }

        [HttpPost()]
        public IActionResult Add(Portfolio model)
        {
            model.PortfolioId = Guid.NewGuid();

            _repository.Add(model);
            _repository.SaveChanges();

            return Ok();
        }


        [HttpPut("{portfolioId:Guid}")]
        public IActionResult Put(Guid portfolioId, Portfolio model)
        {
            var existingPortfolio = _repository.Find(x => x.PortfolioId == model.PortfolioId).FirstOrDefault();

            if (existingPortfolio != null && portfolioId == existingPortfolio.PortfolioId)
            {
                existingPortfolio.PortfolioName = model.PortfolioName;
                existingPortfolio.SymbolName = model.SymbolName;
                existingPortfolio.TradeType = model.TradeType;
                existingPortfolio.Quantity = model.Quantity;
                existingPortfolio.BuyDate = model.BuyDate.GetValueOrDefault().ToLocalTime();
                existingPortfolio.BuyPrice = model.BuyPrice;
                existingPortfolio.BuyCommission = model.BuyCommission;
                existingPortfolio.TargetPrice = model.TargetPrice;
                existingPortfolio.StopLossPrice = model.StopLossPrice;
                existingPortfolio.BreakEvenPrice = model.BuyPrice + model.BuyCommission;
                existingPortfolio.BuyOpen = model.BuyOpen;
                existingPortfolio.BuyHigh = model.BuyHigh;
                existingPortfolio.BuyLow = model.BuyLow;
                existingPortfolio.BuyClose = model.BuyClose;
                existingPortfolio.BuyDayReturn = model.BuyDayReturn;
                existingPortfolio.BuyComment = model.BuyComment;
                existingPortfolio.BuyExecutionTime = model.BuyExecutionTime.GetValueOrDefault().ToLocalTime();

                if(model.SellDate!=null)
                    existingPortfolio.SellDate = model.SellDate.GetValueOrDefault().ToLocalTime();
                existingPortfolio.SellPrice = model.SellPrice;
                existingPortfolio.SellCommission = model.SellCommission;
                existingPortfolio.SellOpen = model.SellOpen;
                existingPortfolio.SellHigh = model.SellHigh;
                existingPortfolio.SellLow = model.SellLow;
                existingPortfolio.SellClose = model.SellClose;
                existingPortfolio.SellDayReturn = model.SellDayReturn;
                existingPortfolio.SellComment = model.SellComment;
                
                if (model.SellExecutionTime != null)
                    existingPortfolio.SellExecutionTime = model.SellExecutionTime.GetValueOrDefault().ToLocalTime();

                existingPortfolio.Status = model.Status;


                existingPortfolio.TradeGrade = model.TradeGrade;
                existingPortfolio.BuyGrade = model.BuyGrade;
                existingPortfolio.SellGrade = model.SellGrade;
                existingPortfolio.AllowedRiskOnBuyDay = model.AllowedRiskOnBuyDay;

                if (model.SellPrice != null || model.SellPrice > 0)
                {
                    existingPortfolio.HoldingProfit = ((model.BuyPrice - model.SellPrice) * model.Quantity);
                    existingPortfolio.ProfitPercent = Utilities.CalculateChange(model.BuyPrice.GetValueOrDefault(), model.SellPrice.GetValueOrDefault());
                    existingPortfolio.CurrentProfit = existingPortfolio.HoldingProfit;
                    existingPortfolio.TradeDays = (int)(model.SellDate.GetValueOrDefault() - model.BuyDate.GetValueOrDefault()).TotalDays;
                }

                _repository.SaveChanges();


            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{portfolioId:Guid}")]
        public IActionResult Delete(Guid portfolioId)
        {
            if (portfolioId == Guid.Empty)
                return BadRequest("Not a valid trade id");

            var existingPortfolio = _repository.Find(x => x.PortfolioId == portfolioId).FirstOrDefault();

            _repository.Remove(existingPortfolio);
            _repository.SaveChanges();
            return Ok();
        }



        [HttpGet("SyncPortfolio/{status}")]
        public IActionResult SyncPortfolio(string status)
        {
            var portfolios = _repository.Find(x => x.Status == status).ToList();

            if (portfolios.Count() == 0)
                throw new Exception("Not Open trade found");

            var codes = portfolios.Select(x => x.SymbolName).Distinct().ToList<string>();
            var lastDate = _context.StockPrices.Max(t => t.Date);


            if (status == "Open")
            {
                UpdateOpenTrades(portfolios, codes, lastDate);
            }
            else if (status == "Closed")
            {
                foreach (var item in portfolios)
                {
                    if(item.SymbolName== "REPL")
                    {
                        Console.WriteLine("Test.in");
                    }

                    var tradePrices = _stockPriceRepository.Find(x => x.SymbolName == item.SymbolName && (x.Date == item.BuyDate.Value.Date ||
                    x.Date == item.SellDate.Value.Date || x.Date == lastDate.Date)).ToList();
                    var buyPrice = tradePrices.FirstOrDefault(x => x.Date == item.BuyDate.Value.Date);
                    var sellPrice = tradePrices.FirstOrDefault(x => x.Date == item.SellDate.Value.Date);
                    var currentPrice = tradePrices.FirstOrDefault(x => x.Date == lastDate.Date);
                    if (currentPrice == null)
                        continue;

                    item.CurrentPrice = currentPrice.Close;

                    if (buyPrice != null)
                    {
                        item.BuyOpen = item.BuyOpen == null ? buyPrice.Open : item.BuyOpen;
                        item.BuyHigh = item.BuyHigh == null ? buyPrice.High : item.BuyHigh;
                        item.BuyLow = item.BuyLow == null ? buyPrice.Low : item.BuyLow;
                        item.BuyClose = item.BuyClose == null ? buyPrice.Close : item.BuyClose;
                        item.BuyDayReturn = item.BuyDayReturn == null ? buyPrice.Percentage : item.BuyDayReturn;
                        item.BuyGrade = item.BuyGrade == null ? GetBuyGrade(item, buyPrice) : item.BuyGrade;

                        item.CurrentProfit = currentPrice != null ? ((currentPrice.Close - item.BuyPrice) * item.Quantity) : item.CurrentProfit;

                        item.HoldingProfit = item.SellPrice == null ? item.CurrentProfit : ((item.SellPrice - item.BuyPrice) * item.Quantity);

                        
                    }

                    if (sellPrice != null)
                    {
                        item.SellOpen = item.SellOpen == null ? sellPrice.Open : item.SellOpen;
                        item.SellHigh = item.SellHigh == null ? sellPrice.High : item.SellHigh;
                        item.SellLow = item.SellLow == null ? sellPrice.Low : item.SellLow;
                        item.SellClose = item.SellClose == null ? sellPrice.Close : item.SellClose;

                        item.SellDayReturn = item.SellDayReturn == null ? sellPrice.Percentage : item.SellDayReturn;
                        item.SellGrade = item.SellGrade == null ? GetSellGrade(item, sellPrice) : item.SellGrade;
                        item.TradeGrade = item.TradeGrade == null && item.SellPrice != 0 ? GetTradeGrade(item, sellPrice) : item.TradeGrade;

                        item.HoldingProfit = item.SellPrice > 0 ? ((item.SellPrice - item.BuyPrice) * item.Quantity) : item.HoldingProfit;
                    }

                    item.BuyCommission = item.BuyCommission == null ? 0 : item.BuyCommission;

                    item.TargetPrice = item.TargetPrice == null ? item.BuyPrice * 10 / 100 : item.TargetPrice;
                    item.StopLossPrice = item.StopLossPrice == null ? item.BuyPrice * 10 / 100 : item.StopLossPrice;

                    item.ProfitPercent = item.ProfitPercent == null ? Utilities.CalculateChange(item.BuyPrice.GetValueOrDefault(), item.SellPrice.GetValueOrDefault()) : item.ProfitPercent;

                    item.TradeDays = (int)(item.SellDate.GetValueOrDefault() - item.BuyDate.GetValueOrDefault()).TotalDays;
                    item.BreakEvenPrice = item.BuyPrice + item.BuyCommission;
                }

            }

            _repository.SaveChanges();

            return Ok();
        }

        private void UpdateOpenTrades(List<Portfolio> portfolios, List<string> codes, DateTime lastDate)
        {
            var latestPrices = _stockPriceRepository.Find(x => codes.Contains(x.SymbolName) && x.Date == lastDate).ToList();

            foreach (var item in portfolios)
            {
                var current = latestPrices.FirstOrDefault(x => x.SymbolName == item.SymbolName);
                if (current != null)
                {
                    item.CurrentProfit = ((current.Close - item.BuyPrice) * item.Quantity);

                    item.SellDayReturn = current.Percentage;
                    item.SellOpen = current.Open;
                    item.SellHigh = current.High;
                    item.SellLow = current.Low;
                    item.SellClose = current.Close;
                    item.HoldingProfit = item.CurrentProfit;
                    item.ProfitPercent = ((current.Close - item.BuyPrice) / item.BuyPrice * 100);

                    item.TradeDays = (int)(lastDate - item.BuyDate.GetValueOrDefault()).TotalDays;
                    item.CurrentPrice = current.Close;
                }
            }
        }


        private static decimal? GetSellGrade(Portfolio portfolio, StockPrice stockprice)
        {
            if (stockprice == null)
                return 0;

            var div = stockprice.High - stockprice.Low;
            if (div <= 0)
                return 0;

            return Math.Round((portfolio.SellPrice.GetValueOrDefault() - stockprice.Low) / (stockprice.High - stockprice.Low), 2);
        }

        private static decimal? GetBuyGrade(Portfolio portfolio, StockPrice stockprice)
        {
            if (stockprice == null)
                return 0;

            var div = stockprice.High - stockprice.Low;
            if (div <= 0)
                return 0;

            return Math.Round((stockprice.High - portfolio.BuyPrice.GetValueOrDefault()) / (stockprice.High - stockprice.Low), 2);
        }

        private static decimal? GetTradeGrade(Portfolio portfolio, StockPrice stockprice)
        {
            if (stockprice == null)
                return 0;

            var div = stockprice.High - stockprice.Low;
            if (div <= 0)
                return 0;

            return Math.Round((portfolio.BuyPrice.GetValueOrDefault() - portfolio.SellPrice.GetValueOrDefault()) / (stockprice.High - stockprice.Low), 2);
        }

    }
}
