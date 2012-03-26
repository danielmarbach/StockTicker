//-------------------------------------------------------------------------------
// <copyright file="ContentViewModelFactoryTest.cs" company="bbv Software Services AG">
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

namespace StockTicker
{
    using System;

    using FluentAssertions;

    using Moq;

    using Ninject;

    using StockTicker.Externals;
    using StockTicker.ManageStocks;
    using StockTicker.News;

    using Xunit;

    public class ContentViewModelFactoryTest : IDisposable
    {
        private readonly StandardKernel kernel;

        private readonly ContentViewModelFactory testee;

        public ContentViewModelFactoryTest()
        {
            this.kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
            this.kernel.Bind<INewsViewModel>().ToConstant(Mock.Of<INewsViewModel>());
            this.kernel.Bind<IStockDetailViewModel>().ToConstant(Mock.Of<IStockDetailViewModel>());

            this.testee = new ContentViewModelFactory(this.kernel);
        }

        [Fact]
        public void ShouldReturnNewsViewModelAsDefaultContent()
        {
            IStockTickerContentViewModel result = this.testee.CreateContent(null);

            result.Should().NotBeNull()
                .And.BeAssignableTo<INewsViewModel>();
        }

        [Fact]
        public void ShouldReturnDetailsViewModelForSearchedContent()
        {
            IStockTickerContentViewModel result =
                this.testee.CreateContent(new StockSearchModel("AnySymbol", "AnyCompany", "AnyFund"));

            result.Should().NotBeNull()
                .And.BeAssignableTo<IStockDetailViewModel>();
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }
    }
}