using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using JEMusicAndPylon.Tiles.Abstract;
using Utils = JEMusicAndPylon.Common.Utils;
using JEMusicAndPylon.Common;

namespace JEMusicAndPylon.TileEntities.Abstract
{
    public abstract class PylonTileEntity<MT> : ModTileEntity where MT : ModTile
    {
        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            //The MyTile class is shown later
            return tile.active() && tile.type == ModContent.TileType<MT>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            /*
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            return Place(i, j);
            */
            
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                int width = 3;
                int height = 4;
                NetMessage.SendTileRange(Main.myPlayer, i, j, width, height);

                //Sync the placement of the tile entity with other clients
                //The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }

            //ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
            //Set "tileOrigin" to the same value you set TileObjectData.newTile.Origin to in the ModTile
            /*
            Point16 tileOrigin = new Point16(1, 3);
            int placedEntity = Place(i - tileOrigin.X, j - tileOrigin.Y);
            return placedEntity;
            */
            return Place(i, j);
        }

        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);

                Vector2 originPosition = Position.ToVector2() + new Vector2(1, 3);
                string[] strArr = Utils.SplitCamelCase(typeof(MT).Name);
                JEMusicAndPylonWorld.Instance.PylonCoordinates.Add(typeof(MT).Name, originPosition);

                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("[Server] " + strArr[0] + " Pylon has been placed at coordinate (" + originPosition.X + ", " + originPosition.Y + ")"), Color.White);
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        
        public override void OnKill()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                string[] strArr = Utils.SplitCamelCase(typeof(MT).Name);
                if (JEMusicAndPylonWorld.Instance.PylonCoordinates.ContainsKey(typeof(MT).Name) && Main.netMode == NetmodeID.Server)
                {
                    Vector2 coordinate = JEMusicAndPylonWorld.Instance.PylonCoordinates[typeof(MT).Name];
                    JEMusicAndPylonWorld.Instance.PylonCoordinates.Remove(typeof(MT).Name);
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("[Server] " + strArr[0] + " Pylon has been removed at coordinate (" + coordinate.X + ", " + coordinate.Y + ")"), Color.White);
                    NetMessage.SendData(MessageID.WorldData);
                }
                else
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("[Server] " + strArr[0] + " Pylon hasn't been registered"), Color.White);
                }
            }
        }
    }
}
