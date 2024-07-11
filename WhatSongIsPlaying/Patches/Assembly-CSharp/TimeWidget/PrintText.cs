using HarmonyLib;
using System.Text;
using UnityEngine;

namespace WhatSongIsPlaying.Patches
{
    [HarmonyPatch(typeof(TimeWidget), "PrintText")]
    internal class TimeWidget__PrintText
    {
        [HarmonyPostfix]
        internal static void ReplaceTimeWithSong(TimeWidget __instance, ref StringBuilder ___s_, ref TextMesh ___textMesh_)
        {
            if (Mod.DisplaySongOnTimer.Value)
            {
                string artistString = string.Empty;
                if (Mod.ShowAlbumArtist.Value)
                    artistString = Mod.Instance.songAlbumArtist + " - ";
                else
                    artistString = Mod.Instance.songArtist + " - ";

                string songDisplayText = artistString + Mod.Instance.songTitle;
                //If songDisplayText is longer than 14 characters, try to shorten it.
                //If it succeeds in shortening it, display the text. If it fails it will do nothing.
                //If it's already short enough it won't bother and display the text
                if (songDisplayText.Length > 14)
                {
                    songDisplayText = Mod.Instance.songTitle;
                    if (songDisplayText.Length <= 14)
                    {
                        ___s_.Clear();
                        ___s_.Append(songDisplayText);
                        ___textMesh_.text = ___s_.ToString();
                    }
                }
                else
                {
                    ___s_.Clear();
                    ___s_.Append(songDisplayText);
                    ___textMesh_.text = ___s_.ToString();
                }
            }
        }
    }
}
