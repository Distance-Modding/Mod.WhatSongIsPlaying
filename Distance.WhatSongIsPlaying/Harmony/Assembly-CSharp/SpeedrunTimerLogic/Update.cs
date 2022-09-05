using HarmonyLib;

namespace Distance.WhatSongIsPlaying.Harmony
{
    [HarmonyPatch(typeof(SpeedrunTimerLogic), "Update")]
    internal class SPeedrunTimerLogic__Update
    {
        [HarmonyPostfix]
        internal static void ReplaceSpeedrunWithSong(SpeedrunTimerLogic __instance)
        {
            if(Mod.Instance.Config.DisplaySongOnSpeedrunTimer)
            {
                string artistString = string.Empty;
                if (Mod.Instance.Config.ShowAlbumArtist)
                    artistString = Mod.Instance.songAlbumArtist + " - ";
                else
                    artistString = Mod.Instance.songArtist + " - ";

                __instance.label_.text = "♪ " + artistString + Mod.Instance.songTitle;
                __instance.label_.color = Colors.white;
            }
        }
    }
}
