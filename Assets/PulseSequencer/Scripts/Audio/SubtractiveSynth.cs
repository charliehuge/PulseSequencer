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
            public enum WaveformType
            {
                Sine,
                Saw,
                Square,
                Noise
            }

            private static readonly double TwoPi;
            private static readonly double SampleRate;

            public WaveformType Waveform = WaveformType.Sine;
            [Range(20f, 22000f)] public float Frequency = 440f;
            [Range(0f, 1f)] public float Gain = 1f;
            [Range(0.1f, 0.9f)] public float PulseWidth = 0.5f;

            private readonly System.Random _random = new System.Random();

            private double _increment;
            private double _phase;

            static Oscillator()
            {
                TwoPi = Math.PI*2;
                SampleRate = AudioSettings.outputSampleRate;
            }

            public float Synthesize()
            {
                _increment = Frequency*TwoPi/SampleRate;

                _phase = _phase + _increment;

                // wrap phase
                if (_phase > TwoPi)
                {
                    _phase -= TwoPi;
                }

                switch (Waveform)
                {
                    case WaveformType.Sine:
                        return (float) Math.Sin(_phase)*Gain;
                    case WaveformType.Saw:
                        return Mathf.Lerp(-1f, 1f, (float) (_phase/TwoPi)*Gain);
                    case WaveformType.Square:
                        return (_phase/TwoPi > PulseWidth ? 1f : -1f)*Gain;
                    case WaveformType.Noise:
                        return ((float) _random.NextDouble() - 0.5f)*2*Gain;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }

        [SerializeField, Range(0f, 1f)] private float _gain = 0.05f;
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