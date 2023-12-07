<template>
  <div class="main-top__center">
    <CurrentGun />
    <CurrentGain />
</div>
</template>

<script>
import CurrentGun from './CurrentGun';
import CurrentGain from './CurrentGain';
export default {
  name: 'MainTopCenter',
  components: {
    CurrentGun,
    CurrentGain,
  },
};
</script>

<style scoped lang="scss"></style>