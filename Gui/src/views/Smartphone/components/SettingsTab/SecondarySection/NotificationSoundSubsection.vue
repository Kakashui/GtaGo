<template>
  <div v-bar>
    <div class="notification-sound-subsection">
      <sound-item
        v-for="item in items"
        :key="item.id"
        :item="item"
        :currentId="configuration.NotificationSound"
        :setCurrentId="setNotificationSound"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'
import SoundItem from './SoundItem'

export default {
  name: 'NotificationSoundSubsection',

  components: { SoundItem },

  data: function() {
    return {
      items: []
    }
  },

  computed: {
    ...mapState('smartphone', ['configuration', 'notificationsSounds'])
  },

  methods: {
    ...mapMutations('sounds', ['stop']),

    setNotificationSound: function(id) {
      const notificationItem = this.items.find(i => i.id === id);
      window.setData('sounds/play', { name: notificationItem.soundName, volume: 0.5 })
      window.setData('smartphone/setConfigNotificationSound', id)

      window.mp.trigger('phone::settings::setConfigNotificationSound', id);
    }
  },

  mounted: function() {
    this.items = this.notificationsSounds;
  }
}
</script>

<style lang="scss" scoped>
.notification-sound-subsection {
  height: 28.2rem !important;
}
</style>
