using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.Localization;
using NetUtils = JEMusicAndPylon.Common.NetUtils;
using Microsoft.Xna.Framework.Graphics;
using JEMusicAndPylon.Common;
using Utils = JEMusicAndPylon.Common.Utils;

namespace JEMusicAndPylon.Tiles.Abstract
{
    public abstract class PylonTile<MI, MT, MTE> : ModTile where MI : ModItem where MT : ModTile where MTE : ModTileEntity
    {
        private readonly string _biome = Utils.SplitCamelCase(typeof(MT).Name)[0];
        private readonly int _animationFrame = 54;

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.LavaDeath = false;
            // Handle tile entity
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<MTE>().Hook_AfterPlacement, -1, 0, true);
            // TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName(null);
            name.SetDefault(_biome + " Pylon");
            AddMapEntry(new Color(200, 180, 100));
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            Texture2D texture = ModContent.GetTexture("JEMusicAndPylon/Tiles/" + typeof(MT).Name);
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Main.spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.frameX, tile.frameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
            return false;
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
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.WorldData);
                    }
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
