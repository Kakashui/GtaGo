<template>
  <div class="transfer-section">
    <transition name="transfer-section">
      <component
        :is="currentTab.subsection"
        :transferModel="transferModel"
        @input="(newTransferModel) => {transferModel = newTransferModel}"
      ></component>
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'
import GeneralSubsection from './GeneralSubsection'
import NotifySubsection from './NotifySection'

export default {
  name: 'TransferSection',

  components: { GeneralSubsection, NotifySubsection },

  data: function() {
    return {
      transferModel: {
        sum: '',
        cardNumber: ''
      }
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.transfer-section {
  height: 100%;
}
.transfer-section-enter-active {
  animation: fade 0.4s;
  position: absolute;
}
</style>