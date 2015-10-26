using System;
using UnityEngine;

namespace DerelictComputer
{
    [RequireComponent(typeof(AudioSource))]
    public class SubtractiveSynth : MonoBehaviour
    {
        [Serializable]
        public class Envelope
        {
            private static readonly double SampleRate;

            [SerializeField] private double _attackTime = 0.0;
            [SerializeField] private double _releaseTime = 0.0;

            static Envelope()
            {
                SampleRate = AudioSettings.outputSampleRate;
            }

            public bool GetGain(out double outGain, uint elapsedSamples)
            {
                var time = elapsedSamples / SampleRate;

                if (_attackTime > 0.0 && time < _attackTime)
                {
                    outGain = time/_attackTime;
                    return true;
                }
                if (_releaseTime > 0.0 && time < _attackTime + _releaseTime)
                {
                    outGain = 1.0 - (time - _attackTime)/_releaseTime;
                    return true;
                }

                outGain = 0f;
                return false;
            }
        }

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

            [SerializeField] private WaveformType _waveform = WaveformType.Sine;
            [SerializeField] private double _gain = 1;
            [SerializeField] private double _detuneSemitones = 0;
            [SerializeField] private double _pulseWidth = 0.5;
            [SerializeField] private Envelope _envelope;

            private readonly System.Random _random = new System.Random();

            static Oscillator()
            {
                TwoPi = Math.PI*2;
                SampleRate = AudioSettings.outputSampleRate;
            }

            public bool Synthesize(out float outSample, double frequency, uint elapsedSamples)
            {
                var phase = elapsedSamples*frequency*MusicMathUtils.SemitonesToPitch(_detuneSemitones)*TwoPi/SampleRate;

                // wrap phase
                while (phase > TwoPi)
                {
                    phase -= TwoPi;
                }

                double sample;

                switch (_waveform)
                {
                    case WaveformType.Sine:
                        sample = Math.Sin(phase);
                        break;
                    case WaveformType.Saw:
                        sample = ((phase / TwoPi) - 0.5)*2.0;
                        break;
                    case WaveformType.Square:
                        sample = (phase / TwoPi > _pulseWidth ? 1.0 : -1.0);
                        break;
                    case WaveformType.Noise:
                        sample = (_random.NextDouble() - 0.5)*2.0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                double envelopeGain;

                if (!_envelope.GetGain(out envelopeGain, elapsedSamples))
                {
                    outSample = 0f;
                    return false;
                }

                sample *= _gain*envelopeGain;

                outSample = (float) sample;
                return true;
            }
        }

        public bool DebugPlayNote;

        [SerializeField, Range(0f, 1f)] private float _gain = 0.05f;
        [SerializeField] private Oscillator[] _oscillators;

        private double _frequency;
        private uint _elapsedSamples;
        private bool _playing;

        public void Play(int midiNote)
        {
            _frequency = MusicMathUtils.MidiNoteToFrequency(midiNote);
            _elapsedSamples = 0;
            _playing = true;
        }

        private void Update()
        {
            if (DebugPlayNote)
            {
                Play(UnityEngine.Random.Range(0, 127));
                DebugPlayNote = false;
            }
        }

        private void OnAudioFilterRead(float[] buffer, int channels)
        {
            if (!_playing)
            {
                return;
            }

            if (_oscillators.Length == 0)
            {
                return;
            }


            for (int i = 0; i < buffer.Length; i += channels)
            {
                if (_playing)
                {
                    var sample = 0f;
                    var numOscillatorsFinished = 0;

                    foreach (var oscillator in _oscillators)
                    {
                        float oscSample;

                        if (!oscillator.Synthesize(out oscSample, _frequency, _elapsedSamples))
                        {
                            numOscillatorsFinished++;
                        }
                        else
                        {
                            sample += oscSample;
                        }
                    }

                    sample = (sample*_gain)/_oscillators.Length;

                    for (int j = 0; j < channels; j++)
                    {
                        buffer[i + j] = sample;
                    }

                    _playing = numOscillatorsFinished < _oscillators.Length;
                }

                _elapsedSamples++;
            }

        }

    }
}