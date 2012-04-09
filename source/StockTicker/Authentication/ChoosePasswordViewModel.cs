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
            get; set;
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

        public void Handle(UserNameChosen message)
        {
            this.FromChosenUserName(message);
        }

        public override void CanClose(Action<bool> callback)
        {
            callback(string.IsNullOrEmpty(this.Error));
        }

        private ValidationResult GetValidationResult()
        {
            var validator = this.validatorFactory.GetValidator<IChoosePasswordViewModel>();
            ValidationResult result = validator.Validate(this);
            return result;
        }
    }
}