<template>
  <div class="wedding-menu">
    <audio id="completeWeddingSound" src="http://54.37.138.110/sounds/marsh_mendelsona.mp3" loop></audio>
    <div class="wedding-menu__header">
      <span class="header__title">{{loc('gui_739')}}</span>
      <div class="header__separator"></div>
      <img
        class="header__logo"
        src="img/weddingMenu/header_logo.png"
        alt="logo"
      >
    </div>
    <div class="wedding-menu__main">
      <span v-show="isWeddingComplete" class="main__congratulations"> {{ loc('gui_740') + congratulationsName + loc('741')}} </span>
      <div class="main__actions" v-show="!isWeddingComplete">
        <div class="actions__field">
          <p class="field__label">{{loc('gui_742')}}</p>
          <input
            class="field__input" 
            type="text"
            :placeholder="loc('gui_747')"
            v-model="name"
          >
        </div>
        <div class="actions__field" v-show="!isWeddingComplete">
          <p class="field__label">{{loc('gui_743')}}</p>
          <input
            class="field__input"
            type="text"
            :placeholder="loc('gui_746')"
            v-model="surename"
          >
        </div>
      </div>
      <div class="main__desc">
        <div class="desc__block">
          <img
            class="block__img"
            src="img/weddingMenu/flowers_bouquet.png"
            alt="bouquet"
          >
          <img
            class="block__img ring"
            src="img/weddingMenu/ring.png"
            alt="ring"
          >
        </div>
      </div>
    </div>
    <div class="wedding-menu__footer">
      <button v-show="!isWeddingComplete" class="footer__btn" @click="propose">{{loc('gui_744')}}</button>
      <button :disabled="isWeddingComplete && disableExit" class="footer__btn" @click="exit">{{loc('gui_745')}}</button>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'WeddingMenu',

  data: function () {
    return {
      name: '',
      surename: ''
    }
  },

  computed: {
    ...mapState('weddingMenu', [
      'isWeddingComplete',
      'congratulationsName',
      'disableExit'
    ]),
    ...mapGetters('localization',['loc'])
  },

  methods: {
    propose: function () {
      if (!this.name.length || !this.surename.length) {
        return
      }

      const player = this.name.concat('_', this.surename)

      this.name = ''
      this.surename = ''
      
      window.mp.trigger('marriage:inputName', player)
    },

    exit: function () {
      window.mp.trigger('marriage:cancelPropose')
    }
  }
}
</script>

<style lang="scss" scoped>
.wedding-menu {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  width: 70%;
  background: rgba(255, 255, 255, 0.95);
  border-radius: 6px;
  padding: 13px 40px 25px 40px;
  &__header {
    display: flex;
    align-items: center;
    .header__title {
      font-size: 24px;
      font-weight: 500;
      text-transform: uppercase;
    }
    .header__separator {
      flex:  1 1 auto;
      display: block;
      height: 1px;
      background-color: #242323;
      margin: 0 16px;
    }
  }
  &__main {
    display: flex;
    margin: 40px 0 0 0;
    .main__congratulations {
      font-size: 30px;
      width: 44%;
    }
    .main__actions {
      display: flex;
      flex-direction: column;
      .actions__field {
        margin: 20px 0 0 0;
        .field__label {
          letter-spacing: .6px;
        }
        .field__input {
          margin: 5px 0 0 0;
          border: 1px solid #242323;
          padding: 15px 17px;
          border-radius: 3px;
          width: 100%;
        }
      }
    }
    .main__desc {
      width: 50%;
      height: 100%;
      display: flex;
      justify-content: center;
      align-items: center;
      .desc__block {
        position: relative;
        width: 300px;
        height: 240px;
        .block__img {
          position: absolute;
          left: 0;
          &.ring {
            left: 45%;
            top: 10%;
          }
        }
      }
    }
  }
  &__footer {
    margin: 40px 0 0 0;
    display: flex;
    justify-content: space-between;
    width: 50%;
    .footer__btn {
      width: 168px;
      height: 38px;
      display: flex;
      justify-content: center;
      align-items: center;
      background: #363636;
      border-radius: 5px;
      border: 0;
      color: #FFFFFF;
      &:disabled {
        background: rgba($color: #363636, $alpha: .6);
      }
    }
  }
}
</style>
