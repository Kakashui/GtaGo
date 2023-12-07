<template>
  <div class="settings-account">
    <div class="settings-account-title">
      {{loc('mm_setts_acc_cpwd_t')}}
    </div>
    <div class="settings-account-subtitle">
      {{loc('mm_setts_acc_cpwd_s')}}
    </div>
    <div class="settings-account-password">
      <input 
        type="password" 
        class="settings-account-input" 
        :placeholder="loc('mm_setts_acc_cpwd_pl1')" 
        v-model="currentPassword"
      >
      <img src="/img/optionsMenu/settings/lock2.svg" alt="lock">
    </div>
    <div class="settings-account-password">
      <input type="password" 
        class="settings-account-input" 
        :placeholder="loc('mm_setts_acc_cpwd_pl2')"
        v-model="newPassword"
      >
      <img src="/img/optionsMenu/settings/lock2.svg" alt="lock">
    </div>
    <div class="settings-account-password">
      <input 
        type="password" 
        class="settings-account-input" 
        :placeholder="loc('mm_setts_acc_cpwd_pl3')"
        v-model="confirmPassword"
      >
      <img src="/img/optionsMenu/settings/lock2.svg" alt="lock">
    </div>
    <div class="settings-account-send" @click="changePassword">
      {{loc('mm_setts_acc_cpwd_bt')}}
    </div>
    <div class="settings-account-hr"/>
    <div class="settings-account-email">
      <div class="settings-account-email-title">{{loc('mm_setts_acc_promo')}}</div>
      <div class="settings-account-email-value">{{loc('mm_setts_acc_entpromo')}}</div>
      <div class="settings-account-email-input">
        <input type="text" v-model="newEmail" :placeholder="loc('mm_setts_acc_plpromo')">
        <div class="settings-account-email-button" @click="usePromo">{{loc('mm_setts_acc_promo_act')}}</div>
        <!--<img src="/img/optionsMenu/settings/promo.svg" alt="lock">-->
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'Chat',
  data() {
    return {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
      newEmail: ''
    }
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    usePromo() {
      window.mp.triggerServer("usepromoopt", this.newEmail)
      this.newEmail = ''
    },
    changePassword(){     
      if(this.newPassword !== this.confirmPassword) {
          this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('auth_16'), time: 3000});
          return;
      }
      window.mp.triggerServer("mmain:pwd:change", this.currentPassword, this.newPassword)
    }
  }
}
</script>

<style lang="scss">
.settings-account{
  &-title{
    font-size: 2rem;
    line-height: 2.4rem;
    letter-spacing: 0.03em;
  }
  &-subtitle{
    margin-top: .5rem;
    margin-bottom: 1rem;
    font-size: .9rem;
    line-height: 1.2rem;
    letter-spacing: .03rem;
    color: rgba(#fff, .5);
  }
  &-password{
    width: 100%;
    height: 3.5rem;
    position: relative;
    margin-top: .5rem;
    input{
      width: 100%;
      height: 100%;
      background: rgba(255, 255, 255, 0.05);
      border: 1px solid rgba(255, 255, 255, 0.1); 
      padding-left: 3.2rem;
      color: #fff;
      font-size: 1.1rem;
      line-height: 1.3rem;
      letter-spacing: .04rem;
      &::placeholder{
        text-transform: uppercase;
      }
    }
    img{
      position: absolute;
      left: 1.1rem;
      top: 1.1rem;
    }
  }
  &-send{
    height: 2.5rem;
    width: 100%;
    font-size: .9rem;
    line-height: 1.1rem;
    border: 2px solid  #B6D300;
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: .75rem;
    letter-spacing: .04rem;
    &:hover{
      background: #B6D300;
      box-shadow: 0px 1rem 3rem rgba(168, 195, 2, 0.5);
      border-radius: 2px;
    }
  }
  &-hr{
    margin: 2.5rem 0;
    background-color: rgba(#fff, .05);
    width: 100%;
    height: 2px;
  }
  &-email{
    &-title{
      font-size: 1.1rem;
      line-height: 1.3rem;
      color: rgba(#fff, .5);
      letter-spacing: .03rem;
    }
    &-value{
      color: #fff;
      font-size: 1.5rem;
      line-height: 1.8rem;
      font-weight: 300;
      letter-spacing: .04rem;
    }
    &-input{
      height: 3.5rem;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin-top: 1.5rem;
      input{
        height: 100%;
        width: 21rem;
        background: rgba(255, 255, 255, 0.05);
        border: 1px solid rgba(255, 255, 255, 0.1); 
        padding-left: 3.2rem;
        color: #fff;
        font-size: 1.1rem;
        line-height: 1.3rem;
        letter-spacing: .04rem;
        &::placeholder{
          text-transform: uppercase;
        }
      } 
      img{
        position: absolute;
        left: 1.1rem;
        top: 1.1rem;
      }
    } 
    &-button{
      height: 100%;
      width: 8rem;
      display: flex;
      align-items: center;
      justify-content: center;
      border: 2px solid #B6D300;
      font-size: .9rem;
      &:hover{
        background: #B6D300;
        box-shadow: 0px 1rem 3rem rgba(168, 195, 2, 0.5);
      }
    }
  }
}
</style>
