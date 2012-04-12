//-------------------------------------------------------------------------------
// <copyright file="Localizer.cs" company="bbv Software Services AG">
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

namespace StockTicker.Localization
{
    using System.Globalization;

    using StockTicker.Properties;

    // NOTE: Sets the culture of the application. When the underlying threads culture is set the localization extension automatically retrieves the correct 
    // localized resources.
    internal class Localizer : ILocalizer
    {
        private readonly ISettings settings;

        private readonly ICultureSetter cultureSetter;

        public Localizer(ISettings settings, ICultureSetter cultureSetter)
        {
            this.cultureSetter = cultureSetter;
            this.settings = settings;
        }

        public void Initialize()
        {
            this.cultureSetter.SetCulture(this.settings.Language);
        }

        public void Save(CultureInfo culture)
        {
            this.settings.Language = culture;
        }
    }
}