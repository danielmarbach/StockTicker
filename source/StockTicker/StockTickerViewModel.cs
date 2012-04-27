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

    using StockTicker.Actions;

    //// TODO: Should be able to host an IStockTickerContentViewModel
    //// TODO: The title of the window should contain localized string out of resource "General". Use the title "Stock Ticker"
    //// TODO: Should pre initialize content with static content view model
    //// TODO: Should provide binding points for IBusyIndicationViewModel and ISearchViewModel

    internal sealed class StockTickerViewModel : IStockTickerViewModel
    {
        // NOTE: Comes from the IUseAction interface
        public Func<IActionBuilder> Actions { private get; set; }
    }
}
