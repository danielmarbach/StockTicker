//-------------------------------------------------------------------------------
// <copyright file="GetStockDetailsTest.cs" company="bbv Software Services AG">
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

    using FluentAssertions;

    using Moq;

    using StockTicker.Actions;
    using StockTicker.Externals;
    using StockTicker.TestHelpers;

    using Xunit;

    using Action = System.Action;

    public class GetStockDetailsTest
    {
        private readonly Mock<IStockService> stockService;

        private readonly string symbol;

        private readonly FutureMock<StockDetailModel> detail;

        public GetStockDetailsTest()
        {
            this.symbol = "AnySymbol";
            this.detail = new FutureMock<StockDetailModel>();

            this.stockService = new Mock<IStockService>();
        }

        [Fact]
        public void ShouldNotAllowNullSearchModel()
        {
            Action act = () => this.CreateTestee(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            var testee = this.CreateTestee();

            testee.ShouldRaiseCompleted();
        }

        [Fact]
        public void ShouldRequestAsynchronousExecution()
        {
            var testee = this.CreateTestee();

            testee.Should().BeDecoratedWith<AsyncAttribute>();
        }

        [Fact]
        public void ShouldReturnDetails()
        {
            var expectedDetail = new AnyStockDetailModel();
            this.stockService.Setup(s => s.Get(this.symbol)).Returns(expectedDetail);

            var testee = this.CreateTestee();

            testee.Execute(new ActionExecutionContext());

            this.detail.Value.Should().Be(expectedDetail);
        }

        private GetStockDetails CreateTestee()
        {
            return this.CreateTestee(this.symbol);
        }

        private GetStockDetails CreateTestee(string symbol)
        {
            return new GetStockDetails(symbol, this.detail, this.stockService.Object);
        }
    }
}