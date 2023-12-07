<template>
  <div class="main-top__left">
    <ButtonExit />
    <ButtonProfile />
    <transition name="makeweaponprofile">
      <Profile v-if="profileVisibility"/>    
    </transition>
  </div>
</template>

<script>
import Profile from './Profile';
import ButtonExit from './ButtonExit';
import ButtonProfile from './ButtonProfile';
import {mapState} from 'vuex'

export default {
  name: 'MainTopLeft',
  components: {
    Profile,
    ButtonExit,
    ButtonProfile,
  },
   computed: {
    ...mapState('gameMakeWeapon/pageState', ['profileVisibility']),
  }
};
</script>

<style scoped lang="scss">
.main-top__left {
  width: 23.2rem;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  padding: 2rem 0 0 2rem;
}
.makeweaponprofile-enter-active, .makeweaponprofile-leave-active {
    transition: all .4s;
    transform: translateX(0);
}
.makeweaponprofile-leave-to, .makeweaponprofile-enter /* .fade-leave-active to version 2.1.8 */ {    
    transform: translateX(-100%);
}
</style>
