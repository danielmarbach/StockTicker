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

    // NOTE: Async annotation advices the action builder pipeline to wrap the IResult implementation with an asynchronous decorator. 
    // This is especially in acceptance tests very flexible approach because the asynchronous execution can be removed during acceptance tests.
    [Async]
    internal class GetStockDetails : IGetStockDetails
    {
        private readonly string symbol;

        private readonly IFutureValueSetter<StockDetailModel> detailModel;

        private readonly IStockService stockService;

        public GetStockDetails(IStockService stockService, string symbol, IFutureValueSetter<StockDetailModel> detailModel)
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
            //// TODO: Get the stock details from the external service and set the future. Don't forget to complete the result
        }
    }
}