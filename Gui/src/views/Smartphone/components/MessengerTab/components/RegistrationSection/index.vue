<template>
  <div class="registration-section">
    <div class="registration-section__img"></div>
    <div class="registration-section__greeting">
      <span class="subtitle">{{loc('phone:reg:7')}}</span>
      <span class="title">Messenger</span>
      <p class="desc">{{loc('phone:reg:8')}}</p>
    </div>
    <button class="registration-section__action" @click="setIsActionPopup(true)">{{loc('phone:reg:9')}}</button>
    <transition name="smartphone-fade">
      <div class="registration-section__layer" v-if="isActionPopup"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <action-popup v-if="isActionPopup" @close="isActionPopup = false" />
    </transition>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import ActionPopup from './ActionPopup'

export default {
  name: 'RegistrationSection',

  components: {
    ActionPopup
  },

  data: function() {
    return {
      isActionPopup: false
    }
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    setIsActionPopup: function(value) {
      this.isActionPopup = value
    }
  }
}
</script>

<style lang="scss" scoped>
.registration-section {
  width: 100%;
  height: 100%;
  background: #4E6ED9;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 2.25rem 0 0 0;
  &__img {
    margin: 0 0 1.3rem 0;
    &::after {
      content: "";
      width: 13rem;
      height: 13rem;
      background: center / cover no-repeat url("/img/hud/smartphone/messengerTab/registrationSection/bg.png");
      display: block;
    }
  }
  &__greeting {
    display: flex;
    flex-direction: column;
    align-items: center;
    color: #fff;
    margin: 0 0 2rem 0;
    .subtitle {
      font-size: 1rem;
      line-height: 1.15rem;
    }
    .title {
      font-size: 2rem;
      line-height: 2.35rem;
      font-weight: bold;
      margin: 0 0 0.5rem 0;
    }
    .desc {
      font-size: 0.7rem;
      line-height: 0.9rem;
      width: 12.2rem;
      text-align: center;
    }
  }
  &__action {
    width: 13.2rem;
    height: 2rem;
    font-size: 0.7rem;
    color: #4E6ED9;
    border-radius: 5rem;
    box-shadow: 0px 0.5rem 1.5rem rgba(30, 55, 139, 0.3);
    background: #FFFFFF;
    transition: all 0.15s;
    &:hover {
      box-shadow: none;
    }
  }
  &__layer {
    position: absolute;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7);
    top: 0;
    left: 0;
    z-index: 6;
  }
}
</style>
