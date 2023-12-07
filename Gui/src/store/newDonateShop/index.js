//import configs from '../../configs/newDonateShop'
import inventory from './inventory.js'
import roulette from './roulette.js'
import prime from './prime.js'

export default {
    namespaced: true,
    state: {
        currentTab: 'Shop',       
        begineAction: false,
        coins: 1000,
        money: 2000000,
        tryMode: false,
        gender: true,
        takeItems: [],
        exchangeCources: {"coinToVirtual":100,"rubToCoin":70},
        currency: "USD",
        coinKits:[
            {
                "Id": 0,
                "Price": 71,
                "Coins": 5500
            },
            {
                "Id": 1,
                "Price": 142,
                "Coins": 11000
            },
            {
                "Id": 2,
                "Price": 285,
                "Coins": 23000
            },
            {
                "Id": 3,
                "Price": 428,
                "Coins": 35000
            },
            {
                "Id": 4,
                "Price": 714,
                "Coins": 57000
            }
        ],
        updatedPrices: {

        }
    },
    mutations: {
        updateExchangeCource(state, cources){
            state.exchangeCources["coinToVirtual"] = cources[0];
            state.exchangeCources["rubToCoin"] = cources[1];
        },
        updateCoinKits(state, coinKits){
            state.coinKits = coinKits;
        },
        updateCurrency(state, currency){
            state.currency = currency;
        },
        setCurrenTab(state, tab){
            if(state.begineAction) return;
            state.currentTab = tab
        },
        setCoins(state, coins){
            state.coins = coins;
        },
        setBegineAction(state, satus){
            state.begineAction = satus;
        },
        setGender(state, gender){
            state.gender = gender;
        },
        tryClothes(state, items){
            if(items !== undefined){
                state.tryMode = true;
            }else state.tryMode = false;
        },
        setTakeItems(state, items){
            state.takeItems = items;
        },
        setUpdatedPrices(state, data){
            data.forEach(item => {
                state.updatedPrices[item[0]] = item[1];
            });
        },
        updateUpdatedPrices(state, item){
            state.updatedPrices[item[0]] = item[1];
        }
    },
    actions: {
    },
    modules: {
        inventory,
        roulette,
        prime
    },
    getters:{
    }
}
  