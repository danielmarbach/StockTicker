//-------------------------------------------------------------------------------
// <copyright file="StockTickerViewModel.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.Externals;
    using StockTicker.FindStocks;
    using StockTicker.ManageStocks;

    internal sealed class StockTickerViewModel : Conductor<IStockTickerContentViewModel>, IStockTickerViewModel
    {
        public StockTickerViewModel(ISearchViewModel searchViewModel, IBusyIndicationViewModel busyIndication)
        {
            this.Search = searchViewModel;
            this.BusyIndication = busyIndication;

            this.DisplayName = General.StockTickerTitle;
        }

        // NOTE: Comes from the IUseAction interface
        public Func<IActionBuilder> Actions { private get; set; }

        // NOTE: Here for bindings to the busy indicator.
        public IBusyIndicationViewModel BusyIndication { get; private set; }

        public ISearchViewModel Search { get; private set; }

        // NOTE: Belongs to ManageStocks
        public IEnumerable<IResult> Display(StockSearchModel searched)
        {
            // NOTE: Future is a type which holds a reference to a value which will be set later.
            // this is necessary because we use constructor injection. The GetDetails method below only fetches
            // the details model upon execution of the underlying IResult. This then sets the value of the future
            // ConductContent can then retrieve the set value upon execution of the underlying IResult.
            // If you'd pass only a reference to a StockDetailModel this wouldn't work.
            var detailModel = new Future<StockDetailModel>();

            return this.Actions()
                .WithBusyIndication(
                    busyScope => busyScope
                        .GetDetails(searched, detailModel)
                        .ConductContent(detailModel, this),
                    General.DisplayStockDetails);
        }

        // NOTE: Default initialization triggers conduction of a details content for a NULL detail (therefore Future<StockDetailModel> is null)
        // the content factory then needs to return default content which is then conducted by this view model.
        //// TODO: Upon initialization fetch default content with Coroutine
    }
}
