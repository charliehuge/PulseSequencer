using UnityEngine;
using System;

namespace DerelictComputer
{
	[Serializable]
	public class VolumeEnvelope 
	{
		[HideInInspector] public float ReleaseTime = 0.25f;

		public AudioClip Apply(AudioClip inClip)
		{
			float[] sampleData = new float[inClip.samples];

			if (!inClip.GetData(sampleData, 0))
			{
				Debug.LogWarning("Couldn't get sample data. Returning source clip.");
				return inClip;
			}

			var releaseSamples = ReleaseTime * inClip.frequency;

			for (int i = 0; i < inClip.samples; i++)
			{
				var samplesFromStart = i / inClip.channels;
				var volume = Mathf.Lerp(1f, 0f, samplesFromStart / releaseSamples);
				sampleData[i] *= Mathf.Clamp(volume, 0f, 1f);
			}

			var outClip = AudioClip.Create(inClip.name + "_VolumeEnvelope", inClip.samples, inClip.channels, inClip.frequency, false);
			outClip.SetData(sampleData, 0);
			return outClip;
		}
	}
}
