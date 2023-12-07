<template>
  <div class="spawn-select">
    <SoundSwitch />
    <spawn-item
      v-for="(item, index) in spawns"
      :item="item"
      :key="index"
      :index="index"
    />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import SpawnItem from './SpawnItem'
import SoundSwitch from '../Auth/SoundSwitch'
export default {
  name: 'SpawnSelect',

  components: {
    SpawnItem,
    SoundSwitch
  },

  computed: {
    ...mapState('spawnSelect', ['spawns'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes decorate-view{
  from{
    transform: translateX(-15%);
  }
  to{
    transform: translateX(0);
  }
}
.spawn-select{
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 1;
  text-transform: uppercase;
  font-style: normal;
  padding: 4.5rem 7.5rem 5.75rem 4.25rem;
  background-size: cover;
  background-repeat: no-repeat;
  background-position: center;
  background-image: url('/img/characterSelect/bg.png');
  overflow: hidden;
  &::before{
    content: '';
    width: 74vw;
    height: 10.8rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: top;
    background-image: url('/img/spawnSelect/decorate.svg');
    position: absolute;
    top: 0;
    left: 0;
    animation: decorate-view 1s;
  }
}
</style>
