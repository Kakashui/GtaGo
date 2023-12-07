<template>
  <div class="tab">
    <div class="tab__side">
      <div class="title">{{loc('auth:new:tit')}}</div>
      <div class="desc">{{loc('auth:new:desc')}}</div>
    </div>
    <div class="tab__main">
      <div class="labels">
        <input-label
          v-for="(item, index) in labels"
          :key="index"
          :item="item"
          v-model="item.model"
        />
      </div>
      <action-checkbox v-model="isAgree" :styles="{ margin: '0 0 1.5rem 0'}">
        <div class="checkbox__text">{{loc('auth:new:access')}}</div>
      </action-checkbox>
      <action-button
        :text="loc('auth:new:reg')"
        @event="register"
        :style="{ width: '25rem' }"
      />
    </div>
  </div>
</template>

<script>
import ActionButton from '../common/ActionButton.vue'
import ActionCheckbox from '../common/ActionCheckbox.vue'
import InputLabel from '../common/InputLabel'
import { mapGetters } from 'vuex'
export default {
  components: {
    InputLabel,
    ActionCheckbox,
    ActionButton
  },
  name: 'CreateNewAccount',

  data: function() {
    return {
      isAgree: false,
      labels: [
        { name: 'auth:new:login', model: '', key: 'login', icon: 'user', type: 'text' },
        { name: 'auth:new:pwd', model: '', key: 'password', icon: 'key', type: 'password' },
        { name: 'auth:new:pwdc', model: '', key: 'password', icon: 'key', type: 'password' },
        { name: 'auth:new:email', model: '', key: 'password', icon: 'mail', type: 'email' },
      ],
      flood: 0,
      clickInterval: 5000,
    }
  },
  computed:{
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    getModels: function() {
      return this.labels.map(item => item.model)
    },

    clearModels: function() {
      this.labels.forEach(item => item.model = '')
    },

    register: function() {
      if(this.flood > Date.now())return;
      this.flood = Date.now() + this.clickInterval;
      const login = this.labels[0].model;
      const password = this.labels[1].model;
      const confirm = this.labels[2].model;
      const email = this.labels[3].model;
      
      if(this.checkEmail(email)){
          this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('auth_17'), time: 3000});
          return;
      }
      if(login.length < 4 || password.length < 4) {
          this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('auth_15'), time: 3000});
          return;
      }
      if(password !== confirm) {
          this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('auth_16'), time: 3000});
          return;
      }
      window.mp.triggerServer("signup", email, login, password)
      //this.clearModels()
    },
    checkPwd(pwd){
        return pwd.search(/^[a-zA-Z0-9_]+$/) === -1;
    },
    checkEmail(email){
        return email.search(/^.+@.+\..+$/) == -1;
    },
    enter(e){
      if(e.keyCode == 13)
        this.register();
    }
  },
  mounted(){
    document.addEventListener("keyup", this.enter)
  },
  beforeDestroy(){
    document.removeEventListener("keyup", this.enter)
  }
}
</script>

<style lang="scss" scoped>
.tab {
  display: flex;
  &__side {
    display: flex;
    flex-direction: column;
    color: #FFFFFF;
    .title {
      font-size: 6rem;
      line-height: 5rem;
      font-weight: bold;
      width: 25rem;
      margin: 0 0 0.5rem 0;
    }
    .desc {
      font-size: 1.2rem;
      text-transform: uppercase;
      width: 25rem;
    }
  }
  &__main {
    display: flex;
    flex-direction: column;
    .labels {
      display: flex;
      flex-direction: column;
      margin: 0 0 1rem 0;
    }
    .checkbox__text {
      font-size: 0.9rem;
      color: #fff;
      position: relative;
      &::after {
        content: "";
        position: absolute;
        top: 100%;
        right: 0;
        height: 0.1rem;
        width: 5.4rem;
        background: rgba(255, 255, 255, 0.15);
        display: block;
      }
    }
  }
}
</style>
