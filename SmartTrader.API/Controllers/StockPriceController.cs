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
    public class StockPriceController : ControllerBase
    {
        private readonly ISmartTraderContext _context;
        private readonly IStockPriceRepository _repository;
        private readonly IEarningReportRepository _earningRepository;
        private readonly ISymbolRepository _symbolRepository;

        public StockPriceController(ISmartTraderContext context, IStockPriceRepository repository,
            IEarningReportRepository earningRepository,
            ISymbolRepository symbolRepository)
        {

            _context = context;
            _repository = repository;
            _earningRepository = earningRepository;
            _symbolRepository = symbolRepository;

        }

        [HttpGet("{stockname}/{date:DateTime}")]
        public IActionResult GetByName(string stockname, DateTime date)
        {
            var result = _repository.GetByName(stockname, date);
            return Ok(result);
        }


        [HttpGet("UpdateEarningReports/{year}")]
        public IActionResult UpdateEarningReports(int year)
        {

            var earningReport = _earningRepository.Find(x => x.Year == year).ToList();
            var companyNames = earningReport.Select(x => x.Company).Distinct().ToList();
            var symbols = _symbolRepository.Find(x => companyNames.Contains(x.Name)).ToList();

            foreach (var symbol in symbols)
            {
                var currentReport = earningReport.FirstOrDefault(x => x.Company == symbol.Name);
                if (currentReport == null)
                {
                    continue;
                }

                var earningDate = Utilities.PreviousWorkDay(currentReport.Date.Date);
                var stockPrice = _repository.Find(x => x.SymbolName == symbol.Code && x.Date == earningDate).FirstOrDefault();
                if (stockPrice == null)
                    continue;

                SetTooltip(currentReport.QoQGrossProfit_Q1,
             currentReport.QoQNetProfit_Q1,
             currentReport.QoQSales_Q1,
             currentReport.YoYGrossProfit_Q1,
             currentReport.YoYNetProfit_Q1,
             currentReport.YoYSales_Q1,
             stockPrice);

            }
            _repository.SaveChanges();
            return Ok();
        }


        [HttpGet("Split/{stockname}/{date:DateTime}/{operationType}/{oldFaceValue}/{newFaceValue}")]
        public IActionResult Split(string stockname, DateTime date, string operationType, int oldFaceValue, int newFaceValue)
        {
            if (newFaceValue > 0)
            {
                var allStockPrices = _repository.Find(x => x.SymbolName == stockname).OrderBy(x => x.Date).ToList();
                var splitDate = Utilities.PreviousWorkDay(date);
                var currentPrice = allStockPrices.FirstOrDefault(x => x.Date == splitDate);
                if (currentPrice.Reason == "S" || currentPrice.Reason == "B")
                {
                    return NotFound();
                }

                var beforeSplit = allStockPrices.Where(x => x.Date <= splitDate).ToList();

                foreach (var item in beforeSplit)
                {
                    if (operationType == "Split")
                    {
                        item.Open /= (decimal)(oldFaceValue / newFaceValue);
                        item.High /= (decimal)(oldFaceValue / newFaceValue);
                        item.Low /= (decimal)(oldFaceValue / newFaceValue);
                        item.Close /= (decimal)(oldFaceValue / newFaceValue);
                    }
                    else if (operationType == "Bonus")
                    {
                        item.Open *= (oldFaceValue / (decimal)(oldFaceValue + newFaceValue));
                        item.High *= (oldFaceValue / (decimal)(oldFaceValue + newFaceValue));
                        item.Low *= (oldFaceValue / (decimal)(oldFaceValue + newFaceValue));
                        item.Close *= (oldFaceValue / (decimal)(oldFaceValue + newFaceValue));
                    }
                }

                foreach (var entry in allStockPrices)
                {
                    var monthlyData = allStockPrices.Where(x => x.Date <= entry.Date).Select(x => Convert.ToDecimal(x.Close)).TakeLast(22).ToArray();
                    if (monthlyData.Length >= 22)
                    {
                        entry.Monthly = Utilities.CalculateChange((decimal)monthlyData.FirstOrDefault(), (decimal)monthlyData.LastOrDefault());
                    }

                    var weeklyData = allStockPrices.Where(x => x.Date <= entry.Date).OrderBy(x => x.Date).Select(x => Convert.ToDecimal(x.Close)).TakeLast(7).ToArray();
                    if (weeklyData.Length >= 7)
                    {
                        entry.Weekly = Utilities.CalculateChange((decimal)weeklyData.FirstOrDefault(), (decimal)weeklyData.LastOrDefault());
                    }
                }


                currentPrice.Reason = operationType == "Split" ? "S" : "B";
                currentPrice.Tooltip = $"{operationType}({oldFaceValue}:{newFaceValue})";

                _repository.SaveChanges();
            }

            return Ok();
        }


        [HttpGet("UpdateIndicators/{stockname}/{date:DateTime}")]
        public IActionResult UpdateIndicators(string stockname, DateTime date)
        {
            var stockPrices = _repository.Find(x => x.Date <= date && x.SymbolName == stockname).OrderBy(x => x.Date).ToList();

            var lowVolDays = 0;

            var previousPrice = stockPrices.Last();
            foreach (var item in stockPrices)
            {
                if (!string.IsNullOrEmpty(item.Reason) && item.Reason != "S" && item.Reason != "B" && !item.Reason.Contains("E("))
                {
                    item.Reason = string.Empty;
                }

                if (item.Date == date)
                {
                    Console.WriteLine("Current Date");
                }

                if (string.IsNullOrEmpty(item.Reason))
                    lowVolDays = CheckIfVolumeSpickAfter4Days(lowVolDays, item);

                CheckNearLow(previousPrice, item);


                //CheckIfLast40DaysHighBroken(stockPrices, item);

                //if (string.IsNullOrEmpty(item.Reason))
                //    SetCandleStickPattern(stockPrices, item);

                previousPrice = item;

            }

            _repository.SaveChanges();

            return Ok();
        }

        private static void CheckNearLow(StockPrice previousPrice, StockPrice item)
        {
            var nearPrice = item.Low - (item.Low * 2 / 100);

            if (item.Q1Low > 0 && nearPrice < item.Q1Low && item.Low > item.Q1Low && item.Weekly < -1)
            {
                previousPrice.Reason = !string.IsNullOrEmpty(previousPrice.Reason) && previousPrice.Reason != "S" && previousPrice.Reason != "B" && !previousPrice.Reason.Contains("E(") ? "" : previousPrice.Reason;
                item.Reason = string.IsNullOrEmpty(item.Reason) ? "Near Q1Low" : $"{ item.Reason},Near Q1Low";
            }

            if (item.Q2Low > 0 && nearPrice < item.Q2Low && item.Low > item.Q2Low && item.Weekly < -1)
            {
                previousPrice.Reason = !string.IsNullOrEmpty(previousPrice.Reason) && previousPrice.Reason != "S" && previousPrice.Reason != "B" && !previousPrice.Reason.Contains("E(") ? "" : previousPrice.Reason;
                item.Reason = string.IsNullOrEmpty(item.Reason) ? "Near Q2Low" : $"{ item.Reason},Near Q2Low";
            }

            if (item.Q3Low > 0 && nearPrice < item.Q3Low && item.Low > item.Q3Low && item.Weekly < -1)
            {
                previousPrice.Reason = !string.IsNullOrEmpty(previousPrice.Reason) && previousPrice.Reason != "S" && previousPrice.Reason != "B" && !previousPrice.Reason.Contains("E(") ? "" : previousPrice.Reason;
                item.Reason = string.IsNullOrEmpty(item.Reason) ? "Near Q3Low" : $"{ item.Reason},Near Q3Low";
            }

            if (item.Q4Low > 0 && nearPrice < item.Q4Low && item.Low > item.Q4Low && item.Weekly < -1)
            {
                previousPrice.Reason = !string.IsNullOrEmpty(previousPrice.Reason) && previousPrice.Reason != "S" && previousPrice.Reason != "B" && !previousPrice.Reason.Contains("E(") ? "" : previousPrice.Reason;
                item.Reason = string.IsNullOrEmpty(item.Reason) ? "Near Q4Low" : $"{ item.Reason},Near Q4Low";
            }

            if (previousPrice.Close > item.Close)
            {
                previousPrice.Reason = !string.IsNullOrEmpty(previousPrice.Reason) && previousPrice.Reason != "S" && previousPrice.Reason != "B" && !previousPrice.Reason.Contains("E(") ? "" : previousPrice.Reason;
            }
        }

        [HttpGet("BuildPortfolio")]
        public async Task<IActionResult> BuildPortfolioAsync()
        {
            await BuildPortfolioAsync("Group-Samee");

            return Ok();
        }

        private static void SetCandleStickPattern(List<StockPrice> stockPrices, StockPrice currentStockPrice)
        {

            var previousDay = stockPrices.Where(x => x.Date < currentStockPrice.Date).OrderBy(x => x.Date).TakeLast(1).LastOrDefault();

            //consider only last red day.
            if (previousDay != null
                && previousDay.Weekly < 6
                && currentStockPrice.Close < previousDay.Close
                && currentStockPrice.Open < previousDay.Close)
            {

                var open = currentStockPrice.Open;
                var high = currentStockPrice.High;
                var low = currentStockPrice.Low;
                var close = currentStockPrice.Close;

                var isGreenCandle = open < close;
                var isRedCandle = open > close;

                if (isGreenCandle)
                    HammerCandlePattern(currentStockPrice, open, high, low, close);
                else if (isRedCandle)
                    HammerCandlePattern(currentStockPrice, close, high, low, open);
            }
        }

        private static void HammerCandlePattern(StockPrice currentStockPrice, decimal open, decimal high, decimal low, decimal close)
        {
            var upperWick = high - close;
            var body = close - open;
            var lowerWick = open - low;

            if (upperWick < lowerWick && 3 * body < lowerWick)
                currentStockPrice.Reason = "Hammer";
            if (currentStockPrice.Reason == "Hammer" && currentStockPrice.DelRatio >= 2)
            {
                currentStockPrice.Reason = "Strong Hammer";
            }
        }


        private static int CheckIfVolumeSpickAfter4Days(int lowVolDays, StockPrice item)
        {
            if (item.DelRatio < 1)
            {
                lowVolDays++;
            }
            else if ((double)item.DelRatio > 1.5 && lowVolDays >= 4 && string.IsNullOrEmpty(item.Reason))
            {
                item.Reason = "Volume Spike";
                lowVolDays = 0;
            }
            else if ((double)item.DelRatio > 5 && string.IsNullOrEmpty(item.Reason))
            {
                item.Reason = "Volume Spike";
                lowVolDays = 0;
            }
            else if (item.DelRatio >= 2)
            {
                lowVolDays = 0;
            }
            else
                lowVolDays = 0;
            return lowVolDays;
        }

        private static void CheckIfLast40DaysHighBroken(List<StockPrice> stockPrices, StockPrice item)
        {
            //take last 40 days high
            var last3MonthData = stockPrices.Where(x => x.Date < item.Date).TakeLast(30).ToList();
            var last3MonthHigh = item.High;

            var is3MBrokenRecently = last3MonthData.Any(x => x.Reason.Contains("3M broken"));

            if (last3MonthData.Count() == 30 && !is3MBrokenRecently && (item.Reason != "B" || item.Reason != "S"))
            {
                last3MonthHigh = last3MonthData.Max(x => x.High);

                if (item.Close > last3MonthHigh)
                {
                    item.Reason = "3M broken";
                }
            }

            var last4DaysList = stockPrices.Where(x => x.Date < item.Date).TakeLast(4).OrderBy(x => x.Date).ToList();
            var last4DaysDelivery = last4DaysList.Select(x => x.DeliveryQty).ToArray();

            if (last4DaysDelivery.Length == 4 && last4DaysDelivery[0] > last4DaysDelivery[1] &&
               last4DaysDelivery[1] > last4DaysDelivery[2] &&
               last4DaysDelivery[2] > last4DaysDelivery[3] &&
               last4DaysDelivery[3] < item.DeliveryQty &&
               last4DaysList.Last().Weekly < 6 &&
               item.Open < last4DaysList.Last().Close &&
               item.Close < last4DaysList.Last().Close &&
               !item.Reason.Contains("4D Strategy")
                )
                item.Reason = string.IsNullOrEmpty(item.Reason) ? "4D Strategy" : $"{item.Reason} & 4D Strategy";
        }

        private static void SetTooltip(double qoqGrossProfit, double qoqNetProfit, double qoqSales,
    double yoyGrossProfit, double yoyNetProfit, double yoySales, StockPrice stockPrice)
        {
            string tooltip;
            bool qStatus;
            bool yStatus;

            tooltip = $"<b>QoQ</b>" +
                        $"<br> GP:{ GetColouredSpan(qoqGrossProfit)}% " +
                        $"<br> NP: {GetColouredSpan(qoqNetProfit)}% " +
                        $"<br> Sales:{GetColouredSpan(qoqSales)}% <br>" +
                      $"<br> <b>YoY</b>" +
                      $"<br> GP:{GetColouredSpan(yoyGrossProfit)}% " +
                      $"<br> NP: {GetColouredSpan(yoyNetProfit)}% " +
                      $"<br> Sales:{GetColouredSpan(yoySales)}%";

            qStatus = qoqGrossProfit > 0 && qoqNetProfit > 0 && qoqSales > 0;
            yStatus = yoyGrossProfit > 0 && yoyNetProfit > 0 && yoySales > 0;
            stockPrice.Reason = "E(Q" + (qStatus ? "+" : "-") + ",Y" + (yStatus ? "+)" : "-)");
            stockPrice.Tooltip = tooltip;
        }

        private static string GetColouredSpan(double value)
        {

            var result = "";
            if (value < 0)
                result = "<span style='text-align:right; color:red; width:100%; display:block;'>" + value + "</span>";
            else if (value > 0)
                result = "<span style='text-align:right; color:green; width:100%; display:block;'>" + value + "</span>";

            return result;
        }

        #region Portfolio

        private async Task BuildPortfolioAsync(string portfolioName)
        {
            try
            {
                var trades = _context.Trades.Where(x => x.BalanceQty > 0).OrderBy(x => x.ExecutionTime).ToList();
                if (trades.Count() == 0)
                    throw new Exception("No Trades found");

                var groupTrades = trades.GroupBy(x => new
                {
                    x.OrderId
                }).Select(g => new Trade
                {
                    OrderId = g.Key.OrderId,
                    BalanceQty = g.Sum(x => x.BalanceQty),
                    Price = g.Average(x => x.Price)
                }).ToList();

                var portfolioEnteries = _context.Portfolios.Where(x => x.PortfolioName == portfolioName && x.SellPrice == null)
                        .OrderBy(x => x.BuyDate)
                        .ToList();

                var tradedSymbols = trades.Select(x => x.ScriptName).Distinct().ToList();
                var tradedDates = trades.Select(x => x.TradeDate).Distinct().ToList();

                var stockpriceEntries = _context.StockPrices.Where(x => tradedSymbols.Contains(x.SymbolName) && tradedDates.Contains(x.Date)).ToList();

                foreach (var groupTrade in groupTrades)
                {
                    await UpdatePortfolio(portfolioEnteries, stockpriceEntries, portfolioName, trades, groupTrade, groupTrade.BalanceQty);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task UpdatePortfolio(List<Portfolio> portfolioEntries, List<StockPrice> stockpriceEntries, string portfolioName, List<Trade> trades, Trade groupTrade, int tradeQuantity)
        {

            var testStockName = "";

            var trade = trades.FirstOrDefault(x => x.OrderId == groupTrade.OrderId);
            var stockprice = stockpriceEntries.FirstOrDefault(x => x.SymbolName == trade.ScriptName && x.Date == trade.TradeDate);
            var longPortfolioEntry = portfolioEntries.Where(x => x.PortfolioName == portfolioName &&
                                                        x.SymbolName == trade.ScriptName &&
                                                        x.SellPrice == null)
                                                     .OrderBy(x => x.BuyDate)
                                                     .FirstOrDefault();

            var shortSellPortfolioEntry = portfolioEntries.Where(x => x.PortfolioName == portfolioName &&
                                                                 x.SymbolName == trade.ScriptName &&
                                                                 x.BuyPrice == null)
                                                           .OrderBy(x => x.SellDate)
                                                           .FirstOrDefault();

            if (shortSellPortfolioEntry == null && trade.TradeType == "buy")
            {
                if (trade.ScriptName == testStockName)
                {
                    Console.WriteLine("longbuy");
                }
                AddLongTrade(portfolioEntries, portfolioName, trades, groupTrade, trade, stockprice);
            }
            else if (longPortfolioEntry == null && trade.TradeType == "sell")
            {
                if (trade.ScriptName == testStockName)
                {
                    Console.WriteLine("Shortsell");
                }
                AddShortSellTrade(portfolioEntries, portfolioName, trades, groupTrade, trade, stockprice);
            }
            else if (longPortfolioEntry != null && trade.TradeType == "sell")
            {
                if (trade.ScriptName == testStockName)
                {
                    Console.WriteLine("longclose");
                }

                await CloseLongPosition(portfolioEntries, stockpriceEntries, portfolioName, trades, groupTrade, tradeQuantity, trade, stockprice, longPortfolioEntry);
            }
            else if (shortSellPortfolioEntry != null && trade.TradeType == "buy")
            {
                if (trade.ScriptName == testStockName)
                {
                    Console.WriteLine("shortclose");
                }

                await CloseShortPosition(portfolioEntries, stockpriceEntries, portfolioName, trades, groupTrade, tradeQuantity, trade, stockprice, shortSellPortfolioEntry);
            }
        }

        private void AddShortSellTrade(List<Portfolio> portfolioEntries, string portfolioName, List<Trade> trades, Trade groupTrade, Trade trade, StockPrice stockprice)
        {
            var entry = AddPortfolioEntryShortSell(groupTrade, trade, stockprice, portfolioName);
            entry.SellGrade = GetSellGrade(entry, stockprice);
            entry.TradeType = "Intraday-ShortSell";
            var tradesByOrder = trades.Where(x => x.OrderId == groupTrade.OrderId).ToList();

            foreach (var item in tradesByOrder)
            {
                item.PortfolioName = entry.PortfolioName;
                item.BalanceQty = 0;
            }

            portfolioEntries.Add(entry);
            _context.Portfolios.Add(entry);
        }

        private void AddLongTrade(List<Portfolio> portfolioEntries, string portfolioName, List<Trade> trades, Trade groupTrade, Trade trade, StockPrice stockprice)
        {
            var entry = AddPortfolioEntry(groupTrade, trade, stockprice, portfolioName);
            entry.BuyGrade = GetBuyGrade(entry, stockprice);
            var tradesByOrder = trades.Where(x => x.OrderId == groupTrade.OrderId).ToList();

            foreach (var item in tradesByOrder)
            {
                item.PortfolioName = entry.PortfolioName;
                item.BalanceQty = 0;
            }

            portfolioEntries.Add(entry);
            _context.Portfolios.Add(entry);
        }

        private async Task CloseLongPosition(List<Portfolio> portfolioEntries, List<StockPrice> stockpriceEntries, string portfolioName, List<Trade> trades, Trade groupTrade, int tradeQuantity, Trade trade, StockPrice stockprice, Portfolio longPortfolioEntry)
        {
            if (tradeQuantity < longPortfolioEntry.Quantity)
            {
                var newPortfolio = longPortfolioEntry.DeepCopy();
                newPortfolio.Quantity = longPortfolioEntry.Quantity - tradeQuantity;
                longPortfolioEntry.Quantity = tradeQuantity;
                portfolioEntries.Add(newPortfolio);
                _context.Portfolios.Add(newPortfolio);
            }

            longPortfolioEntry.SellDate = trade.TradeDate;
            longPortfolioEntry.SellPrice = groupTrade.Price;
            longPortfolioEntry.SellCommission = 0;
            longPortfolioEntry.SellOpen = stockprice?.Open;
            longPortfolioEntry.SellHigh = stockprice?.High;
            longPortfolioEntry.SellLow = stockprice?.Low;
            longPortfolioEntry.SellClose = stockprice?.Close;
            longPortfolioEntry.SellDayReturn = stockprice?.Percentage;
            longPortfolioEntry.SellComment = "";
            longPortfolioEntry.BuyGrade = GetBuyGrade(longPortfolioEntry, stockprice);
            longPortfolioEntry.SellGrade = GetSellGrade(longPortfolioEntry, stockprice);
            longPortfolioEntry.TradeGrade = GetTradeGrade(longPortfolioEntry, stockprice);
            longPortfolioEntry.TradeDays = (int)(trade.TradeDate - longPortfolioEntry.BuyDate.GetValueOrDefault()).TotalDays;
            longPortfolioEntry.CurrentProfit = (groupTrade.Price - longPortfolioEntry.BuyPrice) * groupTrade.BalanceQty;
            longPortfolioEntry.ProfitPercent = Utilities.CalculateChange(longPortfolioEntry.BuyPrice.GetValueOrDefault(), groupTrade.Price);
            longPortfolioEntry.HoldingProfit = 0;
            longPortfolioEntry.Status = "Close";
            longPortfolioEntry.SellExecutionTime = trade.ExecutionTime;


            if (longPortfolioEntry.BuyDate.GetValueOrDefault().Date == longPortfolioEntry.SellDate.GetValueOrDefault().Date)
                longPortfolioEntry.TradeType = "Intraday-Long";
            else
                longPortfolioEntry.TradeType = "Holding";

            var tradesByOrder = trades.Where(x => x.OrderId == groupTrade.OrderId).ToList();

            foreach (var item in tradesByOrder)
            {
                item.PortfolioName = longPortfolioEntry.PortfolioName;
                item.BalanceQty = 0;
            }

            var balanceQuantity = longPortfolioEntry.Quantity - tradeQuantity;
            if (balanceQuantity > 0)
            {
                trade.PortfolioName = null;
                trade.BalanceQty = balanceQuantity;
            }

            if (tradeQuantity > longPortfolioEntry.Quantity)
            {
                await UpdatePortfolio(portfolioEntries, stockpriceEntries, portfolioName, trades, groupTrade, tradeQuantity - longPortfolioEntry.Quantity);
            }
        }

        private async Task CloseShortPosition(List<Portfolio> portfolioEntries, List<StockPrice> stockpriceEntries, string portfolioName, List<Trade> trades, Trade groupTrade, int tradeQuantity, Trade trade, StockPrice stockprice, Portfolio shortPortfolioEntry)
        {
            if (tradeQuantity < shortPortfolioEntry.Quantity)
            {
                var newPortfolio = shortPortfolioEntry.DeepCopy();
                newPortfolio.Quantity = shortPortfolioEntry.Quantity - tradeQuantity;
                shortPortfolioEntry.Quantity = tradeQuantity;
                portfolioEntries.Add(newPortfolio);
                _context.Portfolios.Add(newPortfolio);
            }

            shortPortfolioEntry.BuyDate = trade.TradeDate;
            shortPortfolioEntry.BuyPrice = groupTrade.Price;
            shortPortfolioEntry.BuyCommission = 0;
            shortPortfolioEntry.BuyOpen = stockprice?.Open;
            shortPortfolioEntry.BuyHigh = stockprice?.High;
            shortPortfolioEntry.BuyLow = stockprice?.Low;
            shortPortfolioEntry.BuyClose = stockprice?.Close;
            shortPortfolioEntry.BuyDayReturn = stockprice?.Percentage;
            shortPortfolioEntry.BuyComment = "";
            shortPortfolioEntry.BuyGrade = GetBuyGrade(shortPortfolioEntry, stockprice);
            shortPortfolioEntry.SellGrade = GetSellGrade(shortPortfolioEntry, stockprice);
            shortPortfolioEntry.TradeGrade = GetTradeGrade(shortPortfolioEntry, stockprice);
            shortPortfolioEntry.TradeDays = (int)(trade.TradeDate - shortPortfolioEntry.BuyDate.GetValueOrDefault()).TotalDays;
            shortPortfolioEntry.CurrentProfit = (shortPortfolioEntry.SellPrice - groupTrade.Price) * groupTrade.BalanceQty;
            shortPortfolioEntry.ProfitPercent = Utilities.CalculateChange(groupTrade.Price, shortPortfolioEntry.SellPrice.GetValueOrDefault());
            shortPortfolioEntry.HoldingProfit = 0;
            shortPortfolioEntry.Status = "Close";
            shortPortfolioEntry.BuyExecutionTime = trade.ExecutionTime;

            if (shortPortfolioEntry.BuyDate.GetValueOrDefault().Date == shortPortfolioEntry.SellDate.GetValueOrDefault().Date)
                shortPortfolioEntry.TradeType = "Intraday-ShortSell";
            else
                shortPortfolioEntry.TradeType = "Holding";

            var tradesByOrder = trades.Where(x => x.OrderId == groupTrade.OrderId).ToList();

            foreach (var item in tradesByOrder)
            {
                item.PortfolioName = shortPortfolioEntry.PortfolioName;
                item.BalanceQty = 0;
            }

            var balanceQuantity = shortPortfolioEntry.Quantity - tradeQuantity;
            if (balanceQuantity > 0)
            {
                trade.PortfolioName = null;
                trade.BalanceQty = balanceQuantity;
            }

            if (tradeQuantity > shortPortfolioEntry.Quantity)
            {
                await UpdatePortfolio(portfolioEntries, stockpriceEntries, portfolioName, trades, groupTrade, tradeQuantity - shortPortfolioEntry.Quantity);
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

        private Portfolio AddPortfolioEntry(Trade groupTrade, Trade trade, StockPrice stockprice, string portfolioName)
        {
            return new Portfolio
            {
                PortfolioId = Guid.NewGuid(),
                PortfolioName = portfolioName,
                SymbolName = trade.ScriptName,
                Quantity = groupTrade.BalanceQty,
                BuyDate = trade.TradeDate,
                BuyPrice = groupTrade.Price,
                AllowedRiskOnBuyDay = 0, //Calculate
                BuyCommission = 0, // Add commission manually or get logic
                TargetPrice = 0, // Calculate target price
                StopLossPrice = 0, // calculate stoploss
                BreakEvenPrice = 0, // Get break even
                BuyOpen = stockprice?.Open,
                BuyHigh = stockprice?.High,
                BuyLow = stockprice?.Low,
                BuyClose = stockprice?.Close,
                BuyDayReturn = stockprice?.Percentage,
                BuyComment = "",
                Status = "Open",
                CurrentProfit = GetProfit(trade.Price, trade.BalanceQty, stockprice),
                ProfitPercent = 0,
                HoldingProfit = 0,
                TradeType = "",
                BuyExecutionTime = trade.ExecutionTime
            };
        }

        private Portfolio AddPortfolioEntryShortSell(Trade groupTrade, Trade trade, StockPrice stockprice, string portfolioName)
        {
            return new Portfolio
            {
                PortfolioId = Guid.NewGuid(),
                PortfolioName = portfolioName,
                SymbolName = trade.ScriptName,
                Quantity = groupTrade.BalanceQty,
                SellDate = trade.TradeDate,
                SellPrice = groupTrade.Price,
                AllowedRiskOnBuyDay = 0,
                SellCommission = 0,
                TargetPrice = 0,
                StopLossPrice = 0,
                BreakEvenPrice = 0,
                SellOpen = stockprice?.Open,
                SellHigh = stockprice?.High,
                SellLow = stockprice?.Low,
                SellClose = stockprice?.Close,
                SellDayReturn = stockprice?.Percentage,
                SellComment = "",
                Status = "Open",
                CurrentProfit = GetProfit(trade.Price, trade.BalanceQty, stockprice),
                ProfitPercent = 0,
                HoldingProfit = 0,
                TradeType = "",
                SellExecutionTime = trade.ExecutionTime
            };
        }

        private static decimal? GetProfit(decimal price, int qty, StockPrice stockprice)
        {
            if (stockprice == null)
                return 0;

            return Math.Round((stockprice.Close - price) * qty, 2);
        }

        #endregion

    }
}
