//-------------------------------------------------------------------------------
// <copyright file="ActionBuilderMock.cs" company="bbv Software Services AG">
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

namespace StockTicker.TestHelpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Caliburn.Micro;

    using Moq;

    using StockTicker.Actions;

    public class ActionBuilderMock : IActionBuilder, IEnumerable<ActionInfo>
    {
        private readonly List<ActionInfo> actionInfo;

        public ActionBuilderMock()
            : this(new List<ActionInfo>())
        {
        }

        private ActionBuilderMock(List<ActionInfo> actionInfo)
        {
            this.actionInfo = actionInfo;
        }

        IEnumerator<ActionInfo> IEnumerable<ActionInfo>.GetEnumerator()
        {
            return this.actionInfo.GetEnumerator();
        }

        public IEnumerator<IResult> GetEnumerator()
        {
            return this.actionInfo
                .Select(info => Activator.CreateInstance(typeof(Mock<>).MakeGenericType(info.ResultType)))
                .Select(mock => ((dynamic)mock).Object)
                .Cast<IResult>()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IActionBuilder Execute<TResult>() where TResult : IResult
        {
            this.actionInfo.Add(new ActionInfo(typeof(TResult), null));
            return this;
        }

        public IActionBuilder Execute<TResult>(object prototype) where TResult : IResult
        {
            this.actionInfo.Add(new ActionInfo(typeof(TResult), prototype));

            return this;
        }

        public IActionBuilder ScopeWith<TEnterResult, TExitResult>(Action<IActionBuilder> builder, object enterPrototype, object exitPrototype) where TEnterResult : IResult where TExitResult : IResult
        {
            this.actionInfo.Add(new ActionInfo(typeof(TEnterResult), enterPrototype));

            builder(new ActionBuilderMock(this.actionInfo));

            this.actionInfo.Add(new ActionInfo(typeof(TExitResult), exitPrototype));

            return this;
        }

        public IActionBuilder ScopeWith<TEnterResult, TExitResult>(Action<IActionBuilder> builder, object enterPrototype) where TEnterResult : IResult where TExitResult : IResult
        {
            this.actionInfo.Add(new ActionInfo(typeof(TEnterResult), enterPrototype));

            builder(new ActionBuilderMock(this.actionInfo));

            this.actionInfo.Add(new ActionInfo(typeof(TExitResult), null));

            return this;
        }

        public IActionBuilder ScopeWith<TEnterResult, TExitResult>(Action<IActionBuilder> builder) where TEnterResult : IResult where TExitResult : IResult
        {
            this.actionInfo.Add(new ActionInfo(typeof(TEnterResult), null));

            builder(new ActionBuilderMock(this.actionInfo));

            this.actionInfo.Add(new ActionInfo(typeof(TExitResult), null));

            return this;
        }
    }
}