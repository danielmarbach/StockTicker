//-------------------------------------------------------------------------------
// <copyright file="ChooseUserNameViewModel.cs" company="bbv Software Services AG">
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

namespace StockTicker.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.Externals;

    internal class ChooseUserNameViewModel : Screen, IChooseUserNameViewModel
    {
        private string userName;

        private string firstName;

        private string lastName;

        public ChooseUserNameViewModel()
        {
            this.Suggestions = new BindableCollection<string>();
            this.Suggestions.CollectionChanged += this.HandleSuggestionsChanged;
        }

        public Func<IActionBuilder> Actions { private get; set; }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                this.firstName = value;
                this.NotifyOfPropertyChange(() => this.FirstName);
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                this.lastName = value;
                this.NotifyOfPropertyChange(() => this.LastName);
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                this.userName = value;
                this.NotifyOfPropertyChange(() => this.UserName);
            }
        }

        public bool HasSuggestions
        {
            get
            {
                return this.Suggestions.Any();
            }
        }

        public ObservableCollection<string> Suggestions
        {
            get; private set;
        }

        public IEnumerable<IResult> SuggestUsernames(string firstName, string lastName)
        {
            return
                this.Actions().WithBusyIndication(
                    busyScope => busyScope.SuggestUsernames(new PotentialNewUserModel(firstName, lastName), this.Suggestions),
                    Authentication.Suggestion);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            UserNameChosen message = this.ToUserNameChosen();

            Coroutine.BeginExecute(this.Actions().Notify(message).GetEnumerator());
        }

        private void HandleSuggestionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.NotifyOfPropertyChange(() => this.HasSuggestions);
        }
    }
}