using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JEMusicAndPylon.Items.Placeables
{
    public class ForestPylon : ModItem
    {
        public override void SetStaticDefaults()
        {

            Tooltip.SetDefault("This is a modded item.");
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
            item.createTile = ModContent.TileType<Tiles.ForestPylon>();
            item.placeStyle = 0;
            // Set other item.X values here
        }
    }
}
