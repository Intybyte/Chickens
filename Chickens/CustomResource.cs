using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using MelenitasDev.SoundsGood;

namespace Chickens.Spin
{
    public class CustomResource : MonoBehaviour
    {
        private Rigidbody rb;
        private Grabable grabable;
        private static MethodInfo targetLookup;
        private static PlayerController controller;

        public Grabable Grabable { get => grabable; }

        private static void LoadReflection()
        {
            if (controller == null)
            {
                controller = Singleton<PlayerController>.Instance;
                targetLookup = controller.GetType().GetMethod("GetTargetComponent", BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

        public void Awake()
        {
            LoadReflection();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            grabable = GetComponent<Grabable>();
            Grabable.playerDropEvent += OnPlayerDrop;
        }

        public void Update() 
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.Rotate(0, currentRotation.y, 0);
        }

        private void OnPlayerDrop()
        {
            PlayerController controller = Singleton<PlayerController>.Instance;
            ResourceData data = new ResourceData() { 
                Type = ResourceType.Food,
                Amount = 3
            };

            var buildingMethod = targetLookup.MakeGenericMethod(new Type[] { typeof(Building) });
            Building building = (Building) buildingMethod.Invoke(controller, null);
            if (building != null && building.GetRemainingResources() != null && building.GetRemainingResources().Any((ResourceData r) => r.Type == data.Type && r.Amount > 0)) {
                building.AddResource(data);
                Singleton<DestroyManager>.Instance.DestroyClean(this.gameObject);
                return;
            }

            var buildingStorageMethod = targetLookup.MakeGenericMethod(new Type[] { typeof(BuildingStorage) });
            BuildingStorage buildingStorage = (BuildingStorage) buildingStorageMethod.Invoke(controller, null);
            if (buildingStorage != null)
            {
                AddResourceToStorage(data, buildingStorage);
                return;
            }
        }

        private void AddResourceToStorage(ResourceData item, BuildingStorage storage)
        {
            if (!storage.IsUsable())
            {
                return;
            }

            Singleton<InventoryManager>.Instance.Add(item);
            Vector3 position = base.transform.position;
            position.y = 3f;
            FeedbackUtil.SpawnResourceFeedback(position, item);
            Singleton<DestroyManager>.Instance.DestroyClean(gameObject);
            var sfx = new Sound(SFX.InventoryAdd);
            sfx.SetRandomClip(true).SetPosition(base.transform.position).SetRandomPitch(new Vector2(1f, 1.15f))
                .Play(0f);
        }
    }
}
