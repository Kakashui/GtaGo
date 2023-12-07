<template>
  <div class="color-palette">
    <div class="color-palette__header">
      <button
        @mouseenter="play({ name: 'hover' , volume: .4 })"
        :class="[{ active: isSlider }, 'btn']"
        @click="toggleIsSlider"
      ></button>
      <span class="title">{{ title }}</span>
    </div>
    <div v-show="isSlider" class="color-palette__main">
      <div
        v-for="(item, index) in thirdTabData.sliderColors"
        :key="index"
        :style="{ background: `rgb(${item.Red}, ${item.Green}, ${item.Blue}` }"
        :class="[{ active: item.Number === currentColorNumber }, 'item']"
        @click="setCurrentColorNumber(item.Number)"
      ></div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState } from 'vuex'

export default {
  name: 'ColorPalette',

  props: ['title'],

  data: function() {
    return {
      isSlider: false,
      currentColorNumber: null
    }
  },

  computed: {
    ...mapState('carTunningMenu', ['thirdTabData'])
  },

  methods: {    
    ...mapMutations('sounds', ['play']),
    toggleIsSlider: function() {
      this.isSlider = !this.isSlider
      window.mp.trigger('lsCustom:changeToggleIsSlider', this.isSlider);
    },

    setCurrentColorNumber: function(value) {
      this.currentColorNumber = value
      window.mp.trigger('lsCustom:changeColorSlider', value)
    }
  }
}
</script>

<style lang="scss" scoped>
.color-palette {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  width: 19rem;
  &__header {
    display: flex;
    align-items: center;
    margin: 0 0 0.85rem 0;
    .title {
      font-size: 1.05rem;
      line-height: 1.25rem;
      color: #fff;
    }
    .btn {
      background: transparent;
      &.active {
        &:after {
          transform: rotate(0);
        }
      }
      &:after {
        content: "";
        width: 1.4rem;
        height: 0.95rem;
        background: center / contain no-repeat url("/img/carTunningMenu/chevronUp.svg");
        display: block;
        margin: 0 0.8rem 0 0;
        transform: rotate(180deg);
      }
    }
  }
  &__main {
    display: grid;
    grid-template-columns: repeat(19, 1rem);
    grid-auto-rows: 1rem;
    .item {
      border-radius: 50%;
      &:active {
        transition: all 0.15s;
        transform: scale(1.6);
        border: 0.125rem solid #fff;
      }
      &.active {
        transform: scale(1.6);
        border: 0.125rem solid #fff;
      }
    }
  }
}
</style>
