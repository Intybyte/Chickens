using Chickens.Spin;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Chickens
{
    public static class EggLoader
    {

        public static GameObject EGG_prefab;

        public static void LoadEgg()
        {
            EGG_prefab = new GameObject("Egg");

            Animator animator = EGG_prefab.AddComponent<Animator>();

            Rigidbody rigidbody = EGG_prefab.AddComponent<Rigidbody>();
            EGG_prefab.AddComponent<CustomResource>();
            rigidbody.mass = 1.0f;

            BoxCollider boxCollider = EGG_prefab.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(1, 1, 1);

            Grabable grabable = EGG_prefab.AddComponent<Grabable>();
            EGG_prefab.AddComponent<CustomResource>();
            LoadTexture();

            EGG_prefab.SetActive(false);
        }

        private static void LoadTexture() 
        {
            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Sprite icon = LoadSprite(Path.Combine(assemblyLocation, "Assets", "egg.png"));

            SpriteRenderer renderer = EGG_prefab.AddComponent<SpriteRenderer>();
            renderer.sprite = icon;
        }

        public static Sprite LoadSprite(string filePath)
        {
            if(!File.Exists(filePath))
            {
                Debug.LogError("File not found: " + filePath);
                return null;
            }

            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            bool isLoaded = ImageConversion.LoadImage(texture, fileData);

            if (!isLoaded)
            {
                Debug.LogError("Failed to load texture from file: " + filePath);
                return null;
            }

            var spriteFound = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            if (spriteFound == null)
            {
                Debug.LogError("Error while loading texture from file: " + filePath);
            }

            return spriteFound;            
        }
    }
}
