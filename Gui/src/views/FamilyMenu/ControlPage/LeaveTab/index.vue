<template>
  <div class="leave-tab">
    <div class="title">{{loc('familyMenu_7')}}</div>
    <template v-if="accountType.toLowerCase() === 'premium'">
      <div class="desc">{{loc('familyMenu_9')}}</div>
      <div 
        class="btn"
        @click="setModalLeave"
      >{{loc('familyMenu_10')}}</div>
    </template>
    <template v-else>
      <div class="desc">{{loc('familyMenu_13')}}</div>
      <div 
        class="btn btn-sub"
        @click="goToPremium"
      >{{loc('familyMenu_14')}}</div>
    </template>
    <modal-leave 
      v-if="modalLeaveShow"
      @closeModalLeave="setModalLeave"
    />
  </div>
</template>

<script>
import ModalLeave from './ModalLeave'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'LeaveTab',

  components: {
    ModalLeave
  },

  data: function() {
    return{
      modalLeaveShow: false
    }
  },

  computed: {
    ...mapState('familyMenu', ['accountType']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setModalLeave: function() {
      this.modalLeaveShow = !this.modalLeaveShow
    },
    goToPremium: function() {
      window.mp.trigger('familyMenu:goToPremium')
    },
  }
}
</script>

<style lang="scss" scoped>
.leave-tab{
  display: flex;
  flex-flow: column;
  .btn{
    width: fit-content;
    font-weight: bold;
    font-size: 1.2rem;
    line-height: 1.2rem;
    background: #C1E704;
    border-radius: .1rem;
    padding: 1rem 1.6rem;
    &:hover{
      box-shadow: 0 0 1rem #C1E704;
    }
  }
  .title{
    font-size: 2rem;
  }
  .desc{
    font-weight: normal;
    font-size: 1.1rem;
    line-height: 1.3rem;
    text-transform: uppercase;
    color: rgba(255, 255, 255, 0.5);
    max-width: 23.8rem;
    margin-bottom: 1rem;
  }
  .btn-sub{
    &:before{
      content: '';
      width: 1.5rem;
      height: 1.5rem;
      margin-right: .5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/familyMenu/btn.svg');
    }
  }
}
</style>
