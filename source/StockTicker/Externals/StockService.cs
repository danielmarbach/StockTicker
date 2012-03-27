//-------------------------------------------------------------------------------
// <copyright file="StockService.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;

    internal class StockService : IStockService
    {
        private readonly Dictionary<string, StockDetailModel> details;

        public StockService()
        {
            this.details = new Dictionary<string, StockDetailModel>();

            this.CreateDetails();
        }

        public StockDetailModel Get(string symbol)
        {
            StockDetailModel detailModel;
            this.details.TryGetValue(symbol, out detailModel);
            return detailModel;
        }

        private void CreateDetails()
        {
            this.details.Add(StockSymbols.Ubs, new StockDetailModel("Global financial firm serving a discerning global client base. The Company provides private banking services and is a global asset manager.", "Financials", "Capital Markets", 14.32m, 14.24m, new Range<decimal>(14.26m, 14.49m), new Range<decimal>(10.41m, 20.04m), 3615813, 3879664));
            this.details.Add(StockSymbols.Mlpl, new StockDetailModel("UBS E-TRACS 2x Leveraged Long Alerian MLP Infrastructure ETN", "Information Technology", "Electronic Equipment, Instruments and Components", 42.77m, 42.35m, new Range<decimal>(42.25m, 42.77m), new Range<decimal>(25.30m, 45.93m), 29370, 30549));
        }
    }
}