<template>
  <div class="current-gun" >
    <div class="current-gun__name">{{ getGunByIndex(currentGun).name }}</div>
    <div class="current-gun__price">
      $ {{ getGunByIndex(currentGun).price }}
    </div> 
    <BonusForTime v-if="bonusForTime"/>
  </div>
</template>

<script>
import BonusForTime from './BonusForTime'
import { mapState, mapGetters } from 'vuex';
export default {
  name: 'CurrentGun',
  computed: {
    ...mapState('gameMakeWeapon/gameState', ['currentGun', 'bonusForTime']),
    ...mapGetters('gameMakeWeapon/gunsData', ['getGunByIndex']),
  },
  components:{
    BonusForTime
  }
};
</script>

<style scoped lang="scss">
.current-gun {
  margin-top: 3rem;
  text-align: center;
  font-size: 2rem;
  font-weight: normal;
  position: relative;
  &__name, 
  &__price {
    display: inline-block;
    line-height: 2.3rem;
  }
  &__name {
    margin-right: 0.6rem;
    color: #fff;
  }
  &__price {
    margin-left: 0.6rem;
    color: #c1e704;
  }
  
}
</style>
