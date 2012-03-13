//-------------------------------------------------------------------------------
// <copyright file="AsyncResultDecoratorTest.cs" company="bbv Software Services AG">
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
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using FluentAssertions;
    using Moq;
    using StockTicker.TestHelpers;
    using Xunit;

    public class AsyncResultDecoratorTest : IUseFixture<CaliburnIoCFake>
    {
        private readonly Mock<IResult> result;

        private readonly AsyncResultDecorator testee;

        public AsyncResultDecoratorTest()
        {
            this.result = new Mock<IResult>();

            this.testee = new AsyncResultDecorator(this.result.Object);
        }

        [Fact]
        public void WhenCompletedShouldBeCompleted()
        {
            this.result.SetupCompletedOnExecute();

            ResultCompletionEventArgs result = this.ExecuteTestee();

            result.Error.Should().BeNull();
            result.WasCancelled.Should().BeFalse();
        }

        [Fact]
        public void WhenCancelledShouldBeCancelled()
        {
            this.result.SetupCompletedCancelled();

            ResultCompletionEventArgs result = this.ExecuteTestee();

            result.Error.Should().BeNull();
            result.WasCancelled.Should().BeTrue();
        }

        [Fact]
        public void WhenExceptionShouldHaveError()
        {
            var expectedException = new Exception();
            this.result.SetupCompletedWithException(expectedException);

            ResultCompletionEventArgs result = this.ExecuteTestee();

            result.Error.Should().Be(expectedException);
            result.WasCancelled.Should().BeFalse();
        }

        [Fact]
        public void WhenExceptionShouldOnExecuteShouldCompleteWithException()
        {
            var expectedException = new InvalidOperationException();
            this.result.Setup(r => r.Execute(It.IsAny<ActionExecutionContext>())).Throws(expectedException);

            ResultCompletionEventArgs result = this.ExecuteTestee();

            result.Error.Should().Be(expectedException);
            result.WasCancelled.Should().BeFalse();
        }

        public void SetFixture(CaliburnIoCFake data)
        {
        }

        private ResultCompletionEventArgs ExecuteTestee()
        {
            var completionSource = new TaskCompletionSource<ResultCompletionEventArgs>();
            this.testee.Completed += (sender, args) => completionSource.SetResult(args);

            this.testee.Execute(new ActionExecutionContext());

            ResultCompletionEventArgs result = completionSource.Task.Result;
            return result;
        }
    }
}