<template>
  <div class="penalty-subsection">
    <div class="penalty-subsection__header">
      <div class="btn" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: 'GeneralSubsection' })">
        <div class="btn__icon"></div>
      </div>
      <div class="title">{{loc('phone:bank:15')}}</div>
    </div>
    <div class="penalty-subsection__main">
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
      <div class="info">
        <span class="info__title">{{loc('phone:bank:16')}}</span>
        <span class="info__value">{{ penaltySum.toLocaleString('en') }}$</span>
        <p class="info__desc">{{loc('phone:bank:17')}}</p>
      </div>
      <button class="button" @click="pay">{{loc('phone:bank:18')}}</button>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'PenaltySubsection',

  mounted() {
    this.$emit('input', this.penaltySum)
  },
  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/bankPage', ['balance', 'penaltySum']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    pay: function() {
      if (this.penaltySum <= 0)
        return;
      //window.setData('smartphone/setCurrentTab', { key: this.currentTab.key, section: this.currentTab.section, subsection: 'NotifySubsection' })
      window.mp.triggerServer('phone::bank::payPenalty');
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
.penalty-subsection {
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
      justify-content: center;
      align-items: center;
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
    .info {
      display: flex;
      flex-direction: column;
      margin: 1.5rem 0 0 0;
      &__title {
        font-size: 0.65rem;
        color: rgba(39, 59, 64, 0.5);
      }
      &__value {
        margin: 0.25rem 0 0 0;
        font-size: 2rem;
        color: #273B40;
        font-weight: bold;
        padding: 0 0 0.5rem 0;
        border-bottom: 1px solid rgba(0, 0, 0, 0.1);
      }
      &__desc {
        margin: 0.5rem 0 0 0;
        font-size: 0.65rem;
        color: rgba(39, 59, 64, 0.5);
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
