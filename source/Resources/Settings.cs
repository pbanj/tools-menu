﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using YamlDotNet.Serialization;

using Toggle = ToggleSwitch.ToggleSwitch;

namespace Bread_Tools.Resources
{
    /*
    ** I wanted to use MsgPack-CSharp but it isn't working
    ** Temporarily using YAMLDotNet
    */

    internal static class Settings
    {
        private static string APPDATA_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string SAVE_DIRECTORY = APPDATA_DIRECTORY + "/Bread Tools";
        private static string SAVE_FILE = SAVE_DIRECTORY + "/Settings";

        public struct Info
        {
            public Types.GeneralTools.Settings  general;
            public Types.CommandTools.Settings  command;
            public Types.PowerTools.Settings    power;
            public Types.SettingsTools.Settings settings;
        };

        public static Info Data = new Info();

        public static bool HavePageSettingsChanged<T>(List<UIElement> elements, dynamic structValue)
        {
            var info = typeof(T).GetFields();

            foreach (UIElement item in elements)
            {
                foreach (var current in info)
                {
                    if (item is Toggle && (item as Toggle).Name == current.Name)
                    {
                        if ((item as Toggle).IsOn != (bool)current.GetValue(structValue))
                            return true;
                        else if (item is RadioButton)
                        {
                            if (current.Name != "position")
                                continue;

                            RadioButton radio = (item as RadioButton);

                            string pos = (string)current.GetValue(structValue);

                            if (radio.Content.ToString() != pos)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void LoadUISettings<T>(List<UIElement> elements, dynamic structValue)
        {
            var info = typeof(T).GetFields();
            
            foreach (UIElement item in elements)
            {
                foreach (var field in info)
                {
                    if (item is Toggle && (item as Toggle).Name == field.Name)
                        (item as Toggle).IsOn = (bool)field.GetValue(structValue);
                    else if (item is RadioButton)
                    {
                        if (field.Name != "position")
                            continue;

                        RadioButton radio = (item as RadioButton);

                        string pos = (string)field.GetValue(structValue);

                        if (radio.Content.ToString() == pos)
                            radio.IsChecked = true;
                    }
                }
            }
        }

        public static void SaveUISettings<T>(List<UIElement> elements, dynamic structValue)
        {
            var info = typeof(T).GetFields();

            foreach (UIElement item in elements)
            {
                foreach (var field in info)
                {
                    object boxed = structValue;

                    if (item is Toggle && (item as Toggle).Name == field.Name)
                        field.SetValue(boxed, (item as Toggle).IsOn);
                    else if (item is RadioButton)
                    {
                        if (field.Name != "position")
                            continue;

                        RadioButton radio = (item as RadioButton);

                        if (radio.IsChecked == true)
                            field.SetValue(boxed, radio.Content.ToString());
                    }

                    structValue = (T)boxed;
                }
            }
        }

        public static bool HasSettings()
            => File.Exists(SAVE_FILE);

        public static void LoadSettings()
        {
            var deserializer = new DeserializerBuilder().Build();
            Data = deserializer.Deserialize<Info>(File.ReadAllText(SAVE_FILE));
        }

        public static void SaveSettings()
        {
            if (!Directory.Exists(SAVE_DIRECTORY))
                Directory.CreateDirectory(SAVE_DIRECTORY);

            var serializer = new SerializerBuilder().Build();
            File.WriteAllText(SAVE_FILE, "# DO NOT EDIT THIS FILE\n\n" + serializer.Serialize(Data));
        }
    }
}