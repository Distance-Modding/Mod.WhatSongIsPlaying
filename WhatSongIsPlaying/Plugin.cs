using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace WhatSongIsPlaying
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public sealed class Mod : BaseUnityPlugin
    {
        public static string OnTimerKey = "Display On Car Timer";
        public static string OnSpeedrunTimerKey = "Display On Speedrun Timer";
        public static string ShowAlbumKey = "Show Album Artist";

        public static ConfigEntry<bool> DisplaySongOnTimer;
        public static ConfigEntry<bool> DisplaySongOnSpeedrunTimer;
        public static ConfigEntry<bool> ShowAlbumArtist;

        public static Mod Instance;

        public string songTitle { get; set; }
        public string songArtist { get; set; }
        public string songAlbumArtist { get; set; }


        private const string modGUID = "Distance.WhatSongIsPlaying";
        private const string modName = "What Song Is Playing";
        private const string modVersion = "1.0.0";

        private static readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource Log;

        

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            Logger.LogInfo("Thanks for using WhatSongIsPlaying");

            songTitle = string.Empty;
            songArtist = string.Empty;
            songAlbumArtist = string.Empty;

            DisplaySongOnTimer = Config.Bind("Settings",
                OnTimerKey,
                false,
                new ConfigDescription("Replace the car screen timer text with text displaying the current custom song"));

            DisplaySongOnSpeedrunTimer = Config.Bind("Settings",
                OnSpeedrunTimerKey,
                true,
                new ConfigDescription("Replace the speedrun timer text with the text displaying the current custom song"));

            ShowAlbumArtist = Config.Bind("Settings",
                ShowAlbumKey,
                false,
                new ConfigDescription("Uses the Album Artist tag instead of the Artist Tag when displaying a song"));

            DisplaySongOnTimer.SettingChanged += ConfigSettingChanged;
            DisplaySongOnSpeedrunTimer.SettingChanged += ConfigSettingChanged;
            ShowAlbumArtist.SettingChanged += ConfigSettingChanged;

            //Apply Patches
            Logger.LogInfo("Loading...");
            harmony.PatchAll();
            Logger.LogInfo("Loaded!");
        }

        private void ConfigSettingChanged(object sender, EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            if (settingChangedEventArgs == null) return;

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == OnTimerKey)
            {
                DisplaySongOnTimer.BoxedValue = settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == OnSpeedrunTimerKey)
            {
                DisplaySongOnSpeedrunTimer.BoxedValue = settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == ShowAlbumKey)
            {
                ShowAlbumArtist.BoxedValue = settingChangedEventArgs.ChangedSetting.BoxedValue;
            }
        }
    }
}
