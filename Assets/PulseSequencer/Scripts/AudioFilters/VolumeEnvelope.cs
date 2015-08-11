using UnityEngine;
using System.Collections;

namespace DerelictComputer
{
	public class VolumeEnvelope : MonoBehaviour 
	{
		[SerializeField, HideInInspector] private AnimationCurve _envelopeCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);


	}
}
