<template>
  <div class="settings-account">
    <div class="settings-account-title">{{loc("settings:auto:title")}}</div>
    <div class="settings-account-subtitle">{{loc("settings:auto:subtitle")}}</div>
    <SettingsSwitch 
      :status="setRememberLogin" 
      class="settings-special-switch"  
      @onSwitch="setRememberLogin"
    />
    <div class="settings-account-hr"/>
    <div class="settings-account-title">{{loc("settings:2fa:title")}}</div>
    <div class="settings-account-subtitle">{{loc("settings:2fa:subtitle")}}</div>
    <SettingsSwitch 
      :status="status" 
      class="settings-special-switch"  
      @onSwitch="set2fa"
    />
    <template v-if="status">
      <div class="settings-account-confirmed" v-if="confirmed">
        <div class="settings-account-confirmed_title">{{loc("settings:2fa:confirmed")}}</div>
        <div class="settings-account-confirmed_phone">{{hiddenNumber}}</div>
      </div>
      <Input2f v-else-if="isShowConfirm" placeholder="settings:2fa:phone:code" @onsend="sendCode"/>
      <Input2f v-else :value="phone" placeholder="settings:2fa:phone:number" @onsend="sendPhone"/>
    </template>
    
   
    <div class="settings-account-hr"/>
    
    <div class="settings-account-exit">
      <img src="/img/optionsMenu/account/exit.svg" alt="exit" class="settings-account-exit_img">
      {{loc("settings:account:exit")}}
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import SettingsSwitch from '../SettingsSwitch.vue'
import Input2f from './Input2f.vue'

export default {
  name: 'Chat',
  computed:{
    ...mapGetters("localization", ["loc"]),
    ...mapState("auth", ["isRememberPass", "auth"]),
    ...mapState("optionsMenu/auth2f", ["status", "isShowConfirm", "confirmed", "phone"]),
    hiddenNumber(){
      if(this.phone.length > 4)
        return `+ ${this.phone[0]}${"*".repeat(this.phone.length-3)}${this.phone[this.phone.length - 2]}${this.phone[this.phone.length - 1]}`;
      else 
        return this.phone;
    }
  },
  components:{
    SettingsSwitch,
    Input2f
  },
  methods:{
    ...mapMutations("optionsMenu/auth2f", ["setStatus"]),
    setRememberLogin(status){
      if(status == this.isRememberPass) return;
      window.mp.trigger("auth:save:pass", this.auth.login, this.auth.password, status);
    },
    set2fa(status){
      if(status == this.status) return;
      this.setStatus(status);
    },
    sendPhone(value){
      console.log(value)
    },
    sendCode(value){
      console.log(value)
    }
  }
}
</script>

<style lang="scss">
.settings-account{
    height: 100%;
    //border: 1px solid #fff;
    &-exit{
      display: flex;
      align-items: center;
      justify-content: center;
      line-height: 1.1rem;
      font-size: .9rem;
      letter-spacing: 0.03em;
      width: 100%;
      height: 3.5rem;
      background: linear-gradient(270deg, rgba(41, 47, 51, 0.9) 0%, rgba(20, 21, 23, 0.9) 100%);
      border: 1px solid rgba(255, 255, 255, 0.05);
      box-sizing: border-box;
      transform: matrix(1, 0, 0, -1, 0, 0);
      &:hover{
         background: linear-gradient(270deg, rgba(78, 88, 95, 0.9) 0%, rgba(20, 21, 23, 0.9) 100%);
      }
      &_img{
        margin-right: 1rem;
      }
    }
    &-confirmed{
      margin-top: 1.5rem;
      &_title{
        color: #B6D300;
        letter-spacing: 0.05rem;
      }
      &_phone{
        font-size: 1.2rem;
        letter-spacing: 0.08rem;
        margin-top: .3rem;
      }
    }
}
</style>
