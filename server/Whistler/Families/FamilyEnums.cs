using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Families
{
    public enum FamilyVehicleAccess
    {
        None,
        Using,
        LSCustom,
        FullAccess,
    }
    public enum FamilyHouseAccess
    {
        EnterHouse,
        OpenDoors,
        UpgradeGarage,
        FullAccess,
    }
    public enum FamilyFurnitureAccess
    {
        None,
        OpenFurniture,
        MovingFurniture,
        ManagementFurniture,
    }
    public enum BattleStatus
    {
        WaitConfirm = 0,
        Confirm = 1,
        Fighting = 2,
        Canceled = 3,
        FinishedWithoutAFight = 4,
        FightIsOver = 5,
        NotCarriedOut = 6
    }
    public enum BattleLocation
    {
        None = -1,
        ElBurroHeights = 1,
        ElysianIsland = 2,
        GrandSenoraDesert = 3,
        TheCayoPerico = 4,
        StabCity = 5,
        Galilee = 6,
        RedwoodLights = 7,
        WeedFarmBattle = 8
    }
    public enum CreateBattleResult
    {
        Success = 0,
        TooEarly,
        BusinessIsAttaked,
        LocationIsOccupied,
        AttackersIsBusy,
        DefendersIsBusy,
        ItIsYourBusiness,
        BadTime,
    }
    public enum FamilyActions
    {
        GoToDemorgan,
        AddMoney,
        SubMoney,
    }


    public enum FamilyMPType
    {
        Invalid,
        IslandCapture,
        BusinessWar,
    }
    public enum WarCompanies
    {
        DavisQuartz,
        TheCayoPerico,
        ElBurroHeights,
        Sawmill,
        MurrietaHeights,
        UnionGrainSupply,
    }
}
