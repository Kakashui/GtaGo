<template>
  <div id="app" :class="{development: isDev}" :style="{'font-family': font}">      
    <div class="app-bg" v-if="background > 0" :style="{'background': `url(/img/backgrounds/${background}.jpg) no-repeat center`}"></div>
    <div class="app-binoculars" v-if="isBinoculars"></div>
    <Hud v-show="!currentPage && hudShow && !inventoryEnable && !deathScreen"/>
    <DeathScreen v-if="deathScreen"/>
    <DevTools v-if="showDevTool"/>
    <smartphone v-show="isSmartphone" />
    <Inventory v-show="inventoryEnable"/>
    <CheckInventory v-if="checkInventory !== undefined" :checkInventory="checkInventory"/>
    <component :is="currentPage"></component>
    <Chat v-show="!inventoryEnable && (currentPage === undefined || currentPage === '')"/>
    <MouseMove />
    <QuestionMenu />
    <notify-list />
    <dialog-menu v-if="dialogMenuShow"/>
    <dialog-menu-overlay v-if="dialogMenuShow"/>
    <TimerTemo v-if="timeTemo > 0" :time="timeTemo" :length="lengthTemo"/>
    <LoadScreen v-if="loadScreen"/>
    <!-- <DevConsole v-if="console.show"/> -->
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import Inventory from "./views/Inventory"
import CheckInventory from "./views/Inventory/CheckInventory"
import JudgeMenu from "./views/JudgeMenu"
import Family from "./views/Family"
import FamilyMenu from './views/FamilyMenu'
import AnimationsMenu from './views/AnimationsMenu'
import StockMenu from './views/StockMenu'
import BusinessMenu from './views/BusinessMenu'
import NotifyList from './views/NotifyList'
import Radio from './views/Radio'
import Poker from './views/Poker'
import ArenaMenu from './views/ArenaMenu'
import WeddingMenu from './views/WeddingMenu'
import WeddingNotification from './views/WeddingNotification'
import Passport from './views/Passport'
import CashMachine from './views/CashMachine'
import Licenses from './views/Licenses'
import FurnitureHud from './views/FurnitureHud'
import GasStation from './views/GasStation'
import ShopAuto from './views/ShopAuto'
import Dial from './views/Dial'
import PlayerList from './views/PlayerList'
import WeaponShop from './views/WeaponShop'
import ChipsTradeMenu from './views/ChipsTradeMenu'
import QuestionMenu from './views/QuestionMenu'
import InputMenu from './views/InputMenu'
import DeathScreen from './views/DeathScreen'
import CraftMenu from './views/CraftMenu'
import StockPoint from './views/StockPoint'
import Auth from './views/Auth'
import CharacterSelect from './views/CharacterSelect'
import ClothingStore from './views/ClothingStore'
import Hud from './views/Hud'
import MouseMove from './views/MouseMove'
import MaskShop from './views/MaskShop'
import CarTunningMenu from './views/CarTunningMenu'
import WorkMiniGame from './views/WorkMiniGame'
import Barbershop from './views/Barbershop'
import HomePurchase from './views/HomePurchase'
import BusinessPurchase from './views/BusinessPurchase'
import DressingUpMenu from './views/DressingUpMenu'
import TattooShop from './views/TattooShop'
import Chat from './views/Chat'
import Captures from './views/Captures'
import Elevator from './views/Elevator'
import StockInfo from './views/StockInfo'
import RoundTheClockShop from './views/RoundTheClockShop'
import Smartphone from './views/Smartphone'
import AutoSchool from './views/AutoSchool'
import DialogMenu from './views/DialogMenu'
import DialogMenuOverlay from './views/DialogMenu/DialogMenuOverlay'
import HuntingShop from './views/HuntingShop'
import VehicleRepairsServices from './views/VehicleRepairsServices'
import TechnicalCertificate from './views/TechnicalCertificate'
import PortOrders from './views/PortOrders'
import DialogWindow from './views/DialogWindow'
import BurgerShop from './views/BurgerShop'
import TruckersMenu from './views/TruckersMenu'
import ReportMenu from './views/ReportMenu'
import IllegalShop from './views/IllegalShop'
import InteractionMenu from './views/InteractionMenu'
import HomeMenu from './views/HomeMenu'
import FurnitureShop from './views/FurnitureShop'
import OptionsMenu from './views/OptionsMenu'
import CarDealership from './views/CarDealership'
import PersonalDigitalAssistant from './views/PersonalDigitalAssistant'
import FriskInterface from './views/FriskInterface'
import GameTablet from './views/GameTablet'
import SuperMario from './views/GameTablet/SuperMario'
import Snake from './views/GameTablet/Snake'
import CityHallWeb from './views/CityHallWeb'
import LoadScreen from './views/LoadScreen'
import Events from './views/Events'
import VoteMenu from './views/VoteMenu'
import NewDonateShop from './views/NewDonateShop'
import ParliamentPortal from './views/ParliamentPortal'
import SpawnSelect from './views/SpawnSelect'
import BattlegroundReg from './views/BattlegroundReg'
import BattlegroundStats from './views/BattlegroundStats'
import Certificate from './views/Certificate'
import TakeDonateItem from './views/NewDonateShop/TakeDonateItem'
import GameMakeWeapon from './views/Minigames/MakeWeapon'
import CasinoOwner from './views/CasinoOwner'
import News from './views/News'
import SettingAmountCarSales from './views/SettingAmountCarSales'
import SellCar from './views/SellCar'
import FarmHouse from './views/FarmHouse'
import Bar from './views/Bar'
import Customization from './views/Customization'
import War from './views/War'
import CarWash from './views/CarWash'
import SelectLanguage from './views/SelectLanguage'
import WarForEnterprice from './views/WarForEnterprice'
import Roulette from './views/Roulette'
import RentVehicle from './views/RentVehicle'
import FishingStore from './views/FishingStore'
import MusicMenu from './views/MusicMenu'
import RemovingTattoo from './views/RemovingTattoo'
import Bank from './views/Bank'
import WeedFarm from './views/WeedFarm'
import TransfersConfirmation from './views/TransfersConfirmation'
import WeaponsSetting from './views/WeaponsSetting'
import PriceMenu from './views/PriceMenu'
import CostumeMenu from './views/CostumeMenu'
import GameMetalPlant from './views/Minigames/MetalPlant'
import TimerTemo from './views/TimerTemo'
import GameLockpick from './views/Minigames/Lockpick'
import HandlingModShop from './views/HandlingModShop'


