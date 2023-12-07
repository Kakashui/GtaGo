<template>
  <div class="chat-section">
    <transition name="fade">
      <component :is="currentTab.subsection"></component>
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'
import PrivateSubsection from './PrivateSubsection'
import GroupSubsection from './GroupSubsection'
import GroupOptionsSubsection from './GroupOptions/GroupOptionsSubsection'
import GroupControlSubsection from './GroupOptions/GroupControlSubsection'
import GroupBlacklistSubsection from './GroupOptions/GroupBlacklistSubsection'
import CanalSubsection from './CanalSubsection'
import CanalOptionsSubsection from './CanalOptions/CanalOptionsSubsection'
import CanalControlSubsection from './CanalOptions/CanalControlSubsection'
import CanalSubscribersSubsection from './CanalOptions/CanalSubscribersSubsection'
import CanalBlacklistSubsection from './CanalOptions/CanalBlacklistSubsection'


export default {
  name: 'ChatSection',

  components: {
    PrivateSubsection,
    GroupSubsection,
    GroupOptionsSubsection,
    GroupBlacklistSubsection,
    CanalSubsection,
    CanalOptionsSubsection,
    CanalControlSubsection,
    CanalSubscribersSubsection,
    CanalBlacklistSubsection,
    GroupControlSubsection
  },

  computed: {
    ...mapState('smartphone', ['currentTab'])
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'none' })
  }
}
</script>

<style lang="scss" scoped>
.chat-section {
  height: 100%;
  width: 100%;
  background: #F3F6FA;
}
.fade-enter-active, .fade-leave-active {
  transition: opacity .2s;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
}
</style>
