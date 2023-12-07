<template>
  <div class="browser-tab">
    <div class="browser-tab__header">
      <div class="close">
        <button class="close__btn close__btn-acting"></button>
        <div class="close__btn"></div>
        <div class="close__btn"></div>
      </div>
      <div class="navigation">
        <button class="navigation__btn navigation__btn-left">
          <div class="icon"></div>
        </button>
        <button class="navigation__btn navigation__btn-right">
          <div class="icon"></div>
        </button>
      </div>
      <div class="search">
        <input
          type="text"
          class="search__field"
          v-model="srcModel"
          @keypress.enter="setCurrentSrc(srcModel)"
        >
        <button class="search__refresh"></button>
      </div>
      <div class="plus"></div>
    </div>
    <iframe
      :src="currentSrc"
      class="browser-tab__content"
      ref="content"
    ></iframe>
  </div>
</template>

<script>
export default {
  name: 'BrowserTab',

  data: function() {
    return {
      currentSrc: 'https://eliterp.ru/',
      srcModel: ''
    }
  },

  methods: {
    setCurrentSrc: function(value) {
      this.currentSrc = value
    }
  }
}
</script>

<style lang="scss" scoped>
.browser-tab {
  
  position: fixed;
  z-index: 5;
  transform: translate(-50%, -50%);
  left: 50%;
  top: 50%;
  // width: 89rem;
  // height: 48.95rem;
  width: 49rem;
  height: 28.95rem;
  background: #fff;
  border-top-right-radius: 0.5rem;
  border-top-left-radius: 0.5rem;
  display: flex;
  flex-direction: column;
  &__header {
    height: 2.65rem;
    display: flex;
    align-items: center;
    padding: 0 2rem 0 1.15rem;
    position: relative;
    .close {
      display: flex;
      &__btn {
        width: 0.6rem;
        height: 0.6rem;
        border-radius: 50%;
        border: 1px solid #C9C9C9;
        background: #D4D4D4;
        margin: 0 0 0 0.4rem;
      }
      &__btn-acting {
        border-color: #CE5347;
        background: #EE6A5F;
        margin: 0;
      }
    }
    .navigation {
      display: flex;
      align-items: center;
      margin: 0 0 0 1rem;
      &__btn {
        background: transparent;
        height: 1.4rem;
        width: 1.4rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 0.1rem;
        &:hover {
          background: #f1f1f1;
        }
        .icon {
          mask: url("/img/hud/smartphone/browserTab/arrow.svg") no-repeat center / contain;
          width: 0.95rem;
          height: 0.95rem;
          background: #BFBFBF;
        }
      }
      &__btn-left {
        transform: rotate(180deg);
      }
    }
    .search {
      position: absolute;
      transform: translate(-50%, 0);
      left: 50%;
      width: 25.8rem;
      height: 1.4rem;
      &::before {
        content: '';
        position: absolute;
        transform: translate(0, -50%);
        top: 50%;
        right: calc(100% + 0.9rem);
        mask: url("/img/hud/smartphone/browserTab/shield.svg") no-repeat center / contain;
        width: 1rem;
        height: 1rem;
        background: #737373;
      }
      &__refresh {
        position: absolute;
        transform: translate(0, -50%);
        top: 50%;
        right: 0.35rem;
        mask: url("/img/hud/smartphone/browserTab/lock.svg") no-repeat center / contain;
        width: 0.8rem;
        height: 0.8rem;
        background: #797979;
        &:hover {
          background: #474747;
        }
      }
      &__field {
        border-radius: 0.3rem;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.05);
        line-height: 1.4rem;
        text-align: center;
        font-size: 0.65rem;
        color: #4C4C4C;
      }
    }
    .plus {
      mask: url("/img/hud/smartphone/plus.svg") no-repeat center / contain;
      width: 0.7rem;
      height: 0.7rem;
      background: #CDCDCD;
      margin: 0 0 0 auto;
    }
  }
  &__content {
    flex: auto;
    border: none;
  }
}
</style>
