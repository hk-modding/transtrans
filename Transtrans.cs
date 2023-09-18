using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Modding;
using SeanprCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Transtrans
{
    public class Transtrans : Mod
    {
        private Texture2D _tex;

        public override void Initialize()
        {
            USceneManager.activeSceneChanged += SceneChanged;

            Assembly asm = Assembly.GetExecutingAssembly();
            
            foreach (string res in asm.GetManifestResourceNames())
            {
                using (Stream s = asm.GetManifestResourceStream(res))
                {
                    if (s == null) continue;

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    
                    _tex = new Texture2D(2, 2);

                    _tex.LoadImage(buffer, true);
                }
            }
        }

        private void SceneChanged(Scene arg0, Scene arg1)
        {
            GameManager.instance.StartCoroutine(SetSprite());
        }

        private IEnumerator SetSprite()
        {
            yield return null;
            
            // Finding just HealthManagers and going up is faster I think
            foreach (GameObject go in Object.FindObjectsOfType<HealthManager>().Select(x => x.gameObject).Where(x => x.gameObject.name.Contains("Climber")))
            {
                go.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = _tex;
            }
        }
    }
}