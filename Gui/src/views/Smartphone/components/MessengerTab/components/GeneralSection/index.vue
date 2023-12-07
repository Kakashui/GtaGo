<template>
  <div class="general-section">
    <div class="general-section__main">
      <transition name="fade">
        <component :is="currentSubsection" />
      </transition>
    </div>
    <div class="general-section__footer">
      <footer-item
        v-for="(item, index) in footerItems"
        :key="index"
        :item="item"
        :currentSubsection="currentSubsection"
      />
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState } from 'vuex'
import GeneralSubsection from './GeneralSubsection'
import ContactsSubsection from './ContactsSubsection'
import SettingsSubsection from './SettingsSubsection'
import FooterItem from './FooterItem'

export default {
  name: 'GeneralSection',

  components: {
    GeneralSubsection,
    ContactsSubsection,
    SettingsSubsection,
    FooterItem
  },

  data: function() {
    return {
      footerItems: [
        { text: 'phone:footer:item:1', icon: 'user1', subsection: 'ContactsSubsection' },
        { text: 'phone:footer:item:2', icon: 'message', subsection: 'GeneralSubsection' },
        { text: 'phone:footer:item:3', icon: 'gear', subsection: 'SettingsSubsection' },
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),

    currentSubsection: function() {
      return this.currentTab.subsection ? this.currentTab.subsection : 'GeneralSubsection'
    }
  },
  
  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
.general-section {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  position: relative;
  &__main {
    height: 28.95rem;
    position: relative;
  }
  &__footer {
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    height: 4.05rem;
    display: flex;
    padding: 0 0.75rem;
    justify-content: space-between;
  }
}
.fade-enter-active, .fade-leave-active {
  transition: opacity .2s;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
  position: absolute;
}
</style>
