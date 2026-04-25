using UnityEngine;
using Core;

namespace Player
{
    public class PlayerResonance : MonoBehaviour
    {
        [Header("Resonance Settings")]
        [Range(1, 9)]
        public int frequency = 1;

        public int Frequency => frequency;

        public bool IsInSync
        {
            get
            {
                if (WorldClock.Instance == null)
                {
                    return false;
                }
                return frequency == WorldClock.Instance.CurrentDay;
            }
        }

        private void Update()
        {
            // Optional: Debug sync state
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Debug.Log($"[PlayerResonance] Sync State: {IsInSync} (Frequency: {frequency}, Day: {WorldClock.Instance.CurrentDay})");
            }
        }

        public void SetFrequency(int newFrequency)
        {
            frequency = Mathf.Clamp(newFrequency, 1, 9);
        }

        public void CycleFrequencyUp()
        {
            frequency++;
            if (frequency > 9)
            {
                frequency = 1;
            }
            Debug.Log($"[PlayerResonance] Frequency set to: {frequency}");
        }

        public void CycleFrequencyDown()
        {
            frequency--;
            if (frequency < 1)
            {
                frequency = 9;
            }
            Debug.Log($"[PlayerResonance] Frequency set to: {frequency}");
        }
    }
}
