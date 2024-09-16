using Chickens.Utils;
using System.Security.Policy;
using TMPro;
using UnityEngine;

namespace Chickens.Loader
{
    public static class CageLoader
    {
        private static GameObject __cage_prefab;
        public static GameObject CagePrefab {  get { return __cage_prefab; } }

        private static float squareSize;

        public static void LoadCage()
        {
            __cage_prefab = new GameObject("Cage");
            __cage_prefab.AddComponent<Animator>();

            var image = ImageUtils.LoadRenderer(__cage_prefab, "cage.png");
            squareSize = ImageUtils.InWorldUnits(image.sprite.pixelsPerUnit, 128f);

            Rigidbody rigidbody = __cage_prefab.AddComponent<Rigidbody>();
            rigidbody.mass = 1.0f;
            rigidbody.isKinematic = false;

            BoxCollider boxCollider = __cage_prefab.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(1, 1, 1);
            Grabable grabable = __cage_prefab.AddComponent<Grabable>();
            grabable.Clickable.Transform.localScale = new Vector2 (squareSize, squareSize);


            TextMeshProUGUI text = SetupText(image);
            CageComponent cage = __cage_prefab.AddComponent<CageComponent>();
            cage._amountHoverText = text;

            __cage_prefab.SetActive(false);
        }

        private static TextMeshProUGUI SetupText(SpriteRenderer image)
        {
            GameObject canvasObject = new GameObject("TextCanvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            RectTransform canvasTrans = canvasObject.GetComponent<RectTransform>();
            canvasTrans.sizeDelta = new Vector2(squareSize, squareSize);
            canvasObject.transform.SetParent(__cage_prefab.transform);
            canvasObject.transform.localPosition = new Vector3(0, 1, 0);

            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(canvasObject.transform);
            TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.fontSize = 0.7f;
            textComponent.enableWordWrapping = false;
            textComponent.alignment = TextAlignmentOptions.Center;
            textObject.transform.localPosition = new Vector3(0, 1, 0);

            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(squareSize, squareSize);

            return textComponent;
        }

        private static void AdjustPivot(Sprite sprite, BoxCollider collider)
        {
            float spriteHeight = sprite.rect.height / sprite.pixelsPerUnit;
            float spriteWidth = sprite.rect.width / sprite.pixelsPerUnit;
            Vector2 pivot = sprite.pivot / sprite.pixelsPerUnit;

            collider.center = new Vector2(pivot.x - (spriteWidth / 2), pivot.y - (spriteHeight / 2));
        }

    }
}
