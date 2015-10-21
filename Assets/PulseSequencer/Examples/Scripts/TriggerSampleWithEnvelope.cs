using UnityEngine;
using System.Collections;
using DerelictComputer;

public class TriggerSampleWithEnvelope : MonoBehaviour
{
    public bool Trigger;

    [SerializeField] private VolumeEnvelopeFilter _envelope;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private double _delay = 0.1;

    private void Update()
    {
        if (Trigger)
        {
            var playTime = AudioSettings.dspTime + _delay;
            _audioSource.PlayScheduled(playTime);
            _envelope.Trigger(playTime);
            Trigger = false;
        }
    }
}
