//-------------------------------------------------------------------------------
// <copyright file="RescueResultDecorator.cs" company="bbv Software Services AG">
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

    internal class RescueResultDecorator<TException> : ResultDecoratorBase
        where TException : Exception
    {
        private readonly Func<TException, IEnumerable<IResult>> rescue;

        private ActionExecutionContext context;

        public RescueResultDecorator(IResult inner, Func<TException, IEnumerable<IResult>> rescue, bool cancelResult)
            : base(inner)
        {
            if (rescue == null)
            {
                throw new ArgumentNullException("rescue");
            }

            this.rescue = rescue;
            this.CancelResult = cancelResult;
        }

        protected bool CancelResult { get; private set; }

        public override void Execute(ActionExecutionContext context)
        {
            this.context = context;
            try
            {
                base.Execute(context);
            }
            catch (TException e)
            {
                this.Handle(e);
            }
        }

        protected override void InnerCompleted(object sender, ResultCompletionEventArgs args)
        {
            base.InnerCompleted(sender, args);

            if (args.Error is TException)
            {
                this.Handle(args.Error as TException);
            }
            else
            {
                this.OnCompleted(args);
            }
        }

        private void Handle(TException error)
        {
            try
            {
                this.Rescue(error);
            }
            catch (Exception e)
            {
                this.OnCompleted(new ResultCompletionEventArgs { Error = e });
            }
        }

        private void Rescue(TException exception)
        {
            var rescueResult = new SequentialResult(this.rescue(exception).GetEnumerator());
            rescueResult.Completed += this.RescueCompleted;

            rescueResult.Execute(this.context);
        }

        private void RescueCompleted(object sender, ResultCompletionEventArgs args)
        {
            ((IResult)sender).Completed -= this.RescueCompleted;

            this.OnCompleted(new ResultCompletionEventArgs { Error = args.Error, WasCancelled = args.WasCancelled || this.CancelResult });
        }
    }
}