using UnityEditor;
using UnityEngine;

namespace DerelictComputer
{
    [CustomEditor(typeof(Pattern)), CanEditMultipleObjects]
    public class PatternInspector : Editor
    {
        private const int StepsPerRow = 8;

        private int _numSteps;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawStepEditor();
        }

        private void OnEnable()
        {
            var pattern = (Pattern) target;	
            _numSteps = pattern.Steps.Count;
        }
	
        private void DrawStepEditor()
        {
            var pattern = (Pattern) target;
		
            _numSteps = EditorGUILayout.IntSlider("Steps", _numSteps, 1, 128);
            var wasEnabled = GUI.enabled;
            GUI.enabled &= _numSteps != pattern.Steps.Count;

            if (GUILayout.Button("Update"))
            {
                while (_numSteps > pattern.Steps.Count)
                {
                    pattern.Steps.Add(new Pattern.StepInfo());
                }
                while (_numSteps < pattern.Steps.Count)
                {
                    pattern.Steps.RemoveAt(pattern.Steps.Count - 1);
                }
            }

            GUI.enabled = wasEnabled;

            var stepIdx = 0;
		
            while (stepIdx < pattern.Steps.Count)
            {
                EditorGUILayout.BeginHorizontal();
			
                for (int i = 0; i < StepsPerRow; i++)
                {
                    if (stepIdx >= pattern.Steps.Count)
                    {
                        break;
                    }

                    pattern.Steps[stepIdx].Active = EditorGUILayout.Toggle(pattern.Steps[stepIdx].Active, GUILayout.Width(40));
                    stepIdx++;
                }
			
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}