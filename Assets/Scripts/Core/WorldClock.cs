using UnityEngine;

namespace Core
{
    public class WorldClock : MonoBehaviour
    {
        private static WorldClock _instance;
        public static WorldClock Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<WorldClock>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("WorldClock");
                        _instance = singletonObject.AddComponent<WorldClock>();
                    }
                }
                return _instance;
            }
        }

        [Header("Day Cycle Settings")]
        public float dayDurationSeconds = 30f;
        public int MaxDay = 9;

        private int currentDay = 1;
        private float dayTimer = 0f;

        public int CurrentDay => currentDay;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            dayTimer += Time.deltaTime;

            if (dayTimer >= dayDurationSeconds)
            {
                dayTimer = 0f;
                AdvanceDay();
            }
        }

        private void AdvanceDay()
        {
            currentDay++;

            if (currentDay > MaxDay)
            {
                currentDay = 1;
            }

            Debug.Log($"[WorldClock] Day changed to: {currentDay}");
        }

        public void ForceAdvanceDay()
        {
            AdvanceDay();
            dayTimer = 0f;
        }

        public void SetDay(int day)
        {
            currentDay = Mathf.Clamp(day, 1, MaxDay);
            dayTimer = 0f;
            Debug.Log($"[WorldClock] Day set to: {currentDay}");
        }

        public float GetDayProgress()
        {
            return dayTimer / dayDurationSeconds;
        }
    }
}
