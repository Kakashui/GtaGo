<template>
  <div class="main">
    <MainTop />
    <MainMiddle />
    <MainBottom />
  </div>
</template>

<script>
import MainTop from './MainTop';
import MainMiddle from './MainMiddle';
import MainBottom from './MainBottom';

export default {
  name: 'Main',
  components: {
    MainTop,
    MainMiddle,
    MainBottom,
  },
};
</script>

<style scoped lang="scss">
.main {
  display: flex;
  flex-flow: column;
  flex-wrap: nowrap;
  justify-content: space-between;
  width: 100%;
  height: 100%;
  background: url('/img/miniGames/makeWeapon/bg-main.jpg');
  background-size: cover;
}
</style>
