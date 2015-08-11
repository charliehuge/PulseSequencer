using UnityEngine;

public static class MusicMathUtils
{
    public static float SemitonesToPitch(float semitones)
    {
        return Mathf.Pow(2f, semitones / 12f);
    }
}
