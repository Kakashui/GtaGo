<template>
  <div class="contacts-tab" :style="{ background: currentTab.subsection === 'UserSubsection' ? '#F3F2F7' : '#fff'}">
    <div class="contacts-tab__main">
      <component :is="currentTab.section" :setContactPopupData="setContactPopupData" />
    </div>
    <div class="contacts-tab__footer">
      <footer-item
        v-for="item in menuItems"
        :key="item.id"
        :item="item"
      />
    </div>
    <contact-popup
      v-if="contactPopupData.show"
      :setContactPopupData="setContactPopupData"
      :data="contactPopupData"
    />
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

import FooterItem from './FooterItem'
import ContactPopup from './ContactPopup'
import GeneralSection from './GeneralSection'
import RecentSection from './RecentSection'
import KeypadSection from './KeypadSection'

export default {
  name: 'ContactsTab',

  components: {
    FooterItem,
    ContactPopup,
    GeneralSection,
    RecentSection,
    KeypadSection
  },

  data: function() {
    return {
      menuItems: [
        { id: 0, title: 'phone:footer:1', key: 'recent', section: 'RecentSection', subsection: 'GeneralSubsection' },
        { id: 1, title: 'phone:footer:2', key: 'general', section: 'GeneralSection', subsection: 'GeneralSubsection' },
        { id: 2, title: 'phone:footer:3', key: 'keypad', section: 'KeypadSection', subsection: null }
      ],
      contactPopupData: {
        show: false,
        type: null,
        id: 0,
        name: '',
        number: ''
      },
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab'])
  },
  
  methods: {
    ...mapMutations('smartphone', ['setColorTheme']),

    setContactPopupData: function(show, type, number = '',  name = '', id = 0) {
      this.contactPopupData.show = show
      this.contactPopupData.type = type
      this.contactPopupData.id = id
      this.contactPopupData.number = number
      this.contactPopupData.name = name
    }
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
.contacts-tab {
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  position: relative;
  &::v-deep .vb > .vb-dragger {
    margin: 0 0.15rem 0 0;
  }
  &__main {
    height: 26rem;
    position: relative;
  }
  &__footer {
    background: rgba(255, 255, 255, 0.95);
    height: 4.05rem;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0.35rem 1.6rem 0 1.6rem;
    display: flex;
    justify-content: space-between;
    z-index: 2;
  }
}
</style>
