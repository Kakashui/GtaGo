<template>
  <div class="general-section">
    <div :class="[{ hidden: isFullHistory }, 'general-section__title']">Bank Go</div>
    <div :class="[{ hidden: isFullHistory }, 'general-section__card']">
      <div class="main">
        <span class="main__number">№ {{ bankAccount }}</span>
        <div class="main__balance">
          <p class="title">{{loc('phone:bank:1')}}</p>
          <p class="value">${{ balance.toLocaleString() }}</p>
        </div>
      </div>
      <img
        src="img/hud/smartphone/bankTab/cardShadow.png"
        alt="shadow"
        class="shadow"
      >
    </div>
    <div :class="[{ hidden: isFullHistory }, 'general-section__actions']">
      <button
        class="btn"
        v-for="(button, index) in buttons"
        :key="index"
        @click="setCurrentTab({ key: currentTab.key, section: button.section, subsection: button.subsection })"
      >
        <div class="btn__icon">
          <img
            :src="`img/hud/smartphone/bankTab/${button.icon}.svg`"
            :alt="button.icon"
            class="img"
          >
        </div>
        <span class="btn__text">{{ loc(button.title) }}</span>
      </button>
    </div>
    <div :class="[{ full: isFullHistory }, 'general-section__history']">
      <span class="title">{{loc('phone:bank:2')}}</span>
      <div v-bar>
        <div class="list">
          <history-item
            v-for="(item, index) in historyItems"
            :key="index"
            :item="item"
          />
        </div>
      </div>
    </div>
    <button class="general-section__btn" @click="isFullHistory =!isFullHistory">
      <span class="text" v-if="!isFullHistory">{{loc('phone:bank:3')}}</span>
      <div class="inner" v-else>
        <div class="inner__icon"></div>
        <span class="inner__text">{{loc('phone:bank:4')}}</span>
      </div>
    </button>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import HistoryItem from './HistoryItem'

