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

        [SerializeField] private Mode _mode = Mode.LowPass;
        [SerializeField, Range(10f, 22000f)] private float _cutoff = 100f;
        [SerializeField, Range(0f, 4f)] private float _resonance = 0f;

        private float in1, in2, in3, in4, out1, out2, out3, out4;
        /*
    Notes : 
    in[x] and out[x] are member variables, init to 0.0 the controls:

    fc = cutoff, nearly linear [0,1] -> [0, fs/2]
    res = resonance [0, 4] -> [no resonance, self-oscillation]

    Code : 
    Tdouble MoogVCF::run(double input, double fc, double res)
    {
      double f = fc * 1.16;
      double fb = res * (1.0 - 0.15 * f * f);
      input -= out4 * fb;
      input *= 0.35013 * (f*f)*(f*f);
      out1 = input + 0.3 * in1 + (1 - f) * out1; // Pole 1
      in1  = input;
      out2 = out1 + 0.3 * in2 + (1 - f) * out2;  // Pole 2
      in2  = out1;
      out3 = out2 + 0.3 * in3 + (1 - f) * out3;  // Pole 3
      in3  = out2;
      out4 = out3 + 0.3 * in4 + (1 - f) * out4;  // Pole 4
      in4  = out3;
      return out4;
    }
    */
        

        public float Apply(float sIn, int sRate)
        {
            var f = (_cutoff / sRate) * 1.16f;
            var fb = _resonance * (1f - 0.15f * f * f);
            sIn -= out4 * fb;
            sIn *= 0.35013f * (f * f) * (f * f);
            out1 = sIn + 0.3f * in1 + (1f - f) * out1; // Pole 1
            in1 = sIn;
            out2 = out1 + 0.3f * in2 + (1f - f) * out2;  // Pole 2
            in2 = out1;
            out3 = out2 + 0.3f * in3 + (1f - f) * out3;  // Pole 3
            in3 = out2;
            out4 = out3 + 0.3f * in4 + (1f - f) * out4;  // Pole 4
            in4 = out3;
            return (float)out4;
        }
    }
}