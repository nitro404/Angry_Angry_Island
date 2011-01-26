using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine
{
    class BouncePlugin : BasePlugin
    {

        private float m_fHeight;
        private float m_fElasticity;

        public BouncePlugin()
        {
            m_fHeight = 1.0f;
            m_fElasticity = 0.0f;
        }

        public override void LoadParameters(PluginDef plugDef)
        {
            // TODO: Load the parameters here.
            if (plugDef.PluginParameters.ContainsKey("Height"))
            {
                m_fHeight = float.Parse(plugDef.PluginParameters["Height"]);
            }
            else if (plugDef.PluginParameters.ContainsKey("Elasticity"))
            {
                m_fElasticity = float.Parse(plugDef.PluginParameters["Elasticity"]);
            }
        }

        public override void Tick()
        {
            
        }


        public override void OnBeginGameplay()
        {
            base.OnBeginGameplay();

        }

        public override void OnEndGameplay()
        {
            base.OnEndGameplay();

        }
    }
}
