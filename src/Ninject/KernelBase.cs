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
using System.Collections.Generic;
using System.Linq;
using Ninject.Activation;
using Ninject.Activation.Blocks;
using Ninject.Activation.Caching;
using Ninject.Activation.Providers;
using Ninject.Components;
using Ninject.Events;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Bindings;
using Ninject.Syntax;
#endregion

namespace Ninject
{
	/// <summary>
	/// The base implementation of an <see cref="IKernel"/>.
	/// </summary>
	public abstract class KernelBase : BindingRoot, IKernel
	{
		private readonly Multimap<Type, IBinding> _bindings = new Multimap<Type, IBinding>();
		private readonly Dictionary<Type, IModule> _modules = new Dictionary<Type, IModule>();

		/// <summary>
		/// Gets the kernel settings.
		/// </summary>
		public INinjectSettings Settings { get; private set; }

		/// <summary>
		/// Gets the component container, which holds components that contribute to Ninject.
		/// </summary>
		public IComponentContainer Components { get; private set; }

		/// <summary>
		/// Occurs when a binding is added.
		/// </summary>
		public event EventHandler<BindingEventArgs> BindingAdded;

		/// <summary>
		/// Occurs when a binding is removed.
		/// </summary>
		public event EventHandler<BindingEventArgs> BindingRemoved;

		/// <summary>
		/// Occurs when a module is loaded.
		/// </summary>
		public event EventHandler<ModuleEventArgs> ModuleLoaded;

		/// <summary>
		/// Occurs when a module is unloaded.
		/// </summary>
		public event EventHandler<ModuleEventArgs> ModuleUnloaded;

		/// <summary>
		/// Initializes a new instance of the <see cref="KernelBase"/> class.
		/// </summary>
		protected KernelBase()
			: this(new ComponentContainer(), new NinjectSettings(), new IModule[0]) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="KernelBase"/> class.
		/// </summary>
		/// <param name="modules">The modules to load into the kernel.</param>
		protected KernelBase(IEnumerable<IModule> modules)
			: this(new ComponentContainer(), new NinjectSettings(), modules) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="KernelBase"/> class.
		/// </summary>
		/// <param name="settings">The configuration to use.</param>
		/// <param name="modules">The modules to load into the kernel.</param>
		protected KernelBase(INinjectSettings settings, IEnumerable<IModule> modules)
			: this(new ComponentContainer(), settings, modules) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="KernelBase"/> class.
		/// </summary>
		/// <param name="components">The component container to use.</param>
		/// <param name="settings">The configuration to use.</param>
		/// <param name="modules">The modules to load into the kernel.</param>
		protected KernelBase(IComponentContainer components, INinjectSettings settings, IEnumerable<IModule> modules)
		{
			Ensure.ArgumentNotNull(components, "components");
			Ensure.ArgumentNotNull(settings, "settings");
			Ensure.ArgumentNotNull(modules, "modules");

			Settings = settings;

			Components = components;
			components.Kernel = this;

			AddComponents();

			modules.Map(LoadModule);
		}

		/// <summary>
		/// Releases resources held by the object.
		/// </summary>
		public override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed && Components != null)
				Components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// Determines whether a module of the specified type has been loaded in the kernel.
		/// </summary>
		/// <param name="moduleType">The type of the module.</param>
		/// <returns><c>True</c> if the specified module has been loaded; otherwise, <c>false</c>.</returns>
		public virtual bool HasModule(Type moduleType)
		{
			Ensure.ArgumentNotNull(moduleType, "moduleType");
			return _modules.ContainsKey(moduleType);
		}

		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		/// <param name="module">The module to load.</param>
		public virtual void LoadModule(IModule module)
		{
			Ensure.ArgumentNotNull(module, "module");

			_modules.Add(module.GetType(), module);
			module.OnLoad(this);

			OnModuleLoaded(module);
		}

        /// <summary>
        /// Called when a module has been loaded.
        /// </summary>
        /// <param name="module">The module.</param>
        protected void OnModuleLoaded(IModule module)
        {
            ModuleLoaded.Raise(this, new ModuleEventArgs(module));
        }

		/// <summary>
		/// Unloads the module with the specified type.
		/// </summary>
		/// <param name="moduleType">The type of the module.</param>
		public virtual void UnloadModule(Type moduleType)
		{
			Ensure.ArgumentNotNull(moduleType, "moduleType");

			IModule module = _modules[moduleType];

			module.OnUnload(this);
			_modules.Remove(moduleType);

			ModuleUnloaded.Raise(this, new ModuleEventArgs(module));
		}

		/// <summary>
		/// Unregisters all bindings for the specified service.
		/// </summary>
		/// <param name="service">The service to unbind.</param>
		public override void Unbind(Type service)
		{
			_bindings.RemoveAll(service);
		}

		/// <summary>
		/// Registers the specified binding.
		/// </summary>
		/// <param name="binding">The binding to add.</param>
		public override void AddBinding(IBinding binding)
		{
			Ensure.ArgumentNotNull(binding, "binding");

			_bindings.Add(binding.Service, binding);
			BindingAdded.Raise(this, new BindingEventArgs(binding));
		}

