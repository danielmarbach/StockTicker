//-------------------------------------------------------------------------------
// <copyright file="AuthenticationService.cs" company="bbv Software Services AG">
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security;

    internal class AuthenticationService : IAuthenticationService
    {
        private static readonly Random Randomizer = new Random();

        private readonly List<UserModel> users;

        public AuthenticationService()
        {
            this.users = new List<UserModel>();
        }

        public IEnumerable<string> SuggestUsernames(PotentialNewUserModel user)
        {
            yield return string.Format(CultureInfo.InvariantCulture, "{0}{1}", user.FirstName.ToLowerInvariant(), user.LastName.ToLowerInvariant());
            yield return string.Format(CultureInfo.InvariantCulture, "{0}{1}", user.LastName.ToLowerInvariant(), user.FirstName.ToLowerInvariant());
            yield return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", user.FirstName.ToLowerInvariant(), user.LastName.ToLowerInvariant());
            yield return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", user.LastName.ToLowerInvariant(), user.FirstName.ToLowerInvariant());
            yield return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", user.FirstName.ToLowerInvariant(), user.LastName.ToLowerInvariant(), Randomizer.Next(1000));
            yield return string.Format(CultureInfo.InvariantCulture, "{0}.{1}{2}", user.FirstName.ToLowerInvariant(), user.LastName.ToLowerInvariant(), Randomizer.Next(1000));
            yield return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", user.LastName.ToLowerInvariant(), user.FirstName.ToLowerInvariant(), Randomizer.Next(1000));
            yield return string.Format(CultureInfo.InvariantCulture, "{0}.{1}{2}", user.LastName.ToLowerInvariant(), user.FirstName.ToLowerInvariant(), Randomizer.Next(1000));
        }

        public void CreateUser(NewUserModel newUser)
        {
            // Normally we would revalidate here
            this.users.Add(newUser.ToUser());
        }

        public void LogOn(UserModel user)
        {
            if (!this.users.Contains(user))
            {
                throw new SecurityException();
            }
        }

        public void LogOff(UserModel user)
        {
        }
    }
}