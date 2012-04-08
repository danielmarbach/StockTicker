//-------------------------------------------------------------------------------
// <copyright file="BindingExtensions.cs" company="bbv Software Services AG">
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

namespace StockTicker.Extensions
{
    using Caliburn.Micro;

    using Ninject;
    using Ninject.Syntax;

    public static class BindingExtensions
    {
        public static IBindingOnSyntax<T> RegisterOnEventAggregator<T>(this IBindingOnSyntax<T> syntax)
        {
            return
                syntax.OnActivation((ctx, instance) => ctx.Kernel.Get<IEventAggregator>().Subscribe(instance))
                      .OnDeactivation((ctx, instance) => ctx.Kernel.Get<IEventAggregator>().Unsubscribe(instance));
        }
    }
}