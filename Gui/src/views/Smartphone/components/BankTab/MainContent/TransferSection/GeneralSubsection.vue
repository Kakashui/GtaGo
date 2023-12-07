<template>
  <div class="general-subsection">
    <div class="general-subsection__header">
      <div class="btn" @click="setCurrentTab({ key: currentTab.key, section: 'GeneralSection' })">
        <div class="btn__icon"></div>
      </div>
      <div class="title">{{loc('phone:bank:19')}}</div>
    </div>
    <div class="general-subsection__cards">
      <div class="main">
        <img
          src="img/hud/smartphone/bankTab/card.svg"
          alt="card"
          class="main__icon"
        >
        <div class="main__info">
          <p class="desc">{{loc('phone:bank:8')}}</p>
          <p class="value">${{ balance.toLocaleString() }}</p>
        </div>
      </div>
      <div class="separator">
        <div class="separator__left"></div>
        <div class="separator__icon"></div>
        <div class="separator__right"></div>
      </div>
      <div class="recipient">
        <img
          src="img/hud/smartphone/bankTab/card.svg"
          alt="card"
          class="recipient__icon"
        >
        <input
          type="number"
          class="recipient__input"
          @placeholder="loc('phone:bank:plh:1')"
          v-model="localTransferModel.cardNumber"
        >
      </div>
    </div>
    <div class="general-subsection__input">
      <span class="title">{{loc('phone:bank:10')}}</span>
      <label class="label">
        <span :class="[{ active: localTransferModel.sum }, 'label__icon']">$</span>
        <input
          type="number"
          placeholder="0"
          class="label__field"
          v-model="localTransferModel.sum"
        >
      </label>
      <p class="desc">{{loc('phone:bank:20')}}</p>
    </div>
    <button class="general-subsection__btn" @click="toTransfer">{{loc('phone:bank:21')}}</button>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'GeneralSection',

  props: {
    transferModel: { type: Object }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/bankPage', ['balance']),
    ...mapGetters('localization', ['loc']),

    localTransferModel: {
      get: function() {
        return this.transferModel
      },
      set: function(value) {
        this.$emit('input', value)
      }
    },
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    toTransfer: function() {
      if (this.localTransferModel.cardNumber && this.localTransferModel.sum)
        window.mp.triggerServer('phone::bank::acceptTransfer', this.localTransferModel.cardNumber, this.localTransferModel.sum);      
      //this.setCurrentTab({ key: this.currentTab.key, section: this.currentTab.section, subsection: 'NotifySubsection' })
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateX(0.5rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.25rem);
  }
}
.general-subsection {
  padding: 3.35rem 0.75rem 0 0.75rem;
  &__header {
    position: relative;
    text-align: center;
    .btn {
      position: absolute;
      transform: translate(0, -50%);
      top: 50%;
      left: 0.25rem;
      width: 2rem;
      height: 2rem;
      background: linear-gradient(0deg, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0.7));
      border-radius: 0.4rem;
      display: flex;
      align-items: center;
      padding: 0 0 0 0.75rem;
      &:hover {
        .btn__icon {
          animation: shake 0.4s;
        }
      }
      &__icon {
        mask: url('/img/hud/smartphone/arrowLeft.svg') no-repeat center / contain;
        width: 0.4rem;
        height: 0.6rem;
        background: #273B40;
      }
    }
    .title {
      font-size: 0.7rem;
      color: #273B40;
      font-weight: 500;
    }
  }
  &__cards {
    height: 6.05rem;
    background: #fff;
    border-radius: 0.6rem;
    margin: 1.6rem 0 0 0;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    .main {
      padding: 0.7rem 0 0 1rem;
      display: flex;
      align-items: center;
      &__icon {
        width: 1.6rem;
        height: 1.6rem;
      }
      &__info {
        margin: 0 0 0 0.75rem;
        .desc {
          font-size: 0.65rem;
          color: rgba(39, 59, 64, 0.5);
        }
        .value {
          font-size: 0.7rem;
          font-weight: 500;
          color: #273B40;
          margin: 0.15rem 0 0 0;
        }
      }
    }
    .separator {
      display: flex;
      align-items: center;
      justify-content: space-between;
      &__left {
        background: rgba(0, 0, 0, 0.1);
        height: 1px;
        width: 1.15rem;
      }
      &__icon {
        height: 0.6rem;
        width: 0.35rem;
        mask: url('/img/hud/smartphone/arrowLeft.svg') no-repeat center / contain;
        background: #273B40;
        transform: rotate(-90deg);
      }
      &__right {
        width: 11.2rem;
        height: 1px;
        background: rgba(0, 0, 0, 0.1);
      }
    }
    .recipient {
      display: flex;
      align-items: center;
      padding: 0 1rem 0.7rem 1rem;
      &__icon {
        filter: grayscale(1);
        opacity: 0.65;
      }
      &__input {
        width: 100%;
        font-size: 0.65rem;
        background: transparent;
        margin: 0 0 0 0.75rem;
        color: #273B40;
        padding: 0.4rem 0;
        &::placeholder {
          color: rgba(39, 59, 64, 0.5);
        }
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          -webkit-appearance: none;
          margin: 0;
        }
      }
    }
  }
  &__input {
    color: rgba(39, 59, 64, 0.5);
    margin: 1.5rem 0 0 0;
    .title {
      color: #5B818E;
      font-size: 0.7rem;
    }
    .label {
      display: flex;
      align-items: center;
      border-bottom: 1px solid rgba(0, 0, 0, 0.1);
      border-radius: 0.6rem;
      padding: 0.3rem 0 0.5rem 0;
      &__icon {
        font-size: 2rem;
        font-weight: 500;
        color: rgba(91, 129, 142, 0.5);
        &.active {
          color: #5B818E;
        }
      }
      &__field {
        font-size: 2rem;
        background: transparent;
        margin: 0 0 0 0.5rem;
        font-weight: 500;
        color: #5B818E;
        width: 100%;
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          -webkit-appearance: none;
          margin: 0;
        }
        &::placeholder {
          color: rgba(91, 129, 142, 0.5);
        }
      }
    }
    .desc {
      font-size: 0.65rem;
      line-height: 0.9rem;
      margin: 0.5rem 0 0 0;
    }
  }
  &__btn {
    height: 3rem;
    width: 100%;
    border-radius: 50rem;
    background: center / cover no-repeat url("/img/hud/smartphone/bankTab/texture2.png");
    color: #fff;
    font-size: 0.8rem;
    letter-spacing: 0.06em;
    margin: 1.25rem 0 0 0;
    transition: all 0.2s;
    &:hover {
      box-shadow: 0px 1rem 2rem rgba(83, 226, 145, 0.5);
      transform: translate(0, -0.2rem);
    }
  }
}
</style>
