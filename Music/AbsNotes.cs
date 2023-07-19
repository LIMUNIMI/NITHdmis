using System;
using System.Windows.Media;

namespace NITHdmis.Music
{
    /// <summary>
    /// Enumeration of all the absolute notes.
    /// </summary>
    public enum AbsNotes
    {
        C,
        sC,
        D,
        sD,
        E,
        F,
        sF,
        G,
        sG,
        A,
        sA,
        B,
        NaN
    }

    public static class AbsNotesMethods
    {
        public static bool IsSharp(this AbsNotes note)
        {
            if (note.ToString().Contains("s"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a note into the note name in standard notation, without "s" (substituted eventually with a "#").
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string ToStandardString(this AbsNotes note)
        {
            string temp = note.ToString();

            if (temp.Contains("s"))
            {
                temp = temp.Remove(0, 1);
                temp += "#";
            }

            return temp;
        }

        public static MidiNotes ToMidiNote(this AbsNotes note, int octave)
        {
            return (MidiNotes)Enum.Parse(typeof(MidiNotes), note.ToString() + octave.ToString(), true);
        }

        public static AbsNotes Next(this AbsNotes absNote)
        {
            var count = Enum.GetNames(typeof(AbsNotes)).Length;

            AbsNotes newNote = absNote + 1;

            if ((int)newNote < count && newNote != AbsNotes.NaN)
            {
                return newNote;
            }
            else
            {
                return 0;
            }
        }

        public static AbsNotes Previous(this AbsNotes absNote)
        {
            var count = Enum.GetNames(typeof(AbsNotes)).Length;

            AbsNotes newNote = absNote - 1;

            if ((int)newNote >= 0)
            {
                return newNote;
            }
            else
            {
                return (AbsNotes)(count - 2);
            }
        }

        public static Color GetStandardColor(this AbsNotes absnote)
        {
            switch (absnote)
            {
                case AbsNotes.C:
                    return  Colors.Red;
                case AbsNotes.sC:
                    return Colors.DarkGray;
                case AbsNotes.D:
                    return Colors.Orange;
                case AbsNotes.sD:
                    return Colors.DarkGray;
                case AbsNotes.E:
                    return Colors.Yellow;
                case AbsNotes.F:
                    return Colors.GreenYellow;
                case AbsNotes.sF:
                    return Colors.DarkGray;
                case AbsNotes.G:
                    return Colors.Blue;
                case AbsNotes.sG:
                    return Colors.DarkGray;
                case AbsNotes.A:
                    return Colors.Purple;
                case AbsNotes.sA:
                    return Colors.DarkGray;
                case AbsNotes.B:
                    return Colors.Coral;
                case AbsNotes.NaN:
                    return Colors.DarkGray;
            }
            return Colors.DarkGray;
        }
    }
}
