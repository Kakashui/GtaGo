<template>
  <div class="color-picker">
    <div class="color-picker__header">
      <button
        @mouseenter="play({ name: 'hover' , volume: .4 })"
        :class="[{ active: isPicker }, 'btn']"
        @click="toggleIsPicker"
      ></button>
      <span class="title">{{ title }}</span>
    </div>
    <div v-show="isPicker" class="color-picker__main">
      <div class="color-set">
        <saturation
          ref="saturation"
          :color="rgbString"
          :hsv="hsv"
          :size="saturationHeight * appWidthUnit"
          @selectSaturation="selectSaturation"
        />
        <hue
          ref="hue"
          :hsv="hsv"
          :width="hueWidth * appWidthUnit"
          :height="hueHeight * appWidthUnit"
          @selectHue="selectHue"
        />
      </div>
      <box
        name="RGB"
        :color="modelRgb"
        @inputColor="inputRgb"
      />
      <type-select v-if="type !== 'Neon'" :type="type" />
    </div>
  </div>
</template>

<script>
import { mapMutations } from 'vuex'
import mixin from './mixin'

import Saturation from './Saturation'
import Hue from './Hue'
import Box from './Box'
import TypeSelect from './Select'

export default {
  name: 'ColorPicker',

  components: {
    Saturation,
    Hue,
    Box,
    TypeSelect
  },

  props: [
    'title',
    'type',
    'initialColor'
  ],

  mixins: [mixin],

  data: function() {
    return {
      isPicker: false,
      hueWidth: 1,
      hueHeight: 9.8,
      saturationHeight: 10,
      modelRgb: '',
      r: 0,
      g: 0,
      b: 0,
      h: 0,
      s: 0,
      v: 0
    }
  },

  computed: {
    appWidthUnit: function () {
      const appWidth = document.getElementById('app').offsetWidth / 100
      const widthUnit = Math.ceil(appWidth)
      return widthUnit;
    },

    totalWidth: function() {
      return this.hueHeight + (this.hueWidth + 8) * 2
    },

    rgb: function() {
      return {
        r: this.r,
        g: this.g,
        b: this.b
      }
    },

    hsv: function() {
      return {
        h: this.h,
        s: this.s,
        v: this.v
      }
    },

    rgbString: function() {
      return `rgb(${this.r}, ${this.g}, ${this.b})`
    },

    rgbStringShort: function() {
      return `${this.r}, ${this.g}, ${this.b}`
    }
  },

  methods: {
    ...mapMutations('sounds', ['play']),
    toggleIsPicker: function() {
      this.isPicker = !this.isPicker
      window.mp.trigger('lsCustom:changeToggleIsPicker', this.isPicker, this.type);
    },

    setText: function() {
      this.modelRgb = this.rgbStringShort
    },

    selectSaturation: function(color) {
      const { r, g, b, h, s, v } = this.setColorValue(color)

      Object.assign(this, { r, g, b, h, s, v })
      this.setText()
    },

    selectHue: function(color) {
      const { r, g, b, h, s, v } = this.setColorValue(color)

      Object.assign(this, { r, g, b, h, s, v })
      this.setText()
      this.$nextTick(() => {
        this.$refs.saturation.renderColor()
        this.$refs.saturation.renderSlide()
      })
    },

    inputRgb: function(color) {
      const { r, g, b, h, s, v } = this.setColorValue(color)

      Object.assign(this, { r, g, b, h, s, v })
      this.modelRgb = color
      this.$nextTick(() => {
        this.$refs.saturation.renderColor()
        this.$refs.saturation.renderSlide()
        this.$refs.hue.renderSlide()
      })
    },

    selectColor: function(color) {
      const { r, g, b, h, s, v } = this.setColorValue(color)

      Object.assign(this, { r, g, b, h, s, v })
      this.setText()
      this.$nextTick(() => {
        this.$refs.saturation.renderColor()
        this.$refs.saturation.renderSlide()
        this.$refs.hue.renderSlide()
      })
    }
  },

  created: function() {
    Object.assign(this, this.setColorValue(this.color))
    this.setText()
    this.inputRgb(this.initialColor)

    this.$watch('rgb', () => {
      this.$emit('changeColor', {
        rgb: this.rgb,
        hsv: this.hsv
        
      })
      window.mp.trigger('lsCustom:changeColor',  this.rgb.r, this.rgb.g, this.rgb.b, this.type);
    })
  }
}
</script>

<style lang="scss" scoped>
.color-picker {
  z-index: 99;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  margin: 0 1.5rem 0 0;
  width: 11.7rem;
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
    .color-set {
      display: flex;
      align-items: center;
    }
  }
}
</style>
