using System;
using UnityEngine;
using UnityEditor;

namespace DerelictComputer
{
    [CustomEditor(typeof(Pulse))]
    public class PulseInspector : Editor
    {
        public enum PeriodType
        {
            Seconds,
            Bpm
        }

        private static readonly GUIContent PeriodLabel = new GUIContent("Period", "How long each pulse lasts in seconds");

        private static readonly GUIContent BpmLabel = new GUIContent("BPM", "How many beats occur in a minute");

        private static readonly GUIContent PpbLabel = new GUIContent("Pulses/Beat", "How many pulses occur in one beat");

        private PeriodType _periodType = PeriodType.Seconds;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawPeriodEditor();
        }

        private void DrawPeriodEditor()
        {
            var pulse = (Pulse)target;

            _periodType = (PeriodType)EditorGUILayout.EnumPopup(_periodType);

            switch (_periodType)
            {
                case PeriodType.Seconds:
                    pulse.Period = EditorGUILayout.DoubleField(PeriodLabel, pulse.Period);
                    break;
                case PeriodType.Bpm:
                    var bpm = 60.0 / (pulse.Period * pulse.PulsesPerBeat);

                    EditorGUI.BeginChangeCheck();

                    bpm = EditorGUILayout.DoubleField(BpmLabel, bpm);

                    var pulsesPerBeat = (uint)EditorGUILayout.IntSlider(PpbLabel, (int)pulse.PulsesPerBeat, 1, 16);

                    if (EditorGUI.EndChangeCheck())
                    {
                        pulse.SetBpm(bpm, pulsesPerBeat);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}