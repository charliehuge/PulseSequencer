using System;
using UnityEngine;
using Random = System.Random;

namespace DerelictComputer
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

        [SerializeField] private WaveformType _waveform = WaveformType.Sine;
        [SerializeField] private double _gain = 1;
        [SerializeField] private double _detuneSemitones = 0;
        [SerializeField] private double _pulseWidth = 0.5;

        private readonly Random _random = new Random();

        private double _phase;

        static Oscillator()
        {
            TwoPi = Math.PI * 2;
        }

        public void Trigger()
        {
            _phase = 0.0;
        }

        public double Synthesize(double frequency, int sampleRate)
        {
            _phase += frequency * MusicMathUtils.SemitonesToPitch(_detuneSemitones) * TwoPi / sampleRate;

            if (_phase > TwoPi)
            {
                _phase -= TwoPi;
            }

            double sample;

            switch (_waveform)
            {
                case WaveformType.Sine:
                    sample = Math.Sin(_phase);
                    break;
                case WaveformType.Saw:
                    sample = 2 * _phase / TwoPi - 1;
                    break;
                case WaveformType.Square:
                    sample = (_phase / TwoPi > _pulseWidth ? 1.0 : -1.0);
                    break;
                case WaveformType.Noise:
                    sample = 2 * _random.NextDouble() - 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return sample*_gain;
        }
    }
}