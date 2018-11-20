using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
