using System;
using UnityEngine;

namespace DerelictComputer
{
    [RequireComponent(typeof(AudioSource))]
    public class SubtractiveSynth : MonoBehaviour
    {
        [SerializeField] private double _frequency = 440.0;
        [SerializeField] private double _gain = 0.05;

        private double _twoPi;
        private double _increment;
        private double _phase;
        private double _sampleRate;

        private void Awake()
        {
            _twoPi = Math.PI*2;
            _sampleRate = AudioSettings.outputSampleRate;
        }

        private void OnAudioFilterRead(float[] buffer, int channels)
        {
            _increment = _frequency*_twoPi/_sampleRate;

            for (int i = 0; i < buffer.Length; i += channels)
            {
                _phase = _phase + _increment;

                // wrap phase
                if (_phase > _twoPi)
                {
                    _phase -= _twoPi;
                }

                var sample = (float) (Math.Sin(_phase)*_gain);

                for (int j = 0; j < channels; j++)
                {
                    buffer[i + j] = sample;
                }
            }
        }

    }
}