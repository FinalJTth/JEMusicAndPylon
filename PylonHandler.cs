using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using JEMusicAndPylon.Common;
using Utils = JEMusicAndPylon.Common.Utils;

namespace JEMusicAndPylon
{
    public class PylonHandler
    {
        public static PylonHandler Instance { get; set; }

        public PylonHandler()
        {
            Instance = this;
        }

		public void PostDrawFullScreenMap()
		{
			if (Main.gameMenu)
			{
				return;
			}
			Player localPlayer = Main.LocalPlayer;
			Zone playerZone = PlayerUtils.GetPlayerZone(localPlayer);
			Vector2 playerCoordinate = localPlayer.Center.ToTileCoordinates().ToVector2();

			// Main.spriteBatch.DrawString(Main.fontMouseText, "Test", new Vector2(15f, Main.screenHeight - 120), Color.White);
			// Main.spriteBatch.Draw(pylonTexture, new Vector2(15f, Main.screenHeight - 140), Color.White);
			// PlayerInput.SetZoom_Unscaled();

			float scale = Main.mapFullscreenScale / 16;
			float dx = Main.screenWidth / 2 - Main.mapFullscreenPos.X * Main.mapFullscreenScale / Main.UIScale;
			float dy = Main.screenHeight / 2 - Main.mapFullscreenPos.Y * Main.mapFullscreenScale / Main.UIScale;
			/*
			Main.spriteBatch.DrawString(Main.fontMouseText, "Player World : " + Main.LocalPlayer.Center, new Vector2(15f, Main.screenHeight - 300), Color.White);
			Main.spriteBatch.DrawString(Main.fontMouseText, "Player Block : " + Main.LocalPlayer.Center.ToTileCoordinates().ToVector2(), new Vector2(15f, Main.screenHeight - 280), Color.White);
			Main.spriteBatch.DrawString(Main.fontMouseText, "Scale : " + scale, new Vector2(15f, Main.screenHeight - 260), Color.White);
			Main.spriteBatch.DrawString(Main.fontMouseText, "Screen X : " + Main.mapFullscreenPos.X + " dx : " + dx, new Vector2(15f, Main.screenHeight - 240), Color.White);
			Main.spriteBatch.DrawString(Main.fontMouseText, "Screen Y : " + Main.mapFullscreenPos.Y + " dy : " + dy, new Vector2(15f, Main.screenHeight - 220), Color.White);
			*/
			// int counter = 0;

			// Initial pylon condition check. The last criteria (whether or not the chosen pylon has enough npc nearby) will be checked in the next for loop after this one
			// pylonCondition must be equal to 4 to pass the pylon teleport check
			bool isPlayerNearPylon = false;
			bool isNPCNearPylon = false;
			bool isCorrectBiome = false;
			bool isNPCNearClickedPylon = false;
			foreach (KeyValuePair<string, Vector2> kvp in JEMusicAndPylonWorld.Instance.PylonCoordinates)
			{
				if ((playerCoordinate - kvp.Value).Length() < 5f)
					isPlayerNearPylon = true;
				if (NPCUtils.FindNearbyNPCsByConditions(kvp.Value, 0, 60, new Dictionary<string, bool>() { { "isTownNPC", true } }).Count > 1)
					isNPCNearPylon = true;
				if (playerZone.ToString() == Utils.SplitCamelCase(kvp.Key)[0] || (kvp.Key == "UniversalPylon" && (playerCoordinate - kvp.Value).Length() < 5f))
					isCorrectBiome = true;
			}

			foreach (KeyValuePair<string, Vector2> kvp in JEMusicAndPylonWorld.Instance.PylonCoordinates)
			{
				Texture2D pylonTexture = ModContent.GetTexture("JEMusicAndPylon/Items/Placeables/" + kvp.Key + "Icon");
				Vector2 pylonWorldCoordinate = kvp.Value.ToWorldCoordinates();
				float x = dx + scale * pylonWorldCoordinate.X / Main.UIScale;
				float y = dy + scale * pylonWorldCoordinate.Y / Main.UIScale;

				float minX = x - pylonTexture.Width / 2 * Main.UIScale;
				float minY = y - pylonTexture.Height / 2 * Main.UIScale;
				float maxX = minX + pylonTexture.Width * Main.UIScale;
				float maxY = minY + pylonTexture.Height * Main.UIScale;

				SpriteEffects effect = SpriteEffects.None;

				Color pylonIconColor = isPlayerNearPylon ? Color.White : new Color(75, 75, 75, 125);
				float pylonIconScale = Main.UIScale - 0.3f;
				// When the pylon is being hovered
				if (Main.mouseX >= minX && Main.mouseX <= maxX && Main.mouseY >= minY && Main.mouseY <= maxY)
				{
					pylonIconScale += 0.5f;
					if (NPCUtils.FindNearbyNPCsByConditions(kvp.Value, 0, 60, new Dictionary<string, bool>() { { "isTownNPC", true } }).Count > 1)
					{
						isNPCNearClickedPylon = true;
						// Main.spriteBatch.DrawString(Main.fontMouseText, kvp.Key + " Teleport : enable", new Vector2(15f, Main.screenHeight - 200), Color.White);
					}
					if (Main.mouseLeft && Main.keyState.IsKeyUp(Keys.LeftControl))
					{
						if (!isPlayerNearPylon)
							Main.NewText("You are not close enough to a pylon to teleport with the pylon network", Color.Yellow);
						else if (!isNPCNearPylon)
							Main.NewText("There are not enough villagers near the current pylon", Color.Yellow);
						else if (!isCorrectBiome)
							Main.NewText("This pylon is not in the correct biome to use", Color.Yellow);
						else if (!isNPCNearClickedPylon)
							Main.NewText("There are not enough villagers near that pylon to access it", Color.Yellow);
						else if (isPlayerNearPylon && isNPCNearPylon && isCorrectBiome && isNPCNearClickedPylon)
							JEMusicAndPylonWorld.Instance.Teleport(localPlayer, kvp.Value);
						else
							Main.NewText("Something went wrong. This pylon should work", Color.Yellow);

						// Force exit the full screen map after the pylon is pressed
						Main.mouseLeftRelease = false;
						Main.mapFullscreen = false;
					}
				}

				// Draw pylon icon on the map
				Main.spriteBatch.Draw(pylonTexture, new Vector2(x, y), pylonTexture.Frame(), pylonIconColor, 0f, pylonTexture.Frame().Size() / 2, pylonIconScale, effect, 0f);
				// counter++;
			}
			PlayerInput.SetZoom_UI();
			return;
		}
	}
}
