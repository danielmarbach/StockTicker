//-------------------------------------------------------------------------------
// <copyright file="ChoosePasswordViewModelValidator.cs" company="bbv Software Services AG">
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
    using System.Runtime.InteropServices;
    using System.Security;

    using FluentValidation;

    // NOTE: Validation magic with fluent validator which uses translatable messages.
    internal class ChoosePasswordViewModelValidator : AbstractValidator<IChoosePasswordViewModel>
    {
        public ChoosePasswordViewModelValidator()
        {
            this.RuleFor(vm => vm.Password).NotNull();
            this.RuleFor(vm => vm.PasswordRetype).NotNull();

            this.RuleFor(vm => vm.Password)
                .Must(BeEqualToPasswordRetype)
                .When(vm => vm.Password != null && vm.PasswordRetype != null)
                .WithMessage(Authentication.PasswordRetypeDoesNotMatch);
        }

        private static bool BeEqualToPasswordRetype(IChoosePasswordViewModel choosePasswordViewModel, SecureString password)
        {
            return SecureStringToString(password).Equals(SecureStringToString(choosePasswordViewModel.PasswordRetype));
        }

        private static string SecureStringToString(SecureString input)
        {
            IntPtr ptr = SecureStringToBSTR(input);
            string output = PtrToStringBSTR(ptr);
            return output;
        }

        private static IntPtr SecureStringToBSTR(SecureString ss)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(ss);
            return ptr;
        }

        private static string PtrToStringBSTR(IntPtr ptr)
        {
            string s = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);
            return s;
        }
    }
}