using UnityEngine;
using System;

namespace DerelictComputer
{
	[Serializable]
	public class VolumeEnvelope 
	{
		[SerializeField] private bool _enabled;
	    [SerializeField] private bool _reverse;
	    [SerializeField] private float _offset = 0f;
		[SerializeField] private float _attackTime = 0f;
		[SerializeField] private float _sustainTime = 10f;
		[SerializeField] private float _releaseTime = 0f;

		public AudioClip Apply(AudioClip inClip)
		{
			if (!_enabled)
			{
				return inClip;
			}

		    var channels = inClip.channels;

		    var offsetSamples = (int) (_offset*inClip.frequency*channels);

			var sampleData = new float[inClip.samples - offsetSamples];

            var tmpSampleData = new float[inClip.samples];

			if (!inClip.GetData(tmpSampleData, 0))
			{
				Debug.LogWarning("Couldn't get sample data. Returning source clip.");
				return inClip;
			}

		    for (int destinationIdx = 0; destinationIdx < sampleData.Length; destinationIdx++)
		    {
		        var sourceIdx = _reverse
		            ? tmpSampleData.Length - destinationIdx - offsetSamples - 1
		            : destinationIdx + offsetSamples;

		        sampleData[destinationIdx] = tmpSampleData[sourceIdx];
		    }

		    var attackSamples = (int) (_attackTime*inClip.frequency);
		    var sustainSamples = (int) (_sustainTime*inClip.frequency);
		    var releaseSamples = (int) (_releaseTime*inClip.frequency);

			for (int sIdx = 0; sIdx < sampleData.Length; sIdx++)
			{
				var samplesFromStart = sIdx / channels;
				var volume = 0f;

				if (samplesFromStart < attackSamples)
				{
					volume = Mathf.Lerp(0f, 1f, samplesFromStart / (float)attackSamples);
				}
				else if (samplesFromStart < sustainSamples + attackSamples)
				{
					volume = 1f;
				}
				else if (samplesFromStart < releaseSamples + sustainSamples + attackSamples)
				{
					volume = Mathf.Lerp(1f, 0f, (samplesFromStart - attackSamples - sustainSamples) / (float)releaseSamples);
				}

			    sampleData[sIdx] *= Mathf.Clamp(volume, 0f, 1f);
			}

			var outClip = AudioClip.Create(inClip.name + "_VolumeEnvelope", sampleData.Length, channels, inClip.frequency, false);
			outClip.SetData(sampleData, 0);
			return outClip;
		}
	}
}
