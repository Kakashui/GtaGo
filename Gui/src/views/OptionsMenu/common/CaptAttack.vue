<template>
    <div class="capt-attack">
        <div class="capt-attack-body">
            <img src="/img/optionsMenu/capt/blood.png" alt="capt" class="capt-attack-blood">
            <div class="capt-attack-sub">{{loc('capt:attack:sub')}}</div>
            <div class="capt-attack-tit">{{loc('capt:attack:tit')}}</div>            
            <div class="capt-attack-btn" @click="openCaptTeam">{{loc('capt:attack:confirm')}}</div>
        </div>        
        <img src="/img/optionsMenu/capt/bgman.png" alt="capt" class="capt-attack-man">
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    data(){
        return {
            lastCheck: 0
        }
    },
    computed:{
        ...mapGetters('localization', ['loc'])
    },
    methods: {      
        confirm(callback, value){
            if(typeof callback == 'function') callback(value);
            this.setDialog(null);
        },
        openCaptTeam(){
            if(this.lastCheck > Date.now()) return;
            this.lastCheck = Date.now() + 1000;
            window.mp.trigger("cef:mmenu:capt:open");
        }
    },
}
</script>


<style lang="scss" scoped>
    .capt-attack{
        position: fixed;
        left: 0;
        top: 0;
        height: 100vh;
        width: 100vw;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: rgba(0,0,0,.7);
        color: #fff;
        &-sub{            
            font-size: 1.5rem;
            text-align: center;
            margin-top: 4rem;
        }
        &-blood{
            position: absolute;
            left: 0;
            top: 0;
        }
        &-tit{
            font-size: 4rem;
            text-align: center;
            text-align: center;
        }
        &-body{
            height: 24rem;
            width: 37rem;
            background-color:rgba(0,0,0,.8);
            position: relative;
        }
        &-man{
            position: absolute;
            bottom: 0;
            right: 0;
        }
        &-btn{
            width: 13rem;
            height: 4.5rem;
            background: url(/img/optionsMenu/capt/button.png) no-repeat center;
            background-size: cover;
            margin: 2rem auto 0;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.5rem;
            letter-spacing: .04rem;
            color: #fff;
            //color: rgba(#000, .5);
            font-weight: 300;
            &:hover{
                transform: scale(1.01);
                color: #fff;
            }
        }
    }
</style>