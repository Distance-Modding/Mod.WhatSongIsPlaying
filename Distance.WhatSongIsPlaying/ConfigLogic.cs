using System;
using Reactor.API.Configuration;
using UnityEngine;

namespace Distance.WhatSongIsPlaying
{
    public class ConfigLogic : MonoBehaviour
    {
        #region Properties
        public bool DisplaySongOnTimer
        {
            get { return Get<bool>("DisplaySongOnTimer"); }
            set { Set("DisplaySongOnTimer", value); }
        }

        public bool DisplaySongOnSpeedrunTimer
        {
            get { return Get<bool>("DisplaySongOnSpeedrunTimer"); }
            set { Set("DisplaySongOnSpeedrunTimer", value); }
        }

        public bool ShowAlbumArtist
        {
            get { return Get<bool>("ShowAlbumArtist"); }
            set { Set("ShowAlbumArtist", value); }
        }
        #endregion

        internal Settings Config;

        public event Action<ConfigLogic> OnChanged;

        //Initialize Config
        private void Load()
        {
            Config = new Settings("Config");
        }

        public void Awake()
        {
            Load();
            //Setting Defaults
            Get("DisplaySongOnTimer", true);
            Get("DisplaySongOnSpeedrunTimer", true);
            Get("ShowAlbumArtist", false);
            //Save settings to Config.json
            Save();
        }

        public T Get<T>(string key, T @default = default(T))
        {
            return Config.GetOrCreate(key, @default);
        }

        public void Set<T>(string key, T value)
        {
            Config[key] = value;
            Save();
        }

        public void Save()
        {
            Config?.Save();
            OnChanged?.Invoke(this);
        }
    }
}
