<template>
  <div 
    :class="[{active: item.key === currentTab}, 'navigation-item']"
    @click="setCurrentTab(item.key)"
  >
    <div 
      class="icon"
      :style="{backgroundImage: 'url(/img/cityHallWeb/' + item.icon + '.svg)'}"
    ></div>
    <div class="text">{{loc(item.text)}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'NavItem',
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  props: {
    item: Object,
    currentTab: String,
    setCurrentTab: Function
  }
}
</script>

<style lang="scss" scoped>
.navigation-item{
  display: flex;
  align-items: center;
  font-size: .7rem;
  font-family: Roboto;
  font-style: normal;
  font-weight: bold;
  opacity: .5;
  transition: .2s;
  &:hover, &.active{
    transition: .2s;
  }
  &:hover{
    opacity: .8;
  }
  &.active{
    opacity: 1;
  }
  .icon{
    min-width: 1.3rem;
    width: 1.3rem;
    height: 1.3rem;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    margin-right: .9rem;
  }
  .text{

  }
}
</style>
