//-------------------------------------------------------------------------------------------------
// <copyright file="StandardScopeCallbacks.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2016, Ninject Project Contributors
//   Authors: Nate Kohari (nate@enkari.com)
//            Remo Gloor (remo.gloor@gmail.com)
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
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
//-------------------------------------------------------------------------------------------------

namespace Ninject.Infrastructure
{
    using System;
    using Ninject.Activation;

    /// <summary>
    /// Scope callbacks for standard scopes.
    /// </summary>
    public class StandardScopeCallbacks
    {
        /// <summary>
        /// Gets the callback for transient scope.
        /// </summary>
        public static readonly Func<IContext, object> Transient = ctx => null;

        /// <summary>
        /// Gets the callback for singleton scope.
        /// </summary>
        public static readonly Func<IContext, object> Singleton = ctx => ctx.Kernel;

#if !NO_THREAD_SCOPE
        /// <summary>
        /// Gets the callback for thread scope.
        /// </summary>
        public static readonly Func<IContext, object> Thread = ctx => System.Threading.Thread.CurrentThread;
#endif
    }
}