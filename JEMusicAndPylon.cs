using Microsoft.Xna.Framework;
using ILTerraria = IL.Terraria;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Audio;
using System;
using System.Collections.Generic;
using System.Threading;
using JourneysEndMusic.Common;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace JEMusicAndPylon
{
    public class JEMusicAndPylon : Mod
    {
        public static JEMusicAndPylon Instance { get; private set; }

        public Music MainMenuMusic;

        public JEMusicAndPylon()
        {
            Instance = this;
        }

        private bool stopTitleMusic;
        private int customTitleMusicSlot;
        private ManualResetEvent titleMusicStopped;

        private Random rand = new Random();
        private double randomDouble;

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (stopTitleMusic || (!Main.gameMenu && customTitleMusicSlot != 6 && Main.ActivePlayerFileData != null && Main.ActiveWorldFileData != null))
            {
                if (!stopTitleMusic)
                {
                    music = 6;
                }
                customTitleMusicSlot = 6;
                Music music2 = GetMusic("Sounds/Music/51_Main_Menu");
                if (music2.IsPlaying)
                {
                    music2.Stop(0);
                }
                titleMusicStopped.Set();
                stopTitleMusic = false;
            }

            if (Main.gameMenu)
                return;
            if (priority > MusicPriority.Environment)
                return;
            if (!Main.LocalPlayer.active)
                return;

            if (Main.musicVolume != 0)
            {
                Player player = Main.player[Main.myPlayer];
                if (Main.myPlayer != -1 && !Main.gameMenu && player.active)
                {
                    SetMusicAndPriority(ref music, ref priority);
                }
            }
        }

        public override void PostSetupContent()
        {
            SetTitleMusic();
        }

        public override void PreSaveAndQuit()
        {
            SetTitleMusic();
        }

        public override void Close()
        {
            try
            {
                int soundSlot = GetSoundSlot(SoundType.Music, "Sounds/Music/51_Main_Menu");
                if (Main.music.IndexInRange(soundSlot))
                {
                    Music obj = Main.music[soundSlot];
                    if (obj != null && obj.IsPlaying)
                    {
                        Main.music[soundSlot].Stop(0);
                    }
                }
                base.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                stopTitleMusic = false;
                titleMusicStopped = new ManualResetEvent(initialState: false);
            }
        }

        public override void Unload()
        {
            ILTerraria.Main.UpdateAudio -= TitleMusicIL;
            customTitleMusicSlot = 6;

            Close();

            titleMusicStopped?.Set();
            Instance = null;
            titleMusicStopped = null;
        }

        private void SetMusicAndPriority(ref int music, ref MusicPriority priority)
        {
            Player player = Main.player[Main.myPlayer];
            Zone playerZone = PlayerUtils.GetPlayerZone(player);

            // Zone condition music. Does not depends on time
            if (playerZone == Zone.JungleDirt || playerZone == Zone.JungleCavern)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/54_Underground_Jungle");
                priority = MusicPriority.BiomeMedium;
                return;
            }

            if (Main.eclipse || Main.bloodMoon)
            {
                return;
            }

            // Special condition music
            if (PlayerUtils.WillPlayMorningRainMusic())
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/59_Morning_Rain");
                priority = MusicPriority.Environment;
                return;
            }

            Vector2 playerCoordinate = player.Center.ToTileCoordinates().ToVector2();
            Dictionary<string, bool> npcProperties = new Dictionary<string, bool>()
            {
                { "isTownNPC", true }
            };
            List<int> nearbyNPCs = NPCUtils.FindNearbyNPCsByConditions(playerCoordinate, 0, 60, npcProperties);

            bool isTownZone = nearbyNPCs.Count > 1;
            bool townMusicCriteria = playerZone == Zone.Forest || playerZone == Zone.Ocean || playerZone == Zone.Cavern;

            // Time condition music
            if (Main.dayTime)
            {
                if (townMusicCriteria && isTownZone)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/46_Town_Day");
                    priority = MusicPriority.BiomeLow;
                    return;
                }
            }
            else
            {
                if (townMusicCriteria && isTownZone)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/47_Town_Night");
                    priority = MusicPriority.BiomeLow;
                    return;
                }
                if (playerZone == Zone.Ocean)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/43_Ocean_Night");
                    priority = MusicPriority.BiomeLow;
                    return;
                }
                if (playerZone == Zone.Jungle)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/55_Jungle_Night");
                    priority = MusicPriority.BiomeMedium;
                    return;
                }
            }
        }

        private void SetTitleMusic()
        {
            customTitleMusicSlot = GetSoundSlot(SoundType.Music, "Sounds/Music/51_Main_Menu");
            ILTerraria.Main.UpdateAudio += TitleMusicIL;
        }

        private void TitleMusicIL(ILContext il)
        {
            ILCursor iLCursor = new ILCursor(il);
            iLCursor.GotoNext(MoveType.After, (Instruction i) => i.MatchLdfld<Main>("newMusic"));
            iLCursor.EmitDelegate<Func<int, int>>((int newMusic) => (newMusic != 6) ? newMusic : customTitleMusicSlot);
        }
    }
}