<template>
  <div class="stock-menu">
    <div class="stock-menu-bg">
      <img src="/img/stockMenu/bg.png" alt="bg">
    </div>
    <div class="stock-menu__list">
      <div
        v-for="item in items"
        :key="item.type"
        :class="[{ active: item.typeKey === type }, 'list__item']"
        @click="setType(item.typeKey)"
      >
        <img
          :src="`/img/stockMenu/stock-icon-${item.typeTitle}.svg`"
          :alt="item.type"
          class="item__icon"
        >
        <span class="item__title">{{ loc(item.title) }}</span>
      </div>
    </div>
    <div class="stock-menu__cards">
      <div class="cards__current">
        <img
          :src="`/img/stockMenu/stock-card-${currentCard.typeTitle}.png`"
          :alt="currentCard.typeTitle"
          :class="[currentCard.typeTitle, 'current__img']"
        >
        <div class="current__title">{{ loc(currentCard.title) }}</div>
        <div class="current__count">{{ cardCountLocale }}</div>
      </div>
      <div class="cards__actions">
        <button class="actions__btn" @click="prevItem">
          <img class="btn__icon" src="/img/stockMenu/stock-arrow.svg" alt="arrow">
        </button>
        <button class="actions__btn" @click="nextItem">
          <img class="btn__icon"  src="/img/stockMenu/stock-arrow.svg" alt="arrow">
        </button>
      </div>
    </div>
    <div class="stock-menu__actions">
      <label class="actions__label">
        <input
          v-model="stockInput"
          type="number"
          class="label__input"
        >
      </label>
      <div class="actions__buttons">
        <button class="buttons__item" @click="getGoods">
          <img
            src="/img/stockMenu/stock-arrow-2.svg"
            alt="arrow"
            class="item__icon"
          >
          <span class="item__title">{{loc('gui_733')}}</span>
        </button>
        <button class="buttons__item" @click="putGoods">
          <img
            src="/img/stockMenu/stock-arrow-2.svg"
            alt="arrow"
            class="item__icon"
          >
          <span class="item__title">{{loc('gui_734')}}</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'StockMenu',

  computed: {
    ...mapState('stockMenu', [
      'items',
      'input',
      'type',
      'show'
    ]),
    ...mapGetters('localization',['loc']),

    stockInput: {
      get () {
        return this.input
      },
      set (value) {
        this.setInput(value)
      }
    },

    currentCard: function () {
      return this.items.find(item => item.typeKey === this.type)
    },

    cardCountLocale: function () {
      return this.currentCard.count.toLocaleString()
    }
  },

  methods: {
    ...mapMutations('stockMenu', [
      'setType',
      'setShow',
      'resetState',
      'setInput',
      'incType',
      'decType'
    ]),

    getGoods () {
      window.mp.trigger('stock:get', this.currentCard.typeTitle, this.stockInput)
      this.resetState()
    },

    putGoods () {
      window.mp.trigger('stock:put', this.currentCard.typeTitle, this.stockInput)
      this.resetState()
    },

    nextItem () {
      if (this.type < this.items.length - 1) {
        this.incType()
      } else {
        return
      }
    },

    prevItem () {
      if (this.type > 0) {
        this.decType()
      } else {
        return
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.stock-menu {
   &-bg{
    background-color: #fff;
    position: fixed;
    left: 0;
    top: 0;
    width: 100vw;
    height: 100vh;
    z-index: -1;
    img{
      position: absolute;
      height: 100%;
    }
  }
  display: flex;
  width: 100%;
  align-items: center;
  justify-content: space-around;
  &__list {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    color: #FFF;
    .list__item {
      display: flex;
      align-items: center;
      margin: .8rem 0 0 0;
      &.active {
        .item__icon {
          filter: invert(.3);
        }
        .item__title {
          color: #96AF0D;
          transform: translateX(.6rem);
        }
      }
      .item__icon {
        width: 1.4rem;
        transition: all .2s;
      }
      .item__title {
        text-transform: uppercase;
        margin: 0 0 0 .6rem;
        transition: all .2s;
      }
    }
  }
  &__cards {
    display: flex;
    flex-direction: column;
    align-items: center;
    .cards__current {
      width: 22rem;
      height: 28rem;
      position: relative;
      background-image: url('../../../public/img/stockMenu/stock-card-bg.png');
      background-size: contain;
      background-repeat: no-repeat;
      .current__img {
        position: absolute;
        transform: translate(-50%, -50%);
        &.money {
          width: 19.3rem;
          height: 18.3rem;
          top: 75%;
          left: 70%;
        }
        &.medkits {
          width: 14.8rem;
          height: 24.5rem;
          top: 60%;
          left: 60%;
        }
        &.drugs {
          width: 21.9rem;
          height: 10.4rem;
          top: 90%;
          left: 70%;
        }
        &.mats {
          width: 14.8rem;
          height: 20.1rem;
          top: 70%;
          left: 60%;
        }
        &.weapons {
          width: 25rem;
          height: 32.5rem;
          top: 55%;
          left: 70%;
        }
      }
      .current__title {
        font-size: 2rem;
        text-transform: uppercase;
        font-weight: bold;
        transform: rotate(-90deg);
        width: 15.75rem;
        display: inline-block;
        position: absolute;
        right: 50%;
        bottom: 30%;
        z-index: 2;
      }
      .current__count {
        position: absolute;
        display: inline-block;
        transform: translate(-50%, -50%);
        left: 50%;
        top: 10%;
        font-size: 2rem;
      }
    }
    .cards__actions {
      margin: 2.4rem 0 0 0;
      width: 120%;
      display: flex;
      justify-content: space-between;
      .actions__btn {
        background-color: transparent;
        background-image: url('../../../public/img/stockMenu/stock-cards-btn-bg-1.png');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        width: 4.5rem;
        height: 4.5rem;
        transition: all .2s;
        display: flex;
        align-items: center;
        justify-content: center;
        &:hover {
          background-image: url('../../../public/img/stockMenu/stock-cards-btn-bg-2.png');
        }
        .btn__icon {
          width: 1.65rem;
        }
        &:last-child {
          .btn__icon {
            transform: rotate(180deg);
          }
        }
      }
    }
  }
  &__actions {
    .actions__label {
      border: 2px solid #000;
      border-radius: 1.1rem;
      display: block;
      width: 20rem;
      height: 4rem;
      display: flex;
      align-items: center;
      justify-content: center;
      transform: skew(20deg);
      background-image: url('../../../public/img/stockMenu/stock-input-bg.png');
      background-size: cover;
      background-position: center;
      cursor: text;
      .label__input {
        transform: skew(-20deg);
        width: 86%;
        background: transparent;
        font-size: 1.4rem;
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          -webkit-appearance: none;
          margin: 0;
        }
      }
    }
    .actions__buttons {
      display: flex;
      justify-content: space-between;
      width: 100%;
      margin: 1.5rem 0 0 0;
      .buttons__item {
        width: 9rem;
        height: 3rem;
        border-radius: .6rem;
        transform: skew(-20deg);
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
        transition: all .2s;
        &:hover {
          transform: skew(-20deg) scale(1.05);
        }
        &:first-child {
          background-color: #C6E621;
          width: 8.7rem;
          height: 2.7rem;
          .item__icon {
            transform: skew(20deg) rotate(180deg);
          }
          &::after {
            content: '';
            width: 9rem;
            height: 3rem;
            border-radius: .6rem;
            transform: translate(-50%, -50%);
            left: 50%;
            top: 50%;
            position: absolute;
            border: 1px solid #C6E621;
          }
        }
        &:last-child {
          background-color: transparent;
          border: 1px solid #000;
        }
        .item__icon {
          transform: skew(20deg);
          width: 1.5rem;
        }
        .item__title {
          display: inline-block;
          transform: skew(20deg);
          margin: 0 0 0 .2rem;
        }
      }
    }
  }
}
</style>
