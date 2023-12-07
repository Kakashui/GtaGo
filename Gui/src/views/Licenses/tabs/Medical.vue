<template>
  <div class="medical-tab body-tab">
    <div class="body-tab__title">
      <div class="desc">{{loc('Licenses_8')}}</div>
      <div class="value">{{loc('Licenses_9')}}</div>
    </div>
    <div class="medical-tab__license">
      <div 
        v-if="dateOfMedical"
        class="block-open"
      >{{loc('Licenses_10')}} {{dateOfMedical}}</div>
      <div 
        v-else
        class="block-locked"
      >{{loc('Licenses_11')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'Medical',

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('licenses', ['licenses']),
    dateOfMedical: function() {
      let date = false
      this.licenses.forEach(element => {
        if (element.type === 'medical') {
          date = element.dateEnd
        }
      })
      return date
    }
  }
}
</script>

<style lang="scss" scoped>
.medical-tab{
  &__license{
    width: 100%;
    height: 11.1rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/licenses/medical.png');
  }
}
</style>
