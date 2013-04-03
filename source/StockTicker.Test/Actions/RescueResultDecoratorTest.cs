//-------------------------------------------------------------------------------
// <copyright file="RescueResultDecoratorTest.cs" company="bbv Software Services AG">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Caliburn.Micro;
    using FluentAssertions;
    using Moq;
    using StockTicker.TestHelpers;
    using Xunit;
    using Xunit.Extensions;

    public class RescueResultDecoratorTest : IUseFixture<CaliburnIoCFake>
    {
        private readonly Mock<IResult> decorated;

        public RescueResultDecoratorTest()
        {
            this.decorated = new Mock<IResult>();
        }

        [Theory]
        [ClassData(typeof(Exceptions))]
        public void ShouldInvokeRescueOnExceptionWhenExecuteThrows<TException>(TException exception) where TException : Exception
        {
            this.decorated.Setup(r => r.Execute(It.IsAny<ActionExecutionContext>())).Throws(exception);

            this.ShouldInvokeRescueOnException<TException>();
        }

        [Theory]
        [ClassData(typeof(Exceptions))]
        public void ShouldInvokeRescueOnExceptionWhenCompletedHasException<TException>(TException exception) where TException : Exception
        {
            this.decorated.SetupCompletedWithException(exception);

            this.ShouldInvokeRescueOnException<TException>();
        }

        [Fact]
        public void ShouldCancelWhenCancellationRequested()
        {
            this.decorated.SetupCompletedWithException(new InvalidOperationException());

            const bool CancelResult = true;

            var testee = this.CreateTestee<Exception>(results => Enumerable.Empty<IResult>(), CancelResult);
            testee.MonitorEvents();

            testee.Execute(new ActionExecutionContext());

            testee.ShouldRaise("Completed").WithArgs<ResultCompletionEventArgs>(args => args.WasCancelled);
        }

        public void SetFixture(CaliburnIoCFake data)
        {
        }

        private void ShouldInvokeRescueOnException<TException>() where TException : Exception
        {
            bool wasCalled = false;
            var testee = this.CreateTestee<TException>(
                rescue =>
                {
                    wasCalled = true;
                    return Enumerable.Empty<IResult>();
                },
                false);

            testee.Execute(new ActionExecutionContext());

            wasCalled.Should().BeTrue();
        }

        private RescueResultDecorator<TException> CreateTestee<TException>(Func<TException, IEnumerable<IResult>> rescue, bool cancelResult)
            where TException : Exception
        {
            return new RescueResultDecorator<TException>(this.decorated.Object, rescue, cancelResult);
        }

        private class Exceptions : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new InvalidOperationException() };
                yield return new object[] { new ArgumentException() };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}