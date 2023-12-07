<template>
  <div class="weapon-tab body-tab">
    <div class="body-tab__title">
      <div class="desc">{{loc('Licenses_18')}}</div>
      <div class="value">{{loc('Licenses_19')}}</div>
    </div>
    <div class="weapon-tab__license">
      <div 
        v-if="isPresent"
        class="block-open"
      >{{loc('Licenses_12')}}</div>
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
  name: 'Weapon',

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('licenses', ['licenses']),
    isPresent: function() {
      let boolean = false
      this.licenses.forEach(element => {
        if (element.type === 'weapon') {
          boolean = true
        }
      })
      return boolean
    }
  }
}
</script>

<style lang="scss" scoped>
.weapon-tab{
  height: 100%;
  width: 100%;
  &__license{
    width: 100%;
    height: 11.1rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/licenses/weapon.png');
  }
}
</style>
