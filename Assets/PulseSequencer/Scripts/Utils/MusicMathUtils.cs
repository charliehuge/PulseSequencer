using System;
using UnityEngine;

namespace DerelictComputer
{
    public static class MusicMathUtils
    {
        public const int MidiNoteA440 = 69;

		private static string[] _noteNames = {"C", "C#(Db)", "D", "D#(Eb)", "E", "F", "F#(Gb)", "G", "G#/Ab", "A", "A#(Bb)", "B"};

		private static string[] _midiNoteNames;

		/// <summary>
		/// A human readable list of MIDI note names, for use by the editor, mainly
		/// </summary>
		/// <value>The midi note names.</value>
		public static string[] MidiNoteNames
		{
			get
			{
				if (_midiNoteNames == null)
				{
					_midiNoteNames = new string[128];

					var octave = 0;

					for (int noteNumber = 0; noteNumber < _midiNoteNames.Length; noteNumber += _noteNames.Length)
					{
						for (int noteName = 0; noteName < _noteNames.Length; noteName++)
						{
							_midiNoteNames[noteNumber + noteName] = _noteNames[noteName] + octave;
						}

						octave++;
					}
				}

				return _midiNoteNames;
			}
		}

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