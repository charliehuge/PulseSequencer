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
        public event Action<double> Triggered;

        /// <summary>
        /// Period in seconds
        /// </summary>
        [SerializeField] private double _period = 1d;

        /// <summary>
        /// How much time should we look ahead?
        /// </summary>
        [SerializeField] private double _latency = 0.1;

        private double _nextPulseTime;

        private void OnEnable()
        {
            _nextPulseTime = AudioSettings.dspTime + _period;
        }

        private void Update()
        {
            if (AudioSettings.dspTime + _latency > _nextPulseTime)
            {
                var thisPulseTime = _nextPulseTime;

                _nextPulseTime += _period;

                if (Triggered != null)
                {
                    Triggered(thisPulseTime);
                }
            }
        }
    }
}