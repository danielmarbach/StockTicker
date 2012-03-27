//-------------------------------------------------------------------------------
// <copyright file="Future.cs" company="bbv Software Services AG">
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

namespace StockTicker
{
    using System.Threading;

    public sealed class Future<TValue>
    {
        private readonly ManualResetEvent syncEvent;

        private TValue value;

        public Future()
        {
            this.syncEvent = new ManualResetEvent(false);
        }

        public TValue Value
        {
            get
            {
                this.syncEvent.WaitOne();
                return this.value;
            }
        }

        public void SetValue(TValue value)
        {
            ThreadPool.QueueUserWorkItem(
                state =>
                    {
                        this.value = value;

                        this.syncEvent.Set();
                    });
        }
    }
}