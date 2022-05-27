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
using Item = JEMusicAndPylon.Items.Placeables;

namespace JEMusicAndPylon.Tiles
{
    public class RedAmongus : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.LavaDeath = false;
            // TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName(null);
            name.SetDefault("Red Amongus");
            AddMapEntry(new Color(197, 17, 17));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Terraria.Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<Item.RedAmongus>(), 1, false, 0, false, false);
        }
        
        public override bool NewRightClick(int i, int j)
        {
            Main.LocalPlayer.KillMe(PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + " touched the sussy baka statue"), 10000000, 0);
            return true;
        }
    }
}
