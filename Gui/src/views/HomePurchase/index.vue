<template>
  <transition name="home-purchase" appear>
    <div class="home-purchase" v-if="Object.keys(homeData).length > 0">
      <img src="img/homePurchase/layer.png" class="home-purchase__layer">
      <div class="home-purchase__header">
        <div class="id">
          <span class="id__title">ID</span>
          <span class="id__value">{{ homeData.ID }}</span>
        </div>
        <input
          class="close"
          type="image"
          src="img/homePurchase/close.svg"
          @click="close"
        >
        <span class="desc">{{ loc('home_purchase_9') }}</span>
        <h6 class="value">{{ homeData.Owner }}</h6>
      </div>
      <img
        :src="`img/homePurchase/photos/${123}.png`"
        alt="photo"
        class="home-purchase__img"
      >
      <div class="home-purchase__main">
        <div class="item">
          <img
            src="img/homePurchase/star.svg"
            alt="star"
            class="item__icon"
          >
          <div class="item__main">
            <span class="desc">{{ loc('home_purchase_1') }}</span>
            <h6 class="value">{{ homeData.Class }}</h6>
          </div>
        </div>
        <div class="item">
          <img
            src="img/homePurchase/person.svg"
            alt="star"
            class="item__icon"
          >
          <div class="item__main">
            <span class="desc">{{ loc('home_purchase_2') }}</span>
            <h6 class="value">{{ homeData.Roommates }}</h6>
          </div>
        </div>
        <div class="item">
          <img
            src="img/homePurchase/car.svg"
            alt="star"
            class="item__icon"
          >
          <div class="item__main">
            <span class="desc">{{ loc('home_purchase_3') }}</span>
            <h6 class="value">{{ homeData.GarageSpace }}</h6>
          </div>
        </div>
        <div class="price">
          <span class="price__title">{{ loc('home_purchase_4') }}</span>
          <span class="price__value">${{ homeData.Price }}</span>
        </div>
        <button
          @click="enter"
          :class="[{ disabled: !homeData.CanEnter && !homeData.IsTarget}, 'btn']"
          :disabled="!homeData.CanEnter && !homeData.IsTarget"
        >
          <div class="btn__main">
            <span class="top" v-if="!homeData.IsLocked">{{ loc('home_purchase_5') }}</span>
            <span class="top" v-else>{{ loc('home_purchase_7') }}</span>
            <span class="bottom" v-if="!homeData.CanEnter && homeData.IsTarget">{{ loc('home_purchase_10') }}</span>
            <span class="bottom" v-else>{{ loc('home_purchase_6') }}</span>
          </div>
          <img
            src="img/homePurchase/arrow.svg"
            alt="arrow"
            class="btn__icon"
          >
        </button>
        <img
          :src="!homeData.IsLocked ? 'img/homePurchase/open.png' : 'img/homePurchase/closed.png'"
          alt="lock"
          class="img"
        >
      </div>
      <button
        @click="buy"
        v-show="!homeData.IsSelled"
        class="home-purchase__btn"
      >{{ loc('home_purchase_8') }}</button>
    </div>
  </transition>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'HomePurchase',

  computed: {
    ...mapState('homePurchase', ['homeData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    buy: function() {
      window.mp.trigger("housepurchase::buy")
    },
    enter: function() {
      if (!this.homeData.CanEnter && this.homeData.IsTarget)
        window.mp.trigger("housepurchase::breakTheDoor")
      else
        window.mp.trigger("housepurchase::enter")
    },
    close: function() {
      window.mp.trigger("housepurchase::close")
    }
  }
}
</script>

