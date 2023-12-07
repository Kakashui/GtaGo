<template>
  <div class="clothing-store">
    <div class="clothing-store__circle">
      <div
        v-for="(type, index) in types"
        :class="[{ active: index === currentTypeId}, 'circle__item']"
        :key="index"
        @click="setType(index)"
      >
        <img
          :src="`img/clothingStore/icon_${type.key}.svg`"
          :alt="type.key"
          class="item__icon"
        >
      </div>
      <div class="circle__inner-circle">
        <span class="inner-circle__title"><span>{{loc('clth_4')}}:</span>  <span>{{rangeModel + 1}}/{{colors.length}}</span></span>    
        <Slider 
          :min="minRangeModelValue"
          :max="colors.length - 1"
          :step="stepRangeModelValue"
          v-model="rangeModel"
        />
      </div>
    </div>
    <button class="clothing-store__btn" @click="buy">
      <div class="btn__circle">
        <img
          src="/img/clothingStore/icon_money.svg"
          alt="money"
          class="circle__icon"
        >
      </div>
      <span class="btn__title">{{loc('clth_1')}}</span>
    </button>
    <div class="clothing-store__stock">
      <div class="stock__list">
        <div
          :class="[{ active: index === currentItemId }, 'list__item']"
          v-for="(item, index) in items"
          :key="'cl_' + index"
          @click="setItem(index)"
        >
          <span class="item__title">{{ getClothingName(item) }}</span>
          <span class="item__price">{{ getPrice(item) }}$</span>
        </div>
      </div>
      <div class="stock__total">
        <span class="total__title">{{loc('clth_3')}}</span>
        <span class="total__title">{{currentPrice}}$</span>
      </div>
    </div>
    <button @click="close" class="clothing-store__exit">
      <img
        class="exit__icon"
        src="/img/clothingStore/icon_exit.svg"
        alt="exit"
      >
      <span class="exit__title">{{loc('clth_2')}}</span>
    </button>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import Slider from 'vue-slider-component'

export default {
  name: 'ClothingStore',

  data: function () {
    return {
      minRangeModelValue: 0,
      stepRangeModelValue: 1,
      rangeModel: 0
    }
  },
  
  computed: {
    ...mapState('clothingStore', [
      'types',
      'currentTypeId',
      'price',
      'currentColorId',
      'currentItemId',
      'gender',
      'configs'
    ]),
    ...mapGetters('localization', ['loc']),
    items(){
      return this.types[this.currentTypeId].config[this.gender]
    },
    colors(){
      return this.currentItem.Colors;
    },

    currentItem(){
      return this.items[this.currentItemId];
    },

    currentPrice(){
      return (this.currentItem.Price * this.price / 100).toFixed();
    }
  },
  methods: {
    ...mapMutations('clothingStore', [
      'setCurrentTypeId',
      'setCurrentColorId',
      'setCurrentItemId'
    ]),
    getClothingName(item){
      const variation = this.currentTypeId == 2 ? item.Top : item.Variation
      return this.$store.getters['inventory/getClothingName']([this.gender, this.currentTypeId, variation]);
    },
    getPrice(item){
      return (item.Price * this.price / 100).toFixed();
    },
    setType(value) {
      this.setCurrentTypeId(value);
      this.set
      this.setItem(0);
    },

    componentVariation(clotheId, vaiation, color){
      window.mp.trigger("componentVariation", clotheId, vaiation, color)
    },
    propVariation(clotheId, vaiation, color){
      window.mp.trigger("propVariation", clotheId, vaiation, color)
    },
    setItem(value) {
      this.setCurrentItemId(value);
      this.rangeModel = 0;
      this.updateClothes();
    },
    updateClothes(){
      let torso;
      switch(this.currentTypeId){
        case 0:
          this.propVariation(0, this.currentItem.Variation, this.colors[this.rangeModel]);
          break;
        case 1:          
          this.componentVariation(11, this.currentItem.Variation, this.colors[this.rangeModel]);
          torso = this.configs['validTorsos'][this.gender][this.currentItem.Variation]
          if(torso) this.componentVariation(3, torso, 0);
          break;
        case 2:
          this.componentVariation(11, this.currentItem.Top, this.colors[this.rangeModel]);
          torso = this.configs['validTorsos'][this.gender][this.currentItem.Top]
          if(torso != undefined) this.componentVariation(3, torso, 0);          
          break;
        case 3:
          this.componentVariation(4, this.currentItem.Variation, this.colors[this.rangeModel]);
          break;
        case 4:
          this.componentVariation(6, this.currentItem.Variation, this.colors[this.rangeModel]);
          break;
        case 5:
          this.componentVariation(3, this.configs.correctGloves[this.gender][this.currentItem.Variation][15], this.colors[this.rangeModel]);
          break;
        case 6:
          this.propVariation(6, this.currentItem.Variation, this.colors[this.rangeModel]);
          break;
        case 7:
          this.propVariation(1, this.currentItem.Variation, this.colors[this.rangeModel]);
          break;
        case 8:
          this.componentVariation(7, this.currentItem.Variation, this.colors[this.rangeModel]);
          break;
      }
    },
    buy() {
      const variation = this.currentTypeId == 2 ? this.currentItem.Top : this.currentItem.Variation;
      window.mp.trigger('buyClothes', this.currentTypeId, variation, this.colors[this.rangeModel]);
    },
    close() {
      window.mp.trigger('closeClothes');
    },
    mounted() {
      this.updateClothes();
    },
  },
  components:{
    Slider
  },
  watch:{
    rangeModel(){
      this.updateClothes();
    }
  }
}
</script>

