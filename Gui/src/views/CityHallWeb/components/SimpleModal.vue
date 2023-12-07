<template>
  <div class="simple-modal">
    <div class="simple-modal__wrap">
      <div 
        class="title"
        v-if="modalData.title"
      >{{loc(modalData.title)}}</div>
      <div
       class="desc"
       v-if="modalData.desc"
      >{{loc(modalData.desc)}}</div>
      <div 
        class="btn btn-continue"
        @click="closeModal"
        
      >{{loc('cityHallWeb_15')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'SimpleModal',

  computed: {
    ...mapState('cityHallWeb', ['modalData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('cityHallWeb', ['closeModal'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes modalShow{
  from {
    transform: translateY(100%);
  }
  to {
    transform: translateY(0%);
  }
}
.simple-modal{
  position: fixed;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0, 0, 0, 0.7);
  z-index: 3;
  &__wrap{
    display: flex;
    flex-flow: column;
    align-items: center;
    background: #FFFFFF;
    text-align: center;
    border-radius: 1rem;
    padding: 2.5rem 4.4rem;
    width: 28.8rem;
    font-family: Roboto;
    .title{
      font-weight: bold;
      font-size: 1.7rem;
      line-height: 2rem;
      text-align: center;
      text-transform: uppercase;
      color: rgba(0, 0, 0, 0.7);
      margin-bottom: .5rem;
    }
    .desc{
      font-style: normal;
      font-weight: normal;
      font-size: .8rem;
      line-height: .95rem;
      text-align: center;
      color: rgba(0, 0, 0, 0.4);
      margin-bottom: 1.7rem;
    }
    .btn-continue{
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 1.1rem;
      text-align: center;
      color: #FFFFFF;
      height: 3.5rem;
      min-height: 3.5rem;
      background: #E00B29;
      border-radius: .3rem;
      width: 100%;
      &:hover{
        background: #C00621;
      }
    }
  }
}
</style>
