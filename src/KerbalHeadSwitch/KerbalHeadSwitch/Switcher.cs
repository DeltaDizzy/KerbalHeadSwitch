using System;
using UnityEngine;

namespace KerbalHeadSwitch
{
    class Switcher
    {
        HeadSwitchConfiguration config;
        Component component;
        ProtoCrewMember kerbal;
        Transform bone = null;
        int layer = 0;

        public Switcher(Component component, ProtoCrewMember kerbal)
        {
            this.component = component;
            this.kerbal = kerbal;
        }

        public void SwitchHead()
        {
            var pony = config.Instance.GetHead(kerbal.name);
            if (pony == null) return;

            foreach (var t in component.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "bn_upperJaw01")
                {
                    bone = t;
                    break;
                }
            }
            System.Diagnostics.Debug.Assert(bone != null, "cannot find bn_upperJaw01");

            var nullMesh = new Mesh();
            foreach (var smr in component.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                switch (smr.name)
                {
                    case "headMesh01":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_polySurface51":
                    case "headMesh":

                    case "eyeballLeft":
                    case "eyeballRight":
                    case "pupilLeft":
                    case "pupilRight":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_eyeballLeft":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_eyeballRight":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pupilLeft":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pupilRight":

                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pCube1": // ponytail
                    case "ponytail":
                    case "tongue":
                    case "upTeeth01":
                    case "upTeeth02":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_upTeeth01":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_downTeeth01":
                    case "downTeeth01":
                        smr.sharedMesh = nullMesh;
                        layer = smr.gameObject.layer;
                        break;
                }
            }

            if (pony.head != null) AddModel("ponyHead", pony.head);
            if (pony.mane != null) AddModel("mane", pony.mane);
            if (pony.eyes != null) AddModel("ponyEyes", pony.eyes);
            if (pony.horn != null) AddModel("horn", pony.horn);

            if (component.GetComponent<EvaModule>() != null)
            {
                component.transform.localScale *= pony.scale;
            }
        }

        public GameObject AddModel(string name, Head head)
        {
            var obj = new GameObject(name);
            var smr = obj.AddComponent<SkinnedMeshRenderer>();
            smr.sharedMesh = head.mesh;
            smr.material = head.headmaterial;
            smr.bones = new Transform[] { bone };
            obj.transform.parent = component.transform;
            obj.layer = layer;
            return obj;
        }
    }
}
