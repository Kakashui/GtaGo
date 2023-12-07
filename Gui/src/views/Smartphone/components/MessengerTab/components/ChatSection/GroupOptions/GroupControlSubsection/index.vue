<template>
  <div class="canal-control-subsection">
    <transition name="smartphone-fade" appear>
      <component
        :is="currentTab"
        :setTab="setTab"
        :profileId="tab.id"
      />
    </transition>
  </div>
</template>

<script>
import GeneralTab from './GeneralTab'
import ProfileTab from './ProfileTab'

export default {
  name: 'CanalControlSubsection',

  components: { GeneralTab, ProfileTab },

  data: function() {
    return {
      tab: { key: null, id: null}
    }
  },

  computed: {
    currentTab: function() {
      return this.tab.key ? this.tab.key : 'GeneralTab'
    }
  },

  methods: {
    setTab: function({ key, id = null }) {
      this.tab = { key, id }
    }
  }
}
</script>

<style lang="scss" scoped>
.canal-control-subsection {
  width: 100%;
  height: 100%;
}
</style>
