<template>
     <div class="settings-account-2f">
      <input type="number" v-model="inputData" :placeholder="loc(placeholder)" @change="send" class="settings-account-2f_input" >
      <img src="/img/optionsMenu/account/send.svg" alt="send" class="settings-account-2f_send" @click="send">
    </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
    props:["placeholder", "value"],
    data() {
        return {
            inputData: ""
        }
    },
    computed:{
        ...mapGetters("localization", ["loc"])
    },
    methods:{
        
        send(){
            this.$emit("onsend", this.inputData)
        }
    },
    mounted() {
        if(this.value) this.inputData = this.value;
    },
}
</script>

<style lang="scss">
    .settings-account-2f{
        width: 100%;
        height: 3rem;
        position: relative;
        display: flex;
        align-items: center;
        justify-content: space-between;
        background-color: rgba(#fff, .1);
        border: 1px solid rgba(#fff, .1);
        margin-top: 1.5rem;
        &_send{
            margin-right: .5rem;
        }
        &_input{
            outline: none;
            border: none;
            background: none;
            color: #fff;
            margin-left: .5rem;
        }

    }
</style>