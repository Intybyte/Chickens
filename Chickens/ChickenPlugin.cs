﻿using BepInEx;
using System.Collections;
using System.Reflection;
using Unity.Collections;
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
       
            EggLoader.LoadEgg();
            yield break;
        }

        public void Update() 
        {
            if(Input.GetMouseButton(0) && Input.GetMouseButton(1) && Input.GetKey(KeyCode.K)) 
            {
                Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vector.z = 0;
                GameObject obj = Instantiate(EggLoader.EGG_prefab, vector, Quaternion.identity);
                obj.SetActive(true);
            }
        }
    }
}
