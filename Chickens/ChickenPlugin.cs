using BepInEx;
using Chickens.Loader;
using System.Collections;
using UnityEngine;

namespace Chickens
{
    [BepInPlugin("me.vaan.chicken", "Chickens", "1.0.0")]
    [BepInDependency("me.vaan.newresources")]
    public class ChickenPlugin : BaseUnityPlugin
    {
        public void Start()
        {
            StartCoroutine(WaitForInitialization());
        }

        private IEnumerator WaitForInitialization()
        {
            while (Singleton<ResourceManager>.Instance == null)
            {
                yield return null;
            }

        X:
            yield return null;
            try {
                Singleton<ResourceManager>.Instance.OnResourceAwake(null);
            } catch {                
                goto X;
            }

            CageLoader.LoadCage();
            yield break;
        }

        public void Update() 
        {
            bool bothMouse = Input.GetMouseButton(0) && Input.GetMouseButton(1);
            Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vector.z = 0;
            if (bothMouse && Input.GetKey(KeyCode.L)) {
                GameObject obj = Instantiate(CageLoader.CagePrefab, vector, Quaternion.identity);
                obj.SetActive(true);
            }
        }
    }
}
