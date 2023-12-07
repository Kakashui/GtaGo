<template>
  <div class="sale-of-chips">
    <div class="sale-of-chips__modal">
      <div 
        class="close-modal"
        @click="$emit('setSaleOfChips', false)"
      >
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 40 40" fill="none">
          <path d="M28.75 11.25L11.25 28.75M28.75 28.75L11.25 11.25L28.75 28.75Z" stroke="white" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
      </div>
      <div class="sale-of-chips__balance">
        <div class="sale-of-chips__balance-desc">{{loc('poker_7')}}</div>
        <div class="sale-of-chips__balance-value">${{currentPlayer.bank.toLocaleString()}}</div>
      </div>
      <div class="sale-of-chips__modal__title">
        <div class="sale-of-chips__modal__title-desc">{{loc('poker_8')}}</div>
        <div class="sale-of-chips__modal__title-value">{{loc('poker_9')}}</div>
      </div>
      <div class="sale-of-chips__modal__info">
        <div class="sale-of-chips__input">
          <input 
            type="number"
            placeholder="0"
            v-model="chipsCount"
          >
        </div>
        <div 
          class="sale-of-chips__btn"
          @click="topUpChips"
        >{{loc('poker_10')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'SaleOfChips',
  data: function() {
    return {
      chipsCount: null,
    }
  },
  computed: {
    ...mapState('poker', ['currentPlayer']),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    topUpChips: function() {
      if (this.chipsCount && this.chipsCount > 0) {
        window.mp.trigger('poker:buyChips', this.chipsCount);
        this.$emit('setSaleOfChips', false);
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.sale-of-chips{
  position: fixed;
  width: 100%;
  height: 100vh;
  left: 0;
  top: 0;
  background-color: rgba(34, 34, 34, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3;
  &__modal{
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    width: 35rem;
    height: 14.55rem;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: left top;
    background-image: url('/img/poker/modal-bg.png');
    position: relative;
    padding: 2rem;
    .close-modal{
      width: 2rem;
      height: 2rem;
      position: absolute;
      top: 0;
      right: -.5rem;
      transform: translateX(100%);
      svg{
        width: 100%;
        height: 100%;
      }
    }
    &__title{
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      color: #fff;
      margin-top: -.75rem;
      &-desc{
        font-weight: 400;
        font-size: .8rem;
        line-height: .9rem;
        letter-spacing: 0.04em;
        text-transform: uppercase;
        opacity: .3;
      }
      &-value{
        margin-top: -.2rem;
        font-style: normal;
        font-weight: 600;
        font-size: 3rem;
        line-height: 3.3rem;
        text-transform: uppercase;
        max-width: 11.2rem;;
      }
    }
  }
  &__info{
    display: flex;
    flex-direction: column;
    width: 10.9rem;
  }
  &__input{
    position: relative;
    border: 1px dashed rgba(255, 255, 255, 0.2);
    height: 2.8rem;
    border-radius: .1rem;
    width: 100%;
    display: flex;
    align-items: center;
    margin-bottom: .8rem;
    &:before, &:after{
      pointer-events: none;
      position: absolute;
    }
    &:before{
      content: '$';
      font-style: normal;
      font-weight: bold;
      font-size: 1rem;
      color: #D6FF00;
      left: .8rem;
    }
    &:after{
      content: '';
      width: 1px;
      height: 1.7rem;
      background: rgba(255, 255, 255, 0.2);
      left: 1.95rem;
    }
    input{
      outline: none;
      box-sizing: border-box;
      height: 100%;
      width: 100%;
      background-color: transparent;
      font-weight: bold;
      font-size: 1rem;
      color: rgba(255, 255, 255, 0.3);
      padding-left: 2.6rem;
      &::-webkit-outer-spin-button,
      &::-webkit-inner-spin-button {
        -webkit-appearance: none;
      }
    }
  }
  &__btn{
    width: 100%;
    height: 3.2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: .8rem;
    text-align: center;
    letter-spacing: 0.1em;
    text-transform: uppercase;
    color: #FFFFFF;
    background: #B5D60D;
    transition: .3s;
    border-radius: .1rem;
    &:hover{
      box-shadow: 0 .5rem 1.5rem rgba(182, 209, 42, 0.5);
    }

  }
  &__balance{
    position: absolute;
    left: 2rem;
    top: -1rem;
    transform: translateY(-100%);
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    font-weight: bold;
    color: #FFFFFF;
    &-desc{
      margin-bottom: .8rem;
      font-size: .8rem;
      letter-spacing: 0.35em;
      text-transform: uppercase;
    }
    &-value{
      font-size: 2.4rem;
      letter-spacing: 0.05em;
      display: flex;
      align-items: center;
      &:after{
        content: '';
        width: 1.8rem;
        min-width: 1.8rem;
        height: 1.8rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/poker/chip_black.png');
        margin-left: .4rem;
      }
    }
  }
}
</style>