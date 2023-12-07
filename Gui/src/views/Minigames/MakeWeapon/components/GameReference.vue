<template>
  <div class="game-reference">    
    <ReferenceGun v-for="gun of guns" v-bind:gun="gun" v-bind:key="gun.id" />
  </div>
</template>
<script>
import ReferenceGun from './ReferenceGun';
import { mapState } from 'vuex';
export default {
  name: 'GameReference',
  computed: {
    ...mapState('gameMakeWeapon/gunsData', ['guns']),
  },
  data() {
    return {
      settings: {
        suppressScrollY: false,
        suppressScrollX: false,
        wheelPropagation: false,
        wheelSpeed: 0.3,
      },
    };
  },
  components: {
    ReferenceGun,
  },
};
</script>

<style scoped lang="scss">
.game-reference {
  width: 100%;
  height: 100%;
  overflow: hidden scroll;
  position: relative;
  margin-bottom: 2rem;
  &::-webkit-scrollbar {
    width: 2px;
  }
  &::-webkit-scrollbar-track {
    border: solid 0 transparent;
    margin: .1rem;
  }
  &::-webkit-scrollbar-thumb {
    border: solid 0 transparent;      
    background-color: rgba(#fff,.1);
  }
}
</style>
<style lang="scss">
.guns-list {
  .ps__rail-y {
    width: 0.3rem !important;
    background: transparent !important;
    .ps__thumb-y {
      width: 0.3rem !important;
    }
    &:hover,
    &:focus {
      background: transparent !important;
      width: 0.3rem !important;
      cursor: pointer !important;
      .ps__thumb-y {
        width: 0.3rem !important;
      }
    }
  }
}
</style>
