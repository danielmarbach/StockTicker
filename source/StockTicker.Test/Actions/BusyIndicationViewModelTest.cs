//-------------------------------------------------------------------------------
// <copyright file="BusyIndicationViewModelTest.cs" company="bbv Software Services AG">
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
    using Xunit;

    public class BusyIndicationViewModelTest
    {
        private readonly BusyIndicationViewModel testee;

        public BusyIndicationViewModelTest()
        {
            this.testee = new BusyIndicationViewModel();
        }

        [Fact]
        public void ShouldBeBusyWhenRequestsAvailable()
        {
            this.testee.Start(Guid.NewGuid(), "Loading...");

            this.testee.IsBusy.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotBeBusyWhenNoRequestsAvailable()
        {
            this.testee.IsBusy.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotBeBusyWhenPreviousRequestRemoved()
        {
            Guid requestId = Guid.NewGuid();

            this.testee.Start(requestId, "Loading...");
            this.testee.Finished(requestId);

            this.testee.IsBusy.Should().BeFalse();
        }

        [Fact]
        public void ShouldRaisePropertyChangedWhenStarted()
        {
            this.testee.MonitorEvents();

            this.testee.Start(Guid.NewGuid(), "Loading...");

            this.testee.ShouldRaisePropertyChangeFor(x => x.IsBusy);
        }

        [Fact]
        public void ShouldRaisePropertyChangedWhenFinished()
        {
            var requestId = Guid.NewGuid();
            this.testee.Start(requestId, "Loading...");

            this.testee.MonitorEvents();
            this.testee.Finished(requestId);

            this.testee.ShouldRaisePropertyChangeFor(x => x.IsBusy);
        }
    }
}