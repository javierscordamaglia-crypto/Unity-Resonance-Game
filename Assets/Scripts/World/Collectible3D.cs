using UnityEngine;
using UI;

namespace World
{
    public class Collectible3D : MonoBehaviour
    {
        [Header("Collectible Settings")]
        public int baseAmount = 10;
        public float rotationSpeed = 50f;
        public float bobHeight = 0.3f;
        public float bobSpeed = 2f;

        private Vector3 startPosition;

        private void Awake()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            // Rotate around Y axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Bob up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        public void Collect(bool isInSync)
        {
            int reward = baseAmount;

            if (isInSync)
            {
                reward *= 2;
                Debug.Log($"[Collectible] Collected with SYNC! Reward: {reward}");
            }
            else
            {
                Debug.Log($"[Collectible] Collected normally. Reward: {reward}");
            }

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowFeedback(isInSync ? $"SYNC BONUS! +{reward}" : $"+{reward}", isInSync);
            }

            Destroy(gameObject);
        }
    }
}
