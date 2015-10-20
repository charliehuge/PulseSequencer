using System;
using UnityEngine;

namespace DerelictComputer
{
    /// <summary>
    /// A thing that does something when a pattern triggers steps
    /// </summary>
    public abstract class PatternFollower : MonoBehaviour 
    {
        [SerializeField] private Pattern _pattern;

        public bool Suspended { get; set; }

        public virtual void Reset()
        {
            // empty
        }

        private void OnEnable()
        {
            if (_pattern == null)
            {
                Debug.LogWarning("I think you forgot to assign a pattern to " + name);
                return;
            }

            _pattern.StepTriggered += OnStepTriggered;
            _pattern.DidReset += OnPatternDidReset;

            Reset();
        }

        private void OnDisable()
        {
            if (_pattern == null)
            {
                return;
            }

            _pattern.StepTriggered -= OnStepTriggered;
            _pattern.DidReset -= OnPatternDidReset;
        }

        protected abstract void OnStepTriggered (int stepIndex, double pulseTime);

        private void OnPatternDidReset()
        {
            Reset();
        }
    }
}