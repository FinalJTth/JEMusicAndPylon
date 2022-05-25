using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JEMusicAndPylon.Tiles
{
    public class ForestPylon : ModTile
    {
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
            name.SetDefault("Pylon");
            AddMapEntry(new Color(200, 200, 200), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Placeables.ForestPylon>(), 1, false, 0, false, false);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            
        }

        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.showItemIcon = true;
            localPlayer.showItemIcon2 = ModContent.ItemType<Items.Placeables.ForestPylon>();
        }

        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Main.NewText("Clicked on Pylon !");
            player.Teleport(new Vector2(Main.leftWorld + 20 * 16, Main.topWorld + 20 * 16 - 32f));
            return true;
        }
    }
}