export default {
  name: 'GeneralSection',

  components: { HistoryItem },

  data: function() {
    return {
      isFullHistory: false,
      buttons: [
        { title: 'phone:bank:5', icon: 'transfer', section: 'TransferSection', subsection: 'GeneralSubsection' },
        { title: 'phone:bank:6', icon: 'blank', section: 'ServicesSection', subsection: 'GeneralSubsection' }
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/bankPage', ['bankAccount', 'balance', 'historyItems']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateY(-0.5rem) rotate(-90deg);
  }
  50% {
    transform: translateY(0) rotate(-90deg);
  }
  75% {
    transform: translateY(-0.25rem) rotate(-90deg);
  }
}
.general-section::v-deep .vb > .vb-dragger {
  margin: 0 0.25rem 0 0;
}
.general-section::v-deep .vb.vb-scrolling-phantom > .vb-dragger > .vb-dragger-styler {
  background-color: #fff;
}

.general-section::v-deep .vb > .vb-dragger:hover > .vb-dragger-styler {
  background-color: #fff;
}

.general-section::v-deep .vb.vb-dragging > .vb-dragger > .vb-dragger-styler {
  background-color: #fff;
}

.general-section::v-deep .vb.vb-dragging-phantom > .vb-dragger > .vb-dragger-styler {
  background-color: #fff;
}
.general-section {
  height: 100%;
  width: 100%;
  padding: 2.45rem 0 0 0;
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  &__title {
    background: linear-gradient(180deg, #FFFFFF 0%, rgba(255, 255, 255, 0) 121.87%);
    background-clip: text;
    -webkit-text-fill-color: transparent;
    font-size: 0.7rem;
    text-transform: uppercase;
    letter-spacing: 0.3rem;
    text-align: center;
    font-weight: 900;
    transition: all 0.5s;
    &.hidden {
      transform: translate(0, -2rem);
      opacity: 0;
    }
  }
  &__card {
    width: 13.2rem;
    height: 8rem;
    position: absolute;
    transform: translate(-50%, 0);
    top: 4rem;
    left: 50%;
    z-index: 1;
    transition: all 0.75s;
    &.hidden {
      transform: translate(-50%, -20rem);
      opacity: 0;
    }
    .main {
      position: absolute;
      background: center / contain no-repeat url("/img/hud/smartphone/bankTab/cardBg.png");
      width: 100%;
      height: 100%;
      z-index: 2;
      border-radius: 0.8rem;
      padding: 1rem;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      color: #fff;
      &__number {
        font-size: 0.7rem;
        letter-spacing: 0.25rem;
        text-shadow: 0px 0.1rem 0.15rem rgba(0, 0, 0, 0.25);
      }
      &__balance {
        .title {
          color: rgba(255, 255, 255, 0.5);
          font-size: 0.7rem;
        }
        .value {
          font-size: 1.3rem;
        }
      }
    }
    .shadow {
      position: absolute;
      transform: translate(-50%, 0);
      top: 3.2rem;
      left: 50%;
      z-index: 1;
      width: 100%;
      height: 100%;
    }
  }
  &__actions {
    height: 14rem;
    width: 14.2rem;
    border-radius: 1.1rem;
    background: #fff;
    margin: 3.75rem 0 0 0;
    display: flex;
    align-items: flex-end;
    justify-content: space-between;
    position: relative;
    padding: 1rem;
    transition: all 0.5s;
    &.hidden {
      transform: translate(0, -16rem);
      opacity: 0;
    }
    &::after {
      content: '';
      position: absolute;
      width: 12.2rem;
      height: 1px;
      background: #E9EBEB;
      transform: translate(-50%, 0);
      top: 5.75rem;
      left: 50%;
    }
    .btn {
      background: transparent;
      border: 1px solid #E3E3E3;
      border-radius: 0.6rem;
      width: 5.75rem;
      height: 6.5rem;
      padding: 0.75rem;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      &:hover {
        background: center / contain no-repeat url("/img/hud/smartphone/bankTab/texture1.png");
        .btn__icon {
          background: #FFFFFF;
        }
        .btn__text {
          color: #FFFFFF;
        }
      }
      &__icon {
        width: 2.5rem;
        height: 2.5rem;
        border-radius: 50%;
        background: #E7F9EF;
        display: flex;
        align-items: center;
        justify-content: center;
        .img {
          width: 1.1rem;
          height: 1.1rem;
        }
      }
      &__text {
        font-size: 0.7rem;
        color: #351E34;
        display: inline-block;
        text-align: left;
        width: 2.5rem;
      }
    }
  }
  &__history {
    color: #273B40;
    margin: 1.25rem 0 0 0;
    transition: all 0.5s;
    position: relative;
    width: 100%;
    display: flex;
    flex-direction: column;
    &.full {
      margin: 0;
      transform: translate(0, -13rem);
      .title {
        font-size: 1rem;
      }
      .list {
        height: 24.3rem !important;
        overflow: hidden scroll !important;
      }
    }
      .title {
        font-size: 0.7rem;
        font-weight: 500;
        transition: all 0.5s;
        padding: 0 0.75rem 0.85rem 0.75rem;
      }
      .list {
        // display: flex !important;
        // flex-direction: column;
        padding: 0 0 0 0.75rem;
        height: 5.5rem !important;
        overflow: hidden !important;
        transition: all 0.5s;
      }
    }
  &__btn {
    width: 14.4rem;
    border-radius: 5rem;
    background: linear-gradient(0deg, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0.7));
    height: 2rem;
    font-size: 0.65rem;
    color: #273B40;
    font-weight: 500;
    position: absolute;
    bottom: 1.5rem;
    left: 50%;
    transform: translate(-50%, 0);
    &:hover {
      .inner .inner__icon {
        animation: shake 0.4s;
      }
    }
    display: flex;
    align-items: center;
    justify-content: center;
    .inner {
      display: flex;
      align-items: center;
      &__icon {
        mask: url('/img/hud/smartphone/arrowLeft.svg') no-repeat center / contain;
        width: 0.35rem;
        height: 0.6rem;
        background: #273B40;
        transform: rotate(-90deg);
      }
      &__text {
        margin: 0 0 0 0.35rem;
      }
    }
  }
}
</style>

<style lang="scss">
.bank-tab__general-section__history-list {
  .simplebar-track {
    width: 0.4rem;
  }
  .simplebar-scrollbar::before {
    background: #fff;
  }
}
</style>
