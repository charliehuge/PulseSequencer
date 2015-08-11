using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PulseSampler)), CanEditMultipleObjects]
public class PulseSamplerInspector : Editor
{
    private static GUIContent _pitchLabel = new GUIContent("Pitch", "pitch in semitones");

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawSampleEditor();
    }

    private void DrawSampleEditor()
    {
        var pulseSampler = (PulseSampler)target;

        var numSamples = pulseSampler.Samples.Count;

        numSamples = EditorGUILayout.IntField("Sample Count", numSamples);

        while (numSamples > pulseSampler.Samples.Count)
        {
            pulseSampler.Samples.Add(new PulseSampler.SampleInfo());
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
                sample.PitchInSemitones = EditorGUILayout.FloatField(_pitchLabel, sample.PitchInSemitones);
            }
            EditorGUI.indentLevel -= 2;
        }
    }
}
