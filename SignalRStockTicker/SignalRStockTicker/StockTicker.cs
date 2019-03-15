using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SignalRStockTicker
{
    public class StockTicker
    {
        //Singleton instance (Lazy initialization for thread-safe)
        private readonly static Lazy<StockTicker> _instance = new Lazy<StockTicker>(() => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));

        //The stocks collection is defined as a ConcurrentDictionary type for thread safety
        private readonly ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        private readonly object _updateStockPricesLock = new object();

        //stock can go up or down by a percentage of this factor on each change
        private readonly double _rangePercent = .002;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Random _updateOrNotRandom = new Random();

        private readonly Timer _timer;
        private volatile bool _updatingStockPrices = false;
        private StockTicker(IHubConnectionContext<dynamic> clients)
        {
            this.Clients = clients;
            this._stocks.Clear();

            var stocks = new List<Stock>
            {
                new Stock { Symbol = "MSFT", Price = 30.31m },
                new Stock { Symbol = "APPL", Price = 578.18m },
                new Stock { Symbol = "GOOG", Price = 570.30m }
            };
            stocks.ForEach(stock => _stocks.TryAdd(stock.Symbol, stock));

            //
            this._timer = new Timer(UpdateStockPrices,null,_updateInterval,_updateInterval);
        }

        public static StockTicker Instance
        {
            get { return _instance.Value; }
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        public IEnumerable<Stock> GetAllStocks()
        {
            return this._stocks.Values;
        }

        public void UpdateStockPrices(object state)
        {
            //take a lock on the _updateStockPricesLock object.
            lock (_updateStockPricesLock)
            {
                //checks if another thread is already updating prices
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;
                    foreach (var stock in _stocks.Values)
                    {
                        if (TryUpdateStockPrice(stock))
                        {
                            //if the stock price changes, the app calls BroadcastStockPrice to broadcast the stock price change to all connected clients
                            BroadcastStockPrice(stock);
                        }
                    }
                    _updatingStockPrices = false;
                }
            }
        }

        /// <summary>
        /// decided whether to change the stock price, and how much to change it
        /// In a real applicatoin, the TryUpdateStockPrice method would call a web service to look up the price
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        private bool TryUpdateStockPrice(Stock stock)
        {
            //Randomly choose whether to update this stock or not
            var r = _updateOrNotRandom.NextDouble();
            if (r > .1)
            {
                return false;
            }

            //Update the stock price by a random factor of the range percent
            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() > .51;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -change;

            stock.Price += change;
            return true;
        }

        private void BroadcastStockPrice(Stock stock)
        {
            //should add the updateStockPrice method in the client
            //You can refer to updateStockPrice method here, because Clients.All is dynamic, which means the app will evaluate the expression at runtime.
            //When this method call executes, SignalR will send the method name and parameter value to the client,
            //and if the client has a method named updateStockPrice, the app will call that method and pass the parameter value to it.
            Clients.All.updateStockPrice(stock);
        }
    }
}