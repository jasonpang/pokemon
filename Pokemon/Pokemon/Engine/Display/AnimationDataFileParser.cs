using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;

namespace Pokemon.Engine.Display
{
    /// <summary>
    /// Parses animation data files to return FrameSequence objects.
    /// </summary>
    public static class AnimationDataFileParser
    {
        private static char[] regex = {'_'};

        /// <summary>
        /// Parses the animation data file and fills the FrameSequences dictionary.
        /// </summary>
        public static void ParseFile(string AnimationDataFilePath, Dictionary<string, FrameSequence> frameSequences)
        {
            frameSequences.Clear();

            Stream fileStream = new FileStream(AnimationDataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (XmlReader reader = XmlReader.Create(fileStream, new XmlReaderSettings { CloseInput = true }))
            {
                while (reader.Read())
                {
                    // reader.Read() returns each starting element with any whitespaces and <xml? so we want to
                    // make sure we just get the starting elements. it reads in order, without depth sequence.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "SubTexture":
                                string name = reader["name"];
                                string namePrefix = GetPrefixFromName(name);
                                int nameNumber = GetNumberFromName(name);                                
                                
                                int x = int.Parse(reader["x"]);
                                int y = int.Parse(reader["y"]);
                                int width = int.Parse(reader["width"]);
                                int height = int.Parse(reader["height"]);

                                try
                                {
                                    FrameSequence currFrameSequence = frameSequences[namePrefix];
                                    frameSequences[namePrefix].Frames.Add(nameNumber, new Rectangle(x, y, width, height));
                                }
                                catch (KeyNotFoundException)
                                {
                                    // New name prefix; empty FrameSequence
                                    FrameSequence frameSequence = new FrameSequence();
                                    frameSequence.Frames.Add(nameNumber, new Rectangle(x, y, width, height));
                                    frameSequences.Add(namePrefix, frameSequence);
                                }
                                break;
                        }
                    }
                }
            }
        }        

        /// <summary>
        /// If the name of the frame is "Left_1", this method returns "Left". If name "Left", should return "Left".
        /// If name "1", should return null.
        /// </summary>
        private static string GetPrefixFromName(string name)
        {
            string[] parts = name.Split(regex, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1)
            {
                if (Char.IsLetter(parts[0], 0))
                {
                    return parts[0];
                }
                else
                {
                    return null;
                }
            }
            else if (parts.Length == 1)
            {
                if (Char.IsLetter(name, 0))
                {
                    return name;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// If the name of the frame is "Left_1", this method returns "1". If name is "Left_Brendan_1" (can't
        /// imagine why), returns "1". If name is "1", should return "1". If name is "Left", should return -1.
        /// </summary>
        private static int GetNumberFromName(string name)
        {
            string[] parts = name.Split(regex, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                if (Char.IsDigit(parts[parts.Length - 1], 0))
                {
                    return int.Parse(parts[parts.Length - 1]);
                }
                else
                {
                    return -1;
                }
            }
            else if (parts.Length == 1)
            {
                if (Char.IsDigit(name, 0))
                {
                    return int.Parse(name);
                }
                else
                {
                    return -1;
                }
            }
            return -1;
        }
    }
}
