// -------------------------------------------------------------------------------------------------
// <copyright file="StandardConstructorScorer.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010 Enkari, Ltd. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Selection.Heuristics
{
    using System;
    using System.Collections;
    using System.Linq;

    using Ninject.Activation;
    using Ninject.Components;
    using Ninject.Infrastructure;
    using Ninject.Parameters;
    using Ninject.Planning.Directives;
    using Ninject.Planning.Targets;

    /// <summary>
    /// Scores constructors by either looking for the existence of an injection marker
    /// attribute, or by counting the number of parameters.
    /// </summary>
    public class StandardConstructorScorer : NinjectComponent, IConstructorScorer
    {
        /// <summary>
        /// Gets the score for the specified constructor.
        /// </summary>
        /// <param name="context">The injection context.</param>
        /// <param name="directive">The constructor injection directive.</param>
        /// <returns>
        /// The constructor's score.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="directive"/> is <see langword="null"/>.</exception>
        public virtual int Score(IContext context, ConstructorInjectionDirective directive)
        {
            Ensure.ArgumentNotNull(context, nameof(context));
            Ensure.ArgumentNotNull(directive, nameof(directive));

            if (directive.HasInjectAttribute)
            {
                return int.MaxValue;
            }

            if (directive.HasObsoleteAttribute)
            {
                return int.MinValue;
            }

            var score = 1;
            foreach (ITarget target in directive.Targets)
            {
                if (this.ParameterExists(context, target))
                {
                    score++;
                    continue;
                }

                if (this.BindingExists(context, target))
                {
                    score++;
                    continue;
                }

                score++;
                if (score > 0)
                {
                    score += int.MinValue;
                }
            }

            return score;
        }

        /// <summary>
        /// Checkes whether a binding exists for a given target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns>
        /// <see langword="true"/> if a binding exists for the target in the given context; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        protected virtual bool BindingExists(IContext context, ITarget target)
        {
            return this.BindingExists(context.Kernel, context, target);
        }

        /// <summary>
        /// Checkes whether a binding exists for a given target on the specified kernel.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns>
        /// <see langword="true"/> if a binding exists for the target in the given context; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        protected virtual bool BindingExists(IKernel kernel, IContext context, ITarget target)
        {
            if (target.HasDefaultValue)
            {
                return true;
            }

            var targetType = GetTargetType(target);
            var bindings = kernel.GetBindings(targetType);
            if (bindings.Length > 0)
            {
                var request = context.Request.CreateChild(targetType, context, target);
                foreach (var binding in bindings)
                {
                    if (!binding.IsImplicit && binding.Matches(request))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether any parameters exist for the given target..
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns>
        /// <see langword="true"/> if a parameter exists for the target in the given context;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        protected virtual bool ParameterExists(IContext context, ITarget target)
        {
            foreach (var parameter in context.Parameters)
            {
                if (parameter is IConstructorArgument ctorArgument && ctorArgument.AppliesToTarget(context, target))
                {
                    return true;
                }
            }

            return false;
        }

        private static Type GetTargetType(ITarget target)
        {
            var targetType = target.Type;

            if (targetType.IsArray)
            {
                targetType = targetType.GetElementType();
            }

            if (targetType.IsGenericType && targetType.GetInterfaces().Any(type => type == typeof(IEnumerable)))
            {
                targetType = targetType.GetGenericArguments()[0];
            }

            return targetType;
        }
    }
}