<template>
  <transition duration="400">
    <div class="profile">
      <UserInfo />
      <AboutGame />
      <ButtonStart />
    </div>
  </transition>
</template>

<script>
import UserInfo from './UserInfo';
import AboutGame from './AboutGame';
import ButtonStart from './ButtonStart';
export default {
  name: 'Profile',
  components: {
    UserInfo,
    AboutGame,
    ButtonStart,
  }
};
</script>

<style scoped lang="scss">
.profile {
  width: 100%;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 0; 
  z-index: 5;
  padding: 3rem 2rem 7.5rem 2rem;
  background: url('/img/miniGames/makeWeapon/bg-profile.png') left top repeat-y;
  background-size: cover;
  box-shadow: 4px 0px 60px rgba(255, 255, 255, 0.14);
  transition: left ease 0.2s;
}
</style>
