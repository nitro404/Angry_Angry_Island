using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine
{
    public abstract class BasePlugin
    {
        public BasePlugin()
        { 
        }

        public abstract void Tick();
        public abstract void LoadParameters(PluginDef plugDef);

        public virtual void OnBeginGameplay() { }
        public virtual void OnEndGameplay() { }
        public virtual void OnPauseGameplay() { }
        public virtual void OnResumeGameplay() { }

    }
}
