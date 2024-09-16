using System.Collections;
using UnityEngine;
using TMPro;

namespace Chickens
{
    public class CageComponent : MonoBehaviour
    {
        public int amount = 0;
        public static int maxAmount = 4;
        public TextMeshProUGUI _amountHoverText;
        private Grabable _grabable;
        private Rigidbody rb;
        private Collider collider;

        public ResourceData Resource
        {
            get
            {
                return new ResourceData() { 
                    Type = ResourceType.Food,
                    Amount = 3 * amount
                };
            }
        }

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            _grabable = GetComponent<Grabable>();
            collider = GetComponent<Collider>();
            StartCoroutine(SpawnEggs());
        }

        public void Update()
        {
            UpdateRotation();
            UpdateShow();
            UpdateGround();            
        }

        public void UpdateRotation()
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.Rotate(0, currentRotation.y, 0);
        }

        public void UpdateShow() 
        {
            bool flag = ShouldShowAmount();
            _amountHoverText.gameObject.SetActive(flag);

            if (!flag)
            {
                return;
            }

            _amountHoverText.text = this.amount + "/" + maxAmount;
        }

        public void UpdateGround()
        {
            if (collider.ComputeMin().y >= 0f)
            {
                return;
            }
            float num = base.transform.GetY() - collider.ComputeMin().y;
            base.transform.SetY(num);
        }

        private bool ShouldShowAmount()
        {
            PlayerController instance = Singleton<PlayerController>.Instance;
            if (instance.AltActive)
            {
                return true;
            }

            if (instance.TargetedObject == this._grabable.Clickable)
            {
                return true;
            }
            return false;
        }

        private IEnumerator SpawnEggs() { 
            for(;;)
            {
                yield return new WaitForSeconds(2f);
                if (this.amount != 0)
                {
                    Singleton<ResourceManager>.Instance.SpawnItem(Resource);
                }
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (amount >= maxAmount)
            {
                return;
            }

            var bird = collider.GetComponent<BirdScript>();
            if (bird == null)
            {
                return;
            }            

            var seed = bird.GetComponent<ResourceStock>();
            if (seed != null && seed.Available > 0)
            {
                seed.Drop();
            }
            Destroy(bird.gameObject);
            amount++;
        }
    }
}
