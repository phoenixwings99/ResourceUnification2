using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Config;

namespace ResourceUnification2.Config
{
    internal class ResourceDefines : IUpdatableSettings
    {

        public List<CombinedScalingResourceEntry> ClassScalingResourceEntries = new();

        public void Init()
        {

        }

        public void OverrideSettings(IUpdatableSettings userSettings)
        {
            ResourceDefines loadedSettings = userSettings as ResourceDefines;
            if (loadedSettings == null) { return; }
        }
    }
}
