<template>
  <div class="item">
    <button class="item__img" @click="openApp">
      <img
        :src="`img/hud/smartphone/desktopTab/apps/${item.icon}.svg`"
        :alt="item.key"
        class="icon"
      >
    </button>
    <span class="item__title">{{ loc(item.title) }}</span>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'

export default {
  name: 'AppsItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('smartphone', ['backgroundTabs']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    openApp: function() {
      if (!this.item.isAvailable) {
        return;
      }
      if (this.item.type == 'triggerEvent') {
        window.mp.trigger(this.item.eventName);
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
  // width: 2.8rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  &__img {
    width: 2.5rem;
    height: 2.5rem;
    background: transparent;
    border-radius: .55rem;
    .icon {
      width: 100%;
      height: 100%;
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
    &:hover {
      animation: shake .4s;
    }
  }
  &__title {
    font-size: .6rem;
    color: #fff;
    letter-spacing: .025rem;
    margin: .25rem 0 0 0;
    width: 100%;
    text-align: center;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 1;
    -webkit-box-orient: vertical;
    text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.7);
  }
}
</style>
