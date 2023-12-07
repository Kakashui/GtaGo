<template>
  <div class="third-tab">
    <button class="third-tab__btn" @click="setCurrentTab({ key: currentTab.key, section: 'SecondarySection', data: { title: currentTab.data.currentItem.title }})"></button>
    <div class="third-tab__content">
      <span class="header">
        <div class="header__icon">
          <div class="img" :style="{ '-webkit-mask': 'url(' + `img/hud/smartphone/mapTab/categories/${currentTab.data.currentItem.icon}.svg` + ') no-repeat center / contain' }"></div>
        </div>
        <div class="header__title">{{ currentTab.data.currentItem.title }}</div>
      </span>
      <p class="address">{{ currentTab.data.currentItem.address }}</p>
      <div class="footer">
        <div class="footer__distance">
          <span class="value">{{ currentTab.data.currentItem.distance }}</span>
          <span class="desc">M</span>
        </div>
        <button class="footer__btn" @click="start(currentTab.key, currentTab.data.currentItem)">Start</button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'ThirdSection',

  computed: {
    ...mapState('smartphone', ['currentTab', 'colorTheme'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme']),

    start: function (tabKey, item) {
      window.mp.trigger('phone::gps::setWaypoint', item.position.x, item.position.y);
      this.setCurrentTab({ key: tabKey, section: 'NotifySection' });
    }
  },

  mounted: function() {
    this.setColorTheme({ header: this.colorTheme.header, button: 'none' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes right-to-left {
  from {
    transform: translateX(100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}
@keyframes slide-in {
  from {
    transform: translateY(100%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
@keyframes shake {
  25% {
    transform: translateX(0.3rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.15rem);
  }
}
.third-tab {
  height: 100%;
  position: relative;
  padding: 2.5rem 0.5rem 0.55rem 0.5rem;
  display: flex;
  align-items: flex-end;
  &::before {
    content: '';
    height: 13rem;
    width: 100%;
    position: absolute;
    bottom: 0;
    right: 0;
    background: linear-gradient(180deg, rgba(0, 0, 0, 0) 0%, rgba(0, 0, 0, 0.9) 38.13%);
  }
  &__btn {
    position: absolute;
    top: 2.5rem;
    left: 1rem;
    width: 2rem;
    height: 2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 0.4rem;
    background: rgba(255, 255, 255, 0.7);
    animation: right-to-left 0.4s;
    &:hover {
      &::after {
        animation: shake 0.4s;
      }
    }
    &::after {
      content: '';
      mask: url('/img/hud/smartphone/arrowLeft.svg') no-repeat center / contain;
      width: 0.35rem;
      height: 0.6rem;
      background: #000;
    }
  }
  &__content {
    background: #fff;
    border-radius: 1rem;
    padding: 1rem 0.75rem;
    animation: slide-in 0.4s;
    z-index: 2;
    .header {
      display: flex;
      align-items: center;
      &__icon {
        min-width: 2.5rem;
        height: 2.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #1BBD1B;
        border-radius: 50%;
        .img {
          width: 1rem;
          height: 1rem;
          background: #fff;
        }
      }
      &__title {
        margin: 0 0 0 0.6rem;
        font-size: 0.9rem;
        text-transform: uppercase;
        color: #323232;
        font-weight: bold;
      }
    }
    .address {
      color: #828282;
      font-size: 0.65rem;
      text-transform: uppercase;
      margin: 0.55rem 0 0 0;
    }
    .footer {
      margin: 0.75rem 0 0 0;
      border-top: 1px solid #ECECEC;
      padding: 0.5rem 0 0 0;
      display: flex;
      align-items: center;
      justify-content: space-between;
      &__distance {
        color: #1BBD1B;
        .value {
          font-size: 1.5rem;
          font-weight: bold;
        }
        .desc {
          font-size: 0.65rem;
        }
      }
      &__btn {
        background: linear-gradient(268.27deg, #1BBD1B 4.17%, #2ADF5D 97.17%);
        color: #fff;
        font-size: 0.7rem;
        font-weight: 500;
        border-radius: 1.6rem;
        width: 6rem;
        height: 2rem;
        text-transform: uppercase;
        &:hover {
          box-shadow: 0px 0.5rem 1.5rem rgba(0, 0, 0, 0.1);
        }
      }
    }
  }
}
</style>
