<template>
  <transition name="modal">
    <div class="modal-wrap">
      <component :is="currentComponent"/>
    </div>
  </transition>
</template>

<script>
import { mapState } from 'vuex'
import ImageModal from './components/ImageModal'
import PriceModal from './components/PriceModal'
import ConfirmationStreamModal from './components/ConfirmationStreamModal'
export default {
  name: 'Modal',

  components: {
    ImageModal,
    PriceModal,
    ConfirmationStreamModal
  },

  data: function() {
    return {
      currentComponent: null
    }
  },

  computed: {
    ...mapState('news', ['modal'])
  },

  mounted() {
    this.currentComponent = this.modal.currentModal
  }
}
</script>

<style lang="scss" scoped>
.modal-wrap{
  width: calc(100% - (6rem));
  height: calc(100% - (6rem));
  position: absolute;
  left: 3rem;
  top: 3rem;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 1rem;
  z-index: 2;
}

.modal-enter-active, .modal-leave-active{
  transition: all .2s cubic-bezier(1.0, 0.5, 0.8, 1.0);
}
.modal-enter, .modal-leave-to{
  opacity: 0;
}
</style>
