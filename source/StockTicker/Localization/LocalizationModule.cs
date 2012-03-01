//-------------------------------------------------------------------------------
// <copyright file="LocalizationModule.cs" company="bbv Software Services AG">
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
    using Ninject.Modules;

    using StockTicker.Properties;

    public class LocalizationModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICultureSetter>().To<LocalizeDictionaryDecorator>()
                .WhenInjectedInto<LocalizationCultureProvider>().InSingletonScope();
            this.Bind<ICultureSetter, ILocalizationCultureProvider>().To<LocalizationCultureProvider>().InSingletonScope();
            this.Bind<ILocalizer>().To<Localizer>().InSingletonScope();

            this.Bind<ISettings>().ToConstant(Settings.Default);
        }
    }
}