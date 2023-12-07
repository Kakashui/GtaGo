<template>
    <div class="customiztion-nav-save" @click="click">
       <div class="customiztion-nav-save_check">
           <img src="/img/customization/common/check.svg" alt="check">
       </div>
       <div class="customiztion-nav-save_title">
           {{loc("customiz:save:btn")}}
       </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

export default {
    data() {
        return {
            flood: 0,
            clickInterval: 1000,
        }
    },
    computed:{
        ...mapState("customization", ["firstName", "lastName", "firstCreate"]),
        ...mapGetters("localization", ["loc"])
    },
    methods: {
        click(){
            if(this.flood > Date.now())return;
            this.flood = Date.now() + this.clickInterval;
            if(this.firstCreate){
                if(this.firstName.length < 3 
                    || this.lastName.length < 3
                    || this.firstName.length > 12
                    || this.lastName.length > 18) {
                    this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('chcr_18'), time: 3000});
                    return;
                }
                var regexp = /^[a-z]+$/i;
                if (!regexp.test(this.firstName) || !regexp.test(this.lastName)){
                    this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('chcr_19'), time: 3000});
                    return;
                }
                window.mp.trigger("customization:save", this.firstName, this.lastName)
            }else
                window.mp.trigger("customization:save", "", "")           
        },
    },
}
</script>

<style lang="scss">
.customiztion-nav-save{ 
    display: flex;
    align-items: center;
    width: 15rem;
    height: 4rem;
    position: absolute;
    bottom: 0;
    margin-left: 3.5rem;
    background-color: rgba(#fff, .12);
    border-radius: 2rem;
    border: 1px solid rgba(#fff, .2);
    &:hover{
        background-color: #fff;
        color: #000;
        font-weight: bold;
    }
    &_check{
        height: 3.6rem;
        width: 3.6rem;
        margin: .2rem;
        border-radius: 50%;
        background-color: #B6D300;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    &_title{
        letter-spacing: .08rem;
        margin-left: 2rem;
    }
}
</style>