//import DevTools from './views/DevTools'
//import DevConsole from './views/DevTools/Console'

export default {
  name: 'App',
  computed:{
    ...mapState(['currentPage', 'showDevTool', 'isSmartphone', 'devPage', 'inventoryEnable', 'background', 'loadScreen', 'isBinoculars']),
    ...mapState('inventory',['checkInventory']),
    timeTemo(){
      return this.$store.state.timerTemo.time;
    },
    lengthTemo(){
      return this.$store.state.timerTemo.length;
    },
    //...mapState('devTools',['console']),
    ...mapGetters('localization', ['font']),
    deathScreen(){
      return this.$store.state.deathScreen.active;
    },
    appWidthUnit: function () {
      const appWidth = document.getElementById('app').offsetWidth;
      const appHeight = document.getElementById('app').offsetHeight;
      let widthUnit = null;
      if (appWidth/appHeight > 1.7) {
        widthUnit = Math.ceil(appHeight/54) + 'px';
      } else {
        widthUnit = Math.ceil(appWidth/100) + 'px';
      }
      return widthUnit;
    },
    hudShow(){
      return this.$store.state.hud.hudShow;
    },
    isDev(){
      return process.env.NODE_ENV == 'development';
    },
    dialogMenuShow: function() {
      return this.$store.state.dialogMenu.show
    }
  },
  methods:{
    ...mapMutations("devTools", ["addConsoleMessage"]),
    setData(fnc, data){
      this.$store.commit(fnc, data)
    },
    dispatch(fnc, data){
      this.$store.dispatch(fnc, data)
    },
    openFamily(isRequest, name="", money=0){      
      this.setData('setFamilyData', {isRequest, name, money})
    },
    close(){
      this.setData("setPage", "")
    }
  },
  components: {
    Inventory,
    CheckInventory,
    JudgeMenu,
    Family,
    FamilyMenu,
    AnimationsMenu,
    StockMenu,
    BusinessMenu,
    NotifyList,
    Radio,
    Poker,
    ArenaMenu,
    WeddingMenu,
    WeddingNotification,
    Passport,
    CashMachine,
    Licenses,
    FurnitureHud,
    GasStation,
    ShopAuto,
    Dial,
    PlayerList,
    WeaponShop,
    ChipsTradeMenu,
    QuestionMenu,
    InputMenu,
    DeathScreen,
    CraftMenu,
    StockPoint,
    Auth,
    CharacterSelect,
    Hud,
    ClothingStore,
    MouseMove,
    MaskShop,
    CarTunningMenu,
    WorkMiniGame,
    Barbershop,
    HomePurchase,
    BusinessPurchase,
    DressingUpMenu,
    TattooShop,
    Chat,
    Captures,
    Elevator,
    StockInfo,
    RoundTheClockShop,
    Smartphone,
    AutoSchool,
    DialogMenu,
    DialogMenuOverlay,
    HuntingShop,
    VehicleRepairsServices,
    TechnicalCertificate,
    TruckersMenu,
    PortOrders,
    DialogWindow,
    BurgerShop,
    ReportMenu,
    IllegalShop,
    InteractionMenu,
    HomeMenu,
    CarDealership,
    FurnitureShop,
    OptionsMenu,
    PersonalDigitalAssistant,
    FriskInterface,
    GameTablet,
    SuperMario,
    Snake,
    CityHallWeb,
    LoadScreen,
    Events,
    VoteMenu,
    NewDonateShop,
    ParliamentPortal,
    SpawnSelect,
    BattlegroundReg,
    BattlegroundStats,
    TakeDonateItem,
    GameMakeWeapon,
    Certificate,
    CasinoOwner,
    News,
    SettingAmountCarSales,
    SellCar,
    FarmHouse,
    Bar,
    Customization,
    War,
    CarWash,
    SelectLanguage,
    WarForEnterprice,
    Roulette,
    RentVehicle,
    FishingStore,
    MusicMenu,
    RemovingTattoo,
    Bank,
    WeedFarm,
    TransfersConfirmation,
    WeaponsSetting,
    PriceMenu,
    CostumeMenu,

    GameMetalPlant,
    TimerTemo,
    GameLockpick,
    HandlingModShop
    
    //DevTools,
    //DevConsole,
  },
  
  async mounted(){
    this.$store.dispatch("localization/loadLangs");
    window.setData = this.setData;
    window.dispatch = this.dispatch;
    window.close=this.close;
    window.openFamily = this.openFamily;
    const htmlTag = document.getElementsByTagName('html')[0];
    htmlTag.style.fontSize = this.appWidthUnit;
    if(this.isDev){
      this.$store.dispatch("localization/setLang", "ru");
      this.setData("setPage", this.devPage);
      this.$store.commit("setLoadScreen", false);
      //this.$store.commit("deathScreen/setTime", 300)
     // this.setData("timerTemo/setTimer", 300)
      // this.$store.commit('inventory/updateCheckInventory', {
      //     equip:[
      //         [[0,-1],[0,-1],[0,-1],[0,-1],[0,-1],[0,-1],[0,-1],[0,-1],[0,-1],[0,-1]],
      //         [[0,-1],[0,-1],[0,-1],[0,-1]]
      //     ],
      //     items:[[23,1,1,0],[13, 1, 2, 1],[76, 1, 13, 0],[18, 15, 5, 1],[5, 1, 7, 0, 2, 5000]]
      // });
    }
    window.consoleLog = this.addConsoleMessage;
  }
}
</script>

<style lang="scss" src="./styles/app.scss"/>
