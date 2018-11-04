using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalHeadSwitch
{

    public class Head
    {
        
        public string name;
        //public PonyPart head, mane, eyes, horn;
        public string trait = "Pilot";
        //public float courage = 0.5f, stupidity = 0.5f, scale = 1.0f;
        public ProtoCrewMember.Gender gender;
        public Mesh mesh;
        public Material material;

        public Head(Mesh mesh, Material material, string name)
        {
            this.name = name;
            this.mesh = mesh;
            this.material = material;
        }
        public Head(string name)
        {
            this.name = name;
        }
    }

    public class HeadConfigs
    {
        Head head;
        public static HeadConfigs instance = null;
        private Dictionary<string, Head> heads = new Dictionary<string, Head>();
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private Matrix4x4 bindpose;
        private bool stop;

        public IEnumerable<string> HeadNames { get { return heads.Keys; } }

        public HeadConfigs()
        {
        }

        void Start()
        {
            //Get nodes
            ConfigNode data = null;
            foreach (ConfigNode node in GameDatabase.Instance.GetConfigNodes("KERBALHEADSWITCH"))
            {
                data = node;
                foreach (ConfigNode headNode in GameDatabase.Instance.GetConfigNodes("HEAD"))
                {
                    data = headNode;
                }
            }
            

            if (data.HasValue("name"))
            {
                head.name = data.name;
            }
            else
            {
                Debug.LogError("[KerbalHeadSwitch]: Each head must have a name. Shutting down.");
                stop = true;
                return;
            }
            if (data.HasValue())
            {

            }
            ShutdownCheck(stop);
        }
        
        private void ShutdownCheck(bool shutdown)
        {
            if (shutdown)
            {
                return;
            }
        }
        private Head CreateHead(string name)
        {
            var _head = new Head();
            _head.name = name;
            return heads[name] = _head;
        }

        public void Load()
        {
            Head head;
            FillTexturesDict();

            bindpose = GetBindPose();
            var ponyHead = LoadMesh("Head");
            var eyes = LoadMesh("Eyes");
            var ponyHorn = LoadMesh("Horn");
            var rainbowMane = LoadMesh("RainbowMane");
            var sweetMane = LoadMesh("BonbonMane");
            var trixieMane = LoadMesh("TrixieMane");
            var twilightMane = LoadMesh("TwilightMane");
            var lyraMane = LoadMesh("LyraMane");
            var applejackMane = LoadMesh("ApplejackMane");

            var bodyTexture = LoadTexture(TEX_DIR + "body.png");
            var hornTexture = LoadTexture(TEX_DIR + "horn.png");

            float foalScale = 0.75f;

            {
                head = CreateHead("Twilight Sparkle");
                var twilightCoatColor = new Color(199 / 255f, 157 / 255f, 215 / 255f);
                var twilightManeColor = new Color(54 / 255f, 59 / 255f, 116 / 255f);
                head.trait = "Scientist";
            }

        }

        private static Matrix4x4 GetBindPose()
        {
            if (_head.gender = ProtoCrewMember.Gender.Female)
            {
                var eva = PartLoader.getPartInfoByName("kerbalEVAfemale").partPrefab.gameObject;
            }
            foreach (SkinnedMeshRenderer smr in eva.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                switch (smr.name)
                {
                    case "headMesh01":
                    case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_polySurface51":
                    case "headMesh":
                        int i = 0;
                        foreach (var bone in smr.bones)
                        {
                            if (bone.name == "bn_upperJaw01")
                            {
                                return smr.sharedMesh.bindposes[i];
                            }
                            i++;
                        }
                        break;
                }
            }
            System.Diagnostics.Debug.Assert(false, "GetBindPose failed");
            return new Matrix4x4();
        }

        private Mesh CreateMesh(Vector3[] vertices, Vector2[] texcoords, int[] triangles)
        {
            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = texcoords;
            BoneWeight[] weights = new BoneWeight[mesh.vertices.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i].boneIndex0 = 0;
                weights[i].weight0 = 1;
            }
            mesh.boneWeights = weights;
            mesh.bindposes = new Matrix4x4[] { bindpose };
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        private Vector3[] LoadVector3Array(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            using (BinaryReader r = new BinaryReader(fs))
            {
                var a = new Vector3[r.ReadUInt32()];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i].x = r.ReadSingle();
                    a[i].y = r.ReadSingle();
                    a[i].z = r.ReadSingle();
                }
                return a;
            }
        }

        private Vector2[] LoadVector2Array(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            using (BinaryReader r = new BinaryReader(fs))
            {
                var a = new Vector2[r.ReadUInt32()];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i].x = r.ReadSingle();
                    a[i].y = r.ReadSingle();
                }
                return a;
            }
        }

        private int[] LoadIntArray(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            using (BinaryReader r = new BinaryReader(fs))
            {
                var a = new int[r.ReadUInt32()];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = r.ReadInt32();
                }
                return a;
            }
        }

        private Mesh LoadMesh(string name)
        {
            string basepath = MODELS_DIR + name;
            return CreateMesh(
                LoadVector3Array(basepath + ".vtx"),
                LoadVector2Array(basepath + ".tex"),
                LoadIntArray(basepath + ".idx")
            );
        }

        private Material CreateMaterial(Color color)
        {
            var mat = new Material(Shader.Find("Diffuse"));
            mat.color = color;
            return mat;
        }

        private Texture2D LoadTexture(string path)
        {
            path = path.Substring(0, path.LastIndexOf('.'));
            Texture2D tex = null;
            textures.TryGetValue(path, out tex);
            return tex;
        }

        private void FillTexturesDict()
        {
            foreach (var tex in Resources.FindObjectsOfTypeAll<Texture2D>())
            {
                textures[tex.name] = tex;
            }
        }

        private Material CreateMaterial(string path)
        {
            var tex = LoadTexture(path);
            return CreateMaterial(tex);
        }

        private Material CreateMaterial(Texture2D texture)
        {
            return CreateMaterial(texture, Color.white);
        }

        private Material CreateMaterial(Texture2D texture, Color color)
        {
            var mat = new Material(Shader.Find("Diffuse"));
            mat.mainTexture = texture;
            mat.color = color;
            return mat;
        }

        public Head GetPony(string name)
        {
            Head pony = null;
            heads.TryGetValue(name, out pony);
            return pony;
        }
    }
}
