using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Utils = JEMusicAndPylon.Common.Utils;
using Tile = JEMusicAndPylon.Tiles;

namespace JEMusicAndPylon.Items.Placeables
{
    public class RedAmongus : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Among Us");
            Tooltip.SetDefault("This item is looking kinda sus...");
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
            item.createTile = ModContent.TileType<Tile.RedAmongus>();
            item.placeStyle = 0;
            // Set other item.X values here
        }

        public override void AddRecipes()
        {
            // Recipes here. See Basic Recipe Guide
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TissueSample, 99);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}