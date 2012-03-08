//-------------------------------------------------------------------------------
// <copyright file="Window.cs" company="bbv Software Services AG">
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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Window
    {
        private readonly object rootModel;

        private bool? dialogResult;

        public Window(object rootModel, bool dialog, object context, IDictionary<string, object> settings)
        {
            this.rootModel = rootModel;
        }

        public event EventHandler Closed = delegate { };

        public event EventHandler<CancelEventArgs> Closing = delegate { };

        public string DisplayName
        {
            get;
            internal set;
        }

        public object RootModel
        {
            get
            {
                return this.rootModel;
            }
        }

        public bool? ShowDialog()
        {
            return this.dialogResult;
        }

        public void Show()
        {
        }

        public void Close()
        {
        }

        internal void SetDialogResult(bool? result)
        {
            this.dialogResult = result;
        }
    }
}