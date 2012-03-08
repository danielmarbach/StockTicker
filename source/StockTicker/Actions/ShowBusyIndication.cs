//-------------------------------------------------------------------------------
// <copyright file="ShowBusyIndication.cs" company="bbv Software Services AG">
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

    using Caliburn.Micro;

    internal class ShowBusyIndication : IShowBusyIndication
    {
        private readonly string message;

        private readonly IStartBusyIndication startBusyIndication;

        private readonly Guid requestId;

        public ShowBusyIndication(IStartBusyIndication startBusyIndication, Guid requestId, string message)
        {
            this.startBusyIndication = startBusyIndication;
            this.requestId = requestId;
            this.message = message;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public void Execute(ActionExecutionContext context)
        {
            this.startBusyIndication.Start(this.requestId, this.message);
            this.Completed(this, new ResultCompletionEventArgs());
        }
    }
}