﻿#if !NO_WEB
#region License
// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 
#endregion
#region Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Activation.Caching;
using Ninject.Infrastructure.Language;
#endregion

namespace Ninject
{
	/// <summary>
	/// Provides callbacks to more aggressively collect objects scoped to HTTP requests.
	/// </summary>
	public class OnePerRequestModule : IHttpModule
	{
		private static List<IKernel> _kernels = new List<IKernel>();

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="application">The <see cref="HttpApplication"/> whose instances will be managed.</param>
		public void Init(HttpApplication application)
		{
			application.EndRequest += (o,e) => DeactivateInstancesForCurrentHttpRequest();
		}

		/// <summary>
		/// Disposes resources held by the object.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Start managing instances for the specified kernel.
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		public static void StartManaging(IKernel kernel)
		{
			_kernels.Add(kernel);
		}

		/// <summary>
		/// Stops managing instances for the specified kernel.
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		public static void StopManaging(IKernel kernel)
		{
			_kernels.Remove(kernel);
		}

		/// <summary>
		/// Deactivates instances owned by the current <see cref="HttpContext"/>.
		/// </summary>
		public static void DeactivateInstancesForCurrentHttpRequest()
		{
			var context = HttpContext.Current;
			_kernels.Select(kernel => kernel.Components.Get<ICache>()).Map(cache => cache.Clear(context));
		}
	}
}
#endif //!NO_WEB