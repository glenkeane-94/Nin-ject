// -------------------------------------------------------------------------------------------------
// <copyright file="StandardInjectionHeuristic.cs" company="Ninject Project Contributors">
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
    using System.Reflection;

    using Ninject.Components;
    using Ninject.Infrastructure;
    using Ninject.Infrastructure.Language;

    /// <summary>
    /// Determines whether members should be injected during activation by checking
    /// if they are decorated with an injection marker attribute.
    /// </summary>
    public class StandardInjectionHeuristic : NinjectComponent, IInjectionHeuristic
    {
        /// <summary>
        /// Returns a value indicating whether the specified member should be injected.
        /// </summary>
        /// <param name="member">The member in question.</param>
        /// <returns><c>True</c> if the member should be injected; otherwise <c>false</c>.</returns>
        public virtual bool ShouldInject(MemberInfo member)
        {
            Ensure.ArgumentNotNull(member, "member");

            if (member is PropertyInfo propertyInfo)
            {
                var injectNonPublic = this.Settings.InjectNonPublic;

                var setMethod = propertyInfo.GetSetMethod(injectNonPublic);

                return member.HasAttribute(this.Settings.InjectAttribute) && setMethod != null;
            }

            return member.HasAttribute(this.Settings.InjectAttribute);
        }
    }
}