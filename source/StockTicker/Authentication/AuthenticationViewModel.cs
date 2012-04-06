//-------------------------------------------------------------------------------
// <copyright file="AuthenticationViewModel.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using Caliburn.Micro;

    internal class AuthenticationViewModel : Conductor<IAuthenticationStep>.Collection.OneActive, IAuthenticationViewModel
    {
        private readonly IAuthenticationStepFactory authenticationStepFactory;

        private IEnumerator<IAuthenticationStep> enumerator;

        public AuthenticationViewModel(IAuthenticationStepFactory authenticationStepFactory)
        {
            this.authenticationStepFactory = authenticationStepFactory;
        }

        public void Next()
        {
            this.DeactivateItem(this.enumerator.Current, true);

            if (this.enumerator.MoveNext())
            {
                this.ActivateItem(this.enumerator.Current);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.Items.AddRange(this.authenticationStepFactory.CreateSteps());

            this.enumerator = this.Items.GetEnumerator();
            this.enumerator.MoveNext();

            this.ActivateItem(this.enumerator.Current);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            this.enumerator.Dispose();
        }
    }
}