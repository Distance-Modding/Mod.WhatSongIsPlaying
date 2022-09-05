using TagLib;
using System;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Data;
using Centrifuge.Distance.GUI.Controls;
using UnityEngine;

namespace Distance.WhatSongIsPlaying
{
	/// <summary>
	/// The mod's main class containing its entry point
	/// </summary>
	[ModEntryPoint("WhatSongIsPlaying")]
	public sealed class Mod : MonoBehaviour
	{
		public static Mod Instance { get; private set; }

		public IManager Manager { get; private set; }

		public Log Logger { get; private set; }

        public ConfigLogic Config { get; private set; }

        public string songTitle { get; set; }

        public string songArtist { get; set; }

        public string songAlbumArtist { get; set; }

        /// <summary>
        /// Method called as soon as the mod is loaded.
        /// WARNING:	Do not load asset bundles/textures in this function
        ///				The unity assets systems are not yet loaded when this
        ///				function is called. Loading assets here can lead to
        ///				unpredictable behaviour and crashes!
        /// </summary>
        public void Initialize(IManager manager)
		{
			// Do not destroy the current game object when loading a new scene
			DontDestroyOnLoad(this);

			Instance = this;

			Manager = manager;

            Config = gameObject.AddComponent<ConfigLogic>();

            songTitle = string.Empty;
            songArtist = string.Empty;
            songAlbumArtist = string.Empty;

            // Create a log file
            Logger = LogManager.GetForCurrentAssembly();

			Logger.Info("Thanks for using What Song Is Playing!");

            try
            {
                CreateSettingsMenu();
            }
            catch (Exception e)
            {
                Logger.Info(e);
                Logger.Info("This likely happened because you have the wrong version of Centrifuge.Distance. \nTo fix this, be sure to use the Centrifuge.Distance.dll file that came included with the mod's zip file. \nDespite this error, the mod will still function, however, you will not have access to the mod's menu.");
            }

            RuntimePatcher.AutoPatch();
		}

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.whatsong", "What Song Settings")
            {
                new CheckBox(MenuDisplayMode.Both, "settings:display_on_timer", "DISPLAY ON CAR SCREEN")
                .WithGetter(() => Config.DisplaySongOnTimer)
                .WithSetter((x) => Config.DisplaySongOnTimer = x)
                .WithDescription("Replace the car screen timer text with text displaying the current custom song"),

                new CheckBox(MenuDisplayMode.Both, "settings:display_on_speedrun", "DISPLAY ON SPEEDRUN TIMER")
                .WithGetter(() => Config.DisplaySongOnSpeedrunTimer)
                .WithSetter((x) => Config.DisplaySongOnSpeedrunTimer = x)
                .WithDescription("Replace the speedrun timer text with the text displaying the current custom song"),

                new CheckBox(MenuDisplayMode.Both, "settings:album_artist", "USE ALBUM ARTIST TAG")
                .WithGetter(() => Config.ShowAlbumArtist)
                .WithSetter((x) => Config.ShowAlbumArtist = x)
                .WithDescription("Uses the Album Artist tag instead of the Artist Tag when displaying a song"),
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "WHAT SONG IS PLAYING", "Settings for the WhatSongIsPlaying mod");
        }
	}
}



