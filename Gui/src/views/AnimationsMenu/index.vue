<template>
  <div class="animations-menu">
      <div class="close" @click="close"></div>
      <div class="header">
        <div class="logo"></div>
        <categories-list
        />
      </div>
      <div class="fast-access">
        <div class="fast-access__title">{{loc('AnimationsMenu_1')}}</div>
        <keys-list/>
      </div>
      <animations-list
        :currentAnimations="currentAnimations"
      />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import AnimationsList from './components/AnimationsList'
import CategoriesList from './components/CategoriesList'
import KeysList from './components/KeysList'

export default {
  name: 'AnimationsMenu',

  components: {
    AnimationsList,
    CategoriesList,
    KeysList
  },
  
  computed: {
    ...mapState('animationsMenu', ['currentAnimations']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    close: function() {
      window.mp.trigger('animations::close');
    }
  }
}
</script>

<style lang="scss" scoped>
.animations-menu {
  width: 100vw;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 0;
  padding-top: 2.3rem;
  text-transform: uppercase;
  font-weight: bold;
  letter-spacing: 0.05em;
  color: #fff;
  display: flex;
  flex-flow: column;
  font-size: .9rem;
  line-height: .9rem;
  text-transform: uppercase;
  background: radial-gradient(50% 50% at 50% 50%, rgba(0, 0, 0, 0) 0%, #000000 100%);
  .close {
    position: absolute;
    top: 2rem;
    right: 1.5rem;
    width: 2.5rem;
    height: 2.5rem;
    background-image: url('/img/animationsMenu/close.png');
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    z-index: 1;
  }
  .header{
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    padding-left: 2.5rem;
    width: 100%;
    position: relative;
    height: 3.9rem;
    &:after{
      content: '';
      width: 100%;
      height: 1px;
      background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.65) 0%, rgba(255, 255, 255, 0) 100%);
      position: absolute;
      left: 0;
      bottom: 0;
    }
    .logo{
      width: 1.9rem;
      height: 1.65rem;
      min-width: 1.9rem;
      min-height: 1.65rem;
      background-image: url('/img/animationsMenu/logo.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      margin-right: 3.2rem;
    }
  }
  .fast-access{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    margin-top: 1.65rem;
    padding-left: 2.5rem;
    padding-bottom: 1.3rem;
    position: relative;
    min-height: 6.85rem;
    &:after{
      content: '';
      width: 100%;
      height: 1px;
      background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.65) 0%, rgba(255, 255, 255, 0) 100%);
      position: absolute;
      left: 0;
      bottom: 0;
    }
    &__title{
      font-size: 1.05rem;
      margin-bottom: 1.85rem;
    }
    &__keys{
      display: flex;
      align-items: center;
      justify-content: flex-start;
      &-slot{
        display: flex;
        align-items: center;
        justify-content: center;
        margin-right: 3.15rem;
        .icon{
          margin-right: .95rem;
          min-width: 2.6rem;
          width: 2.6rem;
          height: 2.6rem;
          padding: .5rem;
          border-radius: .5rem;
          background: rgba(255, 255, 255, 0.15);
          box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
          border: 1px solid rgba(255, 255, 255, 0.15);
          color: rgba(255, 255, 255, 0.65);
        }
        .text{
          max-width: 3rem;
          text-align: left;
        }
      }
    }
  }
}
</style>
