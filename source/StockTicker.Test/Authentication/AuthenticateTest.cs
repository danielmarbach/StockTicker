//-------------------------------------------------------------------------------
// <copyright file="AuthenticateTest.cs" company="bbv Software Services AG">
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
    using System.Dynamic;

    using Caliburn.Micro;

    using Moq;

    using StockTicker.TestHelpers;
    using Xunit;

    public class AuthenticateTest
    {
        private readonly Mock<IAuthenticationViewModel> authenticationViewModel;

        private readonly Mock<IWindowManager> windowManager;

        private readonly Authenticate testee;

        public AuthenticateTest()
        {
            this.authenticationViewModel = new Mock<IAuthenticationViewModel>();
            this.windowManager = new Mock<IWindowManager>();

            this.testee = new Authenticate(this.windowManager.Object, this.authenticationViewModel.Object);
        }

        [Fact]
        public void ShouldRaiseCompleted()
        {
            this.testee.ShouldRaiseCompleted();
        }

        [Fact]
        public void ShouldOpenAuthenticationWizard()
        {
            this.testee.Execute(new ActionExecutionContext());

            this.windowManager.Verify(w => w.ShowDialog(this.authenticationViewModel.Object, null, It.IsAny<ExpandoObject>()));
        }
    }
}