<style lang="scss" scoped>
.home-purchase {
  display: flex;
  flex-direction: column;
  position: relative;
  background: #000;
  width: 30rem;
  height: 43.35rem;
  &__layer {
    position: absolute;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
    z-index: 1;
  }
  &__header {
    color: #fff;
    position: relative;
    background: center / cover no-repeat url("/img/homePurchase/green-texture.png");
    text-transform: uppercase;
    padding: 1.5rem 2rem;
    z-index: 2;
    .id {
      position: absolute;
      transform: translate(0, -50%);
      top: 0;
      left: 2rem;
      background: #fff;
      color: #000;
      padding: .6rem 1.4rem;
      border-radius: 5rem;
    }
    .close {
      position: absolute;
      top: 1rem;
      right: 1rem;
    }
    .desc {
      font-size: .8rem;
      font-weight: bold;
      letter-spacing: 0.05rem;
      position: relative;
    }
    .value {
      font-size: 2rem;
      font-weight: bold;
      letter-spacing: 0.05rem;
      position: relative;
    }
  }
  &__img {
    background-color: #000;
    width: 100%;
    height: auto;
  }
  &__main {
    display: flex;
    flex-direction: column;
    background-color: #000;
    padding: 0 0 1.5rem 2rem;
    position: relative;
    .item {
      z-index: 2;
      display: flex;
      align-items: center;
      margin: 1.2rem 0 0 0;
      position: relative;
      padding: 0 0 .5rem 0;
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
        display: flex;
        flex-direction: column;
        margin: 0 0 0 .9rem;
        text-transform: uppercase;
        font-weight: bold;
        letter-spacing: .05rem;
        .desc {
          color: rgba(255, 255, 255, 0.5);
          font-size: .7rem;
        }
        .value {
          color: #fff;
          font-size: 1rem;
          margin: .25rem 0 0 0;
        }
      }
    }
    .price {
      margin: 1.5rem 0 0 0;
      display: flex;
      flex-direction: column;
      z-index: 2;
      &__title {
        font-size: .7rem;
        color: rgba(255, 255, 255, 0.5);
        text-transform: uppercase;
      }
      &__value {
        font-size: 1.5rem;
        font-weight: bold;
        letter-spacing: 0.05rem;
        color: #B6D300;
        margin: 0.25rem 0 0 0;
      }
    }
    .btn {
      position: absolute;
      display: flex;
      align-items: center;
      justify-content: center;
      width: 9.5rem;
      height: 3.5rem;
      background-color: #fff;
      border-radius: 5rem;
      bottom: 1rem;
      right: 2.5rem;
      z-index: 2;
      border: .1rem solid #fff;
      &:hover {
        box-shadow: 0px 0px 1.5rem rgba(255, 255, 255, 0.5);
      }
      &.disabled {
        border-color: #4A4A4A;
        backdrop-filter: blur(1.5rem);
        background-color: rgba(255, 255, 255, 0.1);
        &:hover {
          box-shadow: none;
        }
        .btn__main {
          .top {
            color: rgba(130, 130, 130, 0.5);
          }
          .bottom {
            color: #616161;
          }
        }
        .btn__icon {
          filter: grayscale(1);
        }
      }
      &__main {
        display: grid;
        text-transform: uppercase;
        letter-spacing: .05rem;
        margin: 0 0 0 .5rem;
        .top {
          font-size: .7rem;
          color: rgba(0, 0, 0, 0.5);
        }
        .bottom {
          font-weight: bold;
        }
      }
      &__icon {
        width: 1.3rem;
        height: auto;
        margin: 0 0 0 1.5rem;
      }
    }
    .img {
      position: absolute;
      right: 0;
      bottom: 0;
    }
  }
  &__btn {
    background: right;
    background: center / cover no-repeat url("/img/homePurchase/scribble.png");
    height: 4.5rem;
    color: #fff;
    text-transform: uppercase;
    font-weight: bold;
    letter-spacing: .05rem;
    padding: 1rem 0 0 0;
    z-index: 2;
  }
}
@keyframes home-purchase {
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
.home-purchase-enter-active {
  animation: home-purchase .4s;
  position: absolute;
}
.home-purchase-leave-active {
  animation: home-purchase .4s reverse;
  position: absolute;
}
</style>
