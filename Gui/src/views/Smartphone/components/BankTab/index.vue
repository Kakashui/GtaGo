<template>
  <div class="bank-tab">
    <main-content v-if="bankAccount" />
    <preloader-content v-else />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import PreloaderContent from './PreloaderContent'
import MainContent from './MainContent'

export default {
  name: 'BankTab',

  components: { PreloaderContent, MainContent },

  computed: {
    ...mapState('smartphone/bankPage', ['bankAccount'])
  }
}
</script>

<style lang="scss" scoped>
.bank-tab {
  height: 100%;
}
</style>
