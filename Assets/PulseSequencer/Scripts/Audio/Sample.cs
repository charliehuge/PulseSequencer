using UnityEngine;
using System;

namespace DerelictComputer
{
    [Serializable]
    public class Sample
    {
        public VolumeEnvelope Envelope = new VolumeEnvelope();

        [SerializeField]
        private AudioClip _clip;
        [SerializeField]
        private float _pitchInSemitones = 0f;

        public AudioClip Clip
        {
            get
            {
                return _clip;
            }
        }

        public float Pitch
        {
            get
            {
                if (Envelope.Enabled && Envelope.Reverse)
                {
                    return MusicMathUtils.SemitonesToPitch(_pitchInSemitones) * -1f;
                }

                return MusicMathUtils.SemitonesToPitch(_pitchInSemitones);
            }
        }

        public int Offset
        {
            get
            {
                if (!Envelope.Enabled)
                {
                    return 0;
                }

                if (Envelope.Reverse)
                {
                    return (int)(_clip.samples - Envelope.Offset * _clip.frequency - 1);
                }

                return (int)(Envelope.Offset * _clip.frequency);
            }
        }
    }
}