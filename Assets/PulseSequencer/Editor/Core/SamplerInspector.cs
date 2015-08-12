using UnityEngine;
using UnityEditor;

namespace DerelictComputer
{
    [CustomEditor(typeof(Sampler)), CanEditMultipleObjects]
    public class SamplerInspector : Editor
    {
        private static readonly GUIContent PitchLabel = new GUIContent("Pitch", "pitch in semitones");

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DrawSampleEditor();
        }

        private void DrawSampleEditor()
        {
            var pulseSampler = (Sampler)target;

            var numSamples = pulseSampler.Samples.Count;

            numSamples = EditorGUILayout.IntField("Sample Count", numSamples);

            while (numSamples > pulseSampler.Samples.Count)
            {
                pulseSampler.Samples.Add(new Sampler.SampleInfo());
            }
            while (numSamples < pulseSampler.Samples.Count)
            {
                pulseSampler.Samples.RemoveAt(pulseSampler.Samples.Count - 1);
            }

            foreach (var sample in pulseSampler.Samples)
            {
                EditorGUILayout.LabelField(sample.Clip != null ? sample.Clip.name : "[no sample]");

                EditorGUI.indentLevel += 2;
                {
                    sample.Clip = (AudioClip)EditorGUILayout.ObjectField("Sample", sample.Clip, typeof (AudioClip), true);
                    sample.PitchInSemitones = EditorGUILayout.FloatField(PitchLabel, sample.PitchInSemitones);
					sample.Envelope.ReleaseTime = EditorGUILayout.FloatField("Release Time", sample.Envelope.ReleaseTime); 
                }
                EditorGUI.indentLevel -= 2;
            }
        }
    }
}