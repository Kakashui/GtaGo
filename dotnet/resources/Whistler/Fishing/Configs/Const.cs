namespace Whistler.Fishing
{
    class Const
    {
        //shared data
        internal const string DATA_ACTIVE = "fshActive";

        //server events
        internal const string EVENT_ACTION = "fshAction";
        internal const string EVENT_BUY_GEAR = "fshBuyGear";
        internal const string EVENT_DROP_FISH = "fshDropFish";
        internal const string EVENT_CELL_FISH = "fshCellFish";
        internal const string EVENT_MINI_GAME_END= "fshMGEnd";

        //client events
        internal const string CLIENT_EVENT_SHOW_ACTION = "fshShow";
        internal const string CLIENT_EVENT_SHOW_FISH_SHOP = "fshFShop";
        internal const string CLIENT_EVENT_UPDATE_CAGE = "fshUpCage";
        internal const string CLIENT_EVENT_MINI_GAME_START = "fshMGStart";
        internal const string CLIENT_EVENET_UPDATE_SEA_SPOTS = "fshUSpots";
        internal const string CLIENT_EVENT_DELETE_SEA_SPOTS = "fshDSpots";

        //variables
        internal const int FISH_BAIT_BY_COUNT = 10;
        internal const int TIME_WAIT_FISH_MIN = 5;
        internal const int TIME_WAIT_FISH_MAX = 20;
        internal const int SPOT_RADIUS = 3;
        internal const float SPOT_RADIUS_IN_SEA = 50f;
        internal const int SPOT_UPDATE_TIME_IN_HOUR = 3;
        internal const int MAP_TIME_EXPIRED_IN_HOUR = 3;
        internal const int MIN_FISH_LVL_IN_SEA = 8;
        internal const int MAX_FISH_LVL_ON_LAND = 17;
        internal const int MAX_FISHER_LVL = 10;

        internal static readonly int[] EXP_ON_LVL = { 0, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120 };

        //rod animations
        internal const string ANIM_DICT = "amb@world_human_stand_fishing@idle_a";
        internal const string ANIM_NAME = "idle_c";

        //rpd model
        internal const string ROD_MODEL = "prop_fishing_rod_01";

        //BONES
        internal const string BONE_LEFTHAND = "SKEL_L_Hand";

        //fishing spot blips
        internal const uint BLIP_SPOT_SPRITE = 317;
        internal const byte BLIP_SPOT_COLOR = 29;

    }
}
