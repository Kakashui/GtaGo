<template>
  <div class="player__menu">
    <div class="player__menu__left-block">
      <img class="logo" src="img/poker/logo.png" alt="exit">
      <menu-clock />
    </div>
    <div class="player__menu__central-block">
      <!-- <div 
        :class="[{active: currentPlayer.isVoise}, 'menu__voise']"
        @click="setCurrentPlayerVoise"
      ></div> -->
      <div class="menu__raise-bid" v-if="currentPlayer.chipsCount && currentPlayerMoveValues.minValue && currentPlayerMoveValues.stepValue">
        <div class="raise-bid__top">
          <span class="top__input">$ {{ raiseInput.toLocaleString() }}</span>
        </div>
        <div class="raise-bid__bottom">
          <menu-range />
        </div>
      </div>
      <button
        :disabled="!currentPlayerMove"
        class="menu__all-in"
        @click="allIn"
      >
        All in
      </button>
      <button
        @click="raise(raiseInput)"
        :disabled="!currentPlayerMove"
        class="top__button"
      >
        <div class="top__button-desc">Raise</div>
        <div class="top__button-value">$ {{ raiseInput.toLocaleString() }}</div>
      </button>
      <div class="player__menu__delimiter"></div>
      <button
        @click="fold"
        :disabled="!currentPlayerMove"
        class="menu__button menu__discard"
      >Fold</button>
      <button
        v-if="!currentPlayerMove && grantAutoCheck"
        :class="{ active: autoCheck }"
        class="menu__button menu__pass"
      >
        <div class="menu__pass-info">
          <input
            class="menu__pass__input"
            type="checkbox"
            id="checkbox"
            v-model="autoCheckIput"
          >
          <label for="checkbox">Auto check</label>
        </div>
      </button>
      <button
        v-else
        @click="check"
        :disabled="!currentPlayerMove"
        class="menu__button menu__pass"
      >
        <div class="menu__pass-info" v-if="currentPlayerMoveValues.checkValue">
          <span>Call</span>
          <span >$ {{ currentPlayerMoveValues.checkValue.toLocaleString() }}</span>
        </div>
        <span v-else>Check</span>
      </button>
    </div>
    <div class="player__menu__right-block">
      <div class="menu__exit" @click="exit">{{loc('poker_6')}}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import MenuClock from './CurrentPlayerMenuClock'
import MenuRange from './CurrentPlayerMenuRange'

export default {
  name: 'CurrentPlayerMenu',

  components: { MenuClock, MenuRange },

  props: ['currentPlayer'],

  computed: {
    ...mapState('poker', [
      'currentPlayerMove',
      'currentPlayerMoveValues',
      'bankAvailable',
      'currentPlayerRaiseInput',
      'currentCheckValue',
      'autoCheck',
      'grantAutoCheck',
    ]),
    ...mapGetters('localization',['loc']),

    raiseInput: {
      get: function () {
        return this.currentPlayerRaiseInput
      },
      set: function (value) {
        this.setCurrentPlayerRaiseInput(value)
      }
    },

    autoCheckIput: {
      get: function () {
        return this.autoCheck
      },
      set: function (value) {
        return this.toggleAutoCheck(value)
      }
    }
  },

  methods: {
    ...mapMutations('poker', [
      'setCurrentPlayerRaiseInput',
      'incrementPlayerRaiseInput',
      'decrementPlayerRaiseInput',
      'toggleAutoCheck'
    ]),

    fold: function() {
      window.mp.trigger('poker:fold')
    },

    check: function () {
      window.mp.trigger('poker:check')
    },

    raise: function (value) {
      window.mp.trigger('poker:raise', value)
    },

    allIn: function () {
      window.mp.trigger('poker:allIn')
    },

    exit: function () {
      this.$emit('setExitWindow')
    },

    setCurrentPlayerVoise: function () {
      window.mp.trigger('poker:setCurrentPlayerVoise')
    }
  },

  mounted: function () {
    this.setCurrentPlayerRaiseInput(this.currentPlayerMoveValues.minValue)
  }
}
</script>

