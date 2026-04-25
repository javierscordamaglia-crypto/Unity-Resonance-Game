using UnityEngine;
using UnityEngine.UI;
using Core;
using Player;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<UIManager>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("UIManager");
                        _instance = singletonObject.AddComponent<UIManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("UI References")]
        public Text dayText;
        public Text frequencyText;
        public Text syncStateText;
        public Text feedbackText;

        [Header("Feedback Settings")]
        public float feedbackDisplayTime = 2f;

        private Canvas canvas;
        private float feedbackTimer = 0f;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            SetupCanvas();
        }

        private void SetupCanvas()
        {
            canvas = GetComponent<Canvas>();

            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            if (GetComponent<CanvasScaler>() == null)
            {
                gameObject.AddComponent<CanvasScaler>();
            }

            if (GetComponent<GraphicRaycaster>() == null)
            {
                gameObject.AddComponent<GraphicRaycaster>();
            }
        }

        private void Update()
        {
            UpdateHUD();
            UpdateFeedback();
        }

        private void UpdateHUD()
        {
            int currentDay = 1;
            int playerFrequency = 1;
            bool isInSync = false;

            if (WorldClock.Instance != null)
            {
                currentDay = WorldClock.Instance.CurrentDay;
            }

            PlayerResonance playerResonance = FindObjectOfType<PlayerResonance>();
            if (playerResonance != null)
            {
                playerFrequency = playerResonance.Frequency;
                isInSync = playerResonance.IsInSync;
            }

            if (dayText != null)
            {
                dayText.text = $"Day: {currentDay}/9";
            }

            if (frequencyText != null)
            {
                frequencyText.text = $"Frequency: {playerFrequency}";
            }

            if (syncStateText != null)
            {
                syncStateText.text = isInSync ? "Status: SYNCHRONIZED" : "Status: OUT OF SYNC";
                syncStateText.color = isInSync ? Color.green : Color.red;
            }
        }

        private void UpdateFeedback()
        {
            if (feedbackTimer > 0f)
            {
                feedbackTimer -= Time.deltaTime;

                if (feedbackTimer <= 0f)
                {
                    HideFeedback();
                }
            }
        }

        public void ShowFeedback(string message, bool isPositive)
        {
            if (feedbackText != null)
            {
                feedbackText.text = message;
                feedbackText.color = isPositive ? Color.yellow : Color.white;
                feedbackText.enabled = true;
                feedbackTimer = feedbackDisplayTime;
            }
        }

        private void HideFeedback()
        {
            if (feedbackText != null)
            {
                feedbackText.enabled = false;
            }
        }

        #region Editor Helpers

        [ContextMenu("Create Default UI")]
        private void CreateDefaultUI()
        {
            // This helps setup UI elements in the editor
            Debug.Log("[UIManager] Use Unity's UI system to create Text elements and assign them to this component.");
        }

        #endregion
    }
}
