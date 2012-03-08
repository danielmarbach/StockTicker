//-------------------------------------------------------------------------------
// <copyright file="ActionBuilder.cs" company="bbv Software Services AG">
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
    using System.Collections;
    using System.Collections.Generic;
    using Caliburn.Micro;

    internal class ActionBuilder : IActionBuilder, ICloneable
    {
        private readonly IResultFactory resultFactory;

        private readonly List<IResult> results;

        private readonly IDecoratorApplicatorPipeline applicatorPipeline;

        private readonly IScopeDecoratorApplicator scopeDecoratorApplicator;

        public ActionBuilder(IResultFactory resultFactory, IDecoratorApplicatorPipeline applicatorPipeline, IScopeDecoratorApplicator scopeDecoratorApplicator)
        {
            this.scopeDecoratorApplicator = scopeDecoratorApplicator;
            this.applicatorPipeline = applicatorPipeline;
            this.resultFactory = resultFactory;

            this.results = new List<IResult>();
        }

        public IEnumerator<IResult> GetEnumerator()
        {
            return this.results.GetEnumerator();
        }

        public IActionBuilder Execute<TResult>() where TResult : IResult
        {
            return this.Execute<TResult>(null);
        }

        public IActionBuilder Execute<TResult>(object prototype) where TResult : IResult
        {
            var result = this.CreateResultAndApplyDecorators<TResult>(prototype);
            this.TrackResult(result);

            return this;
        }

        public IActionBuilder ScopeWith<TEnterResult, TExitResult>(Action<IActionBuilder> builder, object enterPrototype, object exitPrototype)
            where TEnterResult : IResult
            where TExitResult : IResult
        {
            var actionBuilder = (IActionBuilder)this.Clone();
            actionBuilder.Execute<TEnterResult>(enterPrototype);

            builder(actionBuilder);

            actionBuilder.Execute<TExitResult>(exitPrototype);

            this.TrackResult(this.scopeDecoratorApplicator.Apply(actionBuilder));

            return this;
        }

        public IActionBuilder ScopeWith<TEnterResult, TExitResult>(Action<IActionBuilder> builder, object enterPrototype)
            where TEnterResult : IResult
            where TExitResult : IResult
        {
            return this.ScopeWith<TEnterResult, TExitResult>(builder, enterPrototype, null);
        }

        public IActionBuilder ScopeWith<TEnterResult, TExitResult>(Action<IActionBuilder> builder)
            where TEnterResult : IResult
            where TExitResult : IResult
        {
            return this.ScopeWith<TEnterResult, TExitResult>(builder, null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public object Clone()
        {
            return new ActionBuilder(this.resultFactory, this.applicatorPipeline, this.scopeDecoratorApplicator);
        }

        private void TrackResult(IResult result)
        {
            this.results.Add(result);
        }

        private IResult CreateResultAndApplyDecorators<TResult>(object prototype) where TResult : IResult
        {
            IEnumerable<IDecoratorApplicator> applicators = this.applicatorPipeline.GetApplicators();

            IResult result = this.resultFactory.Create<TResult>(prototype);

            foreach (IDecoratorApplicator applicator in applicators)
            {
                IResult decorated = applicator.Apply(result);

                result = decorated;
            }

            return result;
        }
    }
}