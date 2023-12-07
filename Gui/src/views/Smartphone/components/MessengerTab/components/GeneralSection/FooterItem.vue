<template>
  <button :class="[{ active: item.subsection === currentSubsection }, 'footer-item']" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: item.subsection })">
    <div class="footer-item__icon">
      <div class="img" :style="{ '-webkit-mask': 'url(' + `img/hud/smartphone/messengerTab/${item.icon}.svg` + ') no-repeat center / contain' }"></div>
      <span v-if="item.subsection === 'GeneralSubsection' && unreadMessages > 0" class="count">{{ unreadMessages }}</span>
    </div>
    <span class="footer-item__text">{{ loc(item.text) }}</span>
  </button>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'FooterItem',

  props: { item: Object, currentSubsection: String },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),

    unreadMessages: function() {
      return this.messengerData.Chats.reduce((a, b) => a + b.NonReadMessagesCount, 0) || 0
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab'])
  }
}
</script>

<style lang="scss" scoped>
.footer-item {
  width: 4.5rem;
  height: 2.5rem;
  padding: 0.35rem 0 0 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  background: transparent;
  &.active {
    .footer-item__icon .img {
      background: #007AFF;
    }
    .footer-item__text {
      color: #007AFF;
    }
  }
  &__icon {
    width: 1.3rem;
    height: 1.3rem;
    position: relative;
    background: #fff;
    .img {
      width: 100%;
      height: 100%;
      background: #8A8A8E;
    }
    .count {
      position: absolute;
      background: #EE443A;
      border-radius: 50rem;
      font-size: 0.65rem;
      font-weight: 500;
      color: #fff;
      bottom: 30%;
      left: 60%;
      padding: 0 0.25rem;
      height: 1rem;
      line-height: 1rem;
      min-width: 1rem;
    }
  }
  &__text {
    font-size: 0.55rem;
    line-height: 0.65rem;
    margin: 0.2rem 0 0 0;
    color: #8A8A8E;
  }
}
</style>
