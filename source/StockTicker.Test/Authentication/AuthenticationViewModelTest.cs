//-------------------------------------------------------------------------------
// <copyright file="AuthenticationViewModelTest.cs" company="bbv Software Services AG">
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

    using Xunit;

    public class AuthenticationViewModelTest
    {
        private readonly Mock<IAuthenticationStepFactory> authenticationStepFactory;

        private readonly AuthenticationViewModel testee;

        public AuthenticationViewModelTest()
        {
            this.authenticationStepFactory = new Mock<IAuthenticationStepFactory>();

            this.testee = new AuthenticationViewModel(this.authenticationStepFactory.Object, Mock.Of<IBusyIndicationViewModel>());
        }

        [Fact]
        public void ShouldInitializeWizardSteps()
        {
            var firstStep = new FakeStep();
            var secondStep = new FakeStep();

            this.authenticationStepFactory.SetupSequence(f => f.CreateSteps()).Returns(
                new List<IAuthenticationStep> { firstStep, secondStep });

            this.Activate();

            this.testee.Items.Should().HaveCount(2)
                .And.HaveElementAt(0, firstStep)
                .And.HaveElementAt(1, secondStep);

            this.testee.ActiveItem.Should().Be(firstStep);
        }

        [Fact]
        public void Next_ShouldMoveToNextStep()
        {
            var firstStep = new FakeStep();
            var secondStep = new FakeStep();

            this.authenticationStepFactory.SetupSequence(f => f.CreateSteps()).Returns(
                new List<IAuthenticationStep> { firstStep, secondStep });

            this.Activate();

            this.testee.Next();

            this.testee.ActiveItem.Should().Be(secondStep);
        }

        private void Activate()
        {
            this.testee.As<IActivate>().Activate();
        }

        private class FakeStep : Screen, IAuthenticationStep
        {
        }
    }
}