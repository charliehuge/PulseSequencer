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

        private const double PeriodMinimum = 0.016; // really don't want this to be any shorter than a frame at 60FPS

        private const double BpmMinimum = 0.1; // so it doesn't go zero of negative, but really, do you ever want one beat every 10 minutes?

        /// <summary>
        /// Period in seconds
        /// </summary>
        [SerializeField, HideInInspector] private double _period = 1d;

        /// <summary>
        /// How many pulses per beat? This is only used for the inspector, but is serialized so we can keep it around.
        /// </summary>
        [SerializeField, HideInInspector] private uint _pulsesPerBeat = 4;

        /// <summary>
        /// How much time should we look ahead?
        /// </summary>
        [SerializeField] private double _latency = 0.1;

        private double _nextPulseTime;

        public double Period
        {
            get { return _period; }
            set {
                _period = value < PeriodMinimum ? PeriodMinimum : value;
            }
        }

        public uint PulsesPerBeat
        {
            get { return _pulsesPerBeat; }
        }

        public void Reset()
        {
            _nextPulseTime = AudioSettings.dspTime;

            if (DidReset != null)
            {
                DidReset();
            }
        }

        public double GetBpm()
        {
            return 60.0/(Period*_pulsesPerBeat);
        }

        public void SetBpm(double bpm, uint pulsesPerBeat)
        {
            if (bpm < BpmMinimum)
            {
                bpm = BpmMinimum;
            }

            Period = 60.0/(bpm*pulsesPerBeat);
            _pulsesPerBeat = pulsesPerBeat;
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