//-------------------------------------------------------------------------------
// <copyright file="ConductStockTickerContentTest.cs" company="bbv Software Services AG">
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
    using Caliburn.Micro;

    using FluentAssertions;

    using Moq;

    using StockTicker.Externals;
    using StockTicker.TestHelpers;

    using Xunit;

    public class ConductStockTickerContentTest
    {
        private readonly FutureMock<StockDetailModel> searchModel;

        private readonly Mock<IContentViewModelFactory> contentFactory;

        private IStockTickerContentViewModel detail;

        public ConductStockTickerContentTest()
        {
            this.searchModel = new AnyStockDetailModel().AsFuture<StockDetailModel>();
            this.contentFactory = new Mock<IContentViewModelFactory>();
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            ConductStockTickerContent testee = this.CreateTestee();

            testee.ShouldRaiseCompleted();
        }

        [Fact]
        public void ShouldAllowNullSearchModel()
        {
            var future = new FutureMock<StockDetailModel>();

            ConductStockTickerContent testee = this.CreateTestee(future);

            System.Action act = () => testee.Execute(new ActionExecutionContext());

            act.ShouldNotThrow();
        }

        [Fact]
        public void ShouldConductCreatedDetailViewModel()
        {
            var contentViewModel = Mock.Of<IStockTickerContentViewModel>();
            this.contentFactory.Setup(f => f.CreateContent(It.IsAny<StockDetailModel>())).Returns(contentViewModel);

            var testee = this.CreateTestee();

            testee.Execute(new ActionExecutionContext());

            this.detail.Should().Be(contentViewModel);
        }

        private ConductStockTickerContent CreateTestee(FutureMock<StockDetailModel> search)
        {
            return new ConductStockTickerContent(search, d => this.detail = d, this.contentFactory.Object);
        }

        private ConductStockTickerContent CreateTestee()
        {
            return this.CreateTestee(this.searchModel);
        }
    }
}