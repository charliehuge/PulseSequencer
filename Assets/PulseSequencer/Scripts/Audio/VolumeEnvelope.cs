using UnityEngine;
using System;

namespace DerelictComputer
{
	[Serializable]
	public class VolumeEnvelope 
	{
		[SerializeField] private float _attackTime = 0f;
		[SerializeField] private float _sustainTime = 10f;
		[SerializeField] private float _releaseTime = 0f;

		public AudioClip Apply(AudioClip inClip)
		{
			float[] sampleData = new float[inClip.samples];

			if (!inClip.GetData(sampleData, 0))
			{
				Debug.LogWarning("Couldn't get sample data. Returning source clip.");
				return inClip;
			}

			float attackSamples = _attackTime * inClip.frequency;
			float sustainSamples = _sustainTime * inClip.frequency;
			float releaseSamples = _releaseTime * inClip.frequency;

			for (int sIdx = 0; sIdx < inClip.samples; sIdx++)
			{
				var samplesFromStart = sIdx / inClip.channels;
				float volume = 0f;

				if (samplesFromStart < attackSamples)
				{
					volume = Mathf.Lerp(0f, 1f, samplesFromStart / attackSamples);
				}
				else if (samplesFromStart < sustainSamples + attackSamples)
				{
					volume = 1f;
				}
				else if (samplesFromStart < releaseSamples + sustainSamples + attackSamples)
				{
					volume = Mathf.Lerp(1f, 0f, (samplesFromStart - attackSamples - sustainSamples) / releaseSamples);
				}

				sampleData[sIdx] *= Mathf.Clamp(volume, 0f, 1f);
			}

			var outClip = AudioClip.Create(inClip.name + "_VolumeEnvelope", inClip.samples, inClip.channels, inClip.frequency, false);
			outClip.SetData(sampleData, 0);
			return outClip;
		}
	}
}
