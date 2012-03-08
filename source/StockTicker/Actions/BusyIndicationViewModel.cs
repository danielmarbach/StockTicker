//-------------------------------------------------------------------------------
// <copyright file="BusyIndicationViewModel.cs" company="bbv Software Services AG">
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

namespace StockTicker.Actions
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Caliburn.Micro;

    internal class BusyIndicationViewModel : PropertyChangedBase, IBusyIndicationViewModel
    {
        private readonly ObservableCollection<RequestModel> requests;

        public BusyIndicationViewModel()
        {
            this.requests = new ObservableCollection<RequestModel>();
        }

        public bool IsBusy
        {
            get { return this.Requests.Any(); }
        }

        public ObservableCollection<RequestModel> Requests
        {
            get { return this.requests; }
        }

        public void Start(Guid requestId, string message)
        {
            this.Requests.Add(new RequestModel(requestId, message));
            this.NotifyOfPropertyChange(() => this.IsBusy);
        }

        public void Finished(Guid requestId)
        {
            this.Requests.Remove(new RequestModel(requestId));
            this.NotifyOfPropertyChange(() => this.IsBusy);
        }
    }
}