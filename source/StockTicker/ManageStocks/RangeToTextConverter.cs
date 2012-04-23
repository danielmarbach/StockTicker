//-------------------------------------------------------------------------------
// <copyright file="RangeToTextConverter.cs" company="bbv Software Services AG">
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

namespace StockTicker.ManageStocks
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    // NOTE: Special conversion should be done by using converters. Converters can be unit tested and simply attached in the XAML.
    //// TODO: Hint WPF runtime with right value conversion attribute.
    public class RangeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //// TODO: Implement so that range is displayed with dash and currency symbol dollar.

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}