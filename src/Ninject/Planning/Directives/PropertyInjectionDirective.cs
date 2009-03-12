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
using System.Reflection;
using Ninject.Injection;
using Ninject.Planning.Targets;
#endregion

namespace Ninject.Planning.Directives
{
	/// <summary>
	/// Describes the injection of a property.
	/// </summary>
	public class PropertyInjectionDirective : IDirective
	{
		/// <summary>
		/// Gets or sets the injector that will be triggered.
		/// </summary>
		public PropertyInjector Injector { get; private set; }

		/// <summary>
		/// Gets or sets the injection target for the directive.
		/// </summary>
		public ITarget Target { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyInjectionDirective"/> class.
		/// </summary>
		/// <param name="member">The member the directive describes.</param>
		/// <param name="injector">The injector that will be triggered.</param>
		public PropertyInjectionDirective(PropertyInfo member, PropertyInjector injector)
		{
			Injector = injector;
			Target = CreateTarget(member);
		}

		/// <summary>
		/// Creates a target for the property.
		/// </summary>
		/// <param name="propertyInfo">The property.</param>
		/// <returns>The target for the property.</returns>
		protected virtual ITarget CreateTarget(PropertyInfo propertyInfo)
		{
			return new PropertyTarget(propertyInfo);
		}
	}
}
