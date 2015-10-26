using System;
using UnityEngine;

namespace DerelictComputer
{
    public static class MusicMathUtils
    {
        public const int MidiNoteA440 = 69;

        /// <summary>
        /// Converts a semitone offset to a percentage pitch
        /// </summary>
        /// <param name="semitones">number of semitones from center</param>
        /// <returns>percentage-based pitch</returns>
        public static float SemitonesToPitch(float semitones)
        {
            return Mathf.Pow(2f, semitones/12f);
        }

        /// <summary>
        /// Converts a semitone offset to a percentage pitch
        /// </summary>
        /// <param name="semitones">number of semitones from center</param>
        /// <returns>percentage-based pitch</returns>
        public static double SemitonesToPitch(double semitones)
        {
            return Math.Pow(2.0, semitones/12.0);
        }

        /// <summary>
        /// Converts a MIDI note number to a frequency, based on A 440
        /// </summary>
        /// <param name="midiNote">MIDI note number to convert</param>
        /// <returns></returns>
        public static double MidiNoteToFrequency(int midiNote)
        {
            return 440*Math.Pow(2, (midiNote - MidiNoteA440)/12.0);
        }
    }
}