//-------------------------------------------------------------------------------
// <copyright file="ChoosePasswordViewModel.cs" company="bbv Software Services AG">
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
    using System.Linq;
    using System.Security;

    using Caliburn.Micro;

    using FluentValidation;
    using FluentValidation.Results;

    using StockTicker.Actions;

    // NOTE: Implements IDataErrorInfo and uses fluent validation for validation of the password. 
    // The validator is created by using the validator factory. Implementing the data error info pattern is always the same and
    // considered a cross cutting concern. Ideal place to use tools like PostSharp.
    internal class ChoosePasswordViewModel : Screen, IChoosePasswordViewModel
    {
        private readonly IValidatorFactory validatorFactory;

        private SecureString password;
        private SecureString passwordRetype;

        public ChoosePasswordViewModel(IValidatorFactory validatorFactory)
        {
            this.validatorFactory = validatorFactory;
        }

        public Func<IActionBuilder> Actions { private get; set; }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public SecureString Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;
                this.NotifyOfPropertyChange(() => this.Password);
                this.NotifyOfPropertyChange(() => this.PasswordRetype);
            }
        }

        public SecureString PasswordRetype
        {
            get
            {
                return this.passwordRetype;
            }

            set
            {
                this.passwordRetype = value;
                this.NotifyOfPropertyChange(() => this.PasswordRetype);
                this.NotifyOfPropertyChange(() => this.Password);
            }
        }

        public string Error
        {
            get
            {
                var result = this.GetValidationResult();

                if (result.IsValid)
                {
                    return string.Empty;
                }

                var errors = result.Errors.Select(x => x.ErrorMessage);
                return string.Join(Environment.NewLine, errors);
            }
        }

        public string this[string columnName]
        {
            get
            {
                var result = this.GetValidationResult();

                if (result.IsValid)
                {
                    return string.Empty;
                }

                var errors = result.Errors.Where(x => x.PropertyName.Contains(columnName));
                return string.Join(Environment.NewLine, errors);
            }
        }

        // NOTE: Reacts on event aggregator events
        public void Handle(UserNameChosen message)
        {
            //// TODO: Use extension method to expressively map from UserNameChosen to this view model
        }

        // NOTE: We cannot close upon next if we have erros (validation failed).
        //// TODO: Do not allow closing when there are validation errors.

        private ValidationResult GetValidationResult()
        {
            //// TODO: Create the validator which is responsible for this view model
            //// TODO: Validate and return result
            return null;
        }
    }
}