using UnityEngine;
using System;
using System.Collections.Generic;

public delegate void PatternStepDelegate(int stepIndex, double pulseTime);

public class Pattern : MonoBehaviour 
{
	[Serializable]
	public class StepInfo
	{
		public bool Active = false;
	}

	public event PatternStepDelegate StepTriggered;

	[HideInInspector] public List<StepInfo> Steps = new List<StepInfo>();

	[SerializeField] private Pulse _pulse;

	private int _currentStep;

	public void Reset()
	{
		_currentStep = 0;
	}

	private void OnEnable()
	{
		if (_pulse == null)
		{
			Debug.LogWarning("I think you forgot to assign a pulse.");
			return;
		}

		_pulse.Triggered += OnPulseTriggered;

		Reset();
	}

	private void OnDisable()
	{
		if (_pulse == null)
		{
			return;
		}

		_pulse.Triggered -= OnPulseTriggered;
	}

	private void OnPulseTriggered (double pulseTime)
	{
		if (Steps.Count == 0)
		{
			return;
		}
		
		if (Steps[_currentStep].Active && StepTriggered != null)
		{
			StepTriggered(_currentStep, pulseTime);
		}
		
		_currentStep = (_currentStep + 1)%Steps.Count;
	}
}
