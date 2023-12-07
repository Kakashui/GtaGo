<template>
    <div class="wsettings">
        <div class="wsettings-title">Weapons setting</div>
        <div class="wsettings-coef">
            <div class="wsettings-coef-item" v-for="(item, index) in coefsList" :key="index">
                <input type="number" min="0" step="0.01" v-model="item.value" class="wsettings_input">
                <div class="wsettings-coef-item_title">{{getCoefname(item.id)}}</div>
            </div>
            <div class="wsettings_save" @click="save">Save</div>
        </div>
        <div class="wsettings-config">
            <div class="wsettings-config-item" v-for="(item, index) in configList" :key="index">
                <img :src="getWeaponImage(item.hash)" alt="weapon" class="wsettings-config-item_img">
                <div class="wsettings-config-item_info">
                    <div class="wsettings-config-item_title">{{getWeaponName(item.hash)}}</div>
                    <input type="number" min="0" step="1" class="wsettings_input" v-model="item.value">
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import configs from '../../configs/inventory/configs'
export default {
    data() {
        return {
            spamProtect: 0
        }
    },
    methods: {
        getWeaponName(hash){
            const config = Object.values(configs).find(c=>c.WeaponHash == hash);
            return config ? this.loc(config.DisplayName) : "name not found";
        },
        getWeaponImage(hash){
            const config = Object.values(configs).find(c=>c.WeaponHash == hash);
            return config ? `/img/inventory/items/${config.Image}.png` : "no image";
        },
        getCoefname(id){
            switch (id) {
                case "1": return "- head coef";
                case "2": return "- leg/arms coef";    
                case "3": return "- melee coef";
                default: return "- body coef";
            }
        },
        save(){
            if(this.spamProtect > Date.now()) return;
            this.spamProtect = Date.now() + 2000;
            const configs = {};
            const coefs = {};
            this.configList.forEach(c => {
                const val = parseInt(c.value);
                if(!isNaN(val) && val !== this.config[c.hash]){
                    configs[c.hash] = val;
                }
            });
            this.coefsList.forEach(c=> {                
                const val = parseFloat(c.value);
                if(!isNaN(val) && val !== this.coefs[c.id]){
                    coefs[c.id] = val;
                }
            });
            window.mp.triggerServer("weapon:settings:update", JSON.stringify(configs), JSON.stringify(coefs))
        }
    },
    computed:{
        ...mapGetters("localization", ["loc"]),
        ...mapState("weaponsSetting", ["config", "coefs"]),
        configList(){
            const result = [];
            for (const key in this.config) {
                result.push({hash: key, value: this.config[key]})
            }
            return result;
        },
        coefsList(){         
            const result = [];
            for (const key in this.coefs) {
                result.push({id: key, value: this.coefs[key]})
            }
            return result;
        }
    }
}
</script>

<style lang="scss">
.wsettings{
    position: absolute;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: rgba(#000, .9);
    color: #fff;
    &-title{
        font-size: 2rem;
        height:4rem;
        display: flex;
        align-items: center;
        justify-content: center;
        text-transform: uppercase;
        border-bottom: 1px solid #fff;
        letter-spacing: .06rem;
    }
    &-coef{
        display: flex;
        align-items: center;
        justify-content: center;
        border-bottom: 1px solid #fff;
        &-item{
            margin: 0 2rem;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 3rem;
            &_title{
                font-size: 1.5rem;
            }            
        }
    }
    &_input{
        background-color: rgba(#000, .2);
        color: #fff;
        font-size: 1.2rem;
        outline: .1rem solid #ccc;
        width: 5rem;
        height: 2rem;
        text-align: center;
        margin: 0 .5rem;
    }
    &-config{
        display: flex;
        width: 83rem;
        margin: 1rem auto;
        height: 45rem;
        flex-wrap: wrap;
        //border: 1px solid #000;
        overflow-y: scroll;
        &-item{
            margin: .1rem;
            width: 20rem;
            height: 8rem;
            position: relative;
            display: flex;
            align-items: center;
            justify-content: space-between;
            background-color: rgba(#fff, .1);
            text-align: center;
            box-shadow: 0 0 1rem 0 #000;
            &_img{
                height: 100%;
                margin: 1rem;
            }
            &_info{
                margin-left: .5rem;
                width: 10rem;
                height: 90%;
                //border: 1px solid #000;
            }  
            &_title{
                text-align: center;
                font-size: 1.5rem;
                margin: .6rem 0;
            }
        }
    }
    &_save{
        border: .1rem solid #fff;
        width: 5rem;
        margin-left: 10rem;
        height: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.2rem;
        letter-spacing: .05rem;
        &:hover{
            background-color: #fff;
            color: #000;
            font-weight: bold;
            text-transform: uppercase;
        }
    }
}
</style>