<style lang="scss">
.clothing-store {
  position: relative;
  display: flex;
  justify-content: space-around;
  width: 100%;
  text-transform: uppercase;
  letter-spacing: .04rem;
  &__circle {
    width: 40rem;
    height: 40rem;
    border-radius: 50%;
    background: linear-gradient(90deg, rgba(255, 255, 255, 0.45) -3%, rgba(0, 0, 0, 0) 33%);
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    .circle__item {
      background-color: rgba($color: #fff, $alpha: .6);
      width: 4.8rem;
      height: 4.8rem;
      border-radius: 50%;
      position: absolute;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: all .3s;
      &:hover{
        background-color: rgba($color: #fff, $alpha: .7);
        transform: scale(1.1);
      }
      &.active {
        background-image: url('../../../public/img/common/textures/green_texture_long-112x300.jpg');
        background-size: cover;
      }
      &:nth-of-type(1) { top: 1%; left: 46%; }
      &:nth-of-type(2) { top: 5%; left: 30%; }
      &:nth-of-type(3) { top: 14%; left: 16%; }
      &:nth-of-type(4) { top: 28%; left: 6%; }
      &:nth-of-type(5) { top: 44%; left: 3%; }
      &:nth-of-type(6) { top: 60%; left: 6%; }
      &:nth-of-type(7) { top: 74%; left: 16%; }
      &:nth-of-type(8) { top: 83%; left: 30%; }
      &:nth-of-type(9) { top: 88%; left: 46%; }
    }
    .circle__inner-circle {
      width: 26rem;
      height: 26rem;
      border-radius: 50%;
      background: linear-gradient(90deg, rgba(255, 255, 255, 0.47) -3%, rgba(0, 0, 0, 0) 33%);
      position: relative;
      display: flex;
      align-items: center;
      .inner-circle__title {
        color: #fff;
        position: absolute;
        top:-6.5rem;
        left: 37rem;
        width: 5rem;
        display: flex;
        justify-content: space-between;
      }
      //------------------------------------------
      //          SLIDER
      //------------------------------------------
        
        .vue-slider{
          position: absolute;
          top: -9rem;
          left: 22rem;
          &-disabled {
            opacity: 0.5;
            cursor: not-allowed;
          }
          &-rail {
              width: 20rem;
              margin: auto;
              background-color: rgba(#fff, 1);
              border-radius: 15px;
              margin: 1rem auto;
          }
          &-process {
            background: linear-gradient(270deg, #9BAE22 0%, rgba(#9BAE22, .3) 100%);
            border-radius: 15px;
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
              background-color: rgba(0, 0, 0, 1);
            }
            &-label {
              font-size: 14px;
              white-space: nowrap;
            }
          }
          &-dot{
            &-handle {
              cursor: pointer;
              width: 1.4rem;
              height: 1.4rem;
              border-radius: 50%;
              margin-top: -.4rem;
              background: #9BAE22;
              border: 2px solid #FFF;
              box-sizing: border-box;
              color: #fff;
            }
            &-tooltip{
              &-inner {
                opacity: 0;
                // background-color: #9BAE22;
                // border-radius: .4rem;
                // width: 1.7rem;
                // height: 1.7rem;
                // border: 1px solid #FFF;
                // padding: .5rem;
                // display: flex;
                // align-items: center;
                // justify-content: center;
                // margin-left: .5rem;
                // margin-bottom: .5rem;
                // color: #fff;
                // &-bottom::after {
                //   background-color: #9BAE22;
                //   border: 1px solid #FFF;
                // }
                // &-left::after {
                //   background-color: #9BAE22;
                //   border: 1px solid #FFF;
                // }
                // &-right::after {
                //   background-color: #9BAE22;
                //   border: 1px solid #FFF;
                // }
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
        //-----------------------------------------------------
        //      slider
        //-----------------------------------------------------
    }
  }
  &__btn {
    position: absolute;
    bottom: 0;
    left: 42%;
    background-color: transparent;
    background: linear-gradient(270deg, #71B020 0%, #B3D028 135%);
    width: 10rem;
    height: 3.2rem;
    border-radius: .6rem;
    transform: skew(15deg);
    transition: all .2s;
    &:hover {
      transform: skew(15deg) scale(1.05);
    }
    .btn__circle {
      transform: skew(-15deg) translate(-50%, -50%);
      width: 3.5rem;
      height: 3.5rem;
      border-radius: 50%;
      background: linear-gradient(96deg, #7EBD2E -1%, #B2CF1B 87%);
      display: flex;
      align-items: center;
      justify-content: center;
      position: absolute;
      top: 50%;
      left: 0;
      &::after {
        content: '';
        width: 108%;
        height: 108%;
        border-radius: 50%;
        border: 1px solid #B3D028;
        position: absolute;
      }
      .circle__icon {
        width: 65%;
        height: auto;
      }
    }
    .btn__title {
      display: inline-block;
      transform: skew(-15deg);
      color: #fff;
      margin: 0 0 0 1.5rem;
      font-weight: bold;
      letter-spacing: .05rem;
    }
  }
  &__stock {
    padding: 2rem 1rem 1rem 2rem;
    background-image: url('../../../public/img/clothingStore/stock_bg.png');
    background-size: cover;
    width: 21rem;
    height: 37rem;
    box-shadow: 1rem 1rem 2rem 0 #000;
    border-radius: 1rem;
    .stock__list {
      display: flex;
      flex-direction: column;
      height: 28rem;
      padding: 0 .5rem 0 0;
      overflow-y: auto;
      appearance: none;
      &::-webkit-scrollbar-track {
        background-color: transparent;
      }
      &::-webkit-scrollbar {
        width: 2px;
        background-color: transparent;
      }
      &::-webkit-scrollbar-thumb {
        background-color: #fff;
        height: 33.3%;
      }
      .list__item {
        width: 100%;
        display: flex;
        justify-content: space-between;
        margin: .5rem 0;
        &.active {
          color: #fff;
        }
        &:hover{
          color: rgba(#fff, .5);;
        }
      }
    }
    .stock__total {
      margin: 1.5rem 0 0 0;
      border-top: 1px solid #000000;
      padding: 1.5rem 0 0 0;
      color: #fff;
      display: flex;
      justify-content: space-between;
    }
  }
  &__exit {
    background: transparent;
    position: absolute;
    top: 0;
    left: 5%;
    display: flex;
    align-items: center;
    .exit__icon {
      width: 2rem;
      height: auto;
    }
    .exit__title {
      color: #F15454;
    }
  }
}
</style>
