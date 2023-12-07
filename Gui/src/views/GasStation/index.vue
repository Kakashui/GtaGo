<template>
  <div class="gas-station">
    <div class="title"><span>GO</span> fuel</div>
    <div class="btn btn-close" @click="closeGasStation"></div>
    <component :is="currentPage.page" />
  </div>
</template>

<script>
import { mapState } from 'vuex'

import FuelPage from './FuelPage'
import PayPage from './PayPage'

export default {
  name: 'GasStation',

  components: {
    FuelPage,
    PayPage
  },

  computed: {
    ...mapState('gasStation', ['currentPage']),
  },

  methods: {
    closeGasStation: function() {
      window.mp.trigger('gasStation:close')
    }
  }
}
</script>

<style lang="scss" scoped>
.gas-station {
  display: flex;
  text-transform: uppercase;
  color: #fff;
  text-transform: uppercase;  
  background: #111111;
  border: 1px solid #363636;
  box-sizing: border-box;
  border-radius: .5rem;
  padding: 1rem 2rem 2rem 2rem;
  min-width: 33.25rem;
  position: relative;
  .title{
    font-weight: bold;
    font-size: 2rem;
    line-height: 2rem;
    position: absolute;
    top: -.5rem;
    left: 0;
    transform: translateY(-100%);
    span{
      color: #DCFF00;
    }
  }
  .btn-close{
    background-image: url('/img/gasStation/btn-close.svg');
    width: 2rem;
    height: 2rem;
    position: absolute;
    top: -.5rem;
    right: 0;
    transform: translateY(-100%);
    &:hover{
      transform: translateY(-100%) scale(1.1);
    }
  }
}
</style>

<style lang="scss">
.gas-station{
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
