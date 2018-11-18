using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalHeadSwitch
{

    public class Head
    {

        public string name;
        public ProtoCrewMember.Gender gender;
        public Mesh mesh;
        public Material material;

        public Head(Mesh mesh, Material material, string name, ProtoCrewMember.Gender gender)
        {
            this.name = name;
            this.mesh = mesh;
            this.material = material;
            this.gender = gender;
        }
    }

    /*
     *KERBALHEADSWITCH
     * {
     *     HEAD
     *     {
     *         name = string
     *         gender = ProtoCrewMember.Gender
     *         mesh = <>.mu
     *         material = 
     *     }
     * }
     */
    public class HeadSwitchConfiguration : MonoBehaviour
    {
        Dictionary<ProtoCrewMember, string> crew = new Dictionary<ProtoCrewMember, string>();

        public HeadSwitchConfiguration Instance
        {
            get
            {
                return instance;
            }
        }
        private static HeadSwitchConfiguration instance = new HeadSwitchConfiguration(); //create instance

        new private string name; //internal name of head
        private string kerbal; //name of kerbal
        private string headGender; //gender for head
        private string meshPath = null; //path to mu file
        private string material = null; //idk


        public void GetNodes()
        {
            foreach (UrlDir.UrlConfig UrlConfig in GameDatabase.Instance.root.GetConfigs("KERBALHEADSWITCH")) //get KERBALHEADSWITCH nodes
            {
                foreach (ConfigNode node in UrlConfig.config.GetNodes("HEAD")) //get HEAD nodes - Thanks blowfish!
                {
                    name = node.GetValue("name");
                    kerbal = node.GetValue("kerbal");
                    headGender = node.GetValue("gender");
                    meshPath = node.GetValue("mesh");
                    material = node.GetValue("material");
                }
            }
        }

        public void SetGender(string gender) //Set gender to parsed string
        {
            ProtoCrewMember.Gender headGender = gender.ToEnum<ProtoCrewMember.Gender>();
        }

        public void SetKerbal(string kerbalName, ProtoCrewMember kerbal) //Set Kerbal name to parsed name, if it exists
        {
            switch (kerbalName)
            {
                case: "Jebediah Kerman"
                        break;
            }
        }
        
    }
}
