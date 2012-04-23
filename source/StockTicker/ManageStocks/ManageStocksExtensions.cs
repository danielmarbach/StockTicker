//-------------------------------------------------------------------------------
// <copyright file="ManageStocksExtensions.cs" company="bbv Software Services AG">
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

    using StockTicker.Actions;
    using StockTicker.Externals;

    // NOTE: All extension methods here hide complexity of builder execution and provide more expressive and meaningful method names without generics.
    internal static class ManageStocksExtensions
    {
        public static IActionBuilder ConductContent(this IActionBuilder builder, IFutureValue<StockDetailModel> detailModel, Conductor<IStockTickerContentViewModel> conductor)
        {
            return builder.Execute<IConductStockTickerContent>(new { detailModel, conductor = (Action<IStockTickerContentViewModel>)conductor.ActivateItem });
        }

        public static IActionBuilder GetDetails(this IActionBuilder builder, StockSearchModel searchModel, IFutureValueSetter<StockDetailModel> detailModel)
        {
            return builder.Execute<IGetStockDetails>(new { symbol = searchModel.ToSymbol(), detailModel });
        }
    }
}