<template>
  <div 
    :class="[{active: currentTab.section === 'ProfileSection'}, 'btn-close']" 
    v-if="taxiJobData && currentTab.section !== 'NotifySection'"
    @click="setProfile"
  >
    <div class="burger-anim">
      <span></span>
      <span></span>
      <span></span>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  name: 'ProfileButton',

  props: {
    setProfile: Function
  },
  
  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiJobData'])
  },
}
</script>

<style lang="scss" scoped>
.btn-close{  
  background: #FFFFFF;
  width: 2.5rem;
  height: 2.5rem;
  border-radius: .5rem;
  position: absolute;
  left: 1rem;
  top: 2rem;
  box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.3);
  z-index: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  &:hover{
    box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.6);
    transition: .3s;
  }
  .burger-anim{
    width: .85rem;
    height: .85rem;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    span{
      position: absolute;
      width: .85rem;
      height: 1px;
      background: #464646;
      border-radius: .1rem;
      transform-origin: 50% 50%;
      &:nth-child(1){
        transform: translateY(-.25rem);
        transition-property: transform, width;
        transition-duration: .2s, .2s;
      }
      &:nth-child(2){
        opacity: 1;
        transition-property: opacity;
        transition-duration: .3s;
      }
      &:nth-child(3){
        transform: translateY(.25rem);
        transition-property:  transform, width;
        transition-duration: .2s, .2s;
      }
    }
  }
  &.active{
    .burger-anim{
      span{
        &:nth-child(1), &:nth-child(3){
          top: .4rem;
          width: .95rem;
          transition-property: transform, width;
          transition-duration: .7s, .3s;
        }
        &:nth-child(1){
          transform: translateY(0) rotate(45deg);
        }
        &:nth-child(2){
          opacity: 0;
          transition-property: opacity;
          transition-duration: .3s;
        }
        &:nth-child(3){
          transform: translateY(0) rotate(-45deg);
        }
      }
    }
  }
}
</style>
