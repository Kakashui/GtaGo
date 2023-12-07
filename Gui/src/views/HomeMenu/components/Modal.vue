<template>
  <div class="modal-wrap">
    <div class="modal">
      <div class="btn-close" @click="closeModal"></div>
      <template v-if="type === 'rentCost'">
        <div class="title" v-if="type === 'rentCost'">{{loc('HomeMenu_18')}}</div>
        <input type="text" v-model.number="currentCost" :placeholder="loc('home:menu:plh:1')">
        <div class="btn btn-apply" @click="setCurrentCost">{{loc('HomeMenu_19')}}</div>
      </template>
      <template v-else>
        <div class="title">{{loc('HomeMenu_20')}}</div>
        <input type="text" v-model="currentOccupier" :placeholder="loc('home:menu:plh:2')">
        <div class="btn btn-apply" @click="setCurrentOccupier">{{loc('HomeMenu_19')}}</div>
      </template>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'Modal',
  
  props: {
    type: String,
    closeModal: Function
  },

  data: function() {
    return {
      currentCost: null,
      currentOccupier: null,
    }
  },
  computed: {
    ...mapState('homeMenu', ['houseId']),
    ...mapMutations('homeMenu', ['setRentCost', 'rentCost']),
    ...mapGetters('localization', ['loc']),    
  },
  methods: {
    setCurrentOccupier: function() {
      window.mp.triggerServer("house:occupierAddedRequest", this.houseId, this.currentOccupier)
      this.closeModal()
    },
    setCurrentCost: function() {
      this.closeModal()
      window.mp.trigger("homeMenu:rentCostChanged", this.houseId, this.currentCost)
    }
  }
}
</script>

<style lang="scss" scoped>
.modal-wrap{
  position: fixed;
  display: flex;
  align-items: center;
  justify-content: center;
  top: 0;
  left: 0;
  height: 100vh;
  width: 100%;
  background: rgba(0, 0, 0, 0.65);
  z-index: 10;
  .modal{
    background: #17191F;
    box-shadow: 0 1.1rem 1.6rem rgba(0, 0, 0, 0.35);
    display: flex;
    flex-flow: column;
    align-items: center;
    padding: 3rem 1.75rem;
    position: relative;
    width: 33.5rem;
    min-width: 33.5rem;
    .btn-close{
      position: absolute;
      z-index: 1;
      top: 1.5rem;
      right: 1.5rem;
      width: 2rem;
      height: 2rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/homeMenu/btn-close.svg');
      opacity: .45;
      &:hover{
        transition: .2s;
        opacity: 1;
      }
    }
    .title{
      text-transform: uppercase;
      font-size: 2.05rem;
      line-height: 2.45rem;
      color: #FFFFFF;
    }
    input{
      background: #101115;
      border: 1px solid rgba(255, 255, 255, 0.15);
      box-sizing: border-box;
      text-align: center;
      width: 100%;
      padding: 1.35rem;
      font-family: Roboto;
      margin: 2.25rem 0;
      color: #fff;
    }
    .btn-apply{
      background: #C1E704;
      border-radius: .2rem;
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 1.2rem;
      line-height: 1.2rem;
      color: #252D00;
      padding: 1.2rem 4.5rem;
      &:hover{
        box-shadow: 0 .2rem 1.1rem rgba(193, 231, 4, 0.45);
        transition: .2s;
      }
    }
  }
}
</style>
