using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalHeadSwitch
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)] //Only create once, and mainmenu for MM compatibility
    public class HeadSwitchConfiguration : MonoBehaviour
    {
        #region Fields
        Dictionary<ProtoCrewMember, string> crew = new Dictionary<ProtoCrewMember, string>();
        private static Game g;
        private static IEnumerable<ProtoCrewMember> kerbList = g.CrewRoster.Crew;
        private List<ProtoCrewMember> crewList = (List<ProtoCrewMember>)kerbList;

        private bool kerbIsReal;
        private List<Head> headNames = new List<Head>();
        private string headName; //internal name of head
        private string kerbal; //name of kerbal
        private string trait; //kerbal trait
        private string headGender; //gender for head
        private string meshPath = null; //path to mu file
        private string material = null; //idk
        #endregion

        #region Init
        private void Init()
        {
            foreach (ProtoCrewMember k in kerbList)
            {
                if (k.type != ProtoCrewMember.KerbalType.Applicant)
                {
                    crewList.Add(k);
                }
            }
        }
        #endregion

        #region Instance
        public static HeadSwitchConfiguration Instance
        {
            get
            {
                return instance;
            }
        }
        private static HeadSwitchConfiguration instance = new HeadSwitchConfiguration(); //create instance
        #endregion

        #region Parsing Methods
        public void GetNodes()
        {
            foreach (UrlDir.UrlConfig UrlConfig in GameDatabase.Instance.root.GetConfigs("KERBALHEADSWITCH")) //get KERBALHEADSWITCH nodes
            {
                foreach (ConfigNode node in UrlConfig.config.GetNodes("HEAD")) //get HEAD nodes - Thanks blowfish!
                {
                    headName = node.GetValue("name");
                    kerbal = node.GetValue("kerbal");
                    headGender = node.GetValue("gender");
                    meshPath = node.GetValue("mesh");
                    material = node.GetValue("material");
                }
                foreach(ConfigNode node in UrlConfig.config.GetNodes("KERBAL"))
                {
                    kerbal = node.GetValue("kerbalName");
                    trait = node.GetValue("trait");
                }
            }
        }

        public void SetGender(string gender) //Set gender to parsed string
        {
            ProtoCrewMember.Gender headGender = gender.ToEnum<ProtoCrewMember.Gender>();
        }

        public bool KerbalExists(string kerbalName, ProtoCrewMember kerbal) //check to see if kerbalName matches the name of a kerbal
        {
            foreach (ProtoCrewMember kerb in kerbList)
            {
                if (kerb.name == kerbalName)
                {
                    kerbIsReal = true;
                }
                else
                {
                    kerbIsReal = false;
                }
                return kerbIsReal;
            }
            return kerbIsReal;
        }

        public Head GetHead(string name)
        {
            heads.TryGetValue(name, out Head head);
            return head;
        }
        #endregion
    }
}
