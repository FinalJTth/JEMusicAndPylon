using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Utils = JEMusicAndPylon.Common.Utils;

namespace JEMusicAndPylon.Items.Placeables.Abstract
{
    public abstract class PylonItem<MI, MT> : ModItem where MI : ModItem where MT : ModTile
    {
        public static Texture2D GetTexture()
        {
            return ModContent.GetTexture(ModContent.GetModItem(ModContent.ItemType<MI>()).Texture);
        }

        public override void SetStaticDefaults()
        {
            string[] nameArray = Utils.SplitCamelCase(typeof(MI).Name);
            DisplayName.SetDefault(nameArray[0] + " " + nameArray[1]);
            Tooltip.SetDefault("Teleport to another pylon when 2 villagers are neaby\nYou can only place one per type and in the matching biome");
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
            item.createTile = ModContent.TileType<MT>();
            item.placeStyle = 0;
            // Set other item.X values here
        }

        public override void RightClick(Terraria.Player player)
        {
            foreach (KeyValuePair<string, Vector2> kvp in JEMusicAndPylonWorld.Instance.PylonCoordinates)
            {
                Main.NewText(kvp.Key + " : " + "(" + kvp.Value.X + ", " + kvp.Value.Y + ")");
            }
        }
    }
}
