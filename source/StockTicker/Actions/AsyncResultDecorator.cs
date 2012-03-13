//-------------------------------------------------------------------------------
// <copyright file="AsyncResultDecorator.cs" company="bbv Software Services AG">
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
    using System.Threading.Tasks;

    using Caliburn.Micro;
    using Executer = Caliburn.Micro.Execute;

    public class AsyncResultDecorator : ResultDecoratorBase
    {
        public AsyncResultDecorator(IResult inner)
            : base(inner)
        {
        }

        public override void Execute(ActionExecutionContext context)
        {
            Task.Factory.StartNew(() => base.Execute(context));
        }

        protected override void InnerCompleted(object sender, ResultCompletionEventArgs args)
        {
            base.InnerCompleted(sender, args);

            Executer.OnUIThread(() => this.OnCompleted(new ResultCompletionEventArgs { WasCancelled = args.WasCancelled, Error = args.Error }));
        }
    }
}