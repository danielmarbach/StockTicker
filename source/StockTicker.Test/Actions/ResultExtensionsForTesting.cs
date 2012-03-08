//-------------------------------------------------------------------------------
// <copyright file="ResultExtensionsForTesting.cs" company="bbv Software Services AG">
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

    using FluentAssertions.EventMonitoring;

    using Moq;

    public static class ResultExtensionsForTesting
    {
        public static void SetupCompletedOnExecute(this Mock<IResult> result)
        {
            SetupCompletedOnExecute(result, null, false);
        }

        public static void SetupCompletedWithException(this Mock<IResult> result, Exception exception)
        {
            SetupCompletedOnExecute(result, exception, false);
        }

        public static void SetupCompletedCancelled(this Mock<IResult> result)
        {
            SetupCompletedOnExecute(result, null, true);
        }

        public static void SetupCompletedOnExecute(this Mock<IResult> result, Exception exception, bool wasCancelled)
        {
            result.Setup(r => r.Execute(It.IsAny<ActionExecutionContext>())).Raises(
                r => r.Completed += null, new ResultCompletionEventArgs { Error = exception, WasCancelled = wasCancelled });
        }

        public static void ShouldRaiseCompleted(this IResult result)
        {
            result.MonitorEvents();

            result.Execute(new ActionExecutionContext());

            result.ShouldRaise("Completed");
        }
    }
}