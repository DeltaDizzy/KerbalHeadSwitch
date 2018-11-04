using System;
using UnityEngine;

namespace KerbalHeadSwitch
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class SwitchAddon : MonoBehaviour
    {
        private bool isInitialized = false;
        private Game game;

        public void Start()
        {
            HeadConfigs.instance = new HeadConfigs();
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

                HeadConfigs.instance.Load();
            }
        }

        private void onKerbalAdd(ProtoCrewMember crew)
        {
            if (crew.type != ProtoCrewMember.KerbalType.Applicant) return;
            if (crew.name.GetHashCode() % 3 != 0) return;

            foreach (var name in HeadConfigs.instance.HeadNames)
            {
                if (!game.CrewRoster.Exists(name))
                {
                    crew.ChangeName(name);
                    PonifyCrewMember(crew);
                    break;
                }
            }
        }

        private void PonifyCrewMember(ProtoCrewMember crew)
        {
            var pony = HeadConfigs.instance.GetPony(crew.name);
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
                PonifyCrewMember(crew);
            }
        }

    }
}
