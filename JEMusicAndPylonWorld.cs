using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace JEMusicAndPylon
{
    public class JEMusicAndPylonWorld : ModPlayer
    {
		public static JEMusicAndPylonWorld Instance { get; set; }

		private Dictionary<string, Vector2> _pylonCoordinates = new Dictionary<string, Vector2>();

		public Dictionary<string, Vector2> PylonCoordinates
        {
			get { return _pylonCoordinates; }
			set { _pylonCoordinates = value; }
        }

		public JEMusicAndPylonWorld()
		{
			Instance = this;
		}

		public override TagCompound Save()
		{
			List<string> coordinateStringList = new List<string>();
			foreach (KeyValuePair<string, Vector2> kvp in _pylonCoordinates)
			{
				string x = ((int)kvp.Value.X).ToString();
				string y = ((int)kvp.Value.Y).ToString();
				coordinateStringList.Add(kvp.Key + ":" + x + ":" + y);
			}
			return new TagCompound
			{
				{ "pylonCoordinates", coordinateStringList }
			};
		}

		public override void Load(TagCompound tag)
		{
			if (tag.ContainsKey("pylonCoordinates"))
            {
				Main.NewText("Loading tag \"pylonCoordinates\" ...");
				IList<string> coordinateStringList = tag.GetList<string>("pylonCoordinates");
				foreach (string str in coordinateStringList)
                {
					string[] strArray = str.Split(':');
					if (strArray.Length == 3 && int.TryParse(strArray[1], out int resultX) && int.TryParse(strArray[2], out int resultY))
					{
						Main.NewText("Load pylon \"" + strArray[0] + "\"" + " with coordinate (" + resultX + ", " + resultY + ")");
						_pylonCoordinates[strArray[0]] = new Vector2(resultX, resultY);
					}
					else
                    {
						Main.NewText("The coordinate was saved in an invalid format");
					}
				}
            }
		}

		public void Teleport(Player player, Vector2 destination)
		{
			player.Teleport(new Vector2(Main.leftWorld + destination.X * 16, Main.topWorld + destination.Y * 16 - 32f));
		}
	}
}
