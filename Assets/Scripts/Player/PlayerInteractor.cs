using UnityEngine;
using World;

namespace Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Interaction Settings")]
        public float interactionRange = 4f;
        public KeyCode interactKey = KeyCode.E;

        [Header("References")]
        public Camera playerCamera;
        public PlayerResonance playerResonance;

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }

            if (playerResonance == null)
            {
                playerResonance = GetComponent<PlayerResonance>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(interactKey))
            {
                TryInteract();
            }
        }

        private void TryInteract()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionRange))
            {
                Collectible3D collectible = hit.collider.GetComponent<Collectible3D>();

                if (collectible != null)
                {
                    bool isInSync = false;

                    if (playerResonance != null)
                    {
                        isInSync = playerResonance.IsInSync;
                    }

                    collectible.Collect(isInSync);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }

            if (playerCamera != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionRange);
            }
        }
    }
}
