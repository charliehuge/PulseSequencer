using System;
using UnityEngine;

namespace DerelictComputer
{
    [RequireComponent(typeof(AudioSource))]
    public class SubtractiveSynth : MonoBehaviour
    {
        [Serializable]
        public class Oscillator
        {
            private static readonly double TwoPi;
            private static readonly double SampleRate;

            [Range(20f, 22000f)] public float _frequency = 440f;
            [Range(0f, 1f)] public float _gain = 1f;

            private double _increment;
            private double _phase;

            static Oscillator()
            {
                TwoPi = Math.PI*2;
                SampleRate = AudioSettings.outputSampleRate;
            }

            public float Synthesize()
            {
                _increment = _frequency*TwoPi/SampleRate;

                _phase = _phase + _increment;

                // wrap phase
                if (_phase > TwoPi)
                {
                    _phase -= TwoPi;
                }

                return (float) Math.Sin(_phase)*_gain;
            }
        }

        [SerializeField] private float _gain = 0.05f;
        [SerializeField] private Oscillator[] _oscillators;

        private void OnAudioFilterRead(float[] buffer, int channels)
        {
            if (_oscillators.Length == 0)
            {
                return;
            }

            for (int i = 0; i < buffer.Length; i += channels)
            {
                var sample = 0f;

                foreach (var oscillator in _oscillators)
                {
                    sample += oscillator.Synthesize();
                }

                sample /= _oscillators.Length;

                sample *= _gain;

                for (int j = 0; j < channels; j++)
                {
                    buffer[i + j] = sample;
                }
            }
        }

    }
}