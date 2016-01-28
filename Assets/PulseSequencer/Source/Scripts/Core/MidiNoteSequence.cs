using System;
using UnityEngine;

namespace DerelictComputer
{
	public class MidiNoteSequence : PatternFollower 
	{
	    [Serializable]
	    public class MidiNoteInfo
	    {
	        public int MidiNote;
	        public float Duration;
	    }

	    public delegate void MidiNoteTriggerDelegate(MidiNoteInfo noteInfo, double pulseTime);


	    public event MidiNoteTriggerDelegate NoteTriggered; 

	    [SerializeField] private MidiNoteInfo[] _notes;

	    private int _currentNoteIdx;

	    public override void Reset()
	    {
	        base.Reset();

	        _currentNoteIdx = 0;
	    }

	    protected override void OnStepTriggered (int stepIndex, double pulseTime)
		{
		    if (_notes == null || _notes.Length == 0)
		    {
		        return;
		    }

	        var noteIdx = _currentNoteIdx;
	        _currentNoteIdx = (_currentNoteIdx + 1)%_notes.Length;

	        if (Suspended)
	        {
	            return;
	        }

	        if (NoteTriggered != null)
	        {
	            NoteTriggered(_notes[noteIdx], pulseTime);
	        }
        }
	}
}
