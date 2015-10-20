using UnityEngine;
using System;

namespace DerelictComputer
{
    /// <summary>
    /// The heartbeat of a Pulse Sequencer. 
    /// Triggers a pulse/tick/bang/whatever you call it at a specified interval.
    /// </summary>
    public class Pulse : MonoBehaviour
    {
        public event Action DidReset;

        public event Action<double> Triggered;

        /// <summary>
        /// Period in seconds
        /// </summary>
        [HideInInspector] public double Period = 1d;

        /// <summary>
        /// How many pulses per beat? This is only used for the inspector, but is serialized so we can keep it around.
        /// </summary>
        [HideInInspector] public int PulsesPerBeat = 4;

        /// <summary>
        /// How much time should we look ahead?
        /// </summary>
        [SerializeField] private double _latency = 0.1;

        private double _nextPulseTime;

        public void Reset()
        {
            _nextPulseTime = AudioSettings.dspTime;

            if (DidReset != null)
            {
                DidReset();
            }
        }

        private void Awake()
        {
            Reset();
        }

        private void Update()
        {
            while (AudioSettings.dspTime + _latency > _nextPulseTime)
            {
                var thisPulseTime = _nextPulseTime;

                _nextPulseTime += Period;

                if (Triggered != null)
                {
                    Triggered(thisPulseTime);
                }
            }
        }
    }
}