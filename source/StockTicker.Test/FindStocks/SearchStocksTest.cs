//-------------------------------------------------------------------------------
// <copyright file="SearchStocksTest.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Caliburn.Micro;
    using FluentAssertions;
    using Moq;
    using StockTicker.Externals;
    using StockTicker.TestHelpers;
    using Xunit;

    public class SearchStocksTest
    {
        private const string SearchPattern = "AnySearchPattern";

        private readonly ICollection<StockSearchModel> foundStocks;

        private readonly Mock<IStockSearchService> searchService;

        private readonly SearchStocks testee;

        public SearchStocksTest()
        {
            this.searchService = new Mock<IStockSearchService>();
            this.foundStocks = new Collection<StockSearchModel>();

            this.testee = new SearchStocks(this.searchService.Object, SearchPattern, this.foundStocks);
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            this.testee.ShouldRaiseCompleted();
        }

        [Fact]
        public void ShouldFillFoundStocks()
        {
            var expectedSearchModels = new List<StockSearchModel> { new StockSearchModel("First", "First", "First"), new StockSearchModel("Second", "Second", "Second"), };

            this.searchService.Setup(s => s.Find(SearchPattern)).Returns(
                expectedSearchModels);

            this.testee.Execute(new ActionExecutionContext());

            this.foundStocks.Should().BeEquivalentTo(expectedSearchModels);
        }
    }
}