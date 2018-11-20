using System;
using UnityEngine;

namespace KerbalHeadSwitch
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class SwitchAddon : MonoBehaviour
    {
        private bool isInitialized = false;
        private Game game;
        HeadSwitchConfiguration config = HeadSwitchConfiguration.Instance;

        public void Start()
        {
            GameEvents.onKerbalAdded.Add(onKerbalAdd);
            GameEvents.onGameStateCreated.Add(setGame);
            DontDestroyOnLoad(this);
        }

        public void LateUpdate()
        {
            if (!isInitialized && PartLoader.Instance.IsReady())
            {
                isInitialized = true;
                foreach (Kerbal kerbal in Resources.FindObjectsOfTypeAll<Kerbal>())
                {
                    kerbal.gameObject.AddComponent<IvaModule>();
                }
                PartLoader.getPartInfoByName("kerbalEVAfemale").partPrefab.gameObject.AddComponent<EvaModule>();

                HeadSwitchConfiguration.Instance.Load();
            }
        }

        private void onKerbalAdd(ProtoCrewMember crew)
        {
            if (crew.type != ProtoCrewMember.KerbalType.Applicant) return;
            if (crew.name.GetHashCode() % 3 != 0) return;

            foreach (var name in HeadSwitchConfiguration.Instance.headNames)
            {
                if (!game.CrewRoster.Exists(name))
                {
                    crew.ChangeName(name);
                    SwitchHead(crew);
                    break;
                }
            }
        }

        private void SwitchHead(ProtoCrewMember crew)
        {
            var pony = HeadSwitchConfiguration.Instance.GetPony(crew.name);
            if (pony == null) return;

            crew.gender = pony.gender;
            crew.courage = pony.courage;
            crew.stupidity = pony.stupidity;
            crew.isBadass = true;

            KerbalRoster.SetExperienceTrait(crew, pony.trait);
        }

        private void setGame(Game g)
        {
            game = g;
            foreach (var crew in game.CrewRoster.Crew)
            {
                SwitchHead(crew);
            }
        }

    }
}
