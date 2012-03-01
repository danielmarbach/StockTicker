//-------------------------------------------------------------------------------
// <copyright file="AppBootstrapper.cs" company="bbv Software Services AG">
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

namespace StockTicker
{
    using System;
    using System.Collections.Generic;

    using Caliburn.Micro;

    using Ninject;

    using StockTicker.Localization;

    public sealed class AppBootstrapper : Bootstrapper<IStockTickerViewModel>
    {
        private ILocalizer localizer;

        private StandardKernel kernel;

        protected override void BuildUp(object instance)
        {
            this.kernel.Inject(instance);
        }

        protected override void Configure()
        {
            this.kernel = new StandardKernel();
            this.kernel.Load(this.SelectAssemblies());
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            this.localizer = this.kernel.Get<ILocalizer>();
            this.localizer.Initialize();

            base.OnStartup(sender, e);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            this.kernel.Dispose();

            base.OnExit(sender, e);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.kernel.GetAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrEmpty(key) ? this.kernel.Get(service) : this.kernel.Get(service, key);
        }
    }
}