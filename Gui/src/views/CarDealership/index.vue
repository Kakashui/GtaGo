<template>
  <div class="car-dealership">
    <component :is="currentPage" />
  </div>
</template>

<script>
import { mapState } from 'vuex'

import StartPage from './StartPage'
import ViewPage from './ViewPage'

export default {
  name: 'CarDealership',

  components: {
    StartPage,
    ViewPage
  },

  computed: {
    ...mapState('carDealership', ['currentPage'])
  }
}
</script>

<style lang="scss" scoped>
.car-dealership{
  display: flex;
  width: 100%;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 0;
}
</style>

<style lang="scss">
.car-dealership{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
}
</style>

