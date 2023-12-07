<template>
    <div class="slang">
        <div class="slang-body">
            <div class="slang-btns">
                <div class="slang-btn" 
                    v-for="(lname, key, index) in langs.list" :key="index" 
                    :class="{'slang-selected': key === current}"
                    @click="select(key)"
                >
                    <img :src="`${imgUrl + key}.png`" :alt="lname"> {{lname}}
                    <img src="/img/slang/marker.svg" class="slang-marker" v-show="key === current">
                </div>
            </div>
            <div class="slang-info">
                <div class="slang-title">
                    Language <br> Selection
                </div>
                <div class="slang-next" @click="next">
                    {{loc('slang:btn:next')}}
                </div>
                <div class="slang-subtitle">
                    {{loc('slang:info:sub')}}                    
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapActions, mapGetters, mapState } from 'vuex';
export default {
    data() {
        return {
            imgUrl: "https://language-api-zszc.onrender.com/languages/",
            current: "ru"
        }
    },
    methods:{
        select(key){
            this.current = key;
            this.setLang(this.current);
        },
        ...mapActions('localization', ['setLang', 'loadLangs']),
        next(){            
            window.mp.trigger("language:next");
        }
    },
    computed:{
        ...mapGetters('localization', ['loc']),
        ...mapState('localization', ['langs'])
    },
    mounted(){
        this.loadLangs();
        if(this.langs.list === null){
            this.loadLangs();
        }
        this.setLang(this.current);
    }
}
</script>

<style lang="scss">
.slang{
    position: fixed;
    left: 0;
    top: 0;
    width: 100vw;
    height: 100vh;
    background: url(/img/slang/bg.jpg) no-repeat center;
    background-size: cover;
    display: flex;
    align-items: center;
    justify-content: center;
    text-transform: uppercase;
    font-size: 1.5rem;
    font-weight: bold;
    letter-spacing: .07rem;
    z-index: 1000;
    &-body{
        width: 45rem;
        height: 23rem;
        display: flex;
        position: relative;
        align-items: center;
        justify-content: space-between;
    }
    &-btns{
        height: 100%;
        overflow: hidden scroll;
        &::-webkit-scrollbar {
            width: .2rem;
        }
        &::-webkit-scrollbar-track {
            border: solid 0 transparent;
            margin: .2rem;
        }
        &::-webkit-scrollbar-thumb {
            border: solid 0 transparent;      
            background-color: #B6D300;
        }
    }
    &-btn{
        width: 19.5rem;
        height: 4.5rem;
        background-color: rgba(#fff, .2);
        border-radius: .4rem;
        display: flex;
        align-items: center;
        margin: .25rem 0;
        color: #fff;
        position: relative;

        &:hover{
            background-color: #fff;
            color: #000;
        }
        img{
            margin: 0 2rem;
        }
    }
    &-selected{
        background-color: #fff;
        color: #000;
    }
    &-marker{
        position: absolute;
        right: 0;
    }
    &-info{
        height: 100%;
    }
    &-title{
        font-size: 6.5rem;
        color: #fff;
        line-height: 6rem;
        text-align: center;
    }
    &-next{
        width: 20rem;
        height: 4.5rem;
        margin: 0 auto;
        display: flex;
        align-items: center;
        justify-content: center;
        background: linear-gradient(268.14deg, #B0C724 0%, #D5EC45 23.77%, #B6D300 100%);
        border-radius: .4rem;
        margin-top: 1rem;
        font-size: 1.5rem;
        color: #fff;
        box-shadow: 0px .2rem 0 #98B300;
        &:hover{
            transform: scale(1.01);
        }
    }
    &-subtitle{
        font-size: 1.2rem;
        color: rgba(#fff, .3);
        margin-top: .5rem;
        letter-spacing: normal;
        text-align: center;
    }
}
</style>