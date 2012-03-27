//-------------------------------------------------------------------------------
// <copyright file="StockDetailModel.cs" company="bbv Software Services AG">
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
    internal class StockDetailModel
    {
        public StockDetailModel(string symbol, string company, string fund, string description, string sector, string industry, decimal todaysOpen, decimal previousClose, Range dailyRange, Range fiftyTwoWeekRange, long volume, long averageDailyVolume)
        {
            this.Symbol = symbol;
            this.Company = company;
            this.Fund = fund;
            this.Description = description;
            this.Sector = sector;
            this.Industry = industry;
            this.TodaysOpen = todaysOpen;
            this.PreviousClose = previousClose;
            this.DailyRange = dailyRange;
            this.FiftyTwoWeekRange = fiftyTwoWeekRange;
            this.Volume = volume;
            this.AverageDailyVolume = averageDailyVolume;
        }

        public string Symbol { get; private set; }

        public string Company { get; private set; }

        public string Fund { get; private set; }

        public string Description { get; private set; }

        public string Sector { get; private set; }

        public string Industry { get; private set; }

        public decimal TodaysOpen { get; private set; }

        public decimal PreviousClose { get; private set; }

        public Range DailyRange { get; private set; }

        public Range FiftyTwoWeekRange { get; private set; }

        public long Volume { get; private set; }

        public long AverageDailyVolume { get; private set; }
    }
}