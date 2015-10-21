using UnityEngine;

namespace DerelictComputer
{
    public class PatternFollowerGroup : MonoBehaviour
    {
        public bool Suspended;

        private PatternFollower[] _patternFollowers;

        private bool _lastSuspendState;

        public void Reset()
        {
            foreach (var patternFollower in _patternFollowers)
            {
                patternFollower.Reset();
            }
        }

        private void Awake()
        {
            _patternFollowers = GetComponentsInChildren<PatternFollower>();
            UpdateSuspendedState(true);
        }

        private void Update()
        {
            UpdateSuspendedState();
        }

        private void UpdateSuspendedState(bool force = false)
        {
            if (!force && Suspended == _lastSuspendState)
            {
                return;
            }

            foreach (var patternFollower in _patternFollowers)
            {
                patternFollower.Suspended = Suspended;
            }

            _lastSuspendState = Suspended;
        }
    }
}