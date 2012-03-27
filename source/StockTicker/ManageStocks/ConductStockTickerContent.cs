//-------------------------------------------------------------------------------
// <copyright file="ConductStockTickerContent.cs" company="bbv Software Services AG">
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

    using StockTicker.Externals;

    internal class ConductStockTickerContent : IConductStockTickerContent
    {
        private readonly StockDetailModel detailModel;

        private readonly Action<IStockTickerContentViewModel> conductor;

        private readonly IContentViewModelFactory contentFactory;

        public ConductStockTickerContent(StockDetailModel detailModel, Action<IStockTickerContentViewModel> conductor, IContentViewModelFactory contentFactory)
        {
            this.detailModel = detailModel;
            this.conductor = conductor;
            this.contentFactory = contentFactory;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public void Execute(ActionExecutionContext context)
        {
            IStockTickerContentViewModel contentViewModel = this.contentFactory.CreateContent(this.detailModel);
            this.conductor(contentViewModel);

            this.Completed(this, new ResultCompletionEventArgs());
        }
    }
}