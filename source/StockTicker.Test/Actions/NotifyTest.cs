//-------------------------------------------------------------------------------
// <copyright file="NotifyTest.cs" company="bbv Software Services AG">
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
    using Caliburn.Micro;

    using Moq;

    using StockTicker.TestHelpers;

    using Xunit;

    public class NotifyTest
    {
        private readonly Mock<IEventAggregator> eventAggregator;

        private readonly object message;

        private readonly Notify testee;

        public NotifyTest()
        {
            this.eventAggregator = new Mock<IEventAggregator>();
            this.message = new object();

            this.testee = new Notify(this.eventAggregator.Object, this.message);
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            this.testee.ShouldRaiseCompleted();
        }

        [Fact]
        public void ShouldPublish()
        {
            this.testee.Execute(new ActionExecutionContext());

            this.eventAggregator.Verify(e => e.Publish(this.message));
        }
    }
}