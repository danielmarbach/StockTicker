//-------------------------------------------------------------------------------
// <copyright file="ResolutionRootExtensionsTest.cs" company="bbv Software Services AG">
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

namespace StockTicker.Extensions
{
    using System;

    using FluentAssertions;

    using Ninject;
    using Ninject.Activation;
    using Ninject.Planning.Targets;

    using Xunit;

    public class ResolutionRootExtensionsTest : IDisposable
    {
        private readonly StandardKernel kernel;

        public ResolutionRootExtensionsTest()
        {
            this.kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
        }

        [Fact]
        public void GetUsingConstructorArguments_MustUseProvidedArguments()
        {
            var x = new
            {
                firstParameter = "Test",
                secondParameter = 5,
                bar = new Foo(),
                magic = new Func<IContext, object>(ctx => ctx.Kernel.Get<Foo>()),
                ultraMagic = new Func<IContext, ITarget, object>((ctx, target) => ctx.Kernel.Get<Foo>(new { fooValue = target.Name })),
            };

            var result = this.kernel.Get<TestClass>(x);

            result.FirstParameter.Should().Be("Test");
            result.SecondParameter.Should().Be(5);
            result.Bar.Should().BeOfType<Foo>();
            result.Magic.Should().BeOfType<Foo>();
            result.UltraMagic.Should().BeOfType<Foo>();
            result.UltraMagic.FooProperty.Should().Be("ultraMagic");
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }

        private class Foo
        {
            public Foo()
            {
            }

            public Foo(string fooValue)
            {
                this.FooProperty = fooValue;
            }

            public string FooProperty { get; private set; }
        }

        private class TestClass
        {
            private readonly string firstParameter;

            private readonly int secondParameter;

            private readonly Foo bar;

            private readonly Foo magic;

            private readonly Foo ultraMagic;

            public TestClass(string firstParameter, int secondParameter, Foo bar, Foo magic, Foo ultraMagic)
            {
                this.ultraMagic = ultraMagic;
                this.magic = magic;
                this.bar = bar;
                this.secondParameter = secondParameter;
                this.firstParameter = firstParameter;
            }

            public string FirstParameter
            {
                get { return this.firstParameter; }
            }

            public int SecondParameter
            {
                get { return this.secondParameter; }
            }

            public Foo Bar
            {
                get { return this.bar; }
            }

            public Foo Magic
            {
                get { return this.magic; }
            }

            public Foo UltraMagic
            {
                get { return this.ultraMagic; }
            }
        }
    }
}