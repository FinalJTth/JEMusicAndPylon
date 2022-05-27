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
using Utils = JEMusicAndPylon.Common.NetUtils;
using Microsoft.Xna.Framework.Graphics;
using JEMusicAndPylon.TileEntities;
using JEMusicAndPylon.Common;
using TestItem = JEMusicAndPylon.Items.Placeables.Test;

namespace JEMusicAndPylon.Tiles
{
    public class Test : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.LavaDeath = false;
            // Handle tile entity
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TestEntity>().Hook_AfterPlacement, -1, 0, true);
            // TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName(null);
            name.SetDefault("Test");
            AddMapEntry(new Color(200, 180, 100));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // Give item back
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<TestItem>(), 1, false, 0, false, false);
            // Kill tile entity
            Point16 origin = TileUtils.GetTileOrigin(i, j);
            ModContent.GetInstance<TestEntity>().Kill(origin.X, origin.Y);
        }

        public override bool CanPlace(int i, int j)
        {
            return true;
        }
    }
}
