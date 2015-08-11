using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PulseAnimationTrigger : PatternFollower
{
    [SerializeField] private string _triggerName;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void OnStepTriggered(int stepIndex, double pulseTime)
    {
        _animator.SetTrigger(_triggerName);
    }
}
