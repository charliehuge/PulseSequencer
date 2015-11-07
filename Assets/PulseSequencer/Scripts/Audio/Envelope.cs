using System;
using UnityEngine;

namespace DerelictComputer
{
    [Serializable]
    public class Envelope
    {
        public enum Stage
        {
            Inactive,
            Attack,
            Decay,
            Sustain,
            Release,
        }

        [SerializeField] private double _attackTime = 0.0;
        [SerializeField] private double _decayTime = 0.0;
        [SerializeField] private double _sustainLevel = 1.0;
        [SerializeField] private double _releaseTime = 0.0;

        private double _attackGainPerSample;
        private double _decayGainPerSample;
        private double _releaseGainPerSample;
        private double _gain;
        private bool _held;

        public Stage CurrentStage { get; private set; }

        public void Trigger()
        {
            _attackGainPerSample = _attackTime > 0 ? 1 / (_attackTime * AudioSettings.outputSampleRate) : 0;

            if (_decayTime > 0)
            {
                var gainDelta = _sustainLevel - 1.0;
                var decaySamples = _decayTime*AudioSettings.outputSampleRate;
                _decayGainPerSample = gainDelta/decaySamples;
            }
            else
            {
                _decayGainPerSample = 0;
            }

            if (_releaseTime > 0)
            {
                var gainDelta = 0.0 - _sustainLevel;
                var releaseSamples = _releaseTime*AudioSettings.outputSampleRate;
                _releaseGainPerSample = gainDelta/releaseSamples;
            }
            else
            {
                _releaseGainPerSample = 0;
            }
            
            _gain = 0;

            CurrentStage = Stage.Attack;

            _held = true;
        }

        public void Release()
        {
            _held = false;
        }

        public double GetGain()
        {
            // inactive
            if (CurrentStage == Stage.Inactive)
            {
                return 0;
            }

            // attack
            if (CurrentStage == Stage.Attack)
            {
                if (_attackGainPerSample > 0)
                {
                    _gain += _attackGainPerSample;

                    if (_gain < 1)
                    {
                        return _gain;
                    }

                    CurrentStage = Stage.Decay;
                    _gain = 1;
                    return _gain;
                }

                _gain = 1;
                CurrentStage = Stage.Decay;
            }

            // decay
            if (CurrentStage == Stage.Decay)
            {
                if (_decayGainPerSample < 0)
                {
                    _gain += _decayGainPerSample;

                    if (_gain > _sustainLevel)
                    {
                        return _gain;
                    }

                    CurrentStage = _held ? Stage.Sustain : Stage.Release;
                    _gain = _sustainLevel;
                    return _gain;
                }

                _gain = _sustainLevel;
                CurrentStage = _held ? Stage.Sustain : Stage.Release;
            }

            // sustain
            if (CurrentStage == Stage.Sustain)
            {
                if (!_held)
                {
                    CurrentStage = Stage.Release;
                }

                return _sustainLevel;
            }

            // release
            _gain += _releaseGainPerSample;

            if (_gain < 0)
            {
                CurrentStage = Stage.Inactive;
                return 0;
            }

            return _gain;
        }

    }
}