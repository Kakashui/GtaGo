<template>
  <div class="tools">
    <div class="tools-tittle">Dev tool</div>
    <component :is="menus[currentPage] || 'Main'" /> 
  </div>
</template>

<script>
import Main from './Main'
import Animations from './Animations'
import VehicleEdit from './VehicleEdit'

import {mapState} from 'vuex'
export default {
  name: 'DevTools',
  computed: {
    ...mapState('devTools', ['currentPage', 'menus'])
  },
  components:{
    Main,
    Animations,
    VehicleEdit
  }
}
</script>
<style lang="scss" scoped>
.tools{
    margin-right: auto;
    width: 25rem;
    height: 45rem;
    background-color: rgba(#000, .5);
    z-index: 1000;
  &-tittle{
    color: #fff;
    text-align: center;
    padding: .3rem;
    background-color: rgba(#000, .5);
  }
}
</style>