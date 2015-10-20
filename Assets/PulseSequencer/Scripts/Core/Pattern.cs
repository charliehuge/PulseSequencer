using UnityEngine;
using System;
using System.Collections.Generic;

namespace DerelictComputer
{
    /// <summary>
    /// Delegate for when an enabled step triggers
    /// </summary>
    /// <param name="stepIndex">The index of the step in the pattern</param>
    /// <param name="pulseTime">The DSP time of the pulse that triggered the step</param>
    public delegate void PatternStepDelegate(int stepIndex, double pulseTime);

    /// <summary>
    /// A collection of steps that respond to a pulse
    /// </summary>
    public class Pattern : MonoBehaviour 
    {
        [Serializable]
        public class StepInfo
        {
            public bool Active = false;
        }

        public event Action DidReset;

        public event PatternStepDelegate StepTriggered;

        [HideInInspector] public List<StepInfo> Steps = new List<StepInfo>();

        [SerializeField] private Pulse _pulse;

        private int _currentStep;

        public void Reset()
        {
            _currentStep = 0;

            if (DidReset != null)
            {
                DidReset();
            }
        }

        private void OnEnable()
        {
            if (_pulse == null)
            {
                Debug.LogWarning("I think you forgot to assign a pulse. Fix that or I'm not doing anything. Nuh-uh.");
                return;
            }

            _pulse.Triggered += OnPulseTriggered;
            _pulse.DidReset += OnPulseDidReset;

            Reset();
        }

        private void OnDisable()
        {
            if (_pulse == null)
            {
                return;
            }

            _pulse.Triggered -= OnPulseTriggered;
            _pulse.DidReset -= OnPulseDidReset;
        }

        private void OnPulseDidReset()
        {
            Reset();
        }

        private void OnPulseTriggered (double pulseTime)
        {
            if (Steps.Count == 0)
            {
                return;
            }

		
            if (Steps[_currentStep].Active && StepTriggered != null)
            {
                StepTriggered(_currentStep, pulseTime);
            }
		
            _currentStep = (_currentStep + 1)%Steps.Count;
        }
    }
}