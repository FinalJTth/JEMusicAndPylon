using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Utils = JEMusicAndPylon.Common.Utils;

namespace JEMusicAndPylon.Tiles.Abstract
{
    public abstract class PylonTile<MI, MT> : ModTile where MI : ModItem where MT : ModTile
    {
        private readonly string _biome = Utils.SplitCamelCase(typeof(MT).Name)[0];
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName(null);
            name.SetDefault(_biome + " Pylon");
            AddMapEntry(new Color(200, 180, 100));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<MI>(), 1, false, 0, false, false);
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

        public override bool CanPlace(int i, int j)
        {
            if (JEMusicAndPylonWorld.Instance.PylonCoordinates.ContainsKey(typeof(MT).Name))
                return false;
            return true;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            JEMusicAndPylonWorld.Instance.PylonCoordinates.Add(typeof(MT).Name, new Vector2(i, j));
            Main.NewText(_biome + " Pylon has been placed at coordinate (" + i + ", " + j + ")");
            Main.NewText("Test : " + ModContent.GetModItem(ModContent.ItemType<MI>()).Texture);
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
