<template>
  <div class="maskshop">
    <NavButtons />
    <div class="maskshop-left">
      <div class="maskshop-left_subtitle">Магазин масок</div>
      <div class="maskshop-left_title">Maskshop</div>
      <div class="maskshop-left_list">
        <MaskItem v-for="(item, index) in configs" :key="index" :item="item" :price="getPrice(index)" :selected="current === index" @onselect="select(index)"/>
      </div>
    </div>
    <div class="maskshop-right">
      <Colors :item="configs[current]" :price="getPrice(current)" @onSelectColor="selectColor" @onBuyMask="buy"/>
    </div>
    <img src="/img/maskShop/mask_bg.png" alt="mask" class="maskshop_bg">
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import MaskItem from './Mask.vue'
import Colors from './Colors.vue'
import NavButtons from './NavButtons.vue'

export default {
  name: 'MaskShop',
  data() {
    return {
      current: 0
    }
  },

  components: {
    MaskItem,
    Colors,
    NavButtons
  },

  computed: {
    ...mapState('maskShop', [
      'price',
      'configs'
    ]),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('maskShop', [
      'incIndexS',
      'decIndexS'
    ]),
    getPrice(index){
      return (this.configs[index].Price  *  +this.price / 100).toFixed();
    },
    select(index){
      this.current = index;
    },

    close(){
      window.mp.trigger('closeMasks');
    },
    selectColor(color){
      window.mp.trigger('setMask', this.configs[this.current].Variation, color);
    },
    buy(color) {
      window.mp.trigger('buyMasks', this.configs[this.current].Variation, color);
    }
  }
}
</script>

<style lang="scss" scoped>
.maskshop {
  position: relative;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  color: #fff;
  text-transform: uppercase;
  &-left{
    padding-left: 2rem;
    padding-top: 2rem;
    width: 45rem;
    height: 100%;
    background: url(/img/maskShop/bgl.png) no-repeat center;
    background-size: cover;
    position: relative;
    &_title{
      font-size: 5rem;
      line-height: 4rem;
    }
    &_subtitle{
      font-size: 1.2rem;
      letter-spacing: .06rem;
      margin-left: .2rem;
    }
    &_list{
      margin-top: 2rem;
      width: 100%;
      height: 42rem;
      direction: rtl;
      overflow-y: scroll;      
      &::-webkit-scrollbar {
          width: .2rem;
      }
          &::-webkit-scrollbar-track {
          border: solid 0 transparent;
          margin: .1rem;
      }
          &::-webkit-scrollbar-thumb {
          border: solid 0 transparent;      
          background-color: rgba(#fff,.1);
      }
    }
  }
  &_bg{
    position: absolute;
    right: 0;
    pointer-events: none;
    bottom: 0;
    height: 20rem;
  }
  
}
</style>
