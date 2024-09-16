using System.IO;
using System.Reflection;
using UnityEngine;

namespace Chickens.Utils
{
    public static class ImageUtils
    {
        public static SpriteRenderer LoadRenderer(GameObject obj, string fileName)
        {
            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Sprite icon = LoadSprite(Path.Combine(assemblyLocation, "Assets", fileName));

            float targetWidth = InWorldUnits(icon.pixelsPerUnit, 128f);
            float targetHeight = InWorldUnits(icon.pixelsPerUnit, 128f);
            Vector2 spriteSize = icon.bounds.size;

            SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = icon;
            renderer.transform.localScale = new Vector3(targetWidth / spriteSize.x, targetHeight / spriteSize.y, 1f);
            return renderer;
        }

        public static float InWorldUnits(float pixelPerUnit, float selectedPixel)
        {
            return selectedPixel / pixelPerUnit;
        }

        public static Sprite LoadSprite(string filePath)
        {
            if (!File.Exists(filePath))
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
