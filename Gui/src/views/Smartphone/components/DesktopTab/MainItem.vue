<template>
  <button class="item" @click="openApp">
    <img
      :src="`img/hud/smartphone/desktopTab/main/${item.icon}.svg`"
      :alt="item.key"
      class="item__img"
    >
  </button>
</template>

<script>
import { mapMutations, mapState } from 'vuex'

export default {
  name: 'MainItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('smartphone', ['backgroundTabs']),
    ...mapState('smartphone/messagePage', ['messengerData'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    openApp: function() {
      if (!this.item.isAvailable) {
        return;
      }
      
      const key = this.item.key;
      let section = this.item.section;
      let subsection = null;
      let data = null;
      
      if (this.backgroundTabs[key] !== undefined) {
        section = this.backgroundTabs[key].section;
        subsection = this.backgroundTabs[key].subsection;
        data = this.backgroundTabs[key].data;
      }

      if (key === 'MessengerTab') {
        if (this.messengerData.CurrentAccount.Id === -1) {
          section = 'RegistrationSection';
          subsection = null;
          data = null;
        }
      }
      
      this.setCurrentTab({ key, section, subsection, data })
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  0% {
    transform: rotate(5deg) scale(1.05);
  }
  25% {
    transform: rotate(-5deg) scale(1.05);
  }
  50% {
    transform: rotate(5deg);
  }
  75% {
    transform: rotate(-5deg);
  }
}
.item {
  width: 2.5rem;
  height: 2.5rem;
  position: relative;
  border-radius: .55rem;
  background: transparent;
  &__img {
    width: 100%;
    height: 100%;
  }
  &:hover {
    animation: shake .4s;
  }
  &:active {
    transform: scale(0.95);
    &::after {
      content: '';
      position: absolute;
      width: 100%;
      height: 100%;
      top: 0;
      left: 0;
      border-radius: .55rem;
      background: rgba(0, 0, 0, 0.3);
    }
  }
}
</style>
