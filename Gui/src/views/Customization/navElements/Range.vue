<template>
    <div class="customiztion-nav-range">
        <Slider div class="customiztion-nav-range"
          :min="itemData.min"
          :max="itemData.max"
          :interval="itemData.step"
          v-model="itemData.value"
          @change="onChange"
        />
        <div class="customiztion-nav-range_value">{{itemData.value}}</div>
    </div>
</template>

<script>
import Slider from 'vue-slider-component'
export default {   
    props:{
      itemData: Object
    },
    methods:{
      onChange(){
        window.mp.trigger("customization:update", this.itemData.tag, this.itemData.value);
      }
    },    
    components:{
        Slider
    }
}
</script>

<style lang="scss">
.customiztion-nav-range{
    display: flex;
    align-items: center;
    justify-content: space-between;
    position: relative;
    margin: .25rem 0;
    &_value{
      color: #B6D300;
    }
    .vue-slider{
        width: 14rem;
          &-disabled {
            opacity: 0.5;
            cursor: not-allowed;
          }
          &-rail {
            margin: auto;
            background: none;
            background-color: rgba(#fff, .1);
            width: 13rem;
          }
          &-process {
            background: none;
          }
          &-mark{
            z-index: 4;
            &:first-child .vue-slider-mark-step, .vue-slider-mark:last-child .vue-slider-mark-step {
              display: none;
            }
            &-step {
              width: 100%;
              height: 100%;
              border-radius: 50%;
            }
          }
          &-dot{
            &-handle {
              cursor: pointer;
              width: .8rem;
              height: .8rem;
              border-radius: 50%;
              margin-top: -.05rem;
              background: #B6D300;
              border: none;
              box-shadow: none;
              box-sizing: border-box;
            }
            &-tooltip{
              &-inner {
                opacity: 0;
              }
              &-wrapper {
                opacity: 0;
                transition: all 0.3s;
              }
              &-wrapper-show {
                opacity: 1;
              }
            }
          }
        }
}
</style>