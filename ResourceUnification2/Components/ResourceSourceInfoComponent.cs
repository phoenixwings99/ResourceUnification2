using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceUnification2.Components
{
    [AllowedOn(typeof(BlueprintAbilityResource))]
    [AllowMultipleComponents]
    public class ResourceSourceInfoComponent : BlueprintComponent
    {
        public StatType AltStat;

        public bool IsClassFeature;

        public bool NeedsUnlock => m_Unlock is not null;

        public BlueprintFeatureReference m_Unlock;

        public BlueprintFeature Unlock
        {
            get
            {
                return m_Unlock.Get();
            }
        }



        public bool Active(UnitDescriptor unit)
        {
            return m_Unlock is null || unit.Progression.Features.HasFact(m_Unlock.Get());
        }

    }
}
