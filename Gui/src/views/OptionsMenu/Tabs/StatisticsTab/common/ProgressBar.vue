<template>
  <div :style="styles" class="progress-bar">
    <div class="progress-bar__main">
      <div :style="{ width: percentage + '%' }" class="line"></div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProgressBar',

  props: {
    curVal: Number,
    maxVal: Number,
    styles: Object
  },

  data: function() {
    return {
      percentage: null
    }
  },

  methods: {
    getPercentage: function(max, cur) {
      return cur / max * 100
    }
  },

  mounted: function() {
    this.percentage = this.getPercentage(this.maxVal, this.curVal)
  }
}
</script>

<style lang="scss" scoped>
.progress-bar {
  width: 15rem;
  height: 1rem;
  overflow: hidden;
  display: flex;
  align-items: center;
  &__main {
    border: 1px solid rgba(255, 255, 255, 0.2);
    height: 0.5rem;
    width: 100%;
    .line {
      height: 100%;
      background: linear-gradient(270deg, #B6D300 0%, rgba(182, 211, 0, 0) 100%);
      position: relative;
      display: flex;
      &::before {
        content: "";
        position: absolute;
        transform: translate(0, -50%);
        right: 0;
        top: 50%;
        background: radial-gradient(100% 50% at 100% 50%, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
        height: 1rem;
        width: 0.1rem;
      }
      &::after {
        content: "";
        position: absolute;
        transform: translate(0, -50%);
        right: 0;
        top: 50%;
        background: radial-gradient(100% 50% at 100% 50%, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
        height: 1rem;
        width: 3rem;
      }
    }
  }
}
</style>
