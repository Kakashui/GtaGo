<template>
  <div class="business-subsection">
    <div class="business-subsection__header">
      <div class="btn" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: 'GeneralSubsection' })">
        <div class="btn__icon"></div>
      </div>
      <div class="title">{{loc('phone:bank:7')}}</div>
    </div>
    <div class="business-subsection__main">
      <div class="card">
        <img
          src="img/hud/smartphone/bankTab/card.svg"
          alt="card"
          class="card__img"
        >
        <div class="card__info">
          <p class="desc">{{loc('phone:bank:8')}}</p>
          <p class="value">${{ balance.toLocaleString() }}</p>
        </div>
      </div>
      <div class="balance">
        <span class="balance__title">{{loc('phone:bank:9')}}</span>
        <div class="balance__state">
          <span class="current">{{ businessBalance.toLocaleString('en') }}$</span>
          <span class="maximum">/ {{ maxBalance.toLocaleString('en') }}$</span>
        </div>
      </div>
      <div class="input">
        <span class="input__title">{{loc('phone:bank:10')}}</span>
        <label class="input__label">
          <span :class="[{ active: localSumModel.length }, 'icon']">$</span>
          <input
            type="number"
            placeholder="0"
            class="field"
            v-model="localSumModel"
          >
        </label>
        <p class="input__desc">{{loc(`phone:bank:11@${rightSum.toLocaleString('en')}`)}}</p>
      </div>
      <button class="button" @click="toUp">{{loc('phone:bank:12')}}</button>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import primeAccount from '../../../../../../configs/newDonateShop/primeAccount'

export default {
  name: 'BusinessSubsection',

  props: {
    sumModel: String
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/bankPage', ['balance', 'businessBalance', 'maxBusinessBalance']),
    ...mapState('newDonateShop/prime',['days']),
    ...mapGetters('localization', ['loc']),

    localSumModel: {
      get: function() {
        return this.sumModel
      },
      set: function(value) {
        this.$emit('input', value)
      }
    },

    rightSum: function() {
      return this.maxBalance > this.businessBalance ? this.maxBalance - this.businessBalance : 0
    },
    maxBalance: function() {
      return this.maxBusinessBalance * (this.days > 0 ? primeAccount.paymentForBusinessInDays : 14)
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    toUp: function() {
      if (this.localSumModel <= 0)
        return;
      //window.setData('smartphone/setCurrentTab', { key: this.currentTab.key, section: this.currentTab.section, subsection: 'NotifySubsection' })
      window.mp.triggerServer('phone::bank::payBusinessTax', this.localSumModel);
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
.business-subsection {
  padding: 3.35rem 1rem 0 1rem;
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
  &__main {
    margin: 1.6rem 0 0 0;
    .card {
      display: flex;
      align-items: center;
      width: 100%;
      padding: 0.65rem 1rem;
      background: #FFFFFF;
      border-radius: 0.6rem;
      &__img {
        width: 1.1rem;
        height: 1.1rem;
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
          margin: 0.15rem 0 0 0;
        }
      }
    }
    .balance {
      margin: 1.5rem 0 0 0;
      padding: 0 0 0.8rem 0;
      border-bottom: 1px solid rgba(0, 0, 0, 0.1);
      color: rgba(39, 59, 64, 0.5);
      &__title {
        font-size: 0.65rem;
      }
      &__state {
        margin: 0.15rem 0 0 0;
        font-size: 1.1rem;
        font-weight: 500;
        .current {
          color: #273B40;
        }
      }
    }
    .input {
      color: rgba(39, 59, 64, 0.5);
      margin: 0.75rem 0 0 0;
      &__title {
        color: #5B818E;
        font-size: 0.7rem;
      }
      &__label {
        display: flex;
        align-items: center;
        border-bottom: 1px solid rgba(0, 0, 0, 0.1);
        border-radius: 0.6rem;
        padding: 0.3rem 0 0.5rem 0;
        .icon {
          font-size: 2rem;
          font-weight: 500;
          color: rgba(91, 129, 142, 0.5);
          &.active {
            color: #5B818E;
          }
        }
        .field {
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
      &__desc {
        font-size: 0.65rem;
        line-height: 0.9rem;
        margin: 0.5rem 0 0 0;
      }
    }
    .button {
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
}
</style>
