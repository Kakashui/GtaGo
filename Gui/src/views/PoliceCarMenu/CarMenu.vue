<template>
    <div class="person">
        <div class="person-input">
            <span>{{loc('cef_210')}}:</span>  
            <input type="text" v-model="searchString" :placeholder="loc('cef_211')">
            <button class="person-btn" @click="search">{{loc('cef_218')}}</button>
            <button class="person-btn" @click="clear">{{loc('cef_227')}}</button>
            <button class="person-btn" @click="$emit('onChangePage', 'Main')">{{loc('pc_1')}}</button>
        </div>
        <div class="person-result">
            <div v-for="(item, key, index) in carResult" :key="index" class="person-result-item">
                <div class="person-result-tittle">
                    {{loc(item.tittle)}}
                </div>
                <div class="person-result-info">
                    {{loc(item.val)}}
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapState} from 'vuex'

export default {
    data() {
        return {
        }
    },
    methods: {
        clear(){
            this.$store.commit('policeCarMenu/clearCar');
        },
        search(){
            if(this.searchString.length < 3) return;
            window.mp.trigger('pccb', 1, this.searchString)
            this.clear();
        }
    },
    computed:{        
        ...mapGetters('localization',['loc']),
        ...mapState('policeCarMenu',['carValue', 'carResult']),
        searchString:{
            get(){
                return this.carValue;
            },
            set(val){
                this.$store.commit('policeCarMenu/setCarValue',val);
            }
        }
    },
    
}
</script>

<style lang="scss" scoped>
.person{
    margin-top: 2vh;
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    &-input{
        width: 40%;
        margin: 1vh 0;
        span{
            color: #fff;
            display: block;
            margin: 1vh 0;
            width: 90%;
        }
        input{
            color: #000;
            border-radius: 1vh;
            padding: .6vh 1vh;
            font-weight: bold;
            width: 90%;
        }
    }
    &-btn{
        width: 90%;
        display: block;
        padding: 1vh .5vh;
        color: #fff;
        background-color: #fff;
        background: linear-gradient(#0c1a33,#152c53,#152c53,#0c1a33);
        margin: 1vh 0;
        border-radius: .5vh;
        &:hover{
            background: linear-gradient(#1d3b70,#244d92,#244d92,#183361);
            transform: scale(1.02);
        }
    }
    &-result{
        border-left: 1px solid #000;
        width: 50%;
        font-size: 1.7vh;
        &-item{
            display: flex;
            margin: 1vh;
            color: #fff;
            align-items: center;
            justify-content: space-between;
            border-bottom: 2px solid rgba(rgb(182, 179, 179),.3);
        }
    }
}
</style>