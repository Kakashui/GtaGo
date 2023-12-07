<template>
  <div class="messenger-tab">
    <transition name="fade">
      <component :is="currentTab.section" />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'
import GeneralSection from './components/GeneralSection'
import CreateSection from './components/CreateSection'
import ChatSection from './components/ChatSection'
import RegistrationSection from './components/RegistrationSection'
import ViewedProfileSection from './components/ViewedProfileSection'

export default {
  name: 'MessengerTab',

  components: {
    GeneralSection,
    CreateSection,
    ChatSection,
    ViewedProfileSection,
    RegistrationSection
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData'])
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme', 'setCurrentTab'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })

    // if (this.messengerData.CurrentAccount.Id === -1) {
    //   this.setCurrentTab({
    //     key: 'MessengerTab',
    //     section: 'RegistrationSection'
    //   });
    // }
  }
}
</script>

<style lang="scss" scoped>
.messenger-tab::v-deep .vb > .vb-dragger {
  margin: 0 0.1rem 0 0;
}
.messenger-tab {
  background: #fff;
  width: 100%;
  height: 100%;
}
.fade-enter-active, .fade-leave-active {
  transition: opacity .2s;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
}
</style>
