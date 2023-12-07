<template>
  <div class="chips">
    <div class="chips-trade">
      <div class="chips-trade__title">
        <span class="title-text">Welcome to Go Casino</span>
      </div>
      <span class="chips-trade__desc">{{ loc('cashbox_1') }}</span>
      <div class="chips-trade__list">
        <list-item
          v-for="item in chips"
          :key="item.id"
          :item="item"
          :toLocal="toLocal"
        />
      </div>
      <div 
        @click="buyChips" 
        class="chips-trade__btn"
      >
        <span>{{ loc('cashbox_2') }}</span>
        <span v-if="totalAmount > 0"> | {{totalAmount}}</span>
      </div>
      <div class="chips-trade__list chips-trade__list--border">
        <div class="chips-trade__info">
          <span class="info__text">{{ loc('cashbox_4') }}:</span>
          <span class="info__value"> ${{ toLocal(playerBank) }}</span>
        </div>
        <div 
          @click="sellChips" 
          class="chips-trade__btn chips-trade__btn--add"
        >{{ loc('cashbox_3') }}</div>
      </div>
    </div>
    <div 
      class="chips-wrap"
      @click="close"
    ></div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

import ListItem from './components/ListItem'

export default {
  name: 'ChipsTradeMenu',

  components: { ListItem },

  computed: {
    ...mapState('chipsTradeMenu', ['chips', 'isPopup', 'playerBank']),
    ...mapGetters('localization',['loc']),

    totalAmount: function () {
      return this.chips.reduce(function(acc, obj) {
        return acc + obj.price * obj.quantity
      }, 0)
    }
  },

  methods: {
    ...mapMutations('chipsTradeMenu', ['resetAllChips']),

    toLocal: function (value) {
      return value.toLocaleString()
    },
    
    close(){
      window.mp.trigger("guiClose");
    },

    buyChips: function () {
      const chips = this.chips
      if((chips[0].quantity + chips[1].quantity + chips[2].quantity + chips[3].quantity + chips[4].quantity) < 1) return;

      window.mp.trigger("playerBoughtChips", chips[0].quantity, chips[1].quantity, chips[2].quantity, chips[3].quantity, chips[4].quantity)
      this.resetAllChips();
    },

    someMethod: function () {
      window.mp.trigger("playerBoughtChips", null)
    },

    sellChips: function () {
      window.mp.trigger("playerSoldChips")
    }
  }
}
</script>

<style lang="scss" scoped>
.chips{
  width: 100%;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  &-wrap{
    background: rgba(17, 17, 17, 0.7);
    width: 100%;
    height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 0;
    position: absolute;
    left: 0;
    top: 0;
  }
  &-trade {
    width: 40em;
    height: 100vh;
    background-image: url('/img/chipsTradeMenu/bg.png');
    background-size: cover;
    background-position: top;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 5.2rem 2.5rem 0 2.5rem;
    position: relative;
    z-index: 1;
    &:before{
      content: '';
      width: 7.6rem;
      height: 2.1rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/chipsTradeMenu/logo.png');
      position: absolute;
      top: 2rem;
    }
    &__close {
      background-color: transparent;
      color: #bfbfbf;
      font-size: 1.5rem;
      line-height: 1rem;
      position: absolute;
      top: 5%;
      right: 3%;
    }
    &__title {
      font-style: italic;
      font-weight: 600;
      font-size: 3rem;
      line-height: 130%;
      letter-spacing: 0.04em;
      text-transform: uppercase;
      color: #FFFFFF;
      text-shadow: 0 .65rem 1rem rgba(255, 255, 255, 0.3);
      transform: rotate(-5deg);
      margin-bottom: 4.3rem;
    }
    &__desc {
      font-weight: bold;
      font-size: 1rem;
      line-height: 130%;
      letter-spacing: 0.12em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.6);
      margin-bottom: 1.5rem;
    }
    &__list {
      display: flex;
      width: 100%;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 2rem;
      &--border{
        margin-bottom: 0;
        margin-top: 4rem;
        padding-top: 2rem;
        border-top: 1px solid rgba(255, 255, 255, 0.2);
      }
    }
    &__btn {
      background: #AA75BD;
      border-radius: .15rem;
      padding: .9rem 1.5rem;
      letter-spacing: .45rem;
      font-weight: bold;
      color: #fff;
      font-size: .8rem;
      line-height: 130%;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      transition: all .15s ease;
      &--add{
        letter-spacing: 0.2em;
        border: 1px solid rgba(255, 255, 255, 0.4);
        background-color: transparent;
      }
    }
    &__info {
      font-weight: bold;
      font-size: 1rem;
      line-height: 130%;
      letter-spacing: 0.12em;
      text-transform: uppercase;
      .info__text {
        color: rgba(255, 255, 255, 0.7);
      }
      .info__value {
        color: #ffffff;
      }
    }
    &__confirm {
      position: fixed;
      background-color: rgba(0, 0, 0, 0.7);
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      // display: flex;
      // align-items: center;
      // justify-content: center;
    }
  }
}
</style>
