using UnityEngine;
using System.Collections;
using DerelictComputer;

public class ToggleSuspend : MonoBehaviour
{
    [SerializeField] private PatternFollowerGroup _patternFollowerGroup;

    private void OnMouseUpAsButton()
    {
        _patternFollowerGroup.Suspended = !_patternFollowerGroup.Suspended;
    }
}
