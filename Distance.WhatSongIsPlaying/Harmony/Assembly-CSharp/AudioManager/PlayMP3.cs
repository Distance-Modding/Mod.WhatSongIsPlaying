using HarmonyLib;
using TagLib;

namespace Distance.WhatSongIsPlaying.Harmony
{
    [HarmonyPatch(typeof(AudioManager), "PlayMP3")]
    internal class AudioManager__PlayMP3
    {
        [HarmonyPostfix]
        internal static void GetSongPostfix(AudioManager __instance)
        {
            Mod.Instance.Logger.Debug(__instance.currentCustomSongPath_);

            var tfile = File.Create(__instance.currentCustomSongPath_);
            Mod.Instance.songTitle = tfile.Tag.Title;
            Mod.Instance.songArtist = tfile.Tag.FirstPerformer;
            Mod.Instance.songAlbumArtist = tfile.Tag.FirstAlbumArtist;
            tfile.Dispose();
        }
    }
}
