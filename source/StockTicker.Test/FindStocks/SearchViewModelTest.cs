//-------------------------------------------------------------------------------
// <copyright file="SearchViewModelTest.cs" company="bbv Software Services AG">
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
    using System.Windows.Input;

    using FluentAssertions;
    using FluentAssertions.EventMonitoring;

    using StockTicker.Externals;
    using StockTicker.TestHelpers;

    using Xunit;

    public class SearchViewModelTest
    {
        private readonly ActionBuilderMock actionBuilder;

        private readonly SearchViewModel testee;

        public SearchViewModelTest()
        {
            this.actionBuilder = new ActionBuilderMock();

            this.testee = new SearchViewModel()
                .AttachBuilder(this.actionBuilder);
        }

        [Fact]
        public void ShouldRaisePropertyChangedForHasStocks()
        {
            this.testee.MonitorEvents();

            this.testee.FoundStocks.Add(new StockSearchModel("AnySymbol", "AnyCompany", "AnyFund"));

            this.testee.ShouldRaisePropertyChangeFor(x => x.HasStocks);
            this.testee.HasStocks.Should().BeTrue();
        }

        [Fact]
        public void ShouldSearch_WhenEnter()
        {
            this.testee.Search("AnySymbol", new KeyEventArgs(null, new FakePresentationSource(), 21, Key.Enter));

            this.actionBuilder
                .Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldNotSearch_WhenNotEnter()
        {
            this.testee.Search("AnySymbol", new KeyEventArgs(null, new FakePresentationSource(), 21, Key.A));

            this.actionBuilder
                .Should().BeEmpty();
        }
    }
}