using System;
using UnityEngine;
using System.Linq;

namespace KerbalHeadSwitch
{
    public class IvaModule : MonoBehaviour
    {
        public void Start()
        {
            var kerbal = GetComponent<Kerbal>();
            new Switcher(kerbal, kerbal.protoCrewMember).HeadSwitcher();
            gameObject.AddComponent<VisibilityChecker>();
        }
    }

    public class EvaModule : PartModule
    {
        private bool isInitialised = false;

        public override void OnStart(StartState state)
        {
            if (!isInitialised)
            {
                isInitialised = true;
                new Switcher(part, part.protoModuleCrew[0]).HeadSwitcher();
            }
        }
    }

    public class VisibilityChecker : MonoBehaviour
    {
        private Renderer head, kerbalHead;

        public void Start()
        {
            foreach (var smr in GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                switch (smr.name)
                {
                    case "headMesh01":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_polySurface51":
                    case "headMesh":
                        kerbalHead = smr;
                        break;
                    case "Head":
                        head = smr;
                        break;
                }
            }
        }

        public void Update()
        {
            if (!head) return;

            // Hide all head meshes when in IVA first-person view
            bool visible = kerbalHead.enabled;
            foreach (var cam in Camera.allCameras)
            {
                if (cam.enabled && head.bounds.Contains(cam.transform.position))
                {
                    visible = false;
                }
            }
            if (head) head.enabled = visible;
        }
    }
}
