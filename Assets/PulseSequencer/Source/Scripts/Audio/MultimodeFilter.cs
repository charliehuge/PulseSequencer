using System;
using UnityEngine;

namespace DerelictComputer
{
    [Serializable]
    public class MultimodeFilter
    {
        public enum Mode
        {
            LowPass,
            HighPass,
            BandPass
        }

        // used by the editor
        public const float MinCutoff = 400;
        public const float MaxCutoff = 16000;
        public const float MinResonance = 0;
        public const float MaxResonance = 3.9f;

        [SerializeField] private Mode _mode = Mode.LowPass;
        [SerializeField] private double _cutoff = 0;
        [SerializeField] private double _resonance = 0;

        private double _in1, _in2, _in3, _in4, _out1, _out2, _out3, _out4;        

        public double Apply(double sIn, int sRate)
        {
            switch (_mode)
            {
                case Mode.LowPass:
                    var f = (_cutoff / sRate) * 1.16f;
                    var fb = _resonance * (1f - 0.15f * f * f);
                    sIn -= _out4 * fb;
                    sIn *= 0.35013f * (f * f) * (f * f);
                    _out1 = sIn + 0.3f * _in1 + (1f - f) * _out1; // Pole 1
                    _in1 = sIn;
                    _out2 = _out1 + 0.3f * _in2 + (1f - f) * _out2;  // Pole 2
                    _in2 = _out1;
                    _out3 = _out2 + 0.3f * _in3 + (1f - f) * _out3;  // Pole 3
                    _in3 = _out2;
                    _out4 = _out3 + 0.3f * _in4 + (1f - f) * _out4;  // Pole 4
                    _in4 = _out3;
                    return _out4;
                case Mode.HighPass:
                    return 0;
                case Mode.BandPass:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}