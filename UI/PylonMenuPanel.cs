using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace JEMusicAndPylon.UI
{
    internal class PylonMenuPanel : UIPanel
    {
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		public void AddPylons()
		{
			int counter = 0;
			foreach(KeyValuePair<string, Vector2> kvp in JEMusicAndPylonWorld.Instance.PylonCoordinates)
            {
				UIText pylonNameTextUI = new UIText(kvp.Key);
				pylonNameTextUI.Left.Set(50f, 0f);
				pylonNameTextUI.Top.Set(21f + 45f * counter, 0f);
				pylonNameTextUI.Height.Set(22f, 0f);
				pylonNameTextUI.Width.Set(60f, 0f);
				Append(pylonNameTextUI);

				UIPanel pylonTeleportButtonUI = new UIPanel();
				pylonTeleportButtonUI.Left.Set(150f, 0f);
				pylonTeleportButtonUI.Top.Set(19f + 45f * counter, 0f);
				pylonTeleportButtonUI.Height.Set(22f, 0f);
				pylonTeleportButtonUI.Width.Set(60f, 0f);
				pylonTeleportButtonUI.OnClick += delegate
				{
					Teleport(kvp.Value);
				};

				UIText pylonTeleportButtonTextUI = new UIText("Teleport");
				pylonTeleportButtonTextUI.Left.Set(0f, 0f);
				pylonTeleportButtonTextUI.Top.Set(-10f, 0f);
				pylonTeleportButtonUI.Append(pylonTeleportButtonTextUI);
				Append(pylonTeleportButtonUI);
				counter++;
			}
		}

		private void Teleport(Vector2 destination)
        {
			Player player = Main.LocalPlayer;
			player.Teleport(new Vector2(Main.leftWorld + destination.X * 16, Main.topWorld + destination.Y * 16 - 32f));
			JEMusicAndPylon.Instance.TogglePylonUI();
		}
	}
}
