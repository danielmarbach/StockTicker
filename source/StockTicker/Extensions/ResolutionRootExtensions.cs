//-------------------------------------------------------------------------------
// <copyright file="ResolutionRootExtensions.cs" company="bbv Software Services AG">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ninject;
    using Ninject.Activation;
    using Ninject.Parameters;
    using Ninject.Planning.Targets;
    using Ninject.Syntax;

    public static class ResolutionRootExtensions
    {
        /// <summary>
        /// Special get which can be used in situations where constructor
        /// argument need to be provided to resolve. The caller must provide an
        /// anonymous object which acts as prototype for constructor arguments
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="resolutionRoot">The resolution root.</param>
        /// <param name="prototype">The argument provider prototype</param>
        /// <returns>The resolved instance.</returns>
        /// <extensiondoc category="ResolutionRoot" />
        public static T Get<T>(this IResolutionRoot resolutionRoot, object prototype)
        {
            return Get<T>(resolutionRoot, prototype, false);
        }

        /// <summary>
        /// Special get which can be used in situations where constructor
        /// argument need to be provided to resolve. The caller must provide an
        /// anonymous object which acts as prototype for constructor arguments
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="resolutionRoot">The resolution root.</param>
        /// <param name="prototype">The argument provider prototype</param>
        /// <param name="shouldInherit">if set to <c>true</c> the constructor parameters are inherited.</param>
        /// <returns>
        /// The resolved instance.
        /// </returns>
        /// <extensiondoc category="ResolutionRoot"/>
        public static T Get<T>(this IResolutionRoot resolutionRoot, object prototype, bool shouldInherit)
        {
            var arguments = new List<ConstructorArgument>();

            var dictionary = prototype.ToDict();

            var contextAware = dictionary.Where(kvp => kvp.Value is Func<IContext, object>).ToDictionary(
                kvp => kvp.Key, kvp => ((Func<IContext, object>)kvp.Value));

            var contextAndTargetAware = dictionary
                .Where(kvp => kvp.Value is Func<IContext, ITarget, object>)
                .ToDictionary(kvp => kvp.Key, kvp => ((Func<IContext, ITarget, object>)kvp.Value));

            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                Func<IContext, object> contextAwareFunction;
                Func<IContext, ITarget, object> contextAndTargetAwareFunction;

                if (contextAware.TryGetValue(keyValuePair.Key, out contextAwareFunction))
                {
                    arguments.Add(new ConstructorArgument(keyValuePair.Key, ctx => contextAwareFunction(ctx), shouldInherit));
                }
                else if (contextAndTargetAware.TryGetValue(keyValuePair.Key, out contextAndTargetAwareFunction))
                {
                    arguments.Add(new ConstructorArgument(keyValuePair.Key, (ctx, target) => contextAndTargetAwareFunction(ctx, target), shouldInherit));
                }
                else
                {
                    arguments.Add(new ConstructorArgument(keyValuePair.Key, keyValuePair.Value, shouldInherit));
                }
            }

            return ResolutionExtensions.Get<T>(resolutionRoot, arguments.ToArray());
        }

        private static IEnumerable<KeyValuePair<string, object>> ToDict(this object value)
        {
            var publicProperties =
                value.GetType().GetProperties();

            return publicProperties.ToDictionary(publicProperty => publicProperty.Name, publicProperty => publicProperty.GetValue(value, null));
        }
    }
}