<template>
  <div class="image-modal">
    <img :src="modal.data">
    <div 
      class="modal-btn"
      @click="setModal({show: false, currentModal: null, data: null})"
    ></div>
  </div>
</template>

<script>
import { mapMutations, mapState } from 'vuex'
export default {
  name: 'ModalBtn',

  computed: {
    ...mapState('news', ['modal'])
  },

  methods: {
    ...mapMutations('news', ['setModal'])
  }
}
</script>

<style lang="scss" scoped>
.image-modal{
  position: relative;
  img{
    max-width: 37.1rem;
  }
  .modal-btn{
    position: absolute;
    right: -.5rem;
    top: 0;
    transform: translateX(100%);
    width: 2rem;
    height: 2rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/news/btn-close.svg');
    opacity: .7;
    transition: .3s;
    &:hover{
      opacity: 1;
    }
  }
}
</style>
