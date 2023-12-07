<template>
  <div class="enter-tab tab-wrap">
    <div class="enter-tab__icon"></div>
    <div class="enter-tab__prompt">internal portal of parliament</div>
    <div class="enter-tab__title">Congress</div>
    <div 
      class="enter-tab__btn btn"
      @click="setCurrentTab('MainTab')"
    >Enter</div>
  </div>
</template>

<script>
export default {
  name: 'EnterTab',

  props: {
    setCurrentTab: Function
  }
}
</script>

<style lang="scss" scoped>
.enter-tab{
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: center;
  background-size: contain;
  background-position: center;
  background-repeat: no-repeat;
  background-image: url('/img/parliamentPortal/enter-bg.png');
  padding-bottom: 6rem;
  &__icon{
    width: 1.7rem;
    height: 1.7rem;
    position: relative;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-image: url('/img/parliamentPortal/icon-prompt.svg');
    margin-bottom: 1.2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    &:after{
      content: '';
      width: 3.3rem;
      height: .2rem;
      background: #3FA8E3;
      position: absolute;
      bottom: -.5rem;
      transform: translateY(100%);
    }
  }
  &__prompt{
    font-family: Merriweather;
    font-style: normal;
    font-weight: normal;
    font-size: 1rem;
    line-height: 130%;
    color: #46555F;
    margin-bottom: .5rem;
  }
  &__title{
    font-family: Merriweather;
    font-style: normal;
    font-weight: bold;
    font-size: 5rem;
    line-height: 130%;
    color: #46555F;
    margin-bottom: 1rem;
  }
  &__btn{
    padding: 1rem 2.3rem;
    font-family: Roboto;
    font-style: normal;
    font-weight: bold;
    font-size: 1rem;
    color: #FFFFFF;
    background: linear-gradient(0deg, #3FA8E3, #3FA8E3), #E00B29;
    border-radius: .2rem;
    &:after{
      content: '';
      margin-left: 1.15rem;
      width: .85rem;
      height: .4rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/parliamentPortal/icon-enter.svg');
    }
  }
}
</style>
