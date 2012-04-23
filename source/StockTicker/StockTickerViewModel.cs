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

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.FindStocks;

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

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.ActivateItem(new StaticContentViewModel());
        }
    }
}
