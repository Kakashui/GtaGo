<template>
  <div 
    :class="[{ 'item-electro': item.key === 'electro' }, { active: item.key === currentFuel.key }, 'fuel-item']"
    @click="chooseFuel(item)"
  >
    <div class="fuel-item__type">{{item.title}}</div>
    <div class="fuel-item__price">$ {{item.cost}},00</div>
    <div class="fuel-item__img"></div>
  </div>
</template>

<script>
import { mapMutations, mapState } from 'vuex'
export default {
  name: 'FuelPage',

  props: {
    item: Object
  },

  computed: {
    ...mapState('gasStation', ['currentFuel'])
  },

  methods: {
    ...mapMutations('gasStation', ['setCurrentFuel']),
    chooseFuel: function(item) {
      this.setCurrentFuel(item)
      this.$emit('setPrice', item.cost)
    }
  }
}
</script>

<style lang="scss" scoped>
.fuel-item{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: flex-end;
  background: #111111;
  padding: .5rem 1rem;
  border: 1px solid #333333;
  box-sizing: border-box;
  border-radius: .5rem;
  position: relative;
  &>div{
    z-index: 2;
    &.fuel-item__img{
      z-index: 1;
    }
  }
  &.active{
    &:after{
      content: '';
      position: absolute;
      top: 0;
      left: 50%;
      transform: translateX(-50%) translateY(-50%);
      width: 1.5rem;
      height: 1.5rem;
      background: #111111;
      border: 1px solid #454545;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/gasStation/done-icon.svg');
    }
  }
  &.item-electro{
    .fuel-item__img{
      background-image: url('/img/gasStation/electro.svg');
    }
    &.active, &:hover{
      .fuel-item__img{
        background-image: url('/img/gasStation/electro-hover.svg');
      }
    }
  }
  &__type{
    text-transform: initial;
    font-family: Arial;
    font-weight: normal;
    font-size: .8rem;
    line-height: .9rem;
    margin-bottom: .3rem;
  }
  &__price{
    font-style: normal;
    font-weight: normal;
    font-size: 1.4rem;
    line-height: 1.7rem;
  }
  &__img{
    width: 100%;
    height: 100%;
    position: absolute;
    left: 0;
    bottom: 0;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center bottom;
    background-image: url('/img/gasStation/fuel.svg');
    transition: .2s;
  }
  &.active, &:hover{
    transition: .2s;
    .fuel-item__img{
      background-image: url('/img/gasStation/fuel-hover.svg');
    }
  }
  &:before{
    content: '';
    width: 5.65rem;
    height: 4.6rem;
    background: rgba(229, 229, 229, 0.3);
    filter: blur(1rem);
    position: absolute;
    left: .5rem;
    top: 2.25rem;
    border-radius: 50%;

  }
  &>div{
    z-index: 2;
    &.img-wrap, &.img-wrap{
      z-index: 1;
    }
  }
}
</style>
