<template>
  <button 
    class="back-button"
    @click="setCurrentTab(location)"
    :style="styles"
  >
    <img
      src="img/hud/smartphone/messengerTab/chatSection/chevronLeftDark.svg"
      alt="chevron"
      class="back-button__icon"
    >
    <span 
      class="back-button__text"
      v-if="isText" 
    >{{loc('phone:back')}}</span>
  </button>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'BackButton',

  props: {
    styles: Object,
    location: Object,
    isText: Boolean
  },
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
.back-button {
  background: transparent;
  display: flex;
  align-items: center;
  &:hover {
    .back-button__icon {
      animation: shake 0.4s;
    }
  }
  &__icon {
    width: 0.6rem;
    height: 1rem;
  }
  &__text{
    font-size: .7rem;
    line-height: 1rem;
    color: #4E6ED9;
    margin-left: .4rem;
  }
}
</style>
