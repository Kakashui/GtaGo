<template>
    <div
      class="slider with-buttons"
      @mouseup="captureOff"
      @mouseleave="captureOff"
      :class="{ disabled: !currentPlayerMove }"
    >
      <div
        class="slider__wrap"
        id="sliderWrap"
        @click="mouseClick($event)"
        @mouseup="captureOff"
        @mousedown="captureOn"
        @mousemove="move"
      >
        <div class="slider__bar">
          <div class="slider__progress"></div>
        </div>
        <div 
          class="slider__bar-back"
          :style="'width:' + posHandle + 'px'"
        ></div>
        <div
          class="slider__handle"
          :style="'left:' + posHandle + 'px'"
          @mouseup="captureOff"
        ></div>
        
      </div>
      <template>
        <button
          :disabled="!currentPlayerMove"
          class="slider__btn slider__btn-prev"
          @click="btnPrev"
        >-</button>
        <button
          :disabled="!currentPlayerMove"
          class="slider__btn slider__btn-next"
          @click="btnNext"
        >+</button>
      </template>
    </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'CurrentPlayerMenuRange',

  data: function () {
    return {
      captureToggle: false,
      posLastCursor: 0,
      posCurCursor:0
    }
  },

  computed: {
    ...mapState('poker', [
      'currentPlayerMoveValues',
      'currentPlayer',
      'rangeSlider',
      'currentPlayerMove'
    ]),

    min: function () {
      return this.currentPlayerMoveValues.minValue
    },

    max: function () {
      return this.currentPlayer.chipsCount
    },

    step: function () {
      return this.currentPlayerMoveValues.stepValue
    },

    value: function () {
      return this.currentPlayerMoveValues.minValue
    },

    listValue: function () {
      return this.rangeSlider.listValue
    },

    posHandle: {
      get: function () {
        return this.rangeSlider.posHandle
      },
      set: function (value) {
        this.setRangeSliderPosHandle(value)
      }
    },

    currentIndex: {
      get: function () {
        return this.rangeSlider.currentIndex
      },
      set: function (value) {
        this.setRangeSliderCurrentIndex(value)
      }
    },
    
    lastValue: {
      get: function () {
        return this.rangeSlider.lastValue
      },
      set: function (value) {
        this.setRangeSliderLastValue(value)
      }
    },
    
    coodrLeftWrap: function () {
      return this.rangeSlider.coodrLeftWrap
    }
  },

  methods: {
    ...mapMutations('poker', [
      'setCurrentPlayerRaiseInput',
      'initRangeSlider',
      'setRangeSliderCurrentIndex',
      'setRangeSliderPosHandle',
      'setRangeSliderLastValue'
    ]),

    selected: function (dir){
      this.listValue.forEach((item, index) => {
        if (item.period_start < this.posCurCursor && this.posCurCursor <= item.period_end){
          if (this.lastValue !== item.value) {
            this.setRangeSliderCurrentIndex(index)
            switch(dir) {
              case 'plus': {
                this.setRangeSliderPosHandle(item.period_end)
              }
              break
              case 'minus': {
                this.setRangeSliderPosHandle(item.period_start)
              }
              break
              default: {
                if (this.posHandle > this.posLastCursor) {
                  this.setRangeSliderPosHandle(item.period_start)
                } else {
                  this.setRangeSliderPosHandle(item.period_end)
                }
              }
              break
            }
            this.setCurrentPlayerRaiseInput(item.value)
            this.lastValue = item.value
          }
        }
      })
    },

    mouseClick: function (e) {
      this.posCurCursor = e.clientX - this.coodrLeftWrap
      
      this.listValue.forEach((item) => {
        if (item.period_start < this.posCurCursor && this.posCurCursor <= item.period_end) {
          if (this.lastValue !== item.value) {
            this.selected()
          }
        }
      })
    },

    move: function (e) {
      if (this.captureToggle) {
        this.posCurCursor = e.clientX - this.coodrLeftWrap
        if (this.posLastCursor < this.posCurCursor) {
          this.selected('plus')
        }
        if (this.posLastCursor > this.posCurCursor) {
          this.selected('minus')
        }
        this.posLastCursor = e.clientX - this.coodrLeftWrap
      }
    },

    captureOn: function (e) {
      this.captureToggle = true
      this.posLastCursor = e.clientX - this.coodrLeftWrap
    },

    captureOff: function () {
      this.captureToggle = false
    },

    btnPrev: function () {
      this.currentIndex -= 1

      if (this.currentIndex < 0) {
        this.currentIndex = 0
      }

      this.setRangeSliderPosHandle(this.listValue[this.currentIndex].period_start)
      this.setCurrentPlayerRaiseInput(this.listValue[this.currentIndex].value)
    },

    btnNext: function () {
      this.currentIndex += 1

      if (this.currentIndex > this.listValue.length - 1) {
        this.currentIndex = this.listValue.length - 1
      }

      this.setRangeSliderPosHandle(this.listValue[this.currentIndex].period_end)
      this.setCurrentPlayerRaiseInput(this.listValue[this.currentIndex].value)
    }
  },

  mounted: function () {
    this.initRangeSlider()
  }
}
</script>

<style lang="scss" scoped>
.slider {
  position: relative;
  width: 10rem;
  min-width: 10rem;
  &.disabled {
    opacity: 0;
    pointer-events: none;
  }
  &__wrap {
    position: relative;
    width: 100%;
    position: absolute;
    left: 0;
    bottom: 0;
  }
  &__bar {
    width: 100%;
    height: .1rem;
    padding: .1rem;
    background: rgba(255, 255, 255, 0.4);
    &-back{
      height: 100%;
      position: absolute;
      left: 0;
      bottom: 0;
      background: #D6FF00;
    }
  }
  &__handle {
    position: absolute;
    top: 50%;
    left: 0;
    width: .8rem;
    min-width: .8rem;
    height: .8rem;
    background: #D6FF00;
    border-radius: 50%;
    transform: translate(-50%, -50%);
  }
  &__btn {
    outline: none;
    border: 0;
    cursor: pointer;
    background-color: transparent;
    width: 1.2rem;
    height: 1.2rem;
    background: rgba(255, 255, 255, 0.2);
    border-radius: .1rem;
    color: #fff;
    font-size: .8rem;
    &:disabled {
      img {
        filter: grayscale(1) opacity(.5);
        cursor: not-allowed;
      }
    }
    svg {
      width: 100%;
      height: 1rem;
    }
    img {
      width: 100%;
    }
  }
  &__btn-prev {
    left: -.8rem
  }
  &__btn-next {
    right: -.8rem;
  }
}
.with-buttons {
  padding: 0 1.25rem;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
</style>
