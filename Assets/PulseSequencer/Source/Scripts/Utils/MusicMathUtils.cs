using UnityEngine;

namespace DerelictComputer
{
    public static class MusicMathUtils
    {
        /// <summary>
        /// Converts a semitone offset to a percentage pitch
        /// </summary>
        /// <param name="semitones">number of semitones from center</param>
        /// <returns>percentage-based pitch</returns>
        public static float SemitonesToPitch(float semitones)
        {
            return Mathf.Pow(2f, semitones / 12f);
        }
    }
}