<template>
  <div class="notify-section">
    <img
      src="img/hud/smartphone/bankTab/confirm.png"
      alt="confirm"
      class="notify-section__img"
    >
    <span class="notify-section__title">{{loc('phone:bank:transf:noti:1')}}</span>
    <span class="notify-section__sum">$ {{ formattedSum }}</span>
    <div class="notify-section__footer">
      <div class="destintion">
        <p class="destintion__title">{{loc('phone:bank:transf:noti:2')}}</p>
        <p class="destintion__value">{{ transferModel.cardNumber }}</p>
      </div>
      <button class="btn" @click="setCurrentTab({ key: currentTab.key, section: 'GeneralSection' })">
        <img
          src="img/hud/smartphone/bankTab/arrow.svg"
          alt="arrow"
          class="btn__icon"
        >
      </button>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'NotifySection',

  props: {
    transferModel: { type: Object }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc']),

    formattedSum: function() {
      return Number(this.transferModel.sum).toLocaleString()
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'light' })
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
@keyframes scale {
  0% {
    transform: translate(0, 0.2rem) scale(0);
  }
  60% {
    transform: translate(0, -0.2rem) scale(1.1);
  }
  100% {
    transform: translate(0, 0) scale(1);
  }
}

.notify-section {
  height: 100%;
  background: center / contain no-repeat url("/img/hud/smartphone/bankTab/preloaderBg.png");
  display: flex;
  flex-direction: column;
  padding: 3.75rem 1.25rem;
  color: #fff;
  &__img {
    width: 12.2rem;
    height: 10.5rem;
    align-self: center;
    animation: scale 0.7s;
  }
  &__title {
    font-size: 2rem;
    font-weight: 500;
    margin: 1.25rem 0 0 0;
  }
  &__sum {
    margin: 0.5rem 0 0 0;
  }
  &__footer {
    margin: 1rem 0 0 0;
    border-top: 1px solid rgba(255, 255, 255, 0.2);
    padding: 0.75rem 0 0 0;
    display: flex;
    justify-content: space-between;
    align-items: center;
    .destintion {
      &__title {
        font-size: 0.7rem;
      }
      &__value {
        margin: 0.25rem 0 0 0;
      }
    }
    .btn {
      width: 5rem;
      height: 3rem;
      border-radius: 50rem;
      background: #fff;
      &:hover {
        .btn__icon {
          animation: shake 0.4s;
        }
      }
    }
  }
}
</style>
