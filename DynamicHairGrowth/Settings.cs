// Decompiled with JetBrains decompiler
// Type: DynamicHairGrowth.Settings
// Assembly: DynamicHairGrowth, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4CA2C7CF-8C0E-4318-B35F-7FF448EDFE4C
// Assembly location: D:\SteamLibrary\steamapps\common\Grand Theft Auto V\scripts\DynamicHairGrowth.dll

using GTA;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DynamicHairGrowth
{
    public class Settings : Script
    {
        private static ScriptSettings iniFile;
        private const string XmlPath = "scripts\\DynamicHairGrowth\\DynamicHairGrowth.xml";

        public Settings()
        {
            // Load INI for non-character settings (hours / enabled flags).
            DynamicHairGrowth.Settings.iniFile = ScriptSettings.Load("scripts\\DynamicHairGrowth.ini");
            Main.HairGrowthHours = DynamicHairGrowth.Settings.iniFile.GetValue<int>("SETTINGS", "HOURS_HAIR_GROWTH", 48);
            Main.FacialGrowthHours = DynamicHairGrowth.Settings.iniFile.GetValue<int>("SETTINGS", "HOURS_FACIAL_GROWTH", 24);
            Main.HairGrowthEnabled = DynamicHairGrowth.Settings.iniFile.GetValue<bool>("SETTINGS", "HAIR_GROWTH_ENABLED", true);
            Main.FacialGrowthEnabled = DynamicHairGrowth.Settings.iniFile.GetValue<bool>("SETTINGS", "FACIAL_GROWTH_ENABLED", true);

            // Try to load per-character values from XML. If XML is missing or fails, fall back to INI.
            bool xmlLoaded = LoadFromXml();
            if (!xmlLoaded)
            {
                // Fallback: read character settings from INI (existing behavior).
                Main.michael.HairLength = DynamicHairGrowth.Settings.iniFile.GetValue<int>("MICHAEL", "HAIR_LENGTH", 0);
                Main.michael.FacialLength = DynamicHairGrowth.Settings.iniFile.GetValue<int>("MICHAEL", "FACIAL_LENGTH", 0);
                Main.franklin.HairLength = DynamicHairGrowth.Settings.iniFile.GetValue<int>("FRANKLIN", "HAIR_LENGTH", 0);
                Main.franklin.FacialLength = DynamicHairGrowth.Settings.iniFile.GetValue<int>("FRANKLIN", "FACIAL_LENGTH", 0);
                Main.trevor.HairLength = DynamicHairGrowth.Settings.iniFile.GetValue<int>("TREVOR", "HAIR_LENGTH", 0);
                Main.trevor.FacialLength = DynamicHairGrowth.Settings.iniFile.GetValue<int>("TREVOR", "FACIAL_LENGTH", 0);

                // Create XML from INI values so next game start will read from XML instead of INI.
                // This is optional but recommended to ensure persistence exclusively via XML going forward.
                SaveToXml();
            }

            Main.UpdateAllHair();
        }

        public static void Update()
        {
            /*
            DynamicHairGrowth.Settings.iniFile = ScriptSettings.Load("scripts\\DynamicHairGrowth.ini");
            DynamicHairGrowth.Settings.iniFile.SetValue<int>("MICHAEL", "HAIR_LENGTH", Main.michael.HairLength);
            DynamicHairGrowth.Settings.iniFile.SetValue<int>("MICHAEL", "FACIAL_LENGTH", Main.michael.FacialLength);
            DynamicHairGrowth.Settings.iniFile.SetValue<int>("FRANKLIN", "HAIR_LENGTH", Main.franklin.HairLength);
            DynamicHairGrowth.Settings.iniFile.SetValue<int>("FRANKLIN", "FACIAL_LENGTH", Main.franklin.FacialLength);
            DynamicHairGrowth.Settings.iniFile.SetValue<int>("TREVOR", "HAIR_LENGTH", Main.trevor.HairLength);
            DynamicHairGrowth.Settings.iniFile.SetValue<int>("TREVOR", "FACIAL_LENGTH", Main.trevor.FacialLength);
            DynamicHairGrowth.Settings.iniFile.Save();
            */
            // Also persist to XML (catch exceptions internally)
            SaveToXml();
        }

        /// <summary>
        /// Loads per-character hair/facial values from XML.
        /// Returns true when XML existed and deserialization succeeded (even if some characters missing).
        /// Returns false when file is missing or an error occurred, so caller can fallback to INI.
        /// </summary>
        private static bool LoadFromXml()
        {
            try
            {
                if (!File.Exists(XmlPath))
                    return false;

                XmlSerializer serializer = new XmlSerializer(typeof(DynamicHairGrowthXml));
                using (FileStream fs = File.OpenRead(XmlPath))
                {
                    var doc = (DynamicHairGrowthXml)serializer.Deserialize(fs);
                    if (doc != null)
                    {
                        // Only override characters that have entries in the XML.
                        if (doc.Michael != null && doc.Michael.Entries != null && doc.Michael.Entries.Length > 0)
                        {
                            var e = doc.Michael.Entries[doc.Michael.Entries.Length - 1];
                            Main.michael.HairLength = e.HairLength;
                            Main.michael.FacialLength = e.FacialLength;
                        }
                        if (doc.Franklin != null && doc.Franklin.Entries != null && doc.Franklin.Entries.Length > 0)
                        {
                            var e = doc.Franklin.Entries[doc.Franklin.Entries.Length - 1];
                            Main.franklin.HairLength = e.HairLength;
                            Main.franklin.FacialLength = e.FacialLength;
                        }
                        if (doc.Trevor != null && doc.Trevor.Entries != null && doc.Trevor.Entries.Length > 0)
                        {
                            var e = doc.Trevor.Entries[doc.Trevor.Entries.Length - 1];
                            Main.trevor.HairLength = e.HairLength;
                            Main.trevor.FacialLength = e.FacialLength;
                        }

                        return true;
                    }
                }

                return false;
            }
            catch (FileNotFoundException)
            {
                // File not found — signal caller to fallback to INI
                return false;
            }
            catch (Exception)
            {
                // Any other issue reading/parsing the XML — signal caller to fallback to INI
                return false;
            }
        }

        private static void SaveToXml()
        {
            try
            {
                var doc = new DynamicHairGrowthXml
                {
                    Michael = new CharacterList
                    {
                        Entries = new[] { new HairEntry { HairLength = Main.michael.HairLength, FacialLength = Main.michael.FacialLength } }
                    },
                    Franklin = new CharacterList
                    {
                        Entries = new[] { new HairEntry { HairLength = Main.franklin.HairLength, FacialLength = Main.franklin.FacialLength } }
                    },
                    Trevor = new CharacterList
                    {
                        Entries = new[] { new HairEntry { HairLength = Main.trevor.HairLength, FacialLength = Main.trevor.FacialLength } }
                    }
                };

                string dir = Path.GetDirectoryName(XmlPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(DynamicHairGrowthXml));
                using (FileStream fs = File.Create(XmlPath))
                {
                    serializer.Serialize(fs, doc);
                }
            }
            catch (Exception)
            {
                // Ignore save errors (disk write issues, permissions, etc.)
            }
        }

        // XML serializable types used for storing per-character lists of hair/facial states
        [Serializable]
        public class DynamicHairGrowthXml
        {
            [XmlElement("Michael")]
            public CharacterList Michael { get; set; } = new CharacterList();

            [XmlElement("Franklin")]
            public CharacterList Franklin { get; set; } = new CharacterList();

            [XmlElement("Trevor")]
            public CharacterList Trevor { get; set; } = new CharacterList();
        }

        [Serializable]
        public class CharacterList
        {
            // using array for simpler XmlSerialization and stable ordering; element name is "Entry"
            [XmlElement("Entry")]
            public HairEntry[] Entries { get; set; } = new HairEntry[0];
        }

        [Serializable]
        public class HairEntry
        {
            [XmlElement("HairLength")]
            public int HairLength { get; set; }

            [XmlElement("FacialLength")]
            public int FacialLength { get; set; }
        }
    }
}
