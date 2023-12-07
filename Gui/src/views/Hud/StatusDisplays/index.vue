<template>
  <div class="status-displays">
    <div class="status-displays__map"></div>
    <div class="status-displays__interface" v-if="statusDisplayShow">
      <StatusDisplayItem 
        :value="this.statusDisplays.hungerLevel"
        :icon="'hunger'"
      />
      <StatusDisplayItem 
        :value="this.statusDisplays.thirstLevel"
        :icon="'thirst'"
      />
      <StatusDisplayItem 
        :value="this.statusDisplays.restLevel"
        :icon="'rest'"
      />
      <StatusDisplayItem 
        :value="this.statusDisplays.joyLevel"
        :icon="'joy'"
      />
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'

import StatusDisplayItem from './StatusDisplayItem'

export default {
  name: 'StatusDisplays',

  components: {
    StatusDisplayItem
  },

  computed: {
    ...mapState('hud', ['statusDisplays', 'statusDisplayShow'])
  },
}
</script>

<style lang="scss" scoped>
  .status-displays{
    display: flex;
    position: absolute;
    left: 2.5rem;
    bottom: 1.75rem;
    align-items: flex-end;
    text-transform: uppercase;
    &__map{
      width: 13.75rem;
      height: 9.75rem;
      margin-right: 1rem;
    }
    &__interface{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
    }
  }
</style>
