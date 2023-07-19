using System;
using System.Collections.Generic;
using System.Linq;

namespace NITHdmis.Music
{
    public static class MusicConversions
    {
        public static List<MidiNotes> GetAllMidiNotesList()
        {
            return Enum.GetValues(typeof(MidiNotes)).Cast<MidiNotes>().ToList();
        }

        public static AbsNotes ToAbsNote(string str)
        {
            if (str.Contains("#"))
            {
                str = str.Remove(str.Length - 1);
                str = "s" + str;
            }

            var ret = (AbsNotes)Enum.Parse(typeof(AbsNotes), str, true);
            return ret;
        }

        public static ScaleCodes ToScaleCode(string scalecode)
        {
            var ret = (ScaleCodes)Enum.Parse(typeof(ScaleCodes), scalecode, true);
            return ret;
        }
    }
}