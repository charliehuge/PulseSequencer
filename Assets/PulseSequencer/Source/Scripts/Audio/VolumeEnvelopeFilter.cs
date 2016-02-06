using System;
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
        private double _sampleDuration;
        private double _attackFinishTime;
        private double _sustainFinishTime;
        private double _releaseFinishTime;

        public void Trigger(double triggerTime)
        {
            _triggered = true;
            _startTime = triggerTime;
            _sampleDuration = 1.0/AudioSettings.outputSampleRate;
            _attackFinishTime = triggerTime + AttackTime;
            _sustainFinishTime = _attackFinishTime + SustainTime;
            _releaseFinishTime = _sustainFinishTime + ReleaseTime;
        }

        private void OnAudioFilterRead(float[] buffer, int channels)
        {
            // if not enabled, don't do any attenuation
            if (!Enabled)
            {
                return;
            }

            // if enabled, but outside the range of the envelope, attenuate fully
            if (!_triggered || AudioSettings.dspTime < _startTime)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = 0f;
                }

                return;
            }

            float volume;
            double sampleTime = AudioSettings.dspTime;

            for (int i = 0; i < buffer.Length; i += channels)
            {
                sampleTime += _sampleDuration;

                if (AttackTime > 0 && sampleTime < _attackFinishTime)
                {
                    volume = (float)Math.Pow((sampleTime - _startTime)/AttackTime, 4);
                }
                else if (SustainTime > 0 && sampleTime < _sustainFinishTime)
                {
                    volume = 1f;
                }
                else if (ReleaseTime > 0 && sampleTime < _releaseFinishTime)
                {
                    volume = (float)Math.Pow((_releaseFinishTime - sampleTime)/ReleaseTime, 4);
                }
                else
                {
                    volume = 0f;
                    _triggered = false;
                }

                for (int j = 0; j < channels; j++)
                {
                    buffer[i + j] *= volume;
                }
            }
        }
    }
}