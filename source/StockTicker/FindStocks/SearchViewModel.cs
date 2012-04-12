//-------------------------------------------------------------------------------
// <copyright file="SearchViewModel.cs" company="bbv Software Services AG">
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

namespace StockTicker.FindStocks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.Externals;

    internal class SearchViewModel : Screen, ISearchViewModel
    {
        private string pattern;

        public SearchViewModel()
        {
            // NOTE: Bindable collection is a collection which can be modified from different thread but fires collection changed on UI thread
            // This marshaling is necessary because search models are retrieved asynchronously.
            this.FoundStocks = new BindableCollection<StockSearchModel>();
            this.FoundStocks.CollectionChanged += this.HandleFoundStocksChanged;
        }

        public Func<IActionBuilder> Actions { private get; set; }

        public ObservableCollection<StockSearchModel> FoundStocks { get; private set; }

        public bool HasStocks
        {
            get
            {
                return this.FoundStocks.Any();
            }
        }

        public string Pattern
        {
            get
            {
                return this.pattern;
            }

            set
            {
                this.pattern = value;
                this.NotifyOfPropertyChange(() => this.Pattern);
            }
        }

        // NOTE: Search method which appropriate busy indication around it
        public IEnumerable<IResult> Search(string searchPattern)
        {
            string busyMessage = string.Format(CultureInfo.InvariantCulture, FindStocks.Searching, searchPattern);

            return this.Actions()
                .WithBusyIndication(
                    busy => busy.Search(searchPattern, this.FoundStocks), busyMessage);
        }

        // NOTE: Is called when navigated away to clean the search results.
        public void Clear()
        {
            this.Pattern = null;
            this.FoundStocks.Clear();
        }

        // NOTE: When the found stocks are changed HasStocks might also change. We need to indicate this
        private void HandleFoundStocksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.NotifyOfPropertyChange(() => this.HasStocks);
        }
    }
}