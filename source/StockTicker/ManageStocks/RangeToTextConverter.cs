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

    using StockTicker.Externals;

    // NOTE: Special conversion should be done by using converters. Converters can be unit tested and simply attached in the XAML.
    [ValueConversion(typeof(Range), typeof(string))]
    public class RangeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var range = value as Range;

            if (range != null)
            {
                return string.Format(culture, "${0} - ${1}", range.From, range.To);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}