<template>
  <div class="specifications__bar">
    <div
      v-if="type === 'up'"
      class="current"
      :style="{ width: currentPercentage + '%' }"
    ></div>
    <div
      v-else-if="type === 'down'"
      class="current"
      :style="{ width: currentPercentage - differencePercentage + '%' }"
    ></div>
    <div
      v-else
      class="current"
      :style="{ width: currentPercentage + '%' }"
    ></div>
    <div
      v-if="type === 'up'"
      :class="[type, 'difference']"
      :style="{ left: currentPercentage + '%', width: differencePercentage + '%' }"
    ></div>
    <div
      v-else-if="type === 'down'"
      :class="[type, 'difference']"
      :style="{ left: currentPercentage - differencePercentage + '%', width: differencePercentage + '%' }"
    ></div>
  </div>
</template>

<script>
export default {
  name: 'ProgressBar',

  props: [
    'currentValue',
    'maxValue',
    'difference',
    'type'
  ],

  computed: {
    currentPercentage: function() {
      return (this.currentValue / this.maxValue) * 100
    },

    differencePercentage: function() {
      return (this.difference / this.maxValue) * 100
    }
  }
}
</script>

<style lang="scss" scoped>
.specifications__bar {
  background: transparent;
  height: .5rem;
  position: relative;
  width: 100%;
  &::before {
    content: "";
    position: absolute;
    transform: translate(-50%, -50%);
    top: 50%;
    left: 50%;
    width: calc(100% - 2px);
    height: calc(100% - 2px);
    border: 1px solid rgba(255, 255, 255, 0.2);
  }
  .current {
    top: 0;
    left: 0;
    height: 100%;
    background: linear-gradient(270deg, #FFFFFF 55.43%, rgba(255, 255, 255, 0) 100%);
  }
  .difference {
    top: 0;
    position: absolute;
    height: 100%;
    &.up {
      background: #C1E704;
    }
    &.down {
      background: #E03838;
      // transform: rotate(180deg) translate(0, -100%);
      // transform-origin: top left;
    }
  }
}
</style>
