using UnityEngine;
using System;

public class Pulse : MonoBehaviour
{
    public event Action<double> Triggered;

    /// <summary>
    /// Period in seconds
    /// </summary>
    [SerializeField] private double _period = 1d;

    /// <summary>
    /// How much time should we look ahead?
    /// </summary>
    [SerializeField] private double _latency = 0.1;

    private double _nextPulseTime;

    private void OnEnable()
    {
        _nextPulseTime = AudioSettings.dspTime + _period;
    }

    private void Update()
    {
        if (AudioSettings.dspTime + _latency > _nextPulseTime)
        {
            var thisPulseTime = _nextPulseTime;

            _nextPulseTime += _period;

            if (Triggered != null)
            {
                Triggered(thisPulseTime);
            }

            //Debug.Log("Pulse");
        }
    }
}
