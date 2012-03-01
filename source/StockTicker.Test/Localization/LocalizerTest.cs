//-------------------------------------------------------------------------------
// <copyright file="LocalizerTEst.cs" company="bbv Software Services AG">
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
    using System.Threading;

    using Moq;

    using StockTicker.Properties;

    using Xunit;

    public class LocalizerTest
    {
        private readonly Mock<ICultureSetter> cultureSetter;

        private readonly Mock<ISettings> settings;

        private readonly CultureInfo currentCulture;

        private readonly Localizer testee;

        public LocalizerTest()
        {
            this.settings = new Mock<ISettings>();
            this.cultureSetter = new Mock<ICultureSetter>();
            this.currentCulture = Thread.CurrentThread.CurrentCulture;

            this.testee = new Localizer(this.settings.Object, this.cultureSetter.Object);
        }

        [Fact]
        public void ShouldInitializeCultureSetterWithSettings()
        {
            this.settings.SetupGet(s => s.Language).Returns(this.currentCulture);

            this.testee.Initialize();

            this.cultureSetter.Verify(s => s.SetCulture(this.currentCulture));
        }

        [Fact]
        public void ShouldSaveSettings()
        {
            this.testee.Save(this.currentCulture);

            this.settings.VerifySet(s => s.Language = this.currentCulture);
        }
    }
}