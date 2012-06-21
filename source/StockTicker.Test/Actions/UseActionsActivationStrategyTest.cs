//-------------------------------------------------------------------------------
// <copyright file="UseActionsActivationStrategyTest.cs" company="bbv Software Services AG">
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
    using System;

    using FluentAssertions;

    using Moq;

    using Ninject;
    using Ninject.Activation.Strategies;

    using Xunit;

    public class UseActionsActivationStrategyTest
    {
        private readonly StandardKernel kernel;

        public UseActionsActivationStrategyTest()
        {
            this.kernel = new StandardKernel();
            this.kernel.Bind<TestObjectWithUseActions>().ToSelf();
            this.kernel.Bind<IActionBuilder>().ToMethod(ctx => Mock.Of<IActionBuilder>());

            this.kernel.Components.Add<IActivationStrategy, UseActionsActivationStrategy>();
        }

        private interface ITestObjectWithUseAction : IUseActions
        {
        }

        [Fact]
        public void WhenActivatedObjectUsesActions_ShouldAttachActionBuilder()
        {
            var useActions = this.kernel.Get<TestObjectWithUseActions>();

            useActions.Actions.Should().NotBeNull();
            useActions.Actions().Should().NotBeNull();
        }

        private class TestObjectWithUseActions : ITestObjectWithUseAction
        {
            public Func<IActionBuilder> Actions { internal get; set; }
        }
    }
}