//-------------------------------------------------------------------------------
// <copyright file="StockDetailViewModel.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.Authentication;
    using StockTicker.Externals;

    // NOTE: Represents detail information about a stock. This has behavior and therefore can receive actions. 
    internal class StockDetailViewModel : Screen, IStockDetailViewModel
    {
        public StockDetailViewModel(StockDetailModel detailModel)
        {
            this.Model = detailModel;
        }

        public Func<IActionBuilder> Actions { private get; set; }

        // NOTE: Information about the details model are directly exposed as property and not wrapped. Violates law of demeter but is much simpler.
        public StockDetailModel Model { get; private set; }

        // NOTE: Bound to button add portfolio
        public IEnumerable<IResult> AddPortfolio()
        {
            return this.Actions().WithLogin(builder => { });
        }
    }
}