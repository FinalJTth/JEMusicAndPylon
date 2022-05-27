using JEMusicAndPylon.Items.Placeables.Abstract;
using Tile = JEMusicAndPylon.Tiles;
using Terraria.ModLoader;
using TestTile = JEMusicAndPylon.Tiles.Test;

namespace JEMusicAndPylon.Items.Placeables
{
    public class Test : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test");
            Tooltip.SetDefault("Test Tile Entity");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.value = 10000;
            item.rare = 1;
            item.createTile = ModContent.TileType<TestTile>();
            item.placeStyle = 0;
            // Set other item.X values here
        }
    }
}