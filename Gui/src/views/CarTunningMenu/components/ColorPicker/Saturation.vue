<template>
  <div class="saturation" @mousedown.prevent.stop="selectSaturation">
    <canvas ref="canvasSaturation" />
    <div class="saturation__slide" :style="slideSaturationStyle" ></div>
  </div>
</template>

<script>
import mixin from './mixin'

export default {
  name: 'Saturation',

  mixins: [mixin],

  props: {
    color: {
      type: String,
      default: '#000000'
    },
    hsv: {
      type: Object,
      default: null
    },
    size: {
      type: Number,
    }
  },

  data: function() {
    return {
      slideSaturationStyle: {}
    }
  },

  computed: {
    appWidthUnit: function () {
      const appWidth = document.getElementById('app').offsetWidth / 100
      const widthUnit = Math.ceil(appWidth)
      return widthUnit
    }
  },

  methods: {
    renderColor: function() {
      const canvas = this.$refs.canvasSaturation
      const size = this.size
      const ctx = canvas.getContext('2d')
      canvas.width = size
      canvas.height = size

      ctx.fillStyle = this.color
      ctx.fillRect(0, 0, size, size)

      this.createLinearGradient('l', ctx, size, size, '#FFFFFF', 'rgba(255,255,255,0)')
      this.createLinearGradient('p', ctx, size, size, 'rgba(0,0,0,0)', '#000000')
    },

    renderSlide: function() {
      this.slideSaturationStyle = {
        left: this.hsv.s * this.size - (0.4 * this.appWidthUnit) + 'px',
        top: (1 - this.hsv.v) * this.size - (0.4 * this.appWidthUnit) + 'px'
      }
    },

    selectSaturation: function(e) {
      const { top: saturationTop, left: saturationLeft } = this.$el.getBoundingClientRect()
      const ctx = e.target.getContext('2d')

      const mousemove = e => {
        let x = e.clientX - saturationLeft
        let y = e.clientY - saturationTop

        if (x < 0) {
            x = 0
        }
        if (y < 0) {
            y = 0
        }
        if (x > this.size) {
            x = this.size
        }
        if (y > this.size) {
            y = this.size
        }

        this.slideSaturationStyle = {
            left: x - (0.4 * this.appWidthUnit) + 'px',
            top: y - (0.4 * this.appWidthUnit) + 'px'
        }

        const imgData = ctx.getImageData(Math.min(x, this.size - 1), Math.min(y, this.size - 1), 1, 1)
        const [r, g, b] = imgData.data

        this.$emit('selectSaturation', { r, g, b })
      }

      mousemove(e)

      const mouseup = () => {
        document.removeEventListener('mousemove', mousemove)
        document.removeEventListener('mouseup', mouseup)
      }

      document.addEventListener('mousemove', mousemove)
      document.addEventListener('mouseup', mouseup)
    }
  },

  mounted: function() {
    this.renderColor()
    this.renderSlide()
  }
}
</script>

<style lang="scss" scoped>
.saturation {
  position: relative;
  margin: 0 0.7rem 0 0;
  canvas {
    vertical-align: top;
  }
  &__slide {
    position: absolute;
    left: 100%;
    top: 0;
    width: 0.8rem;
    height: 0.8rem;
    border-radius: 50%;
    border: 0.1rem solid #fff;
    box-shadow: 0 0 1px 1px rgba(0, 0, 0, 0.3);
    pointer-events: none;
  }
}
</style>
