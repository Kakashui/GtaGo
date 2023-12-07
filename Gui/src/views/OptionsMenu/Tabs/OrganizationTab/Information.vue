<template>
    <div class="frac-info">
        <div class="frac-info-subtittle">
            {{loc('mmain:frac:info:tit')}}
        </div>
        <div class="frac-info-tittle">
            {{fractionName}}
        </div>
        <div class="frac-info-menu" >
            <div class="frac-info-menu-item" 
                v-for="(item, index) in menuItems" 
                v-show="showTab(item.tab)"
                :key="index"
                :class="{'frac-info-menu-active':index == currentTab}"
                @click="currentTab = index"
            >
                {{loc(item.name)}}
            </div>
            
            <div class="frac-info-capt" v-show="fraction.id > 0 && fraction.id < 6" @click="openCaptTeam">{{loc('optmenu:info:ter')}}</div>
        </div>
        <component :is="menuItems[currentTab].tab" />
    </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import FracMoney from './FracMoney'
import FracFuel from './FracFuel'
import FracAccess from './FracAccess'

export default {
    data() {
        return {
            names:{
                "-1": "Loading...",
                "0": "None",
                "1": "The Families",
                "2": "The Ballas Gang",
                "3": "Los Santos Vagos",
                "4": "Marabunta Grande",
                "5": "Blood Street",
                "6": "Cityhall",
                "7": "Police",
                "8": "Hospital",
                "9": "FIB",
                "10": "La Cosa Nostra",
                "11": "Mexican Mafia",
                "12": "Yakuza",
                "13": "Amogebuli Mafia",
                "14": "Army",
                "15": "News",
                "16": "The Lost",
                "17": "Government",
                "18": "Referee"
            },
            currentTab: 0,
            menuItems:[
                {name: "mmain:frac:info:mmoney", tab: "FracMoney"},
                {name: "mmain:frac:info:mfuel", tab: "FracFuel"},                
                {name: "mmain:frac:info:maccess", tab: "FracAccess"},
            ],
            lastCheck: 0
        }
    },
    methods: {
        showTab(tab){
            if(tab == "FracAccess" && !this.fraction.canAccess) return false;
            if(tab == "FracFuel") return false;
            return true;
        },
        openCaptTeam(){
            if(this.lastCheck > Date.now()) return;
            this.lastCheck = Date.now() + 1000;
            window.mp.trigger("cef:mmenu:capt:open")
        }
    },
    computed:{
        ...mapState('optionsMenu', ['fraction']),
        ...mapGetters('localization', ['loc']),        
        fractionName(){
            return this.names[this.fraction.id] || "noInfo"
        }
    },
    components:{
        FracFuel,
        FracMoney,
        FracAccess
    }
}
</script>

<style lang="scss" scoped>
.frac-info{
    position: relative;
    margin-top: 3.5rem;
    height: calc(100% - 3.5rem);
    width: 40%;
   
    &-subtittle{
        font-size: .9rem;
        color: rgba(#fff, .5);
    }
    &-tittle{
        color: #fff;
        font-size: 5rem;
        line-height: 4rem;
        font-weight: 300;
    } 
    &-capt{
        margin-left: auto;
        border: 2px solid #A2BC1F;
        color: rgba(#fff, .5);
        height: 2rem;
        width: 5.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
        letter-spacing: .04rem;
        &:hover{            
            color:#fff;
        }
    }
    &-menu{
        display: flex;
        width: 100%;
        align-items: center;
        margin-top: 2rem;
        border-bottom: 1px solid rgba(#fff, .1);
        border-top: 1px solid rgba(#fff, .1);
        &-item{
            margin-right: 2rem;
            padding: 1rem 0;
            color: rgba(#fff, .5);
            position: relative;
        }
        &-active{
            color: #fff;
            &:after{
                content: '';
                width: 100%;
                height: .1rem;
                position: absolute;
                bottom: 0;
                left: 0;
                background-color: #A2BC1F;
                box-shadow: 0 -1rem 1.8rem .4rem #A2BC1F;            
            }
        }
    }
}
</style>