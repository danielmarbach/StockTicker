//-------------------------------------------------------------------------------
// <copyright file="Secure.cs" company="bbv Software Services AG">
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
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;

    // NOTE: Shows how to use dependency properties.
    internal static class Secure
    {
        //// TODO: Define attached property password which maps SecureString to HandleBoundPasswordChanged

        public static SecureString GetPassword(DependencyObject dp)
        {
            //// TODO: Implement get for attached property
            return null;
        }

        public static void SetPassword(DependencyObject dp, SecureString value)
        {
            //// TODO: Implement set for attached property
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;

            SetPassword(box, box.SecurePassword);
        }

        private static void HandleBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var box = dp as PasswordBox;

            if (box == null)
            {
                return;
            }

            box.PasswordChanged -= HandlePasswordChanged;
            box.PasswordChanged += HandlePasswordChanged;
        }
    }
}