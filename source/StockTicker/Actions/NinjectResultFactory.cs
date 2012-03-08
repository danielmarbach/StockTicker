//-------------------------------------------------------------------------------
// <copyright file="NinjectResultFactory.cs" company="bbv Software Services AG">
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

namespace StockTicker.Actions
{
    using Ninject;
    using Ninject.Syntax;

    using StockTicker.Extensions;

    internal class NinjectResultFactory : IResultFactory
    {
        private readonly IResolutionRoot resolutionRoot;

        public NinjectResultFactory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public TResult Create<TResult>(object prototype)
        {
            return prototype == null
                       ? this.resolutionRoot.Get<TResult>()
                       : this.resolutionRoot.Get<TResult>(prototype);
        }
    }
}