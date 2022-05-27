using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using NetUtils = JEMusicAndPylon.Common.NetUtils;

namespace JEMusicAndPylon
{
    public class JEMusicAndPylonWorld : ModWorld
    {
		public static JEMusicAndPylonWorld Instance { get; set; }

		private Dictionary<string, Vector2> _pylonCoordinates;

		public Dictionary<string, Vector2> PylonCoordinates
        {
			get { return _pylonCoordinates; }
			set { _pylonCoordinates = value; }
        }

		public JEMusicAndPylonWorld()
		{
			Instance = this;
		}

		public override void Initialize()
		{
			_pylonCoordinates = new Dictionary<string, Vector2>();
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
			IList<string> coordinateStringList = tag.GetList<string>("pylonCoordinates");
			foreach (string str in coordinateStringList)
			{
				string[] strArray = str.Split(':');
				if (strArray.Length == 3 && int.TryParse(strArray[1], out int resultX) && int.TryParse(strArray[2], out int resultY))
				{
					_pylonCoordinates[strArray[0]] = new Vector2(resultX, resultY);
				}
				else
				{
				}
			}
		}
		
        public override void NetSend(BinaryWriter writer)
        {
			/*
			if (Main.netMode == NetmodeID.Server)
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Server Sending Packet ..."), Color.White);
			else
				Main.NewText("Client Sending Packet ...");
			*/

			List<string> coordinateStringList = new List<string>();
			foreach (KeyValuePair<string, Vector2> kvp in _pylonCoordinates)
			{
				string x = ((int)kvp.Value.X).ToString();
				string y = ((int)kvp.Value.Y).ToString();
				coordinateStringList.Add(kvp.Key + ":" + x + ":" + y);
			}
			NetUtils.WriteList(coordinateStringList, writer);
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            base.TileCountsAvailable(tileCounts);
        }

        public override void NetReceive(BinaryReader reader)
        {
			/*
			if (Main.netMode == NetmodeID.Server)
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Server Receiving Packet ..."), Color.White);
			else
				Main.NewText("Client Receiving Packet ...");
			*/
			List<string> coordinateStringList = new List<string>();
			while(true)
            {
				string rStr = reader.ReadString();
				if (rStr == "stopList")
                {
					/*
					if (Main.netMode == NetmodeID.Server)
						NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("server end read"), Color.White);
					else
						Main.NewText("client end read");
					*/
					break;
				}
				/*
				if (Main.netMode == NetmodeID.Server)
					NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("server read -> " + rStr), Color.White);
				else
					Main.NewText("client read -> " + rStr);
				*/
				coordinateStringList.Add(rStr);
			}
			_pylonCoordinates = new Dictionary<string, Vector2>();
			foreach (string str in coordinateStringList)
			{
				string[] strArray = str.Split(':');
				if (strArray.Length == 3 && int.TryParse(strArray[1], out int resultX) && int.TryParse(strArray[2], out int resultY))
				{
					// Main.NewText("Load pylon \"" + strArray[0] + "\"" + " with coordinate (" + resultX + ", " + resultY + ")");
					_pylonCoordinates[strArray[0]] = new Vector2(resultX, resultY);
				}
				else
				{
					// Main.NewText("The coordinate was saved in an invalid format");
				}
			}
		}

        public override void PostUpdate()
        {
            base.PostUpdate();
        }

        public void Teleport(Player player, Vector2 destination)
		{
			player.Teleport(new Vector2(Main.leftWorld + destination.X * 16, Main.topWorld + destination.Y * 16 - 32f));
		}
	}
}
