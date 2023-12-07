<template>
  <transition name="fade">
    <div class="notify-section">
      <span class="notify-section__title">{{loc('phone:taxi:notify:1')}}</span>
      <p class="notify-section__desc">{{loc('phone:taxi:notify:2')}}</p>
      <button class="notify-section__btn">
        <span class="text" @click="setCurrentTab({ key: 'DesktopTab' })">{{loc('phone:taxi:notify:2')}}</span>
        <div class="icon"></div>
      </button>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'NotifySection',
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapMutations('smartphone', ['setCurrentTab'])
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
@keyframes fade {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.notify-section {
  height: 100%;
  background: center / cover no-repeat url("/img/hud/smartphone/taxiTab/notifyBg.png");
  padding: 5rem 1rem 0 1rem;
  &__title {
    font-size: 2.5rem;
    letter-spacing: 0.04em;
    text-transform: uppercase;
    font-weight: 700;
    color: #282828;
    text-transform: uppercase;
  }
  &__desc {
    color: rgba(0, 0, 0, 0.4);
    font-size: 0.65rem;
    margin: 0.25rem 0 0 0;
    width: 10rem;
  }
  &__btn {
    margin: 1.25rem 0 0 0;
    width: 5rem;
    height: 5rem;
    border-radius: 50%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    background: linear-gradient(68.11deg, #0F0F15 0%, #4A4A55 100%);
    padding: 0.8rem 0 0 0;
    &:hover {
      .icon {
        animation: shake 0.4s;
      }
    }
    .text {
      color: #fff;
      font-size: 0.7rem;
    }
    .icon {
      mask: url('/img/hud/smartphone/bankTab/arrow.svg') no-repeat center / contain;
      width: 1.5rem;
      height: 0.6rem;
      background: #fff;
      margin: 0.25rem 0 0 0;
    }
  }
}
.fade-in-enter-active {
  animation: fade 0.4s;
}
</style>
