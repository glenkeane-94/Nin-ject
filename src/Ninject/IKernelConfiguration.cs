using System.Collections.Generic;
using System.Reflection;
using Ninject.Components;
using Ninject.Modules;
using Ninject.Syntax;

namespace Ninject
{
    /// <summary>
    /// Configuration for a Ninject kernel.
    /// </summary>
    public interface IKernelConfiguration : IBindingRoot
    {
        /// <summary>
        /// Gets the kernel settings.
        /// </summary>
        INinjectSettings Settings { get; }

        /// <summary>
        /// Gets the component container, which holds components that contribute to Ninject.
        /// </summary>
        IComponentContainer Components { get; }

        /// <summary>
        /// Gets the modules that have been loaded into the kernel.
        /// </summary>
        /// <returns>A series of loaded modules.</returns>
        IEnumerable<INinjectModule> GetModules();

        /// <summary>
        /// Determines whether a module with the specified name has been loaded in the kernel.
        /// </summary>
        /// <param name="name">The name of the module.</param>
        /// <returns><c>True</c> if the specified module has been loaded; otherwise, <c>false</c>.</returns>
        bool HasModule(string name);

        /// <summary>
        /// Loads the module(s) into the kernel.
        /// </summary>
        /// <param name="m">The modules to load.</param>
        void Load(IEnumerable<INinjectModule> m);

#if !NO_ASSEMBLY_SCANNING
        /// <summary>
        /// Loads modules from the files that match the specified pattern(s).
        /// </summary>
        /// <param name="filePatterns">The file patterns (i.e. "*.dll", "modules/*.rb") to match.</param>
        void Load(IEnumerable<string> filePatterns);

        /// <summary>
        /// Loads modules defined in the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to search.</param>
        void Load(IEnumerable<Assembly> assemblies);
#endif

        /// <summary>
        /// Unloads the plugin with the specified name.
        /// </summary>
        /// <param name="name">The plugin's name.</param>
        void Unload(string name);
    }
}