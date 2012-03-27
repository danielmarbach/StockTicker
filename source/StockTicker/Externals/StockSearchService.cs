//-------------------------------------------------------------------------------
// <copyright file="StockSearchService.cs" company="bbv Software Services AG">
//   Copyright (c) 2012
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace StockTicker.Externals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal class StockSearchService : IStockSearchService
    {
        private readonly List<StockSearchModel> stocks;

        public StockSearchService()
        {
            this.stocks = CreateFakeStocks();
        }

        public IEnumerable<StockSearchModel> Find(string searchString)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            searchString = searchString.ToUpperInvariant();

            return this.stocks.Where(s => s.Company.Contains(searchString) || s.Fund.Contains(searchString) || s.Symbol.Contains(searchString));
        }

        private static List<StockSearchModel> CreateFakeStocks()
        {
            return new List<StockSearchModel>
                {
                    new StockSearchModel(StockSymbols.Ubs, "UBS AG (USA)", StockMarkets.Nyse),
                    new StockSearchModel(StockSymbols.Mlpl, "UBS E-TRACS 2X LEVERAGED LONG ALERIAN MLP INFRASTRUCTURE ETN", StockMarkets.Amex),
                };
        }
    }
}