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
            public enum Stage
            {
                NotStarted,
                Attack,
                Release,
            }

            [SerializeField] private double _attackTime = 0.0;
            [SerializeField] private double _releaseTime = 0.0;

            private double _attackGainPerSample;
            private double _releaseGainPerSample;
            private double _gain;

            public Stage CurrentStage { get; private set; }

            public void Init()
            {
                _attackGainPerSample = _attackTime > 0 ? 1/(_attackTime*AudioSettings.outputSampleRate) : 1;
                _releaseGainPerSample = _releaseTime > 0 ? -1/(_releaseTime*AudioSettings.outputSampleRate) : -1;
                _gain = 0;
                CurrentStage = Stage.Attack;
            }

            public double GetGain()
            {
                switch (CurrentStage)
                {
                    case Stage.NotStarted:
                        return 0;
                    case Stage.Attack:
                        _gain += _attackGainPerSample;
                        if (_gain > 1)
                        {
                            CurrentStage = Stage.Release;
                            return 1;
                        }
                        return _gain;
                    case Stage.Release:
                        _gain += _releaseGainPerSample;
                        if (_gain < 0)
                        {
                            CurrentStage = Stage.NotStarted;
                            return 0;
                        }
                        return _gain;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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

            [SerializeField] private WaveformType _waveform = WaveformType.Sine;
            [SerializeField] private double _gain = 1;
            [SerializeField] private double _detuneSemitones = 0;
            [SerializeField] private double _pulseWidth = 0.5;
            [SerializeField] private Envelope _envelope;

            private readonly System.Random _random = new System.Random();

            private double _phasePerSample;
            private double _phase;

            static Oscillator()
            {
                TwoPi = Math.PI*2;
            }

            public void Init(double frequency)
            {
                _phasePerSample = frequency*MusicMathUtils.SemitonesToPitch(_detuneSemitones)*TwoPi/AudioSettings.outputSampleRate;
                _phase = 0.0;
                _envelope.Init();
            }

            public bool Synthesize(out float outSample)
            {
                if (_envelope.CurrentStage == Envelope.Stage.NotStarted)
                {
                    outSample = 0f;
                    return false;
                }

                _phase += _phasePerSample;

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
                        sample = 2* _phase / TwoPi - 1;
                        break;
                    case WaveformType.Square:
                        sample = (_phase / TwoPi > _pulseWidth ? 1.0 : -1.0);
                        break;
                    case WaveformType.Noise:
                        sample = 2*_random.NextDouble() - 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                sample *= _gain*_envelope.GetGain();

                outSample = (float) sample;
                return true;
            }
        }

        public bool DebugPlayNote;

        [SerializeField, Range(0f, 1f)] private float _gain = 0.05f;
        [SerializeField] private Oscillator[] _oscillators;

        private bool _playing;

        public void Play(int midiNote)
        {
            var frequency = MusicMathUtils.MidiNoteToFrequency(midiNote);

            foreach (var oscillator in _oscillators)
            {
                oscillator.Init(frequency);
            }

            _playing = true;
        }

        private void Update()
        {
            if (DebugPlayNote)
            {
                Play(UnityEngine.Random.Range(50, 70));
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

                        if (!oscillator.Synthesize(out oscSample))
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
            }

        }

    }
}