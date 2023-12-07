import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

import inventory from './inventory';
import familyMenu from './familyMenu'
import animationsMenu from './animationsMenu'
import stockMenu from './stockMenu'
import businessMenu from './businessMenu'
import notifyList from './notifyList'
//import devTools from './devTool'
import radio from './radio'
import poker from './poker'
import arenaMenu from './arenaMenu'
import passport from './passport'
import weddingMenu from './weddingMenu'
import cashMachine from './cashMachine'
import licenses from './licenses'
import localization from './localization'
import speedometer from './speedometer'
import shopAuto from './shopAuto'
import dial from './dial'
import playerList from './playerList'
import gasStation from './gasStation'
import weaponShop from './weaponShop'
import chipsTradeMenu from './chipsTradeMenu'
import questionMenu from './questionMenu'
import inputMenu from './inputMenu'
import sounds from './sounds'
import deathScreen from './deathScreen'
import craftMenu from './craftMenu'
import stockPoint from './stockPoint'
import auth from './auth'
import characterSelect from './characterSelect'
import clothingStore from './clothingStore'
import fishing from './fishing'
import hud from './hud'
import mouseMove from './mouseMove'
import maskShop from './maskShop'
import carTunningMenu from './carTunningMenu'
import barbershop from './barbershop'
import homePurchase from './homePurchase'
import businessPurchase from './businessPurchase'
import dressingUpMenu from './dressingUpMenu'
import tattooShop from './tattooShop'
import roulette from './roulette'
import captures from './captures'
import elevator from './elevator'
import stockInfo from './stockInfo'
import roundTheClockShop from './roundTheClockShop'
import smartphone from './smartphone'
import autoSchool from './autoSchool'
import dialogMenu from './dialogMenu'
import huntingShop from './huntingShop'
import vehicleRepairsServices from './vehicleRepairsServices'
import technicalCertificate from './technicalCertificate'
import truckersMenu from './truckersMenu'
import portOrders from './portOrders'
import dialogWindow from './dialogWindow'
import burgerShop from './burgerShop'
import reportMenu from './reportMenu'
import illegalShop from './illegalShop'
import interactionMenu from './interactionMenu'
import homeMenu from './homeMenu'
import furnitureShop from './furnitureShop'
import optionsMenu from './optionsMenu'
import carDealership from './carDealership'
import personalDigitalAssistant from './personalDigitalAssistant'
import friskInterface from './friskInterface'
import cityHallWeb from './cityHallWeb'
import events from './events'
import voteMenu from './voteMenu'
import newDonateShop from './newDonateShop'
import parliamentPortal from './parliamentPortal'
import spawnSelect from './spawnSelect'
import battlegroundReg from './battlegroundReg'
import battlegroundStats from './battlegroundStats'
import certificate from './certificate'
import casinoOwner from './casinoOwner'
import news from './news'
import settingAmountCarSales from './settingAmountCarSales'
import customization from './customization'
import war from './war'
import carWash from './carWash'
import warForEnterprice from './warForEnterprice'
import rentVehicle from './rentVehicle'
import fishingStore from './fishingStore'
import musicMenu from './musicMenu'
import weedFarm from './weedFarm'
import bank from './bank'
import transfersConfirmation from './transfersConfirmation'
import weaponsSetting from './weaponsSetting'
import hudQuestMessage from './hud/questMessage'

import sellCar from './sellCar'
import farmHouse from './farmHouse'
import bar from './bar'
import priceMenu from './priceMenu'
import costumeMenu from './costumeMenu'
import timerTemo from './timerTemo'
import handlingModShop from './handlingModShop'
import robberyShop from './robberyShop'


import gameMakeWeapon from './minigames/makeWeapon'
import gameMetalPlant from './minigames/metalPlant'
import gameLockpick from './minigames/lockpick'

export default new Vuex.Store({
    namespaced: true,
    state: {
        currentPage: undefined,
        loadScreen: true,
        devPage: "auth",
        showDevTool: false,
        isSmartphone: false,
        inventoryEnable: false,
        isBinoculars: false,
        background: 1,
        family: {
            isRequest: true,
            name: '',
            money: 500
        }
    },
    mutations: {
        setPage(state, page) {
            state.currentPage = page;
        },
        inventoryEnabled(state, val){
            state.inventoryEnable = val;
        },
        setFamilyData(state, data) {
            state.family = data
            state.currentPage = 'Family'
        },
        setPhoneActive(state, isActive) {
            state.isSmartphone = isActive;
        },
        setBackground(state, val){
            state.background = val;
        },
        setLoadScreen(state, toggle){
            state.loadScreen = toggle == true;
        },
        showBinoculars(state, toggle){
            state.isBinoculars = toggle;
        }
    },
    actions: {},
    modules: {
        inventory,
        familyMenu,
        animationsMenu,
        stockMenu,
        businessMenu,
        notifyList,
        //devTools,
        poker,
        radio,
        arenaMenu,
        passport,
        weddingMenu,
        localization,
        cashMachine,
        licenses,
        gasStation,
        shopAuto,
        dial,
        playerList,
        speedometer,
        weaponShop,
        chipsTradeMenu,
        questionMenu,
        inputMenu,
        sounds,
        deathScreen,
        craftMenu,
        stockPoint,
        auth,
        characterSelect,
        fishing,
        hud,
        clothingStore,
        mouseMove,
        maskShop,
        carTunningMenu,
        barbershop,
        homePurchase,
        businessPurchase,
        dressingUpMenu,
        tattooShop,
        roulette,
        captures,
        elevator,
        stockInfo,
        roundTheClockShop,
        smartphone,
        autoSchool,
        dialogMenu,
        huntingShop,
        vehicleRepairsServices,
        technicalCertificate,
        truckersMenu,
        portOrders,
        dialogWindow,
        burgerShop,
        reportMenu,
        illegalShop,
        interactionMenu,
        homeMenu,
        furnitureShop,
        optionsMenu,
        carDealership,
        personalDigitalAssistant,
        friskInterface,
        cityHallWeb,
        events,
        voteMenu,
        newDonateShop,
        parliamentPortal,
        spawnSelect,
        battlegroundReg,
        battlegroundStats,
        gameMakeWeapon,
        certificate,
        casinoOwner,
        news,
        settingAmountCarSales,
        sellCar,
        farmHouse,
        bar,
        customization,
        war,
        carWash,
        warForEnterprice,
        rentVehicle,
        fishingStore,
        musicMenu,
        weedFarm,
        bank,
        transfersConfirmation,
        weaponsSetting,
        priceMenu,
        costumeMenu,
        gameMetalPlant,
        timerTemo,
        gameLockpick,
        hudQuestMessage,
        handlingModShop,
        robberyShop
    }
})