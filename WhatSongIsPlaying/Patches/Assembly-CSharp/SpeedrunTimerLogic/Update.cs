using HarmonyLib;

namespace WhatSongIsPlaying.Patches
{
    [HarmonyPatch(typeof(SpeedrunTimerLogic), "Update")]
    internal class SpeedrunTimerLogic__Update
    {
        [HarmonyPostfix]
        internal static void ReplaceSpeedrunWithSong(SpeedrunTimerLogic __instance, ref UILabel ___label_)
        {
            if (Mod.DisplaySongOnSpeedrunTimer.Value)
            {
                string artistString = string.Empty;
                if (Mod.ShowAlbumArtist.Value)
                    artistString = Mod.Instance.songAlbumArtist + " - ";
                else
                    artistString = Mod.Instance.songArtist + " - ";

                ___label_.text = "♪ " + artistString + Mod.Instance.songTitle;
                ___label_.color = Colors.white;
            }
        }
    }
}
