using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Enums;
using NetUtils = JEMusicAndPylon.Common.NetUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JEMusicAndPylon.Common;
using Utils = JEMusicAndPylon.Common.Utils;

namespace JEMusicAndPylon.Tiles.Abstract
{
    public abstract class PylonTile<MI, MT, MTE> : ModTile where MI : ModItem where MT : ModTile where MTE : ModTileEntity
    {
        private readonly string _biome = Utils.SplitCamelCase(typeof(MT).Name)[0];
        private int animationFrameWidth;
        private double crystalRadian = 0;

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.LavaDeath = false;
            // Handle tile entity
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<MTE>().Hook_AfterPlacement, -1, 0, true);
            // TileObjectData.newTile.DrawYOffset = 2;
            Main.tileLighted[Type] = true;
            Main.tileShine[Type] = 400;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName(null);
            name.SetDefault(_biome + " Pylon");
            AddMapEntry(new Color(200, 180, 100));
            // Set animation frame
            animationFrameWidth = 30;
            animationFrameHeight = 46;
        }
        
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            // We can change frames manually, but since we are just simulating a different tile, we can just use the same value
            // frame = Main.tileFrame[TileID.LunarMonolith];
            
			// Spend 9 ticks on each of 6 frames, looping
			// Or, more compactly:
			if (++frameCounter >= 9) {
				frameCounter = 0;
				frame = ++frame % 8;
			}
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            SpriteEffects basePylonEffects = SpriteEffects.None;
            Texture2D basePylonTexture = ModContent.GetTexture("JEMusicAndPylon/Tiles/" + typeof(MT).Name);

            Tile tile = Main.tile[i, j];

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            Main.spriteBatch.Draw(
                basePylonTexture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 1) + zero,
                new Rectangle(tile.frameX, tile.frameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default(Vector2), 1f, basePylonEffects, 0f);

            return false;
        }
        /*
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            SpriteEffects crystalPylonEffects = SpriteEffects.None;
            Texture2D crystalPylonTexture = ModContent.GetTexture("JEMusicAndPylon/Tiles/" + "ForestPylon" + "Crystal");

            Tile tile = Main.tile[i, j];

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            int frameXOffset = Main.tileFrame[Type] * animationFrameWidth;

            int crystalFloatingOffset = 4;
            int cosineMagnitude = 4;
            double cosineFunc = Math.Cos(crystalRadian);
            Main.spriteBatch.Draw(
                crystalPylonTexture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + crystalFloatingOffset - (int)(cosineMagnitude * cosineFunc)) + zero,
                new Rectangle(tile.frameX + frameXOffset, tile.frameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default(Vector2), 1f, crystalPylonEffects, 1f);
            crystalRadian += Math.PI / 300;
        }
        */

        public int shiftFrameXByTileType()
        {
            if (typeof(MT).Name == "ForestPylon")
                return 90;
            if (typeof(MT).Name == "JunglePylon")
                return 120;
            if (typeof(MT).Name == "HallowPylon")
                return 150;
            if (typeof(MT).Name == "CavernPylon")
                return 180;
            if (typeof(MT).Name == "OceanPylon")
                return 210;
            if (typeof(MT).Name == "DesertPylon")
                return 240;
            if (typeof(MT).Name == "SnowPylon")
                return 270;
            if (typeof(MT).Name == "MushroomPylon")
                return 300;
            if (typeof(MT).Name == "UniversalPylon")
                return 330;
            return 0;
        }

        public Rectangle customRectangleDraw(int i, int j, int? imposeXOffset, int? imposeYOffset, int? preventGlitchedTextureXOffset, int? preventGlitchedTextureYOffset)
        {
            Tile tile = Main.tile[i, j];
            int frameXOffset = imposeXOffset != null ? (int)imposeXOffset : shiftFrameXByTileType();
            int frameYOffset = imposeYOffset != null ? (int)imposeYOffset : Main.tileFrame[Type] * animationFrameHeight;
            int currentFrameX = tile.frameX / 18;
            int currentFrameY = tile.frameY / 18;

            (int, int) rectXProperties = (0, 0);
            (int, int) rectYProperties = (0, 0);
            // Calculate X axis
            if (currentFrameX == 0)
                rectXProperties = (0, 6);
            else if(currentFrameX == 1)
                rectXProperties = (6, 16);
            else if (currentFrameX == 2)
                rectXProperties = (22, 6);
            // Calculate Y axis
            if (currentFrameY == 0)
                rectYProperties = (0, 16);
            else if (currentFrameY == 1)
                rectYProperties = (16, 16);
            else if(currentFrameY == 2)
                rectYProperties = (32, 12);
            else if(currentFrameY == 3)
                rectYProperties = (44, 0);
            int pgctXOffset = preventGlitchedTextureXOffset != null ? (int)preventGlitchedTextureXOffset : 0;
            int pgctYOffset = preventGlitchedTextureYOffset != null ? (int)preventGlitchedTextureYOffset : 0;
            return new Rectangle(rectXProperties.Item1 + frameXOffset, rectYProperties.Item1 + frameYOffset, rectXProperties.Item2 + pgctXOffset, rectYProperties.Item2 + pgctYOffset);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            SpriteEffects crystalPylonEffects = SpriteEffects.None;
            Texture2D crystalPylonTexture = ModContent.GetTexture("JEMusicAndPylon/Tiles/PylonCrystalAnimation");

            Tile tile = Main.tile[i, j];

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            int crystalFloatingOffset = 4;
            int cosineMagnitude = 4;
            double cosineFunc = Math.Cos(crystalRadian);
            Main.spriteBatch.Draw(
                crystalPylonTexture,
                new Vector2((i * 16) + (tile.frameX == 0 ? 11 : 0) - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + crystalFloatingOffset - (int)(cosineMagnitude * cosineFunc)) + zero,
                customRectangleDraw(i, j, null, null, null, 1),
                Lighting.GetColor(i, j), 0f, default(Vector2), 1f, crystalPylonEffects, 1f);
            crystalRadian += Math.PI / 300;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Main.gamePaused || !Main.instance.IsActive || Lighting.UpdateEveryFrame && !Main.rand.NextBool(4))
            {
                return;
            }
            Tile tile = Main.tile[i, j];

            // Return if the lamp is off (when frameX is 0), or if a random check failed.
            /*
            if (!Main.rand.NextBool(40))
            {
                return;
            }
            */
            /*
            if (tile.frameX / 18 % 4 == 0)
            {
                int dustChoice = 229;

                var dust = Dust.NewDustDirect(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);

                if (!Main.rand.NextBool(3))
                {
                    dust.noGravity = true;
                }

                dust.velocity *= 0.3f;
                dust.velocity.Y -= 1.5f;
            }
            */
            int dustChoice = 229;

            var dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.frameX == 18 && tile.frameY == 18)
            {
                // We can support different light colors for different styles here: switch (tile.frameY / 54)
                r = 0.4f;
                g = 0.4f;
                b = 0.4f;
            }
            else if (tile.frameX <= 36 && tile.frameY <= 36)
            {
                r = 0.3f;
                g = 0.3f;
                b = 0.3f;
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // Give item back
            Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<MI>(), 1, false, 0, false, false);
            // Kill tile entity
            Point16 origin = TileUtils.GetTileOrigin(i, j);
            ModContent.GetInstance<MTE>().Kill(origin.X, origin.Y);
            // Handle Pylon world data on single player
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                if (JEMusicAndPylonWorld.Instance.PylonCoordinates.ContainsKey(typeof(MT).Name))
                {
                    Vector2 coordinate = JEMusicAndPylonWorld.Instance.PylonCoordinates[typeof(MT).Name];
                    JEMusicAndPylonWorld.Instance.PylonCoordinates.Remove(typeof(MT).Name);
                    Main.NewText(_biome + " Pylon has been removed at coordinate (" + coordinate.X + ", " + coordinate.Y + ")");
                }
                else
                {
                    Main.NewText(_biome + " Pylon at coordinate (" + i + ", " + j + ") has been removed but wasn't registered");
                }
            }
        }

        public override bool CanPlace(int i, int j)
        {
            if (JEMusicAndPylonWorld.Instance.PylonCoordinates.ContainsKey(typeof(MT).Name))
                return false;
            return true;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            // Main.NewText(_biome + " Pylon has been placed at coordinate (" + i + ", " + j + ")");
            // Handle Pylon world data on single player
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                JEMusicAndPylonWorld.Instance.PylonCoordinates.Add(typeof(MT).Name, new Vector2(i, j));
                Main.NewText(_biome + " Pylon has been placed at coordinate (" + i + ", " + j + ")");
                // NetMessage.SendData(MessageID.WorldData);
                // NetMessage.SendData(MessageID.WorldData, 1, -1, NetworkText.FromLiteral("[Server] " + _biome + " Pylon has been placed at coordinate (" + i + ", " + j + ")"));
            }
            // NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("[Server] " + _biome + " Pylon has been placed at coordinate (" + i + ", " + j + ")"), Color.White);
            // Main.NewText(_biome + " Pylon has been placed at coordinate (" + i + ", " + j + ")");
        }

        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.showItemIcon = true;
            localPlayer.showItemIcon2 = ModContent.ItemType<MI>();
        }

        public override bool NewRightClick(int i, int j)
        {
            Main.playerInventory = false;
            Main.mouseRightRelease = false;
            Main.mapFullscreen = true;
            // Player player = Main.LocalPlayer;
            // JEMusicAndPylon.Instance.TogglePylonUI();
            /*
            Vector2 destination = JEMusicAndPylonWorld.Instance.PylonCoordinates["ForestPylon"];
            player.Teleport(new Vector2(Main.leftWorld + destination.X * 16, Main.topWorld + destination.Y * 16 - 32f));
            */
            return true;
        }
    }
}
