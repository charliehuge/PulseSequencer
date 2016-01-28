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

            // if suspended, keep counting sample indices in order to keep in phase
            if (Suspended)
            {
                return;
            }

            var currentAudioSource = _audioSources[_currentAudioSourceIndex];
            _currentAudioSourceIndex = (_currentAudioSourceIndex + 1)%_audioSources.Count;

            var envelopeFilter = currentAudioSource.GetComponent<VolumeEnvelopeFilter>();

            if (envelopeFilter != null)
            {
                envelopeFilter.Enabled = currentSample.Envelope.Enabled;

                if (currentSample.Envelope.Enabled)
                {
                    envelopeFilter.AttackTime = currentSample.Envelope.AttackTime;
                    envelopeFilter.SustainTime = currentSample.Envelope.SustainTime;
                    envelopeFilter.ReleaseTime = currentSample.Envelope.ReleaseTime;
                    envelopeFilter.Trigger(pulseTime);
                }
            }

            currentAudioSource.clip = currentSample.Clip;
            currentAudioSource.pitch = currentSample.Pitch;
            currentAudioSource.timeSamples = currentSample.Offset;
            currentAudioSource.PlayScheduled(pulseTime);
        }
    }
}