using Terraria;

namespace JEMusicAndPylon.Common
{
    public enum Zone
    {
        Forest,
        ForestDirt,
        Cavern,
        Desert,
        DesertDirt,
        DesertCavern,
        Sandstorm,
        Snow,
        SnowDirt,
        SnowCavern,
        Blizzard,
        Jungle,
        JungleDirt,
        JungleCavern,
        Mushroom,
        Corruption,
        CorruptionDirt,
        CorruptionCavern,
        Crimson,
        CrimsonDirt,
        CrimsonCavern,
        Meteor,
        Space,
        Ocean,
        Hell,
        Dungeon,
        Hallow,
        HallowDirt,
        HallowCavern,
        Nebula,
        Solar,
        Stardust,
        Vortex,
        Town,
        Graveyard,
    }

    public static class PlayerUtils
    {
        public static Zone GetPlayerZone(Player player)
        {
            if (player.ZoneCorrupt && player.ZoneDirtLayerHeight)
                return Zone.CorruptionDirt;
            if (player.ZoneCorrupt && player.ZoneRockLayerHeight)
                return Zone.CorruptionCavern;
            if (player.ZoneCorrupt)
                return Zone.Corruption;
            if (player.ZoneCrimson && player.ZoneDirtLayerHeight)
                return Zone.CrimsonDirt;
            if (player.ZoneCrimson && player.ZoneRockLayerHeight)
                return Zone.CrimsonCavern;
            if (player.ZoneCrimson)
                return Zone.Crimson;
            if (player.ZoneSnow && player.ZoneDirtLayerHeight)
                return Zone.SnowDirt;
            if (player.ZoneSnow && player.ZoneRockLayerHeight)
                return Zone.SnowCavern;
            if (player.ZoneSnow)
                return Zone.Snow;
            if (player.ZoneJungle && player.ZoneDirtLayerHeight)
                return Zone.JungleDirt;
            if (player.ZoneJungle && player.ZoneRockLayerHeight)
                return Zone.JungleCavern;
            if (player.ZoneJungle)
                return Zone.Jungle;
            if (player.ZoneDesert && player.ZoneDirtLayerHeight)
                return Zone.DesertDirt;
            if (player.ZoneDesert && player.ZoneRockLayerHeight || player.ZoneUndergroundDesert)
                return Zone.DesertCavern;
            if (player.ZoneDesert)
                return Zone.Desert;
            if (player.ZoneBeach)
                return Zone.Ocean;
            if (player.ZoneDungeon)
                return Zone.Dungeon;
            if (player.ZoneGlowshroom)
                return Zone.Mushroom;
            if (player.ZoneHoly && player.ZoneDirtLayerHeight)
                return Zone.HallowDirt;
            if (player.ZoneHoly && player.ZoneRockLayerHeight)
                return Zone.HallowCavern;
            if (player.ZoneHoly)
                return Zone.Hallow;
            if (player.ZoneMeteor)
                return Zone.Meteor;
            if (player.ZoneSkyHeight)
                return Zone.Space;
            if (player.ZoneSandstorm && player.ZoneDesert)
                return Zone.Sandstorm;
            if (player.ZoneSandstorm && player.ZoneSnow)
                return Zone.Sandstorm;
            if (player.ZoneSandstorm && player.ZoneSnow)
                return Zone.Sandstorm;
            if (player.ZoneUnderworldHeight)
                return Zone.Hell;
            if (player.ZoneTowerNebula)
                return Zone.Nebula;
            if (player.ZoneTowerSolar)
                return Zone.Solar;
            if (player.ZoneTowerStardust)
                return Zone.Stardust;
            if (player.ZoneTowerVortex)
                return Zone.Vortex;
            return Zone.Forest;
        }
        public static bool WillPlayMorningRainMusic()
        {
            Player player = Main.player[Main.myPlayer];
            Zone playerZone = GetPlayerZone(player);
            if (playerZone == Zone.Forest && Main.dayTime && Main.time >= 0 && Main.time <= 10800 && !Main.eclipse)
            {
                return true;
            }
            return false;
        }
    }
}
