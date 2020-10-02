using System;

[Flags]
public enum TileType
{
    Water = 128, River = 1, Sand = 2, Land = 4, Forest = 8, Road = 16, Mountain = 32, Impassible = 64
}
public enum Direction
{
    North = 0, NorthEast = 1, SouthEast = 2,
    South = 3, SouthWest = 4, NorthWest = 5
}

public enum Side
{
    Blue = 0, Red = 1, None = 2
}
[Flags]
public enum UnitType
{
    SmallUnit = 1, MediumUnit = 2, BigUnit = 4, AirCraft = 8, Bomber = 16, Transport = 32
   //LAV, Tank,MobileArtillery,HAV,MissilePlatform,LandTransports
}
[Flags]
public enum UnitUIType
{
    Normal=1, Naval=2,Special=4
}

public enum FortType
{
    Small = 0, Medium = 1, Large = 2
}

public enum PhaseType
{
   InitialDisclosure= 0,InitialBuying = 1, Guerilla = 2, Combat = 3, Recruitment = 4, Disclosing = 5, DisclosingBuying =6
}