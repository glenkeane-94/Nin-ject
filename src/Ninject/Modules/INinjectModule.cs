﻿#region License
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
#region Using Directives
using System;
using Ninject.Infrastructure;
using Ninject.Syntax;
#endregion

namespace Ninject.Modules
{
	/// <summary>
	/// A pluggable unit that can be loaded into an <see cref="IKernel"/>.
	/// </summary>
	public interface INinjectModule
	{
		/// <summary>
		/// Gets or the kernel that the module is loaded into.
		/// </summary>
		IKernel Kernel { get; }

		/// <summary>
		/// Gets the module's name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Called when the module is loaded into a kernel.
		/// </summary>
		/// <param name="kernel">The kernel that is loading the module.</param>
		void OnLoad(IKernel kernel);

		/// <summary>
		/// Called when the module is unloaded from a kernel.
		/// </summary>
		/// <param name="kernel">The kernel that is unloading the module.</param>
		void OnUnload(IKernel kernel);
	}
}