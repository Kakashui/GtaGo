<template>
  <div class="exit-window">
    <div class="exit-window__modal">
       <span>
        {{loc('gui_726')}}          
      </span>
      <div class="exit-window__actions">
        <div 
          @click="exitClose"
          class="exit-window__btn disagree"
        > {{loc('gui_728')}} </div>
        <div 
          @click="exitDone"
          class="exit-window__btn agree"
        > {{loc('gui_727')}} </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ExitWindow',
  computed: {
    ...mapGetters('localization',['loc']),
  },
  methods: {
    exitDone: function() {
      this.$emit('setExitWindow', false)
      window.mp.trigger('poker:exit')
    },
    exitClose: function() {
      this.$emit('setExitWindow', false)
    }
  }
}
</script>

<style lang="scss" scoped>
.exit-window{
  position: fixed;
  width: 100%;
  height: 100vh;
  left: 0;
  top: 0;
  background-color: rgba(34, 34, 34, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3;
  span{
    font-weight: bold;
    font-size: 1.8rem;
    line-height: 2.05rem;
    text-align: center;
    text-transform: uppercase;
    color: #FFFFFF;
    max-width: 16.7rem;
    margin-bottom: 1rem;
  }
  &__modal{
    display: flex;
    align-items: center;
    flex-direction: column;
    justify-content: center;
    width: 35rem;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: left top;
    background-image: url('/img/poker/modal-bg.png');
    position: relative;
    padding: 2.5rem;
  }
  &__actions{
    display: flex;
    align-items: center;
  }
  &__btn{
    height: 3.2rem;
    width: 6.8rem;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: .8rem;
    text-align: center;
    letter-spacing: 0.1em;
    text-transform: uppercase;
    color: #FFFFFF;
    transition: .3s;
    border-radius: .1rem;
    margin-right: 1rem;
    &:last-child{
      margin-right: 0;
    }
    &.agree{
      background-color: #B5D60D;
      &:hover{
        box-shadow: 0 .5rem 1.5rem rgba(182, 209, 42, 0.5);
      }
    }
    &.disagree{
      background-color: rgba(255, 255, 255, 0.2);
      &:hover{
        background-color: rgba(255, 255, 255, 0.4);
      }
    }
  }
}
</style>
