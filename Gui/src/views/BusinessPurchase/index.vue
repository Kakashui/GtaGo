<template>
  <transition name="business-purchase" appear>
    <div class="business-purchase" v-if="Object.keys(businessData).length > 0">
      <div class="business-purchase__layer"></div>
      <div class="business-purchase__header">
        <input
          class="close"
          type="image"
          src="img/businessPurchase/close.svg"
          @click="close"
        >
        <div class="id">
          <div class="id__title">ID:</div>
          <div class="id__value">{{ businessData.ID }}</div>
        </div>
        <h5 class="desc">{{ businessData.Description }}</h5>
        <h4 class="title">{{ businessData.Name }}</h4>
      </div>
      <img
        :src="`img/businessPurchase/photos/${businessData.Type}.png`"
        class="business-purchase__img"
      >
      <div class="business-purchase__main">
        <div class="item">
          <img
            src="img/businessPurchase/businessman.svg"
            alt="businessman"
            class="item__icon"
          >
          <span class="item__main">
            <span class="desc">{{ loc('businesspurchase_owner') }}</span>
            <h6 class="value">{{ businessData.Owner }}</h6>
          </span>
        </div>
        <div class="item">
          <img
            src="img/businessPurchase/mafia.svg"
            alt="businessman"
            class="item__icon"
          >
          <span class="item__main">
            <span class="desc">{{ loc('businesspurchase_overseer') }}</span>
            <h6 class="value">{{ businessData.Overseer }}</h6>
          </span>
        </div>
        <div class="price" v-if="businessData.Purchaseable">
          <span class="price__desc">{{ loc('businesspurchase_govprice') }}</span>
          <span class="price__value">${{ businessData.Price.toLocaleString() }}</span>
        </div>
        <div :class="[{ disabled: !businessData.Purchaseable }, 'buy']">
          <button
            @click="buy"
            class="buy__btn"
            :disabled="!businessData.Purchaseable"
          >{{ loc('businesspurchase_buybtn') }}</button>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapState } from 'vuex'
import { mapGetters } from 'vuex'

export default {
  name: 'BusinessPurchase',

  computed: {
    ...mapState('businessPurchase', ['businessData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    buy: function() {
      window.mp.trigger('businesses::infoPanel_buyClick');
    },

    close: function() {
      window.mp.trigger('businesses::infoPanel_closeClick');
    }
  }
}
</script>

<style lang="scss" scoped>
.business-purchase {
  width: 30rem;
  display: flex;
  flex-direction: column;
  position: relative;
  &__layer {
    background: center / cover no-repeat url("/img/businessPurchase/layer.png");
    position: absolute;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
    z-index: 1;
  }
  &__header {
    position: relative;
    color: #fff;
    background-color: #d9ff00;
    padding: 2rem 0 2rem 2rem;
    .close {
      position: absolute;
      right: 1rem;
      top: 1rem;
      width: 1rem;
      height: 1rem;
      z-index: 2;
    }
    .id {
      position: absolute;
      transform: translate(0, -50%);
      left: 2rem;
      top: 0;
      background: #fff;
      padding: .6rem 1.4rem;
      color: #000;
      display: flex;
      font-size: .7rem;
      font-weight: bold;
      letter-spacing: .05rem;
      border-radius: 5rem;
      z-index: 2;
      &__value {
        margin: 0 0 0 .2rem;
      }
    }
    .desc {
      text-transform: uppercase;
      font-size: .8rem;
      position: relative;
      z-index: 2;
    }
    .title {
      text-transform: uppercase;
      font-size: 2rem;
      position: relative;
      z-index: 2;
    }
  }
  &__img {
    width: 100%;
    height: auto;
    background-color: #000;
  }
  &__main {
    background-color: #000;
    padding: 0 0 3.75rem 2.5rem;
    position: relative;
    .item {
      color: #fff;
      display: flex;
      align-items: center;
      position: relative;
      padding: 0 0 .5rem 0;
      margin: 1.4rem 0 0 0;
      position: relative;
      z-index: 2;
      &::after {
        content: '';
        position: absolute;
        bottom: 0;
        left: 0;
        height: .1rem;
        width: 20.2rem;
        background: linear-gradient(90deg, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%);
      }
      &__icon {
        width: 2rem;
        height: 2rem;
        opacity: 0.3;
      }
      &__main {
        text-transform: uppercase;
        margin: 0 0 0 .75rem;
        .desc {
          font-size: .7rem;
          opacity: 0.5;
        }
        .value {
          font-weight: bold;
          font-size: 1rem;
          letter-spacing: .05rem;
        }
      }
    }
    .price {
      margin: 1.4rem 0 0 0;
      display: flex;
      flex-direction: column;
      position: relative;
      z-index: 2;
      &__desc {
        color: rgba(255, 255, 255, 0.5);
        text-transform: uppercase;
        font-size: .7rem;
      }
      &__value {
        color: #B6D300;
        font-size: 1.5rem;
        font-weight: bold;
        letter-spacing: .1rem;
        margin: .2rem 0 0 0;
      }
    }
    .buy {
      position: absolute;
      bottom: 0;
      right: 0;
      width: 11.9rem;
      height: 11.9rem;
      z-index: 2;
      background: center / contain no-repeat url("/img/businessPurchase/scribble.png");
      &.disabled {
        background: center / contain no-repeat url("/img/businessPurchase/scribble-1.png");
        .buy__btn {
          color: rgba(255, 255, 255, 0.2);
        }
      }
      &__btn {
        color: #fff;
        background-color: transparent;
        text-transform: uppercase;
        letter-spacing: .05rem;
        font-weight: bold;
        transform: translate(-50%, -50%) rotate(-45deg);
        position: absolute;
        top: 72%;
        left: 72%;
        padding: .4rem .8rem;
        &:hover {
          &::after {
            content: '';
            position: absolute;
            transform: translate(-50%, -50%) rotate(-45deg);
            top: 50%;
            left: 50%;
            width: 12.3rem;
            height: 12.3rem;
            background-color: rgba(182, 211, 0, 0.3);
            filter: blur(100px);
          }
        }
        &:disabled {
          &::after {
            display: none;
          }
        }
      }
    }
  }
}
@keyframes business-purchase {
  0% {
    transform: scale(0.2);
  }
  50% {
    transform: scale(1.2);
  }
  100% {
    transform: scale(1);
  }
}
.business-purchase-enter-active {
  animation: home-purchase .4s;
  position: absolute;
}
.business-purchase-leave-active {
  animation: home-purchase .4s reverse;
  position: absolute;
}
</style>
