using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DerelictComputer
{
    /// <summary>
    /// Cycles through specified sounds each time the assigned pattern steps
    /// </summary>
    public class Sampler : PatternFollower
    {
        [Serializable]
        public class Sample
        {
            [SerializeField] private AudioClip _clip;
            [SerializeField] private float _pitchInSemitones = 0f;
			[SerializeField] private VolumeEnvelope _envelope = new VolumeEnvelope();

			private AudioClip _processedClip;

			public AudioClip Clip
			{
				get
				{
					if (_processedClip == null)
					{
						if (_clip == null)
						{
							return null;
						}

						_processedClip = _envelope.Apply(_clip);
					}

					return _processedClip;
				}
			}

			public float Pitch
			{
				get
				{
					return MusicMathUtils.SemitonesToPitch(_pitchInSemitones);
				}
			}
        }

        /// <summary>
        /// The collection of samples
        /// </summary>
        public List<Sample> Samples = new List<Sample>();

        /// <summary>
        /// Optionally override the AudioMixerGroup assigned in the prefab
        /// </summary>
        [SerializeField] private AudioMixerGroup _mixerGroup;

        /// <summary>
        /// The AudioSource prefab that should be used as a template for voice instances
        /// </summary>
        [SerializeField] private AudioSource _audioSourcePrefab;

        /// <summary>
        /// How many samples should play at once?
        /// If the number of requested samples exceeds this, voice stealing happens (first in, first out)
        /// </summary>
        [SerializeField] private uint _voices = 2;

        private readonly List<AudioSource> _audioSources = new List<AudioSource>();

        private int _currentSampleIndex;

        private int _currentAudioSourceIndex;

        public override void Reset()
        {
            base.Reset();
		
            _currentSampleIndex = 0;
		
            _currentAudioSourceIndex = 0;
        }

        private void Awake()
        {
            for (var i = 0; i < _voices; i++)
            {
                var audioSource = Instantiate(_audioSourcePrefab);
                audioSource.transform.parent = transform;
                audioSource.transform.localPosition = Vector3.zero;

                if (_mixerGroup != null)
                {
                    audioSource.outputAudioMixerGroup = _mixerGroup;
                }

                _audioSources.Add(audioSource);
            }
        }
	
        protected override void OnStepTriggered(int stepIndex, double pulseTime)
        {
            if (Samples.Count == 0)
            {
                return;
            }

            var currentSample = Samples[_currentSampleIndex];
            _currentSampleIndex = (_currentSampleIndex + 1)%Samples.Count;

            var currentAudioSource = _audioSources[_currentAudioSourceIndex];
            _currentAudioSourceIndex = (_currentAudioSourceIndex + 1)%_audioSources.Count;

			currentAudioSource.clip = currentSample.Clip;
			currentAudioSource.pitch = currentSample.Pitch;
            currentAudioSource.PlayScheduled(pulseTime);
        }
    }
}