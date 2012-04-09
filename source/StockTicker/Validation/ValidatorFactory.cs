//-------------------------------------------------------------------------------
// <copyright file="ValidatorFactory.cs" company="bbv Software Services AG">
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

namespace StockTicker.Validation
{
    using System;

    using FluentValidation;

    using Ninject;
    using Ninject.Syntax;

    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IResolutionRoot resolutionRoot;

        public ValidatorFactory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public IValidator<T> GetValidator<T>()
        {
            return this.resolutionRoot.Get<IValidator<T>>();
        }

        public IValidator GetValidator(Type type)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(type);

            return (IValidator)this.resolutionRoot.Get(validatorType);
        }
    }
}