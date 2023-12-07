<template>
    <div>        
        <div class="list-tittle">{{loc('cef_215')}}</div>
        <div class="list">
            <div class="list-item" v-for="(item, index) in list" 
            :key="index" @click="selected = index" :class="{'list-item-selected':index==selected}">
                {{`${item.fName} ${item.lName} - ${item.lvl}`}}
            </div>
        </div>
        <div class="list-btns">
            <button @click="update">{{loc('pc_2')}}</button>
            <button @click="clearWanted">{{loc('cef_225')}}</button>
            <button @click="$emit('onChangePage', 'Main')">{{loc('pc_1')}}</button>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapState} from 'vuex'

export default {
    data() {
        return {            
            selected: 0
        }
    },
    computed: {        
        ...mapGetters('localization',['loc']),
        ...mapState('policeCarMenu',['list']),
    },
    methods: {
        clearWanted(){
            let item = this.list[this.selected];
            if(item.fName.length < 3 || item.lName.length < 3) return;
            window.mp.trigger('pccb',3,`${item.fName}_${item.lName}`);
        },
        update(){            
            window.mp.trigger('pccb',2);
        }
    },
}
</script>
<style lang="scss" scoped>
    .list{
        width: 40vh;
        height: 29vh;
        margin: auto;
        overflow: hidden auto;
        &-tittle{
            color: #fff;
            text-align: center;
            margin: 1vh 0;
        }
         &::-webkit-scrollbar {
            width: .5vh;
            background: rgba(229, 226, 226, 0.9);
        }
        &::-webkit-scrollbar-thumb {
            background: #152c53;
            border-radius: 2vh;
        }
        &-item{
            padding: 1vh;
            color: #fff;
            text-align: center;
            background: linear-gradient(#0c1a33,#152c53,#152c53,#0c1a33);
            &:hover{
                background: linear-gradient(#1d3b70,#244d92,#244d92,#183361);
                transform: scale(1.02);
            }
            &-selected{
                background: linear-gradient(#18315c,#204481,#204481,#18315c);
            }
        }
        &-btns{
            width: 60vh;
            position: relative;
            display: flex;
            justify-content: space-between;
            margin: auto;
            button{
                background: linear-gradient(#0c1a33,#152c53,#152c53,#0c1a33);
                padding: 1vh .5vh;
                width: 33%;
                color: white;
                &:hover{
                    background: linear-gradient(#1d3b70,#244d92,#244d92,#183361);
                    transform: scale(1.02);
                }
            }
        }
    }
</style>