//-------------------------------------------------------------------------------
// <copyright file="SuggestUsernamesTest.cs" company="bbv Software Services AG">
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

namespace StockTicker.Authentication
{
    using System.Collections.Generic;

    using Caliburn.Micro;

    using FluentAssertions;

    using Moq;

    using StockTicker.Actions;
    using StockTicker.Externals;
    using StockTicker.TestHelpers;

    using Xunit;

    public class SuggestUsernamesTest
    {
        private readonly Mock<IAuthenticationService> authenticationService;

        private readonly PotentialNewUserModel potentialNewUser;

        private readonly SuggestUsernames testee;

        private readonly List<string> suggestions;

        public SuggestUsernamesTest()
        {
            this.authenticationService = new Mock<IAuthenticationService>();
            this.potentialNewUser = new PotentialNewUserModel("AnyFirstname", "AnyLastname");
            this.suggestions = new List<string>();

            this.testee = new SuggestUsernames(this.authenticationService.Object, this.potentialNewUser, this.suggestions);
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            this.testee.ShouldRaiseCompleted();
        }

        [Fact]
        public void ShouldRequestAsynchronousExecution()
        {
            this.testee.Should().BeDecoratedWith<AsyncAttribute>();
        }

        [Fact]
        public void ShouldFillFoundStocks()
        {
            var expectedSearchModels = new List<string> { "First", "Second", };

            this.authenticationService.Setup(s => s.SuggestUsernames(this.potentialNewUser))
                .Returns(expectedSearchModels);

            this.testee.Execute(new ActionExecutionContext());

            this.suggestions.Should().BeEquivalentTo(expectedSearchModels);
        }

        [Fact]
        public void ShouldClearInputCollection()
        {
            this.suggestions.Add("First");
            this.suggestions.Add("Second");

            this.testee.Execute(new ActionExecutionContext());

            this.suggestions.Should().BeEmpty();
        }
    }
}