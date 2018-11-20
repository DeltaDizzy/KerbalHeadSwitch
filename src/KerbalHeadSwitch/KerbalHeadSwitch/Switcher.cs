using System;
using UnityEngine;

namespace KerbalHeadSwitch
{
    public class Switcher
    {
        #region Fields
        HeadSwitchConfiguration headConfig = HeadSwitchConfiguration.Instance;
        Component component;
        ProtoCrewMember kerbal;
        #endregion

        #region Constructors
        public Switcher(Component component, ProtoCrewMember kerbal)
        {
            this.component = component;
        }
        #endregion
        public void HeadSwitcher()
        {
            var head = headConfig.GetHead(kerbal.name);
        }
    }
}
