//-------------------------------------------------------------------------------
// <copyright file="SearchStocks.cs" company="bbv Software Services AG">
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

namespace StockTicker.FindStocks
{
    using System;
    using System.Collections.Generic;

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.Externals;

    // NOTE: IResult with asynchronous advice which loads search information from external service. The result itself fills the found stocks into a collection.
    // The client view model of this search result uses a bindable collection
    [Async]
    internal class SearchStocks : ISearchStocks
    {
        private readonly IStockSearchService searchService;

        private readonly string searchPattern;

        private readonly ICollection<StockSearchModel> foundStocks;

        public SearchStocks(IStockSearchService searchService, string searchPattern, ICollection<StockSearchModel> foundStocks)
        {
            this.searchPattern = searchPattern;
            this.foundStocks = foundStocks;
            this.searchService = searchService;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public void Execute(ActionExecutionContext context)
        {
            //// TODO: Implement searching on external service here
        }
    }
}