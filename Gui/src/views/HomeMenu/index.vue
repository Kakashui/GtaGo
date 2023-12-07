<template>
  <div class="home-menu">
    <div class="btn btn-close" @click="closeHomeMenu"></div>
    <menu-nav/>
    <component :is="currentPage" />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import MenuNav from './components/MenuNav'
import InteriorsPage from './InteriorsPage'
import FurniturePage from './FurniturePage'
import GaragePage from './GaragePage'
import ResidentsPage from './ResidentsPage'

export default {
  name: 'HomeMenu',

  components: {
    MenuNav,
    InteriorsPage,
    FurniturePage,
    GaragePage,
    ResidentsPage
  },

  computed: {
    ...mapState('homeMenu', ['currentPage'])
  },

  methods: {
    closeHomeMenu: function() {
      window.mp.trigger('homeMenu:closeHomeMenu')
    }
  }
}
</script>

<style lang="scss" scoped>
.home-menu{
  display: flex;
  width: 100%;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 0;
  background-image: url('/img/homeMenu/bg.png');
  background-size: cover;
  background-repeat: no-repeat;
  text-transform: uppercase;
  color: #fff;
  .btn-close{
    position: absolute;
    right: 2.8rem;
    top: 2.8rem;
    width: 2rem;
    height: 2rem;
    background-image: url('/img/homeMenu/btn-close.svg');
    z-index: 1;
    opacity: .45;
    &:hover{
      opacity: 1;
    }
  }
}
</style>

<style lang="scss">
.home-menu{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
  .header{
    &__nav{
      position: absolute;
      top: .5rem;
      right: 0;
      display: flex;
      align-items: center;
      justify-content: flex-end;
      &-item{
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(0, 0, 0, 0.15);
        border: 1px solid rgba(255, 255, 255, 0.35);
        box-sizing: border-box;
        border-radius: 3rem;
        padding: .8rem 1.6rem;
        font-size: 1.2rem;
        color: rgba(255, 255, 255, 0.65);
        transition: .2s;
        &:before{
          content: '';
          width: 1.6rem;
          height: 1.6rem;
          margin-right: .5rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
        &:hover{
          background: rgba(255, 255, 255, 0.35);
          color: rgba(255, 255, 255, 1);
          border-color: transparent;
          transition: .2s;
        }
      }
    }
  }
}
</style>
