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

    // NOTE: This IResult cannot be asynchronous because it conducts view models. View models must be created and conducted on the main dispatcher.
    internal class ConductStockTickerContent : IConductStockTickerContent
    {
        private readonly IFutureValue<StockDetailModel> detailModel;

        private readonly Action<IStockTickerContentViewModel> conductor;

        private readonly IContentViewModelFactory contentFactory;

        // NOTE: Lazy initialized detail model is provided here.
        public ConductStockTickerContent(IFutureValue<StockDetailModel> detailModel, Action<IStockTickerContentViewModel> conductor, IContentViewModelFactory contentFactory)
        {
            this.detailModel = detailModel;
            this.conductor = conductor;
            this.contentFactory = contentFactory;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public void Execute(ActionExecutionContext context)
        {
            // NOTE: We access the future value here
            IStockTickerContentViewModel contentViewModel = this.contentFactory.CreateContent(this.detailModel.Value);
            this.conductor(contentViewModel);

            this.Completed(this, new ResultCompletionEventArgs());
        }
    }
}