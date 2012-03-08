//-------------------------------------------------------------------------------
// <copyright file="ActionBuilderTest.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Caliburn.Micro;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class ActionBuilderTest
    {
        private readonly Mock<IResultFactory> resultFactory;

        private readonly Mock<IDecoratorApplicatorPipeline> applicatorPipeline;

        private readonly Mock<IScopeDecoratorApplicator> scopeDecoratorApplicator;

        private readonly ActionBuilder testee;

        public ActionBuilderTest()
        {
            this.resultFactory = new Mock<IResultFactory>();
            this.applicatorPipeline = new Mock<IDecoratorApplicatorPipeline>();
            this.scopeDecoratorApplicator = new Mock<IScopeDecoratorApplicator>();

            this.testee = new ActionBuilder(this.resultFactory.Object, this.applicatorPipeline.Object, this.scopeDecoratorApplicator.Object);
        }

        [Fact]
        public void ShouldDecorateIfNecessary()
        {
            this.SetupResultFactory();
            this.SetupDecoratorApplicatorPipelineWithTwoDecoratorApplicators();

            IResult decorated = this.testee.ExecuteITestResult().Single();

            decorated.As<OuterDecorator>().Result.As<InnerDecorator>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldReturnBuiltResults()
        {
            this.SetupPipelinePassthrough();
            this.SetupResultFactory();

            this.testee.ExecuteITestResult().ExecuteITestResult()
                .Should().HaveCount(2);
        }

        [Fact]
        public void ShouldPassContructorArguments()
        {
            this.SetupPipelinePassthrough();
            this.SetupResultFactory();

            const string Anyinput = "AnyInput";

            var results =
                this.testee.ExecuteITestResult(Anyinput).ExecuteITestResult(Anyinput)
                .OfType<ITestResult>();

            results.Should().HaveCount(2).And.OnlyContain(r => r.Input == Anyinput);
        }

        [Fact]
        public void ShouldUseNestedScopes()
        {
            this.SetupPipelinePassthrough();
            this.SetupResultFactory();
            var decoratedResults = this.SetupScopeDecoratorApplicator();

            var results =
                this.testee
                    .ExecuteITestResult("First")
                    .ScopeWithITestResult(
                        outer => outer
                            .ExecuteITestResult("FirstInner")
                            .ScopeWithITestResult(
                                inner => inner
                                     .ExecuteITestResult("FirstInnerInner"),
                                "EnterInnerInner",
                                "ExitInnerInner")
                                     .ExecuteITestResult(),
                        "EnterInner",
                        "ExitInner")
                        .ExecuteITestResult("Last");

            results.Should().HaveCount(3);
            decoratedResults.Should().HaveCount(8);

            results.ElementAt(0).As<ITestResult>().Input.Should().Be("First");
            results.ElementAt(1).As<IResult>().Should().NotBeNull();
            results.ElementAt(2).As<ITestResult>().Input.Should().Be("Last");

            this.scopeDecoratorApplicator.Verify(a => a.Apply(It.IsAny<IEnumerable<IResult>>()), Times.Exactly(2));
        }

        private void SetupPipelinePassthrough()
        {
            var applicator = new Mock<IDecoratorApplicator>();
            IResult result = null;
            applicator.Setup(d => d.Apply(It.IsAny<IResult>()))
                .Callback<IResult>(r => result = r)
                .Returns(() => result);

            this.applicatorPipeline.Setup(p => p.GetApplicators())
                .Returns(new List<IDecoratorApplicator> { applicator.Object });
        }

        private void SetupDecoratorApplicatorPipelineWithTwoDecoratorApplicators()
        {
            IResult result = null;
            var innerApplicator = new Mock<IDecoratorApplicator>();
            innerApplicator.Setup(a => a.Apply(It.IsAny<IResult>())).Callback<IResult>(r => result = r).Returns(
                () => new InnerDecorator(result));

            var outerApplicator = new Mock<IDecoratorApplicator>();
            outerApplicator.Setup(a => a.Apply(It.IsAny<IResult>())).Callback<IResult>(r => result = r).Returns(
                () => new OuterDecorator(result));

            this.applicatorPipeline.Setup(p => p.GetApplicators()).Returns(
                new List<IDecoratorApplicator> { innerApplicator.Object, outerApplicator.Object });
        }

        private void SetupResultFactory()
        {
            ITestResult result = null;

            this.resultFactory.Setup(f => f.Create<ITestResult>(It.IsAny<object>())).Callback<object>(
                @in =>
                {
                    string input = null;
                    if (@in != null)
                    {
                        input = (string)@in.GetType().GetProperty("input").GetValue(@in, null);
                    }

                    result = Mock.Of<ITestResult>(r => r.Input == input);
                }).Returns(() => result);
        }

        private IEnumerable<IResult> SetupScopeDecoratorApplicator()
        {
            var decoratedScopeResults = new Collection<IResult>();
            this.scopeDecoratorApplicator.Setup(a => a.Apply(It.IsAny<IEnumerable<IResult>>()))
                .Callback<IEnumerable<IResult>>(results => results.ToList().ForEach(r => decoratedScopeResults.Add(r)))
                .Returns(Mock.Of<IResult>());

            return decoratedScopeResults;
        }

        private class InnerDecorator : IResult
        {
            public InnerDecorator(IResult result)
            {
                this.Result = result;
            }

            public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

            public IResult Result { get; private set; }

            public void Execute(ActionExecutionContext context)
            {
            }
        }

        private class OuterDecorator : IResult
        {
            public OuterDecorator(IResult result)
            {
                this.Result = result;
            }

            public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

            public IResult Result { get; private set; }

            public void Execute(ActionExecutionContext context)
            {
            }
        }
    }
}