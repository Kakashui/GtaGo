<template>
  <div 
    :class="[{active: currentCar && item.key === currentCar.key}, 'car-item']"
    @click="setCurrentCar(item.key)"
  >
    <div class="car-item__model">{{item.carName}}</div>
    <div class="car-item__number">{{item.carNumber}}</div>
    <div class="btn icon"></div>
  </div>
</template>

<script>
export default {
  name: 'CarItem',

  props: {
    item: Object,
    cars: Array,
    currentCar: Object
  },

  methods: {
    setCurrentCar: function(key) {
      this.$emit('setCurrentCar', key)
    }
  },
}
</script>

<style lang="scss" scoped>
.car-item{
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: .8rem 0;
  min-height: 2.8rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.2);
  width: 100%;
  position: relative;
  &__number, &__model{
    font-size: 1.2rem;
    line-height: 1.2rem;
    transition: .2s;
    z-index: 2;
  }
  &__model{
    font-weight: bold;
  }
  .icon{
    display: none;
    position: absolute;
    transform: translateX(100%);
    right: 0;
    top: .65rem;
    width: 1.9rem;
    height: 1.2rem;
    background-image: url('/img/familyMenu/btn-options.svg');
    background-position: center right;
  }
  &:hover{
    .icon{
      display: flex;
    }
    .car-item__number, .car-item__model{
      color: #DCFF00;
      transition: .2s;
    }
  }
  &.active{
    &:before{
      content: '';
      height: 100%;
      width: calc(100% + (2.2rem));
      position: absolute;
      left: -2.2rem;
      top: 0;
      background: linear-gradient(90deg, #DCFF00 0%, rgba(48, 53, 57, 0) 100%);
      border-radius: 2rem 0 0 2rem;
      z-index: 4;
    }
    &:hover{
      .icon{
        display: none;
      }
      .car-item__model, .car-item__number{
        color: #fff;
      }
    }
    .car-item__model, .car-item__number{
      z-index: 5;
      color: #fff;
    }
  }
}
</style>
