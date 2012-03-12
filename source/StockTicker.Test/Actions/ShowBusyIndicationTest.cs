//-------------------------------------------------------------------------------
// <copyright file="ShowBusyIndicationTest.cs" company="bbv Software Services AG">
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

    public class ShowBusyIndicationTest
    {
        private readonly Guid requestId;
        private readonly string message;

        private readonly Mock<IStartBusyIndication> startBusyIndication;

        private readonly ShowBusyIndication testee;

        public ShowBusyIndicationTest()
        {
            this.requestId = Guid.NewGuid();
            this.message = "Any message";

            this.startBusyIndication = new Mock<IStartBusyIndication>();
            this.testee = new ShowBusyIndication(this.startBusyIndication.Object, this.requestId, this.message);
        }

        [Fact]
        public void Execute_MustStartBusyIndication()
        {
            this.testee.Execute(new ActionExecutionContext());

            this.startBusyIndication.Verify(x => x.Start(this.requestId, this.message));
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            this.testee.ShouldRaiseCompleted();
        }
    }
}