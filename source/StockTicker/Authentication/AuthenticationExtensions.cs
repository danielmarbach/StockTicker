//-------------------------------------------------------------------------------
// <copyright file="AuthenticationExtensions.cs" company="bbv Software Services AG">
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

    using StockTicker.Actions;
    using StockTicker.Externals;

    // NOTE: Provides not only smarter syntax for actions but also some convenience mapping methods which could also be achieved with tools like AutoMapper.
    internal static class AuthenticationExtensions
    {
        public static IActionBuilder WithLogin(this IActionBuilder builder, Action<IActionBuilder> configure)
        {
            return builder.ScopeWith<IAuthenticate, IMissing>(configure);
        }

        public static IActionBuilder SuggestUsernames(this IActionBuilder builder, PotentialNewUserModel potentialNewUser, ICollection<string> suggestedUsernames)
        {
            return builder.Execute<ISuggestUsernames>(new { potentialNewUser, suggestedUsernames });
        }

        public static UserNameChosen ToUserNameChosen(this IChooseUserNameViewModel chooseUserNameViewModel)
        {
            return new UserNameChosen(chooseUserNameViewModel.FirstName, chooseUserNameViewModel.LastName, chooseUserNameViewModel.UserName);
        }

        public static void FromChosenUserName(this IChoosePasswordViewModel choosePasswordViewModel, UserNameChosen message)
        {
            choosePasswordViewModel.FirstName = message.FirstName;
            choosePasswordViewModel.LastName = message.LastName;
            choosePasswordViewModel.UserName = message.UserName;
        }
    }
}