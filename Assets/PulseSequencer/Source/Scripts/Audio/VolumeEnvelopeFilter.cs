using UnityEngine;

namespace DerelictComputer
{
    public class VolumeEnvelopeFilter : MonoBehaviour
    {
        public bool Enabled;
        public float AttackTime = 0f;
        public float SustainTime = 10f;
        public float ReleaseTime = 0f;

        private bool _triggered;
        private double _startTime;
        private int _frequency;
        private int _attackSamples;
        private int _sustainSamples;
        private int _releaseSamples;

        public void Trigger(double triggerTime)
        {
            _triggered = true;
            _startTime = AudioSettings.dspTime;
            _frequency = AudioSettings.outputSampleRate;
            _attackSamples = (int) (AttackTime* _frequency);
            _sustainSamples = (int) (SustainTime* _frequency);
            _releaseSamples = (int) (ReleaseTime* _frequency);
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            // if not enabled, don't do any attenuation
            if (!Enabled)
            {
                return;
            }

            // if enabled, but outside the range of the envelope, attenuate fully
            if (!_triggered || AudioSettings.dspTime < _startTime)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0f;
                }

                return;
            }

            var samplesSinceTrigger = (int) (_frequency*(AudioSettings.dspTime - _startTime));

            for (int i = 0; i < data.Length; i++)
            {
                var samplesElapsed = samplesSinceTrigger + i/channels;

                var volume = 0f;

                if (samplesElapsed < _attackSamples)
                {
                    volume = Mathf.Lerp(0f, 1f, samplesElapsed / (float)_attackSamples);
                }
                else if (samplesElapsed < _sustainSamples + _attackSamples)
                {
                    volume = 1f;
                }
                else if (samplesElapsed < _releaseSamples + _sustainSamples + _attackSamples)
                {
                    volume = Mathf.Lerp(1f, 0f, (samplesElapsed - _attackSamples - _sustainSamples) / (float)_releaseSamples);
                }
                else
                {
                    volume = 0f;
                    _triggered = false;
                }

                data[i] *= Mathf.Clamp(volume, 0f, 1f);
            }
        }
    }
}