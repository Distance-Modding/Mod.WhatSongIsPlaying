using HarmonyLib;

namespace Distance.WhatSongIsPlaying.Harmony
{
    [HarmonyPatch(typeof(TimeWidget), "PrintText")]
    internal class TimeWidget__PrintText
    {
        [HarmonyPostfix]
        internal static void ReplaceTimeWithSong(TimeWidget __instance)
        {
            if(Mod.Instance.Config.DisplaySongOnTimer)
            {
                string artistString = string.Empty;
                if (Mod.Instance.Config.ShowAlbumArtist)
                    artistString = Mod.Instance.songAlbumArtist + " - ";
                else
                    artistString = Mod.Instance.songArtist + " - ";

                string songDisplayText = artistString + Mod.Instance.songTitle;
                //If songDisplayText is longer than 14 characters, try to shorten it.
                //If it succeeds in shortening it, display the text. If it fails it will do nothing.
                //If it's already short enough it won't bother and display the text
                if(songDisplayText.Length > 14)
                {
                   songDisplayText = Mod.Instance.songTitle;
                   if(songDisplayText.Length <= 14)
                    {
                        __instance.s_.Clear();
                        __instance.s_.Append(songDisplayText);
                        __instance.textMesh_.text = __instance.s_.ToString();
                    }
                }
                else
                {
                    __instance.s_.Clear();
                    __instance.s_.Append(songDisplayText);
                    __instance.textMesh_.text = __instance.s_.ToString();
                }
            }
        }
    }
}
