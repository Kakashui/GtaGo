<template>
    <div class="frac-access">
        <div class="frac-access-item" v-for="(acc, key, index) in fraction.access" :key="index">
            <div class="frac-access-name">{{key}}</div>
            <div class="frac-access-val">{{acc}}</div>
            <div class="frac-access-btn" @click="change(key)">{{loc("mmain:frac:access:change")}}</div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState, mapMutations } from 'vuex'

export default {
    computed: {
        ...mapGetters('localization', ['loc']),
        ...mapState('optionsMenu', ['fraction'])
    },
    methods: {
        ...mapMutations('optionsMenu', ['setDialog']),
        change(key){
            this.setDialog({
               input: 'number',
               callback: (val)=>{
                   val = parseInt(val);
                    if(isNaN(val) || val < 0){
                        window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:dialog:data:wrong", time: 3000});
                    }else{
                       window.mp.triggerServer('mmenu:frac:access:change', key, +val);
                    }
               },
               value: '',
               placeholder: 'mmain:frac:access:pl',
               tittle: `mmain:frac:access:tit`,
               subtittle: 'mmain:frac:access:sub',
               bg: 'invite'
           });
        }
    },
    mounted(){
        if(this.fraction.canAccess) window.mp.triggerServer("mmenu:frac:access:request")
        //this.$store.commit('optionsMenu/setFractionAccess', {"Access_1": 2,"Access_2": 2,"Access_3": 2,"Access_4": 2});
    }
}
</script>

<style lang="scss" scoped>
.frac-access{
    margin-top: 1.5rem;
    //border: 1px solid #000;
    height: 30rem;
    overflow: hidden scroll;
    &::-webkit-scrollbar {
        width: .1rem;
    }
    &::-webkit-scrollbar-track {
        border: solid 0 transparent;
        margin: .1rem;
    }
    &::-webkit-scrollbar-thumb {
        border: solid 0 transparent;      
        background-color: rgba(#fff,.1);
    };
    &-item{
        margin: .5rem 0;
        padding: 0 1rem;
        display: flex;
        align-items: center;
        justify-content: space-between;
        color: #fff;
        height: 3rem;
        background-color: rgba(#fff, .05);
    }
    &-name{
        width: 12rem;
    }
    &-val{
        margin-left: 3.5rem;
    }
    &-btn{
        border: 2px solid #B6D300;
        height: 2rem;
        width: 10rem;
        display: flex;
        justify-content: center;
        align-items: center;
        &:hover{
            background-color: #B6D300;
        }
    }

}
</style>