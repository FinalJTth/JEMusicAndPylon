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
		private Dictionary<string, Vector2> pylonCoordinates = new Dictionary<string, Vector2>();

		public Dictionary<string, Vector2> PylonCoordinates
        {
			get { return pylonCoordinates; }
			set { pylonCoordinates = value; }
        }

		public override TagCompound Save()
		{
			List<string> coordinateStringList = new List<string>();
			foreach (KeyValuePair<string, Vector2> keyValuePair in pylonCoordinates)
			{
				string x = ((int)keyValuePair.Value.X).ToString();
				string y = ((int)keyValuePair.Value.Y).ToString();
				coordinateStringList.Add(keyValuePair.Key + ":" + x + ":" + y);
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
				IList<string> coordinateStringList = tag.GetList<string>("pylonCoordinates");
				foreach (string keyValueString in coordinateStringList)
                {
					string[] strArray = keyValueString.Split(':');
					if (strArray.Length == 3 && int.TryParse(strArray[1], out int resultX) && int.TryParse(strArray[2], out int resultY))
					{
						pylonCoordinates[strArray[0]] = new Vector2(resultX, resultY);
					}
				}
            }
		}
	}
}
