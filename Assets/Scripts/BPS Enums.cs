namespace BPS
{
    public struct PlayerIntentions
    {
        public Player player;
        public int playerId;
        public int playerVotes;
        public float playerPct;
    }

    public static class Constants
    {
        public const string DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        public const string TIME_FORMAT = "HH:mm:ss";
        public const int MAX_ACTIONS = 15;
    }

    public enum CursorState
    {
        Default,
        PanUp,
        PanLeft,
        PanDown,
        PanRight,
        Select,
        Move,
        Defend,
        Attack,
        RallyPoint,
        Ability
    }

    public enum InGameUseMode
    {
        UNDEFINED,
        EDITOR,
        PLAY
    }

    public enum MatchStatus
    {
        NO_MATCH,
        LOADING,
        INITIALIZING,
        FIRST_ROUND,
        FIRST_RESULTS,
        SECOND_ROUND,
        SECOND_RESULTS
    }

    public enum WindowCodes_Editor
    {
        EDITOR_MENU = -1,
        NO_WINDOW,
        OBJECTIVES_AND_HELP,
        QUICK_SAVE,
        QUICK_LOAD,
        PLAY_OR_EDIT,
        MAP_EDIT,
        CITY_EDIT,
        ZONING_EDIT,
        POPULATION_EDIT,
        VIEWER_OPTIONS,
        LEVEL_STATISTICS
    }

    public enum WindowCodes_Play
    {
        PLAY_MENU = -1,
        NO_WINDOW,
        OBJECTIVES_AND_HELP,
        QUICK_SAVE,
        QUICK_LOAD,
        PLAY_OR_EDIT,
        ELECTORAL_CAMPAIGN,
        BUILDINGS,
        COMMON_UNITS,
        UNDESIRABLES,
        FINANCES_AND_CORRUPTION,
        VOTES_AND_DIPLOMACY
    }
}

namespace BPS.Map
{
    public enum TileChunkTypes
    {
        UNKNOWN,
        LAND,
        CLIFFLAND,
        SUBMERGED,
        FLOODED
    }

    public enum TileTypes
    {
        UNKNOWN,
        GROUND,
        WATER,
        CLIFF_GROUND,
        RIVER,
        VEGETATION,
        //SUBMERGED,
        //FLOODED,
        ROAD,
        CONCRETE
    }
}

namespace BPS.Population
{
    public enum CityBlockUsage
    {
        UNKNOWN,
        TOWN_CENTER,
        NOT_ZONED,
        ZONED
    }

    public enum PurposeZoning
    {
        UNKNOWN,
        RESIDENTIAL,
        COMMERCIAL,
        INDUSTRIAL
    }

    public enum EconomicalZoning
    {
        UNKNOWN,
        POOR,
        MEDIUM,
        RICH
    }

    public enum ReligionLevel
    {
        No_Religion,
        Low, 
        Medium, 
        High
    }

    public enum EducationLevel
    {
        No_Education,
        Low,
        Medium,
        High
    }

    public enum SoccerFanaticism
    {
        No_Soccer_Fanaticism,
        Low,
        Medium,
        High
    }

    public enum PoliticalPosition
    {
        None = -1,
        AuthoriatianLeft, AuthoriatianCenter, AuthoriatianRight,
        EconomicalLeft, EconomicalCenter, EconomicalRight,
        LibertarianLeft, LibertarianCenter, LibertarianRight
    }

    //public enum PoliticalCompass_Horizontal
    //{
    //    Undefined,
    //    Leftist,
    //    Centrist,
    //    Rightist
    //}

    //public enum PoliticalCompass_Vertical
    //{
    //    Undefined,
    //    Authoritarian,
    //    Neutral,
    //    Libertarian
    //}
}

namespace BPS.InGame
{
    public enum PoliticalPositionComparison
    {
        NO_DIFFERENCE,
        GOOD,
        VERY_GOOD,
        BAD,
        VERY_BAD
    }

    public enum LevelObjectType
    {
        UNDEFINED,
        BUILDING,
        UNIT
    }

    public enum PlacementType
    {
        UNDEFINED,
        GROUND,
        AIR
    }

    public enum CombatStance
    {
        NO_COMBAT,      //also known as PASSIVE
        DEFENSIVE,
        OFFENSIVE
    }

    public enum ActionCommandType
    {
        MOVE = 0,
        STOP = 1,
        //PATROL = 2,
        DEFEND = 3,
        ATTACK = 4,
        //LOAD_OR_GRAB,
        //UNLOAD_OR_RELEASE,
        //DEPLOY_OR_MOBILIZE,
        RALLY_POINT = 9,
        ABILITY = 10
    }

    public enum CastType
    {
        INSTANTANEOUS,
        TARGETABLE,
        PASSIVE
    }

    public enum RangeType
    {
        SELF,
        SAME_AS_ATTACK_RANGE,
        SAME_AS_SIGHT_RANGE,
        MELEE,
        RANGED,
        GLOBAL
    }

    public enum TargetType
    {
        NO_TYPE,
        POINT,
        BUILDING,
        UNIT,
        ANY
    }

    public enum TargetDiplomacy
    {
        NO_DIPLOMACY,
        NEUTRAL,
        ALLY,
        ENEMY,
        ANY
    }

    public enum TargetOwner
    {
        NO_OWNER,
        SELF,
        ANOTHER_PLAYER,
        ANY_PLAYER,
        THE_CITY
    }
}

namespace BPS.InGame.Error
{
    public enum ActionCostError
    {
        OK,
        IN_COOLDOWN,
        NO_MONEY,
        NO_HP,
        NO_MP
    }

    public enum ActionRangeTypeError
    {
        OK,
        OUTSIDE_MIN_RANGE,
        OUTSIDE_MAX_RANGE,
        OUTSIDE_PLAYING_AREA
    }

    public enum ActionTargetTypeError
    {
        OK,
        NOT_AN_BUILDING,
        NOT_AN_UNIT
    }

    public enum ActionTargetDiplomacyError
    {
        OK,
        NOT_NEUTRAL,
        NOT_ALLIED,
        NOT_ENEMY
    }

    public enum ActionTargetOwnerError
    {
        OK,
        NOT_SELF,
        NOT_OTHER_PLAYER,
        NOT_ANY_PLAYER,
        NOT_THE_CITY
    }
}

namespace BPS.EventLog
{
    public enum EventLogType
    {
        DEBUG = -1,
        GENERIC
    }

    public enum EventLogSource
    {
        DEBUG = -1,
        GENERIC,
        PLAYER,
        THE_CITY
    }
}