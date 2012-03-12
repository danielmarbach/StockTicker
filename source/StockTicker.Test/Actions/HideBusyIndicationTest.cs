//-------------------------------------------------------------------------------
// <copyright file="HideBusyIndicationTest.cs" company="bbv Software Services AG">
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

    using Moq;

    using StockTicker.TestHelpers;

    using Xunit;

    public class HideBusyIndicationTest
    {
        private readonly Guid requestId;

        private readonly Mock<IFinishBusyIndication> finishBusyIndication;

        private readonly HideBusyIndication testee;

        public HideBusyIndicationTest()
        {
            this.finishBusyIndication = new Mock<IFinishBusyIndication>();

            this.requestId = Guid.NewGuid();

            this.testee = new HideBusyIndication(this.finishBusyIndication.Object, this.requestId);
        }

        [Fact]
        public void Execute_MustFinishBusyIndication()
        {
            this.testee.Execute(new ActionExecutionContext());

            this.finishBusyIndication.Verify(x => x.Finished(this.requestId));
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            this.testee.ShouldRaiseCompleted();
        }
    }
}