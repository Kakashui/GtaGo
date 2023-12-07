<template>
  <div class="param-item">
    <div 
      class="param-item-image"
      :style="{backgroundImage: `url(/img/sellCar/${item.name}.png)`}"
    ></div>
    <div :class="[{'param-item_info--max': item.currentLevel >= item.maxLevel}, 'param-item_info']">
      <div class="param-item_info-values">
        <div class="param-item_info-values-desc">{{item.paramName}}</div>
        <div class="param-item_info-values-label">{{tuningName}}</div>
      </div>
      <div class="param-item__progress-bar">
        <div 
          class="param-item__progress-bar-item"
          v-for="progress in item.currentLevel"
          :key="progress"
        ></div>
        <div 
          class="param-item__progress-bar-back"
          v-for="back in (item.maxLevel - item.currentLevel)"
          :key="10*back"
        ></div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'PrarmItem',

  props: {
    item: Object
  },

  computed: {
    tuningName: function() {
      if (this.item.currentLevel <= 1)
        return 'Stock';
      else if (this.item.currentLevel >= this.item.maxLevel)
        return 'Max Stage'
      else
        return `Stage ${this.item.currentLevel}`
    }
  }
}
</script>

<style lang="scss" scoped>
.param-item{
  display: flex;
  align-items: flex-end;
  margin-bottom: 1.25rem;
  width: 22rem;
  position: relative;
  &:before{
    content: '';
    width: .7rem;
    height: .7rem;
    background: #B6D300;
    box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
    border-radius: 1px;
    position: absolute;
    left: -2.25rem;
    top: 50%;
    transform: translateX(-50%) translateY(-50%) rotate(45deg);
  }
  &:last-child{
    margin-bottom: 0;
  }
  &-image{
    margin-right: 1rem;
    min-width: 4rem;
    width: 4rem;
    height: 4rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  &_info{
    width: 100%;
    display: flex;
    flex-direction: column;
    &--max{
      .param-item_info-values-label{
        color: #B6D300;
        text-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
      }
      .param-item__progress-bar-item{
        background-color: #B6D300;
        box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
      }
    }
    &-values{
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      margin-bottom: .5rem;
      &-desc{
        font-weight: normal;
        font-size: 1.7rem;
        line-height: 2.05rem;
      }
      &-label{
        font-weight: normal;
        font-size: 1.3rem;
        line-height: 1.55rem;
        letter-spacing: 0.03em;
      }
    }
  }
  &__progress-bar{
    width: 100%;
    height: .4rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
    &-item, &-back{
      width: 17.6%;
      height: 100%;
      border-radius: 1px;
    }
    &-item{
      background: #FFFFFF;
    }
    &-back{
      background: rgba(255, 255, 255, 0.2);
    }
  }
}
</style>
