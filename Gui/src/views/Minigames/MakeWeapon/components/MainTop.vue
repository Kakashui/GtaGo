<template>
  <div class="main-top">
    <MainTopLeft />
    <MainTopCenter />
    <MainTopRight />
  </div>
</template>

<script>
import MainTopCenter from './MainTopCenter'
import MainTopLeft from './MainTopLeft'
import MainTopRight from './MainTopRight'
export default {
  name: 'MainTop',
  components: {
    MainTopLeft,
    MainTopCenter,
    MainTopRight,
  },
};
</script>

<style scoped lang="scss">
.main-top {
  width: 100%;
  height: 9rem;

  &__center {
    width: 45rem;
    margin: 0 auto;
  }

  &__right {
    position: absolute;
    top: 0;
    right: 0;
  }
}
</style>
