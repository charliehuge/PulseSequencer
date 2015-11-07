using UnityEngine;

namespace DerelictComputer
{
    [RequireComponent(typeof(AudioSource))]
    public class SubtractiveSynth : MonoBehaviour
    {
        public bool DebugPlayNote;

        [SerializeField] private MidiNoteSequence _sequence;
        [SerializeField, Range(0f, 1f)] private float _gain = 0.05f;
        [SerializeField] private Oscillator[] _oscillators;
		[SerializeField] private Envelope _envelope;
        [SerializeField] private MultimodeFilter _filter;

        private bool _playing;
        private double _startTime;
        private double _releaseTime;
        private int _outputSampleRate;

        public void Play(int midiNote, double startTime = 0, double releaseTime = 0)
        {
            var frequency = MusicMathUtils.MidiNoteToFrequency(midiNote);

            foreach (var oscillator in _oscillators)
            {
                oscillator.Trigger(frequency);
            }

            _envelope.Trigger();

            _playing = true;

            _startTime = startTime;
            _releaseTime = releaseTime;
        }

        private void Awake()
        {
            _outputSampleRate = AudioSettings.outputSampleRate;
        }

        private void OnEnable()
        {
            if (_sequence != null)
            {
                _sequence.NoteTriggered += OnSequenceNoteTriggered;
            }
        }

        private void OnDisable()
        {
            if (_sequence != null)
            {
                _sequence.NoteTriggered -= OnSequenceNoteTriggered;
            }
        }

        private void OnSequenceNoteTriggered(MidiNoteSequence.MidiNoteInfo midiNoteInfo, double pulseTime)
        {
            Play(midiNoteInfo.MidiNote, pulseTime, midiNoteInfo.Duration + pulseTime);
        }

        private void OnAudioFilterRead(float[] buffer, int channels)
        {
            if (!_playing || AudioSettings.dspTime < _startTime)
            {
                return;
            }

            if (_oscillators.Length == 0)
            {
                return;
            }

            if (AudioSettings.dspTime > _releaseTime)
            {
                _envelope.Release();
            }

            for (int i = 0; i < buffer.Length; i += channels)
            {
				if (_envelope.CurrentStage == Envelope.Stage.Inactive)
				{
					break;
				}

                var sample = 0f;

                foreach (var oscillator in _oscillators)
                {
                    sample += oscillator.Synthesize();
                }

                sample = (sample * _gain * (float)_envelope.GetGain()) / _oscillators.Length;

                sample = _filter.Apply(sample, _outputSampleRate);

                for (int j = 0; j < channels; j++)
                {
                    buffer[i + j] = sample;
                }
            }
        }

    }
}