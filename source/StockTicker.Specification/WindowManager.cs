//-------------------------------------------------------------------------------
// <copyright file="WindowManager.cs" company="bbv Software Services AG">
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
    using System.Windows.Controls.Primitives;

    using Caliburn.Micro;

    public class WindowManager : IWindowManager
    {
        /// <summary>
        /// Shows a modal dialog for the specified model.
        /// </summary>
        /// <param name="rootModel">The root model.</param>
        /// <param name="context">The context.</param>
        /// <param name="settings">The dialog popup settings.</param>
        /// <returns>The dialog result.</returns>
        public virtual bool? ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            return this.CreateWindow(rootModel, true, context, settings).ShowDialog();
        }

        /// <summary>
        /// Shows a window for the specified model.
        /// </summary>
        /// <param name="rootModel">The root model.</param>
        /// <param name="context">The context.</param>
        /// <param name="settings">The optional window settings.</param>
        public virtual void ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            this.CreateWindow(rootModel, false, context, settings).Show();
        }

        /// <summary>
        /// Shows a popup at the current mouse position.
        /// </summary>
        /// <param name="rootModel">The root model.</param>
        /// <param name="context">The view context.</param>
        /// <param name="settings">The optional popup settings.</param>
        public virtual void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            var popup = this.CreatePopup(rootModel, settings);
            var view = this.CreateWindow(rootModel, false, context, settings);

            popup.Child = view;

            var activatable = rootModel as IActivate;
            if (activatable != null)
            {
                activatable.Activate();
            }

            var deactivator = rootModel as IDeactivate;
            if (deactivator != null)
            {
                popup.Closed += delegate { deactivator.Deactivate(true); };
            }

            popup.IsOpen = true;
            popup.CaptureMouse();
        }

        /// <summary>
        /// Creates a popup for hosting a popup window.
        /// </summary>
        /// <param name="rootModel">The model.</param>
        /// <param name="settings">The optional popup settings.</param>
        /// <returns>The popup.</returns>
        protected virtual Popup CreatePopup(object rootModel, IDictionary<string, object> settings)
        {
            var popup = new Popup();

            if (this.ApplySettings(popup, settings))
            {
                if (!settings.ContainsKey("PlacementTarget") && !settings.ContainsKey("Placement"))
                {
                    popup.Placement = PlacementMode.MousePoint;
                }

                if (!settings.ContainsKey("AllowsTransparency"))
                {
                    popup.AllowsTransparency = true;
                }
            }
            else
            {
                popup.AllowsTransparency = true;
                popup.Placement = PlacementMode.MousePoint;
            }

            return popup;
        }

        /// <summary>
        /// Creates a window.
        /// </summary>
        /// <param name="rootModel">The view model.</param>
        /// <param name="dialog">Whethor or not the window is being shown as a dialog.</param>
        /// <param name="context">The view context.</param>
        /// <param name="settings">The optional popup settings.</param>
        /// <returns>The window.</returns>
        protected virtual Window CreateWindow(object rootModel, bool dialog, object context, IDictionary<string, object> settings)
        {
            var view = new Window(rootModel, dialog, context, settings);

            var haveDisplayName = rootModel as IHaveDisplayName;
            if (haveDisplayName != null)
            {
                view.DisplayName = haveDisplayName.DisplayName;
            }

            this.ApplySettings(view, settings);

            new WindowConductor(rootModel, view);

            return view;
        }

        bool ApplySettings(object target, IEnumerable<KeyValuePair<string, object>> settings)
        {
            if (settings != null)
            {
                var type = target.GetType();

                foreach (var pair in settings)
                {
                    var propertyInfo = type.GetProperty(pair.Key);

                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(target, pair.Value, null);
                    }
                }

                return true;
            }

            return false;
        }

        private class WindowConductor
        {
            private readonly Window view;

            private readonly object model;

            private bool deactivatingFromView;

            private bool deactivateFromViewModel;

            private bool actuallyClosing;

            public WindowConductor(object model, Window view)
            {
                this.model = model;
                this.view = view;

                var activatable = model as IActivate;
                if (activatable != null)
                {
                    activatable.Activate();
                }

                var deactivatable = model as IDeactivate;
                if (deactivatable != null)
                {
                    view.Closed += this.Closed;
                    deactivatable.Deactivated += this.Deactivated;
                }

                var guard = model as IGuardClose;
                if (guard != null)
                {
                    view.Closing += this.Closing;
                }
            }

            void Closed(object sender, EventArgs e)
            {
                this.view.Closed -= this.Closed;
                this.view.Closing -= this.Closing;

                if (this.deactivateFromViewModel)
                {
                    return;
                }

                var deactivatable = (IDeactivate)this.model;

                this.deactivatingFromView = true;
                deactivatable.Deactivate(true);
                this.deactivatingFromView = false;
            }

            void Deactivated(object sender, DeactivationEventArgs e)
            {
                if (!e.WasClosed)
                {
                    return;
                }

                ((IDeactivate)this.model).Deactivated -= this.Deactivated;

                if (this.deactivatingFromView)
                {
                    return;
                }

                this.deactivateFromViewModel = true;
                this.actuallyClosing = true;
                this.view.Close();
                this.actuallyClosing = false;
                this.deactivateFromViewModel = false;
            }

            void Closing(object sender, CancelEventArgs e)
            {
                if (e.Cancel)
                {
                    return;
                }

                var guard = (IGuardClose)this.model;

                if (this.actuallyClosing)
                {
                    this.actuallyClosing = false;
                    return;
                }

                bool runningAsync = false, shouldEnd = false;

                guard.CanClose(canClose =>
                    {
                        Execute.OnUIThread(() =>
                            {
                                if (runningAsync && canClose)
                                {
                                    this.actuallyClosing = true;
                                    this.view.Close();
                                }
                                else
                                {
                                    e.Cancel = !canClose;
                                }

                                shouldEnd = true;
                            });
                    });

                if (shouldEnd)
                {
                    return;
                }

                runningAsync = e.Cancel = true;
            }
        }
    }
}