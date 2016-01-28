using System;

namespace DerelictComputer
{
	[Serializable]
	public class VolumeEnvelope 
	{
		public bool Enabled;
        public bool Reverse;
        public float Offset = 0f;
        public float AttackTime = 0f;
        public float SustainTime = 10f;
        public float ReleaseTime = 0f;
	}
}