		/// <summary>
		/// Unregisters the specified binding.
		/// </summary>
		/// <param name="binding">The binding to remove.</param>
		public override void RemoveBinding(IBinding binding)
		{
			Ensure.ArgumentNotNull(binding, "binding");

			_bindings.Remove(binding.Service, binding);
			BindingRemoved.Raise(this, new BindingEventArgs(binding));
		}

		/// <summary>
		/// Injects the specified existing instance, without managing its lifecycle.
		/// </summary>
		/// <param name="instance">The instance to inject.</param>
		/// <param name="parameters">The parameters to pass to the request.</param>
		public void Inject(object instance, params IParameter[] parameters)
		{
			Ensure.ArgumentNotNull(instance, "instance");
			Ensure.ArgumentNotNull(parameters, "parameters");

			Type service = instance.GetType();

			var planner = Components.Get<IPlanner>();
			var pipeline = Components.Get<IPipeline>();

			var binding = new Binding(service) { ScopeCallback = StandardScopeCallbacks.Transient };
			var request = CreateRequest(service, null, parameters, false);
			var context = CreateContext(request, binding);

			context.Plan = planner.GetPlan(service);
			context.Instance = instance;

			pipeline.Activate(context);
		}

		/// <summary>
		/// Determines whether the specified request can be resolved.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns><c>True</c> if the request can be resolved; otherwise, <c>false</c>.</returns>
		public virtual bool CanResolve(IRequest request)
		{
			Ensure.ArgumentNotNull(request, "request");

			if (_bindings.ContainsKey(request.Service))
				return true;

			if (request.Service.IsGenericType && _bindings.ContainsKey(request.Service.GetGenericTypeDefinition()))
				return true;

			return false;
		}

		/// <summary>
		/// Resolves instances for the specified request. The instances are not actually resolved
		/// until a consumer iterates over the enumerator.
		/// </summary>
		/// <param name="request">The request to resolve.</param>
		/// <returns>An enumerator of instances that match the request.</returns>
		public virtual IEnumerable<object> Resolve(IRequest request)
		{
			Ensure.ArgumentNotNull(request, "request");

			if (request.Service == typeof(IKernel))
				return new[] { this };

			if (!CanResolve(request) && !HandleMissingBinding(request.Service))
			{
				if (request.IsOptional)
					return Enumerable.Empty<object>();
				else
					throw new ActivationException(ExceptionFormatter.CouldNotResolveBinding(request));
			}

			return GetBindings(request.Service)
				.Where(binding => binding.Matches(request) && request.Matches(binding))
				.Select(binding => CreateContext(request, binding))
				.Select(context => context.Resolve());
		}

		/// <summary>
		/// Creates a request for the specified service.
		/// </summary>
		/// <param name="service">The service that is being requested.</param>
		/// <param name="constraint">The constraint to apply to the bindings to determine if they match the request.</param>
		/// <param name="parameters">The parameters to pass to the resolution.</param>
		/// <param name="isOptional"><c>True</c> if the request is optional; otherwise, <c>false</c>.</param>
		/// <returns>The created request.</returns>
		public virtual IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional)
		{
			Ensure.ArgumentNotNull(service, "service");
			Ensure.ArgumentNotNull(parameters, "parameters");

			return new Request(service, constraint, parameters, null, isOptional);
		}

		/// <summary>
		/// Gets the bindings registered for the specified service.
		/// </summary>
		/// <param name="service">The service in question.</param>
		/// <returns>A series of bindings that are registered for the service.</returns>
		public virtual IEnumerable<IBinding> GetBindings(Type service)
		{
			Ensure.ArgumentNotNull(service, "service");

			foreach (IBinding binding in _bindings[service])
				yield return binding;

			if (service.IsGenericType)
			{
				Type gtd = service.GetGenericTypeDefinition();

				foreach (IBinding binding in _bindings[gtd])
					yield return binding;
			}
		}

		/// <summary>
		/// Begins a new activation block, which can be used to deterministically dispose resolved instances.
		/// </summary>
		/// <returns>The new activation block.</returns>
		public IActivationBlock BeginBlock()
		{
			return new ActivationBlock(this);
		}

		/// <summary>
		/// Adds components to the kernel during startup.
		/// </summary>
		protected abstract void AddComponents();

		/// <summary>
		/// Tries to handle a missing binding by registering an implicit self-binding for the
		/// specified service.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <returns><c>True</c> if the type is self-bindable; otherwise <c>false</c>.</returns>
		protected virtual bool HandleMissingBinding(Type service)
		{
			Ensure.ArgumentNotNull(service, "service");

			if (service.IsInterface || service.IsAbstract || service.ContainsGenericParameters)
				return false;

			var binding = new Binding(service)
			{
				ProviderCallback = StandardProvider.GetCreationCallback(service),
				ScopeCallback = StandardScopeCallbacks.Transient,
				IsImplicit = true
			};

			AddBinding(binding);

			return true;
		}

		/// <summary>
		/// Creates a context for the specified request and binding.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="binding">The binding.</param>
		/// <returns>The created context.</returns>
		protected virtual IContext CreateContext(IRequest request, IBinding binding)
		{
			Ensure.ArgumentNotNull(request, "request");
			Ensure.ArgumentNotNull(binding, "binding");

			return new Context(this, request, binding, Components.Get<ICache>(), Components.Get<IPlanner>(), Components.Get<IPipeline>());
		}
	}
}
