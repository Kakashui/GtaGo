<template>
  <div class="auth">
    <SoundSwitch />
    <div class="auth-body">      
      <div class="auth__line auth__line-left">
        <div class="text">{{loc("auth:greateng")}}</div>
        <div class="main"></div>
      </div>
      <div class="auth__line auth__line-right">
        <img src="img/auth/logo.png" class="logo">
        <div class="main"></div>
        <div class="info" v-if="socialClub">
          <div class="info__desc">{{loc("auth:ylogin")}}</div>
          <div class="info__value">{{ socialClub }}</div>
        </div>
      </div>
      <component :is="currentTab" />
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import GreetingsTab from './tabs/GreetingsTab'
import CharacterCreaeteTab from './tabs/CharacterCreaeteTab'
import LoginTab from './tabs/LoginTab'
import CreateNewAccountTab from './tabs/CreaeteNewAccountTab'
import PasswordRecovery from './tabs/PasswordRecovery'
import SoundSwitch from './SoundSwitch'

export default {
  name: 'Auth',

  components: {
    GreetingsTab,
    CharacterCreaeteTab,
    LoginTab,
    CreateNewAccountTab,
    PasswordRecovery,
    SoundSwitch
  },

  computed: {
    ...mapState('auth', ['currentTab', 'socialClub']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.auth {
  position: absolute;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: center / cover no-repeat url("/img/auth/mainBg.png");
 text-transform: uppercase;
  &-body{
    max-width: 1920px;
    height: 100%;
    margin: auto;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
  }
  &__line-left {
    position: relative;
    position: absolute;
    display: flex;
    flex-direction: column;
    align-items: center;
    top: 4.5rem;
    left: 3rem;
    height: 44rem;
    .text {
      writing-mode: vertical-rl;
      text-orientation: mixed;
      transform: rotate(180deg);
      font-size: 1.2rem;
      flex: 0 0 auto;
      color: #ffffff;
      margin: 0 0 0.5rem 0;
    }
    .main {
      flex: 1 1 auto;
      width: 0.1rem;
      background: rgba(255, 255, 255, 0.1);
      // position: absolute;
      transform: translate(-50%, 0);
      // top: calc(100% + 1rem);
      left: 50%;
    }
  }
  &__line-right {
    position: relative;
    position: absolute;
    top: 4.5rem;
    right: 2.15rem;
    .logo {
      width: 2.8rem;
      height: 3.05rem;
    }
    .main {
      height: 38.9rem;
      width: 0.1rem;
      background: rgba(255, 255, 255, 0.1);
      position: absolute;
      transform: translate(-50%, 0);
      top: calc(100% + 1rem);
      left: 50%;
    }
    .info {
      position: absolute;
      bottom: 0;
      right: calc(100% + 1.3rem);
      text-transform: uppercase;
      text-align: right;
      padding: 0 0 0.4rem 0;
      &__desc {
        font-size: 1rem;
        line-height: 1.2rem;
        color: rgba(255, 255, 255, 0.3);
        margin: 0 0 0.25rem 0;
      }
      &__value {
        font-size: 1.2rem;
        line-height: 1.2rem;
        color: #ffffff;
      }
    }
  }
}
</style>
