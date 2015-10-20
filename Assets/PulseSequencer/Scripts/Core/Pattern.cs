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
        public enum FollowType
        {
            Pulse,
            Pattern
        }

        [Serializable]
        public class StepInfo
        {
            public bool Active = false;
        }

        public event Action DidReset;

        public event PatternStepDelegate StepTriggered;

        [HideInInspector] public List<StepInfo> Steps = new List<StepInfo>();

        [SerializeField] private FollowType _followType = FollowType.Pulse;

        [SerializeField] private Pulse _pulse;

        [SerializeField] private Pattern _pattern;

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
            switch (_followType)
            {
                case FollowType.Pulse:
                    if (_pulse == null)
                    {
                        Debug.LogWarning("I think you forgot to assign a pulse. Fix that or nothing will happen.");
                        return;
                    }

                    _pulse.Triggered += OnPulseTriggered;
                    _pulse.DidReset += OnDidReset;
                    break;
                case FollowType.Pattern:
                    if (_pattern == null)
                    {
                        Debug.LogWarning("I think you forgot to assign a pattern. Fix that or nothing will happen.");
                        return;
                    }

                    _pattern.StepTriggered += OnPatternStepTriggered;
                    _pattern.DidReset += OnDidReset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Reset();
        }

        private void OnDisable()
        {
            switch (_followType)
            {
                case FollowType.Pulse:
                    if (_pulse == null)
                    {
                        return;
                    }

                    _pulse.Triggered -= OnPulseTriggered;
                    _pulse.DidReset -= OnDidReset;
                    break;
                case FollowType.Pattern:
                    if (_pattern == null)
                    {
                        return;
                    }

                    _pattern.StepTriggered -= OnPatternStepTriggered;
                    _pattern.DidReset -= OnDidReset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void OnDidReset()
        {
            Reset();
        }

        private void OnPulseTriggered (double pulseTime)
        {
            Step(pulseTime);
        }

        private void OnPatternStepTriggered(int stepIndex, double pulseTime)
        {
            Step(pulseTime);
        }

        private void Step(double pulseTime)
        {
            if (Steps.Count == 0)
            {
                return;
            }


            if (Steps[_currentStep].Active && StepTriggered != null)
            {
                StepTriggered(_currentStep, pulseTime);
            }

            _currentStep = (_currentStep + 1) % Steps.Count;
        }
    }
}