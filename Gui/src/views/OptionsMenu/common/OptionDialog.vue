<template>
    <div class="option-dialog">
        <div 
            class="option-dialog-body" 
            :style="{'background': dialog.bg ? `url(img/optionsMenu/dialog/${dialog.bg}.png) no-repeat ` : '#000'}"
        >
            <div class="option-dialog-sub">{{loc(dialog.subtittle)}}</div>
            <div class="option-dialog-tit">{{loc(dialog.tittle)}}</div>
            <input 
                v-if="dialog.input" 
                :type="dialog.input" 
                class="option-dialog-input" 
                v-model="dialog.value"
                :placeholder="loc(dialog.placeholder)"
            >  
            <input 
                v-if="dialog.input2" 
                :type="dialog.input2" 
                class="option-dialog-input" 
                v-model="dialog.value2"
                :placeholder="loc(dialog.placeholder2)"
            >
            <div v-if="dialog.keyHandler"  class="option-dialog-key">{{dialog.value}}</div>
            <div class="option-dialog-btns">
                <div class="option-dialog-btn" @click="confirm(dialog.callback, dialog.value, dialog.value2)">{{loc('mmain:frac:dialog:success')}}</div>
                <div class="option-dialog-btn" @click="cancel">{{loc('mmain:frac:dialog:cancel')}}</div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
    props:['dialog'],
    computed:{
        ...mapGetters('localization', ['loc'])
    },
    methods: {
        cancel(){
            this.setDialog(null);
        },
        confirm(callback, value, value2){
            if(typeof callback == 'function') callback(value, value2);
            this.setDialog(null);
        },
        onKeyPress(e){
            this.dialog.value = e.keyCode;
        },
        ...mapMutations('optionsMenu', ['setDialog'])
    },
    mounted(){
        if(this.dialog && this.dialog.keyHandler) document.addEventListener('keyup', this.onKeyPress)
    },
    beforeDestroy(){
        document.removeEventListener('keyup', this.onKeyPress);
    }
}
</script>


<style lang="scss" scoped>
.option-dialog{
    position: fixed;
    top: 0;
    left: 0;
    height: 100vh;
    width: 100vw;
    background-color: rgba(#000, .8);
    z-index: 1000;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 300;
    text-align: center;
    &-key{
        width: 19rem;
        height: 3.5rem;
        margin: 0 auto;
        display: flex;
        font-size: 1.5rem;
        letter-spacing: .04rem;
        align-items: center;
        justify-content: center;
        color: #fff;
    }
    &-body{
        width: 26.5rem;
        min-height: 20rem;
        padding: 2.5rem 1.5rem;
    }
    &-sub{
        text-align: center;
        letter-spacing: .04rem;
        color: rgba(#fff, .5);        
    }
    &-tit{
        color: #fff;
        font-size: 1.5rem;
        letter-spacing: .06rem;
        text-align: center;
        margin: .5rem 0;
    }
    &-input{
        color: #fff;
        width: 19rem;
        height: 3.5rem;
        background: rgba(255, 255, 255, 0.1);
        border-radius: .2rem;
        padding: 0 1.5rem;
        border: 1px solid rgba(255, 255, 255, 0.1);
        margin-top: 1.3rem;
        &::placeholder{
            text-transform: uppercase;
            letter-spacing: .04rem;
            color: rgba(#fff, .5);
        }
    }
    &-btns{
        width: 19rem;
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin: 1.5rem auto;
    }
    &-btn{
        width: 9rem;
        height: 3.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: .2rem;
        color: rgba(#fff, .5);
        background: rgba(255, 255, 255, 0.1);
        &:hover{
            background: #B6D300;
            box-shadow: 0px .5rem 2.5rem rgba(168, 195, 2, 0.6);
            color: #fff;
        }
    }
}
</style>