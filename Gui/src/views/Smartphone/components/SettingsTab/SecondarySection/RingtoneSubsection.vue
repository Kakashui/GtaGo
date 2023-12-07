<template>
  <div v-bar>
    <div class="ringtone-subsection">
      <sound-item
        v-for="item in items"
        :key="item.id"
        :item="item"
        :currentId="configuration.Ringtone"
        :setCurrentId="setRingtoneSound"
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
    ...mapState('smartphone', ['configuration', 'ringtonesSounds'])
  },

  methods: {
    ...mapMutations('sounds', ['stop']),

    setRingtoneSound: function(id) {
      const ringtoneItem = this.items.find(i => i.id === id);
      window.setData('sounds/play', { name: ringtoneItem.soundName, volume: 0.5 })
      window.setData('smartphone/setConfigRingtoneSound', id)

      window.mp.trigger('phone::settings::setConfigRingtone', id);
    }
  },

  mounted: function() {
    this.items = this.ringtonesSounds;
  }
}
</script>

<style lang="scss" scoped>
.ringtone-subsection {
  height: 28.2rem !important;
}
</style>
