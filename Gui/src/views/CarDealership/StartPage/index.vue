<template>
  <div class="start-page">
    <component :is="currentVehicle" />
  </div>
</template>

<script>
import { mapState } from 'vuex'

import MotorPage from './MotorPage'
import LamboPage from './LamboPage'
import BugattiPage from './BugattiPage'
import MercedesPage from './MercedesPage'
import BmwPage from './BmwPage'
import RollsPage from './RollsPage'
import BentleyPage from './BentleyPage'
import FerrariPage from './FerrariPage'
import MiddlePage from './MiddlePage'
import DiamondPage from './DiamondPage'
import LuxePage from './LuxePage'
import JeepPage from './JeepPage'
import CasualPage from './CasualPage'
import GaragePage from './GaragePage'

export default {
  name: 'StartPage',

  components: {
    MotorPage,
    LamboPage,
    BugattiPage,
    MercedesPage,
    BmwPage,
    RollsPage,
    BentleyPage,
    FerrariPage,
    MiddlePage,
    DiamondPage,
    LuxePage,
    JeepPage,
    CasualPage,
    GaragePage
  },

  computed: {
    ...mapState('carDealership', ['currentVehicle'])
  }
}
</script>

<style lang="scss" scoped>
.start-page{
  z-index: 99;
  width: 100%;
  height: 100%;
}
</style>
