//-------------------------------------------------------------------------------
// <copyright file="ChooseUserNameViewModelTest.cs" company="bbv Software Services AG">
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
    using FluentAssertions;

    using Xunit;

    public class ChooseUserNameViewModelTest
    {
        private readonly ChooseUserNameViewModel testee;

        public ChooseUserNameViewModelTest()
        {
            this.testee = new ChooseUserNameViewModel();
        }

        [Fact]
        public void UserName_ShouldRaisePropertyChanged()
        {
            this.testee.MonitorEvents();

            this.testee.UserName = "AnyName";

            this.testee.ShouldRaisePropertyChangeFor(t => t.UserName);
        }

        [Fact]
        public void ShouldRaisePropertyChangedForHasSuggestions()
        {
            this.testee.MonitorEvents();

            this.testee.Suggestions.Add("AnySuggestion");

            this.testee.ShouldRaisePropertyChangeFor(x => x.HasSuggestions);
            this.testee.HasSuggestions.Should().BeTrue();
        }
    }
}