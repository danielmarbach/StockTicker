//-------------------------------------------------------------------------------
// <copyright file="ResultDecoraterBaseTest.cs" company="bbv Software Services AG">
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
    using Caliburn.Micro;
    using FluentAssertions;
    using FluentAssertions.EventMonitoring;
    using Moq;

    using StockTicker.TestHelpers;

    using Xunit;

    public class ResultDecoraterBaseTest : IUseFixture<CaliburnIoCFake>
    {
        private readonly Mock<IResult> result;

        private readonly TestableResultDecorator testee;

        public ResultDecoraterBaseTest()
        {
            this.result = new Mock<IResult>();

            this.testee = new TestableResultDecorator(this.result.Object);
        }

        [Fact]
        public void Constructor_WhenInnerIsNull_ThrowsException()
        {
            this.Invoking(test => new TestableResultDecorator(null)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Execute_WhenCompleted_FiresCompleted()
        {
            this.result.SetupCompletedOnExecute();

            this.testee.MonitorEvents();

            this.testee.Execute(new ActionExecutionContext());

            this.testee.ShouldRaise("Completed").WithArgs<ResultCompletionEventArgs>(e => e.Error == null && e.WasCancelled == false);
        }

        [Fact]
        public void Execute_WhenCompletedWithError_FiresCompleted()
        {
            var exception = new Exception();
            this.result.SetupCompletedWithException(exception);

            this.testee.MonitorEvents();

            this.testee.Execute(new ActionExecutionContext());

            this.testee.ShouldRaise("Completed").WithArgs<ResultCompletionEventArgs>(e => e.Error == exception && e.WasCancelled == false);
        }

        [Fact]
        public void Execute_WhenCompletedWithCancellation_FiresCompleted()
        {
            this.result.SetupCompletedCancelled();

            this.testee.MonitorEvents();

            this.testee.Execute(new ActionExecutionContext());

            this.testee.ShouldRaise("Completed").WithArgs<ResultCompletionEventArgs>(e => e.Error == null && e.WasCancelled == true);
        }

        public void SetFixture(CaliburnIoCFake data)
        {
        }

        private class TestableResultDecorator : ResultDecoratorBase
        {
            public TestableResultDecorator(IResult inner)
                : base(inner)
            {
            }

            protected override void InnerCompleted(object sender, ResultCompletionEventArgs args)
            {
                base.InnerCompleted(sender, args);

                this.OnCompleted(args);
            }
        }
    }
}