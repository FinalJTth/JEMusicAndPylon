using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace JEMusicAndPylon.Common
{
    public static class NPCUtils
    {
        public static List<int> FindNearbyNPCsByConditions(Vector2 position, int minDistance, int maxDistance, Dictionary<string, bool> npcProperties)
        {
            List<int> nearbyNPCs = new List<int>();
            int maxNPCs = Main.npc.Length;
            int minSqr = minDistance * minDistance;
            int maxSqr = maxDistance * maxDistance;

            for (int i = 0; i < maxNPCs; ++i)
            {
                NPC currentNPC = Main.npc[i];
                if (currentNPC.townNPC)
                    // Main.NewText(">> NPC ID : " + currentNPC.GetFullNetName() + " (" + currentNPC.position.X + ", " + currentNPC.position.Y + ")");
                if (currentNPC?.active != true)
                {
                    continue;
                }

                bool invalidNpc = false;
                foreach (KeyValuePair<string, bool> kvp in npcProperties)
                {
                    switch (kvp.Key)
                    {
                        case "isFriendly":
                            if (currentNPC.friendly != kvp.Value)
                                invalidNpc = true;
                            break;
                        case "isTownNPC":
                            if (currentNPC.townNPC != kvp.Value)
                                invalidNpc = true;
                            break;
                    }
                }

                if (invalidNpc)
                    continue;

                Vector2 npcPosition = currentNPC.position.ToTileCoordinates().ToVector2();

                float distSqr = (npcPosition - position).LengthSquared();
                // Main.NewText(">> >> distSqr : " + distSqr);
                if (distSqr < minSqr || distSqr >= maxSqr)
                {
                    continue;
                }

                nearbyNPCs.Add(i);
            }

            return nearbyNPCs;
        }
    }
}