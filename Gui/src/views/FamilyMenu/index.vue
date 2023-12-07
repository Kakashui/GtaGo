<template>
  <div class="family-menu">
    <transition name="nav-slide">
      <div v-if="navData.show" class="family-menu__nav">
        <nav-item
          v-for="item in navList"
          :item="item"
          :key="item.id"
        />
      </div>
    </transition>
    <div class="body">
      <transition name="slide-fade">
      <component :is="currentPage" />
      </transition>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import NavItem from './components/NavItem'
import EventPage from './EventPage'
import InfoPage from './InfoPage'
import LevelPage from './LevelPage'
import MemberPage from './MemberPage'
import PropertyPage from './PropertyPage'
import BattlePage from './BattlePage'
import ControlPage from './ControlPage'
import RatingPage from './RatingPage'

export default {
  name: 'FamilyMenu',

  components: {
    NavItem,
    EventPage,
    InfoPage,
    LevelPage,
    MemberPage,
    PropertyPage,
    BattlePage,
    ControlPage,
    RatingPage,
  },

  data: function() {
    return {
      navList: [
        {
          key: 'InfoPage',
          text: 'familyMenu_0'
        },
        {
          key: 'ControlPage',
          text: 'familyMenu_1'
        },
        {
          key: 'LevelPage',
          text: 'familyMenu_2'
        },
        {
          key: 'MemberPage',
          text: 'familyMenu_3'
        },
        {
          key: 'PropertyPage',
          text: 'familyMenu_4'
        },
        {
          key: 'EventPage',
          text: 'familyMenu_5'
        },
        {
          key: 'BattlePage',
          text: 'familyMenu_6'
        },
      ],
      modalLeaders: false
    }
  },

  computed: {
    ...mapState('familyMenu', ['currentPage', 'navData']),
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
.family-menu {
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: flex-start;
  width: 100%;
  height: 100%;
  background: rgba(25, 28, 30, 0.8);
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  background-image: url('/img/familyMenu/bg.png');
  padding: 2rem 12.5rem 0;
 text-transform: uppercase;
  z-index: 2;
  color: #fff;
  transition: .3s;
  .nav-slide-enter-active {
    transition: all 0.3s ease;
  }
  .nav-slide-leave-active {
    transition: all 0.3s ease;
  }
  .nav-slide-enter {
    transform: translateY(-20rem);
    opacity: 0;
  }
  .nav-slide-leave-to {
    transform: translateY(-20rem);
    opacity: 0;
  }

  .slide-fade-enter-active {
    transition: all .3s ease;
  }
  .slide-fade-enter, .slide-fade-leave-to {
    transform: translateX(3rem);
    opacity: 0;
  }
  &.control{
    background-image: url('/img/familyMenu/bg-control.png');
    transition: .3s;
  }
  &__nav{
    display: flex;
    align-items: flex-start;
    justify-content: space-between;
    width: 100%;
    position: relative;
    z-index: 1;
    &:after{
      content: '';
      width: 100%;
      height: 1px;
      background: rgba(255, 255, 255, 0.3);
      position: absolute;
      bottom: 0;
      left: 0;
    }
    .btn-leave{
      font-size: 1.1rem;
      line-height: 1.55rem;
      padding-bottom: 1rem;
      border-bottom: .1rem solid transparent;
      color: rgba(255, 255, 255, 0.3);
      position: absolute;
      right: 0;
      top: 0;
      &:hover{
        color: rgba(255, 255, 255, 0.6);
      }
    }
  }
  .body{
    width: 100%;
    display: flex;
  }
}
</style>

<style lang="scss" >
.family-menu {
  @keyframes modalShow{
    from{
      opacity: 0;
    }
    to{
      opacity: 1;
    }
  }
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
  .modal-wrap{
    display: flex;
    align-items: center;
    justify-content: center;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100vh;
    background: rgba(0, 0, 0, 0.7);
    z-index: 3;
    animation: modalShow .2s;
  }
}
</style>
