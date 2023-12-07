<template>
  <div class="tab">
    <div class="tab__side">
      <div class="title">{{loc("ВХОД")}}</div>
      <div class="desc">{{loc("auth:login:desc")}}</div>
    </div>
    <div class="tab__main">
      <div class="labels">
        <input-label :item="labels[0]" v-model="auth.login"/>
        <input-label :item="labels[1]" v-model="auth.password"/>
      </div>
      <div class="labels__block">
        <action-checkbox v-model="passModel">
          <div class="checkbox__text">{{loc("auth:login:cbtext")}}</div>
        </action-checkbox>
        <empty-button
            @event="toRecover"
            :text="loc('auth:login:forget')"
        />
      </div>
      <action-button
        :styles="{ width: '25rem' }"
        :text="loc('auth:login:btn:enter')"
        @event="login"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import ActionCheckbox from '../common/ActionCheckbox'
import InputLabel from '../common/InputLabel'
import ActionButton from '../common/ActionButton'
import EmptyButton from '../common/EmptyButton.vue'
export default {
  name: 'LoginTab',

  components: {
    InputLabel,
    ActionCheckbox,
    ActionButton,
    EmptyButton
  },
  
  data: function() {
    return {
      flood: 0,
      clickInterval: 5000,
      labels: [
        { name: 'auth:login:lbl:login', model: '', key: 'login', icon: 'user', type: 'text' },
        { name: 'auth:login:lbl:pwd', model: '', key: 'password', icon: 'key', type: 'password' }
      ],
    }
  },

  computed: {
    ...mapState('auth', ['isRememberPass', 'auth']),
    ...mapGetters('localization', ['loc']),
    passModel: {
      get: function() {
        return this.isRememberPass
      },
      set: function(value) {
        this.setIsRememberPass(value)
      }
    }
  },

  methods: {
    ...mapMutations('auth', ['setIsRememberPass', 'setCurrentTab']),

    getModels: function() {
      return this.labels.map(item => item.model)
    },

    clearModels: function() {
      this.labels.forEach(item => item.model = '')
    },

    login: function() {
      if(this.flood > Date.now())return;
      this.flood = Date.now() + this.clickInterval;
      const login = this.auth.login;
      const password = this.auth.password;
      if(login.length < 4 || password.length < 4) {
        this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('auth_15'), time: 3000});
        return;
      }
      window.mp.trigger("auth:save:pass", login, password, this.isRememberPass);
      window.mp.triggerServer("signin", login, password);
      //this.clearModels()
    },
    enter(e){
      if(e.keyCode == 13)
        this.login();
    },
     toRecover: function() {
      this.setCurrentTab({ page: 'PasswordRecovery' })
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
    color: #ffffff;
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
      &__block {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin: 0 0 2rem 0;
      }
    }
    .checkbox__text {
      font-size: 0.9rem;
      color: #fff;
    }
  }
}
</style>
