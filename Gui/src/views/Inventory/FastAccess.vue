<template>
    <div class="access" v-if="fastAccessButtons">
        <img src="/img/inventory/common/fast.svg" alt="fast" class="access-img">
        <div class="access-tittle">{{loc('inv_fast_tittle')}}</div>
        <div class="access-list nopoint">
            <div class="access-item"  id="drop" 
                v-for="(button, key) in fastAccessButtons" 
                :key="key"
                :adress="adress(key)"
                :class="{'access-active': isActive(key), 'access-with-item': button !== null}"
                @mousedown="onMouseDown(adress(key), button, $event)"
            >
                <div class="access-item-tittle" :class="{'access-item-with-item': button !== null && getItem(button) !== null}">{{key}}</div>
                <img 
                    v-if="button !== null && getItem(button) !== null" 
                    :src="getImage(button)" 
                    alt="image" 
                    class="access-item-img" 
                    :style="{
                        'width': getItem(button) == null ? '50%' : '70%',
                        'height': getItem(button) == null ? '50%' : '70%'
                    }"
                >
                <div v-if="button !== null && getItemCount(button, key) > 0" class="access-item-count">
                    X{{getItemCount(button)}}
                </div>
            </div>
        </div>
        
    </div>
</template>

<script>
import {mapState, mapGetters} from 'vuex'
import dragObject from './classes/dargObject'

export default {
    props:['drag'],
    data() {
        return {
            empty: false
        }
    },
    methods: {
        isActive(key){
            return this.drag && this.adress(key) == this.drag.overAdress
        },
        adress(key){
            return `access_${key}`;
        },
        getItemCount(button){
            const item = this.getItem(button);
            if(button.type == "eq") return -1;
            if(item == null || !item.isStackable()){
                //this.$store.commit("inventory/resetFastAccessButton", key);
                return -1
            }else return item.count;
        },
        getItem(button){
             if(button.type == "eq")
                return this.equip.weapons[button.id];
            else{
                const inventory = this.getById(this.personalId);
                return inventory.items.find(i=>i.id == button.id) || null;
            }
        },
        getImage(button){
            const item = this.getItem(button);
            if(item == null){
                this.empty = true;
                return button.type == "eq" ? `/img/inventory/equip/weap${button.id}.svg` : '';
            }
            else{
                this.empty = false;
                return item.getImage();
            }
        },
        onMouseDown(adress, button, e){
            if(e.button !== 0 || button == null) return;
            const object = new dragObject(adress, this.getItem(button), e.clientX, e.clientY, true);
            this.$emit("onMouseDown", object)
        }
    },
    computed: {
        ...mapState('inventory', ['fastAccessButtons', 'equip', 'personalId']),
        ...mapGetters('inventory', ['getById']),
        ...mapGetters('localization', ['loc'])
    },
    mounted(){
        if(this.fastAccessButtons == undefined){
            if(process.env.NODE_ENV == 'development'){
                this.$store.commit('inventory/setFastAccessData', {"1": null, "2": null, "3": null, "4": null, "5": null, "6": null, "7": null, "8": null, "9": null, "10": null});
            }else{
                window.mp.trigger('cef:access:requestData')
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.access{
    height: 14rem;
    width: 57rem;
    margin-left: 4.3rem;
    position: relative;
    padding-top: 3rem;
    &-list{
        display: flex;
        justify-content: flex-start;
        align-items: center;
        padding: 1.5rem 0;
    }
    &-img{
        position: absolute;
        top: 0rem;
        pointer-events: none;
    }
    &-tittle{
        color: #fff;
        position: absolute;
        left: 0;
        top: 3.4rem;
        font-size: 1rem;
        line-height: 1rem;
        letter-spacing: 0.04rem;
        pointer-events: none;
    }
    &-item{
        width: 4.5rem;
        height: 4.5rem;
        //background-color: rgba(#000, .5);
        border: 2px solid rgba(255, 255, 255, 0.1);
        position: relative;
        margin-right: 1.1rem;
        display: flex;
        align-items: center;
        justify-content: center;
        &-img{
            -webkit-user-drag: none;
            -o-user-drag: none;
            pointer-events: none;
            width: 70%;
            height: 70%;   
        }
        &-count{
            position: absolute;
            bottom: 0;
            right: .2rem;
            color: #fff;
            font-size: .8rem;
            letter-spacing: .05rem;
        }
        &-tittle{
            color: #fff;
            background-color: rgba(#fff, .2);
            font-size: .8rem;
            line-height: .85rem;
            letter-spacing: 0.03em;
            font-weight: normal;
            color: rgba(255, 255, 255, 0.5);
            position: absolute;
            left: -1px;
            top: -1px;
            width: 1.1rem;
            height: 1.1rem;
            display: flex;align-items: center;
            justify-content: center;
        }
        &-with-item{
            background-color: #FF3380;
        }   
    }    
    &-with-item{
        background: radial-gradient(80.56% 80.56% at 50% 76.11%, rgba(255, 255, 255, 0.15) 0%, rgba(255, 255, 255, 0) 100%);
    }    

    &-active{
        border: 2px solid rgba(255, 255, 255, 0.6);
    }
}
</style>