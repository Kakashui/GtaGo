<template>
    <div class="checkinv">
        <ListItems 
            class="checkinv-inv"
            :id ="0"
            :drag="false"
            :width="6"
            :sortByIndex ="true"
            :canDrop="false"
            :checkInventory="checkInventory.inventory"
        />
        <Equip
            class="checkinv-equip"
            :checkInventory="checkInventory.eqiuip"
        />
        <div class="checkinv-close" @click="close">Close</div>
    </div>
</template>

<script>
import ListItems from './ListItems'
import Equip from './Equip'

export default {
    props:['checkInventory'],
    methods: {
       close(){
           this.$store.commit("inventory/closeCheckInventory")
       }
    },
    components:{
        ListItems,
        Equip
    }
}
</script>

<style lang="scss" scoped>
    .checkinv{
        position: fixed;
        width: 100vw;
        height: 100vh;
        left: 0;
        top: 0;
        background: url(/img/inventory/common/bg.png) no-repeat center;
        background-size: cover;
        text-transform: uppercase;
        font-style: normal;
        display: flex;
        justify-content: center;
        align-items: center;
        &-equip{
            width: 25rem;
            height: 40rem;
        }
        &-close{
            position: absolute;
            color: #fff;
            font-size: 1.5rem;
            bottom: 2rem;
            width: 10rem;
            height: 3rem;
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: rgba(0, 0, 0, 0.164);
            border: 2px solid rgba(#FFF, .1);
            &:hover{                
                background-color: rgba(0, 0, 0, 0.3);
            }
        }
    }
</style>