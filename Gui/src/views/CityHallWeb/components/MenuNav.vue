<template>
  <div class="menu-nav">
    <div 
      :class="[{active: item.key === currentPage}, 'menu-nav__item']"
      v-for="item in list"
      :key="item.id"
      @click="setCurrentPage(item.key)"
    >
      <div class="text">{{loc(item.text)}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'MenuNav',

  props: {
    list: Array,
    currentPage: String,
    setCurrentPage: Function
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.menu-nav{
  display: flex;
  flex-flow: column;
  width: fit-content;
  padding-top: 1.3rem;
  &__item{
    display: flex;
    align-items: center;
    justify-content: flex-start;
    margin-bottom: 2rem;
    margin-left: 1rem;
    position: relative;
    &:before, &:after{
      content: '';
      position: absolute;
      height: 3.5rem;
      opacity: 0;
      z-index: 0;
    }
    &:before{
      width: calc(100% + (1.5rem));
      left: -1rem;
      background: #F3F5F7;
    }
    &:after{
      right: -.5rem;
      width: 1.3rem;
      transform: translateX(100%);
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center left;
      background-image: url('/img/cityHallWeb/nav-triangle.svg');
    }
    &:last-child{
      margin-bottom: 0;
    }
    &:hover{
      .text{
        color: rgba(70, 85, 95, 0.8);
      }
    }
    &.active, &.active:hover{
      .text{
        color: #46555F;
      }
    }
    &.active{
      &:before, &:after{
        opacity: 1;
      }
      .text{
        border-color: #E00B29;
      }
    }
    .text{
      font-family: Roboto;
      font-style: normal;
      font-weight: 500;
      font-size: .8rem;
      color: rgba(70, 85, 95, 0.5);
      text-transform: uppercase;
      padding-left: .7rem;
      border-left: .1rem solid transparent;
      text-align: left;
      z-index: 1;
    }
  }
}
</style>
