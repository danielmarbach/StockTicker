//-------------------------------------------------------------------------------
// <copyright file="ResultDecoratorBase.cs" company="bbv Software Services AG">
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

    using Caliburn.Micro;

    public class ResultDecoratorBase : IResult
    {
        protected ResultDecoratorBase(IResult inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            this.Inner = inner;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        protected IResult Inner { get; private set; }

        public virtual void Execute(ActionExecutionContext context)
        {
            var wrapper = this.IsAlreadyDecorated()
                              ? this.Inner
                              : this.Decorate();

            wrapper.Completed += this.InnerCompleted;

            wrapper.Execute(context);
        }

        protected virtual void OnCompleted(ResultCompletionEventArgs args)
        {
            this.Completed(this, args);
        }

        protected virtual void InnerCompleted(object sender, ResultCompletionEventArgs args)
        {
            ((IResult)sender).Completed -= this.InnerCompleted;
        }

        private SequentialResult Decorate()
        {
            return new SequentialResult(new List<IResult> { this.Inner }.GetEnumerator());
        }

        private bool IsAlreadyDecorated()
        {
            return this.Inner is SequentialResult || this.Inner is ResultDecoratorBase;
        }
    }
}