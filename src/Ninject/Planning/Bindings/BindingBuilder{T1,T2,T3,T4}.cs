﻿//-------------------------------------------------------------------------------
// <copyright file="BindingBuilder{T1,T2,T3,T4}.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2009, Enkari, Ltd.
//   Copyright (c) 2009-2011 Ninject Project Contributors
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
//-------------------------------------------------------------------------------

namespace Ninject.Planning.Bindings
{
    using System;
#if !NETCF
    using System.Linq.Expressions;
#endif    
    using Ninject.Activation;
    using Ninject.Activation.Providers;
    using Ninject.Infrastructure;
    using Ninject.Syntax;

    /// <summary>
    /// Provides a root for the fluent syntax associated with an <see cref="BindingBuilder.BindingConfiguration"/>.
    /// </summary>
    /// <typeparam name="T1">The first service type.</typeparam>
    /// <typeparam name="T2">The second service type.</typeparam>
    /// <typeparam name="T3">The third service type.</typeparam>
    /// <typeparam name="T4">The fourth service type.</typeparam>
    public class BindingBuilder<T1, T2, T3, T4> : BindingBuilder, IBindingToSyntax<T1, T2, T3, T4>
    {
        /// <summary>
        /// The names of the services.
        /// </summary>
        private readonly string serviceNames;

#pragma warning disable 1584 //mono compiler bug
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingBuilder{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="bindingConfigurationConfiguration">The binding to build.</param>
        /// <param name="kernel">The kernel.</param>
        /// <param name="serviceNames">The names of the services.</param>
        public BindingBuilder(IBindingConfiguration bindingConfigurationConfiguration, IKernel kernel, string serviceNames)
            : base(bindingConfigurationConfiguration, kernel)
        {
            this.serviceNames = serviceNames;
        }
#pragma warning restore 1584

        /// <summary>
        /// Indicates that the service should be bound to the specified implementation type.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>()
            where TImplementation : T1, T2, T3, T4
        {
            this.BindingConfiguration.ProviderCallback = StandardProvider.GetCreationCallback(typeof(TImplementation));
            this.BindingConfiguration.Target = BindingTarget.Type;

            return new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }

        /// <summary>
        /// Indicates that the service should be bound to the specified implementation type.
        /// </summary>
        /// <param name="implementation">The implementation type.</param>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation)
        {
            this.BindingConfiguration.ProviderCallback = StandardProvider.GetCreationCallback(implementation);
            this.BindingConfiguration.Target = BindingTarget.Type;

            return new BindingConfigurationBuilder<object>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }

  #if !NETCF
        /// <summary>
        /// Indicates that the service should be bound to the speecified constructor.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="newExpression">The expression that specifies the constructor.</param>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(
            Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression)
            where TImplementation : T1, T2, T3, T4
        {
            var ctorExpression = newExpression.Body as NewExpression;
            if (ctorExpression == null)
            {
                throw new ArgumentException("The expression must be a constructor call.", "newExpression");
            }

            this.BindingConfiguration.ProviderCallback = StandardProvider.GetCreationCallback(ctorExpression.Type, ctorExpression.Constructor);
            this.BindingConfiguration.Target = BindingTarget.Type;
            this.AddConstructorArguments(ctorExpression, newExpression.Parameters[0]);

            return new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }
#endif

        /// <summary>
        /// Indicates that the service should be bound to an instance of the specified provider type.
        /// The instance will be activated via the kernel when an instance of the service is activated.
        /// </summary>
        /// <typeparam name="TProvider">The type of provider to activate.</typeparam>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>()
            where TProvider : IProvider
        {
            return this.ToProviderInternal<TProvider, object>();
        }

        /// <summary>
        /// Indicates that the service should be bound to an instance of the specified provider type.
        /// The instance will be activated via the kernel when an instance of the service is activated.
        /// </summary>
        /// <typeparam name="TProvider">The type of provider to activate.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>() 
            where TProvider : IProvider<TImplementation> 
            where TImplementation : T1, T2, T3, T4
        {
            return this.ToProviderInternal<TProvider, TImplementation>();
        }
        
        /// <summary>
        /// Indicates that the service should be bound to an instance of the specified provider type.
        /// The instance will be activated via the kernel when an instance of the service is activated.
        /// </summary>
        /// <param name="providerType">The type of provider to activate.</param>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType)
        {
            this.BindingConfiguration.ProviderCallback = ctx => ctx.Kernel.Get(providerType) as IProvider;
            this.BindingConfiguration.Target = BindingTarget.Provider;

            return new BindingConfigurationBuilder<object>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }

        /// <summary>
        /// Indicates that the service should be bound to the specified provider.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="provider">The provider.</param>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider)
            where TImplementation : T1, T2, T3, T4
        {
            this.BindingConfiguration.ProviderCallback = ctx => provider;
            this.BindingConfiguration.Target = BindingTarget.Provider;

            return new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }

        /// <summary>
        /// Indicates that the service should be bound to the specified callback method.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="method">The method.</param>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method)
            where TImplementation : T1, T2, T3
        {
            this.BindingConfiguration.ProviderCallback = ctx => new CallbackProvider<TImplementation>(method);
            this.BindingConfiguration.Target = BindingTarget.Method;

            return new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }

        /// <summary>
        /// Indicates that the service should be bound to the specified constant value.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="value">The constant value.</param>
        /// <returns>The fluent syntax.</returns>
        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value)
            where TImplementation : T1, T2, T3, T4
        {
            this.BindingConfiguration.ProviderCallback = ctx => new ConstantProvider<TImplementation>(value);
            this.BindingConfiguration.Target = BindingTarget.Constant;
            this.BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Singleton;

            return new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }

        /// <summary>
        /// Indicates that the service should be bound to an instance of the specified provider type.
        /// The instance will be activated via the kernel when an instance of the service is activated.
        /// </summary>
        /// <typeparam name="TProvider">The type of provider to activate.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>The fluent syntax.</returns>
        private IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProviderInternal<TProvider, TImplementation>()
            where TProvider : IProvider
        {
            this.BindingConfiguration.ProviderCallback = ctx => ctx.Kernel.Get<TProvider>();
            this.BindingConfiguration.Target = BindingTarget.Provider;

            return new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.serviceNames, this.Kernel);
        }
    }
}