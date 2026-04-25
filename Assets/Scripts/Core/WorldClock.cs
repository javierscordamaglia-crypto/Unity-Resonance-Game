using UnityEngine;

namespace Core
{
    /// <summary>
    /// A singleton MonoBehaviour that manages global world time.
    /// Persists across scene loads and provides a centralized time source.
    /// </summary>
    public class WorldClock : MonoBehaviour
    {
        private static WorldClock _instance;
        private static readonly object _lock = new object();

        [Header("Time Settings")]
        [Tooltip("If true, time will advance automatically.")]
        [SerializeField] private bool _autoAdvance = true;

        [Tooltip("Multiplier for time progression. 1 = real-time, 2 = 2x speed, etc.")]
        [SerializeField] private float _timeScale = 1f;

        [Tooltip("Current world time in seconds.")]
        [SerializeField] private float _currentTime = 0f;

        public static WorldClock Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (_lock)
                {
                    if (_instance != null)
                    {
                        return _instance;
                    }

                    GameObject existingObject = GameObject.FindObjectOfType<WorldClock>()?.gameObject;

                    if (existingObject == null)
                    {
                        existingObject = new GameObject("WorldClock");
                        _instance = existingObject.AddComponent<WorldClock>();
                    }
                    else
                    {
                        _instance = existingObject.GetComponent<WorldClock>();
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the clock auto-advances.
        /// </summary>
        public bool AutoAdvance
        {
            get => _autoAdvance;
            set => _autoAdvance = value;
        }

        /// <summary>
        /// Gets or sets the time scale multiplier.
        /// </summary>
        public float TimeScale
        {
            get => _timeScale;
            set => _timeScale = Mathf.Max(0f, value);
        }

        /// <summary>
        /// Gets the current world time in seconds.
        /// </summary>
        public float CurrentTime => _currentTime;

        /// <summary>
        /// Gets the current world time in a formatted HH:MM:SS string.
        /// </summary>
        public string FormattedTime => FormatTime(_currentTime);

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
            if (_autoAdvance)
            {
                _currentTime += Time.deltaTime * _timeScale;
            }
        }

        /// <summary>
        /// Adds time to the world clock.
        /// </summary>
        /// <param name="seconds">Amount of seconds to add.</param>
        public void AddTime(float seconds)
        {
            _currentTime += seconds;
        }

        /// <summary>
        /// Sets the world time to a specific value.
        /// </summary>
        /// <param name="seconds">The new time in seconds.</param>
        public void SetTime(float seconds)
        {
            _currentTime = Mathf.Max(0f, seconds);
        }

        /// <summary>
        /// Resets the world time to zero.
        /// </summary>
        public void ResetTime()
        {
            _currentTime = 0f;
        }

        /// <summary>
        /// Formats seconds into a HH:MM:SS string.
        /// </summary>
        /// <param name="seconds">Time in seconds.</param>
        /// <returns>Formatted time string.</returns>
        public static string FormatTime(float seconds)
        {
            int hours = Mathf.FloorToInt(seconds / 3600f);
            int minutes = Mathf.FloorToInt((seconds % 3600f) / 60f);
            int secs = Mathf.FloorToInt(seconds % 60f);

            return $"{hours:D2}:{minutes:D2}:{secs:D2}";
        }
    }
}
