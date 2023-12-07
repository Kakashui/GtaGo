<template>
  <input class="slider" type="range" min="1" v-model.number="fuelLitersQuantity" :max="maxLiters - currentLiters">
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'CurrentPlayerMenuRange',

  data: function () {
    return {
      fuelLitersQuantity: 1
    }
  },

  computed: {
    ...mapState('gasStation', ['currentLiters', 'maxLiters'])
  }
}
</script>

<style lang="scss" scoped>
.slider {
  width: 100%;
  margin: 1.9rem 0 1rem 0;
}
</style>
