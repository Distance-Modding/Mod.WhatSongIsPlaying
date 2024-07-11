using HarmonyLib;
using TagLib;

namespace WhatSongIsPlaying.Patches
{
    [HarmonyPatch(typeof(AudioManager), "PlayMP3")]
    internal class AudioManager__PlayMP3
    {
        [HarmonyPostfix]
        internal static void GetSongPostfix(AudioManager __instance, ref string ___currentCustomSongPath_)
        {
            Mod.Log.LogInfo(___currentCustomSongPath_);

            var tfile = File.Create(___currentCustomSongPath_);
            Mod.Instance.songTitle = tfile.Tag.Title;
            Mod.Instance.songArtist = tfile.Tag.FirstPerformer;
            Mod.Instance.songAlbumArtist = tfile.Tag.FirstAlbumArtist;
            tfile.Dispose();
        }
    }
}
