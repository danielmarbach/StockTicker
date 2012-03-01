//-------------------------------------------------------------------------------
// <copyright file="LocalizationCultureProviderTest.cs" company="bbv Software Services AG">
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

namespace StockTicker.Localization
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class LocalizationCultureProviderTest
    {
        private readonly Mock<ICultureSetter> cultureSetter;
        
        private readonly LocalizationCultureProvider testee;

        public LocalizationCultureProviderTest()
        {
            this.cultureSetter = new Mock<ICultureSetter>();
            
            this.testee = new LocalizationCultureProvider(this.cultureSetter.Object);
        }

        [Fact]
        public void ShouldSetCurrentThreadsUiCulture()
        {
            CultureInfo anyCulture = GetAnyCulture();

            this.testee.SetCulture(anyCulture);

            Thread.CurrentThread.CurrentUICulture.Should().Be(anyCulture);
        }

        [Fact]
        public void ShouldSetCultureOnSetter()
        {
            CultureInfo expectedCulture = GetCurrentUiCulture();

            this.testee.SetCulture(expectedCulture);

            this.cultureSetter.Verify(s => s.SetCulture(expectedCulture));
        }

        [Fact]
        public void ShouldGetCurrentUiCulture()
        {
            CultureInfo currentUiCulture = GetCurrentUiCulture();

            this.testee.CurrentCulture.Should().Be(currentUiCulture);
        }

        [Fact]
        public void WhenCultureChanged_ShouldIndicateCurrentUiCulture()
        {
            CultureInfo anyCulture = GetAnyCulture();
            this.testee.SetCulture(anyCulture);

            this.testee.CurrentCulture.Should().Be(anyCulture);
        }

        private static CultureInfo GetAnyCulture()
        {
            return GetAnyCulture(GetCurrentUiCulture());
        }

        private static CultureInfo GetAnyCulture(CultureInfo currentUiCulture)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => c != currentUiCulture)
                .Take(1)
                .Single();
        }

        private static CultureInfo GetCurrentUiCulture()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }
    }
}