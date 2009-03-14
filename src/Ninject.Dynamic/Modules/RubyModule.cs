#region Usings

using System.IO;
using IronRuby.Builtins;
using Ninject.Modules;
using Ninject.Planning.Bindings;

#endregion

namespace Ninject.Dynamic.Modules
{
    public class RubyModule : Module
    {
        private readonly IRubyEngine _engine;
        private readonly string _scriptPath;

        public string ScriptPath
        {
            get { return _scriptPath; }
        }


        public RubyModule(IRubyEngine engine, string scriptPath)
        {
            _engine = engine;
            _scriptPath = scriptPath;
        }

        #region Overrides of Module

        public override string Name
        {
            get
            {
                return _scriptPath;
            }
        }

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            var bindings = ((RubyEngine) _engine).ExecuteFile<RubyArray>(_scriptPath);

            bindings.ForEach(item => AddBinding((IBinding) item));
        }

        #endregion
    }
}