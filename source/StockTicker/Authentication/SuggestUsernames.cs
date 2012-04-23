//-------------------------------------------------------------------------------
// <copyright file="SuggestUsernames.cs" company="bbv Software Services AG">
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

    using Caliburn.Micro;

    using StockTicker.Actions;
    using StockTicker.Externals;

    // NOTE: Gets the suggested usernames with asynchronous advice from the external services.
    [Async]
    internal class SuggestUsernames : ISuggestUsernames
    {
        private readonly IAuthenticationService authenticationService;

        private readonly PotentialNewUserModel potentialNewUser;

        private readonly ICollection<string> suggestedUsernames;

        public SuggestUsernames(IAuthenticationService authenticationService, PotentialNewUserModel potentialNewUser, ICollection<string> suggestedUsernames)
        {
            this.authenticationService = authenticationService;
            this.potentialNewUser = potentialNewUser;
            this.suggestedUsernames = suggestedUsernames;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public void Execute(ActionExecutionContext context)
        {
            this.suggestedUsernames.Clear();

            IEnumerable<string> suggestions = this.authenticationService.SuggestUsernames(this.potentialNewUser);

            foreach (string suggestion in suggestions)
            {
                this.suggestedUsernames.Add(suggestion);
            }

            this.Completed(this, new ResultCompletionEventArgs());
        }
    }
}