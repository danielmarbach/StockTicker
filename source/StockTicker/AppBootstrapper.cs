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
    using System.Reflection;

    using Caliburn.Micro;

    using Ninject;

    using StockTicker.Localization;

    public class AppBootstrapper : Bootstrapper
    {
        private ILocalizer localizer;

        public AppBootstrapper()
            : this(true)
        {
        }

        public AppBootstrapper(bool useApplication)
            : base(useApplication)
        {
        }

        protected StandardKernel Kernel { get; private set; }

        protected override void BuildUp(object instance)
        {
            this.Kernel.Inject(instance);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            yield return Assembly.GetAssembly(typeof(AppBootstrapper));
        }

        protected override void Configure()
        {
            this.Kernel = new StandardKernel();
            this.Kernel.Load(this.SelectAssemblies());
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            this.localizer = this.Kernel.Get<ILocalizer>();
            this.localizer.Initialize();

            this.DisplayRootViewFor(typeof(IStockTickerViewModel));
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            this.Kernel.Dispose();

            base.OnExit(sender, e);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.Kernel.GetAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrEmpty(key) ? this.Kernel.Get(service) : this.Kernel.Get(service, key);
        }
    }
}