<style lang="scss" scoped>
.player__menu {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 7rem;
  position: relative;
  border-top: .1rem solid rgba(255, 255, 255, 0.1);
  &__delimiter{
    height: 100%;
    width: .1rem;
    background: rgba(255, 255, 255, 0.1);
  }
  &__left-block,  &__right-block{
    position: absolute;
    display: flex;
    align-items: center;
    justify-content: center;
  }
  &__left-block{
    left: 2rem;
  }
  &__right-block{
    right: 2rem;
  }
  &__central-block{
    display: flex;
    align-items: center;
    justify-content: center;
    height: 3rem;
    position: relative;
    &>div, &>button{
      margin-right: .4rem;
      height: 100%;
      background: rgba(255, 255, 255, 0.1);
      border-radius: .1rem; 
      &:last-child{
        margin-right: 0;
      }
    }
  }
  &:before{
    content: '';
    position: absolute;
    left: 0;
    bottom: 0;
    width: 100%;
    height: 7.6rem;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center bottom;
    background-image: url('/img/poker/shadow.png');
    pointer-events: none;
  }
  .logo{
    width: 7.6rem;
    margin-right: 1rem;
  }
  .menu{
    &__voise{
      position: absolute;
      right: -.8rem;
      bottom: 0;
      transform: translateX(100%);
      width: 2rem;
      height: 2rem;
      background: rgba(255, 255, 255, 0.1);
      border-radius: 50%;
      transition: .3s;
      display: flex;
      align-items: center;
      justify-content: center;
      &:hover{
        transform: translateX(100%) scale(1.1);
      }
      &:active{
        transform: translateX(100%) scale(1.05);
      }
      &:before{
        content: '';
        width: .9rem;
        height: .9rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/poker/voise-icon.png');
        opacity: .5;
      }
      &.active{
        &:before{
          opacity: 1;
        }
      }
    }
    &__raise-bid{
      position: relative;
    }
    &__all-in{
      font-weight: bold;
      font-size: .9rem;
      text-align: center;
      letter-spacing: 0.04em;
      text-transform: uppercase;
      padding: 0 .6rem;
      color: #B6D12A;
      transition: .3s;
      &:hover{
        transform: scale(1.1);
      }
      &:active{
        transform: scale(1.05);
      }
    }
    &__button{
      padding: 0 1rem;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: bold;
      font-size: .9rem;
      text-transform: uppercase;
      color: #FFFFFF;
      transition: .3s;
      &:hover{
        transform: scale(1.1);
      }
      &:active{
        transform: scale(1.05);
      }
      &:before{
        content: '';
        width: 1.2rem;
        height: 1.2rem;
        margin-right: .5rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
      }
    }
    &__discard{
      &:before{
        background-image: url('/img/poker/fold-icon.png');
      }
    }
    &__pass{
      width: 12rem;
      &-info{
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: space-between;
      }
      &:before{
        background-image: url('/img/poker/check-icon.png');
      }
      &__input{
        opacity: 0;
        &+label{
          color: rgba(255, 255, 255, 0.5);
        }
        &:checked{
          &+label{
            color: #fff;
          }
        }
      }
    }
    &__exit {
      height: 3rem;
      padding: 0 1.2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: bold;
      font-size: .9rem;
      letter-spacing: 0.1em;
      color: #FFFFFF;
      position: relative;
      background: rgba(255, 255, 255, 0.1);
      border-radius: .1rem;
      transition: .3s;
      &:hover{
        transform: scale(1.1);
      }
      &:active{
        transform: scale(1.05);
      }
      &::after{
        content: '';
        width: 1rem;
        height: 1rem;
        margin-left: .5rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/poker/arrow-back-outline.png');
      }
    }
  }
  .raise-bid{
    &__top{
      position: absolute;
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      height: 100%;
      pointer-events: none;
      font-weight: bold;
      font-size: .9rem;
      text-align: center;
      color: #FFFFFF;
    }
    &__bottom{
      height: 100%;
    }
  }
  .top__button {
    display: flex;
    align-items: center;
    justify-content: space-between;
    min-width: 11.9rem;
    font-weight: bold;
    font-size: .9rem;
    color: #FFFFFF;
    padding: 0 1rem 0 2.5rem;
    background: #A3BC22;
    border-radius: .1rem;
    transition: .3s;
    position: relative;
    &:before{
      content: '';
      width: 1.2rem;
      height: 1.2rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/poker/raise-icon.png');
      position: absolute;
      left: .8rem;
    }
    &:hover {
      box-shadow: 0 .5rem 1.5rem rgba(182, 209, 42, 0.5);
    }
    &:disabled {
      
    }
  }
}
</style>
