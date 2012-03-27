//-------------------------------------------------------------------------------
// <copyright file="GetStockDetails.cs" company="bbv Software Services AG">
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

namespace StockTicker.ManageStocks
{
    using System;

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.Externals;

    [Async]
    internal class GetStockDetails : IGetStockDetails
    {
        private readonly string symbol;

        private readonly IFutureValueSetter<StockDetailModel> detailModel;

        private readonly IStockService stockService;

        public GetStockDetails(string symbol, IFutureValueSetter<StockDetailModel> detailModel, IStockService stockService)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException("symbol");
            }

            this.symbol = symbol;
            this.detailModel = detailModel;
            this.stockService = stockService;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public void Execute(ActionExecutionContext context)
        {
            StockDetailModel stockDetail = this.stockService.Get(this.symbol);
            this.detailModel.SetValue(stockDetail);

            this.Completed(this, new ResultCompletionEventArgs());
        }
    }
}