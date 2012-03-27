//-------------------------------------------------------------------------------
// <copyright file="AnyStockDetailModel.cs" company="bbv Software Services AG">
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

namespace StockTicker.Externals
{
    internal class AnyStockDetailModel : StockDetailModel
    {
        public AnyStockDetailModel()
            : base("AnySymbol", "AnyCompany", "AnyFund", "AnyDescription", "AnySector", "AnyIndustry", 10.01m, 9.05m, new Range(5.15m, 12.0m), new Range(3.66m, 20.12m), 10233, 11333)
        {
        }
    }
}