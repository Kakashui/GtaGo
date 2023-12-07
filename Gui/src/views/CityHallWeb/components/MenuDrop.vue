<template>
  <div 
    v-if="currentItem"
    :class="[{focused : focusedMenu}, 'menu-drop']"
  >
    <input 
      class="menu-drop__value"
      type="text"
      @click="focusedMenu = !focusedMenu"
      :value='loc(currentItem.text)'
      readonly
    />
    <div 
      class="menu-drop__focused"
      v-if="focusedMenu"
    >
      <div 
        :class="[{active: currentItem.key === item.key}, 'item']"
        v-for="item in dropList"
        :key="item.id"
        @click="selectCurrentItem(item)"
      >{{loc(item.text)}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'MenuDrop',

  props: {
    dropList: Array,
    currentItem: Object,
    setCurrentItem: Function
  },

  data: function() {
    return{
      focusedMenu: false,
    }
  },

  computed:{
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    selectCurrentItem: function(item) {
      this.focusedMenu = false
      this.setCurrentItem(item)
    }
  }

}
</script>

<style lang="scss" scoped>
.menu-drop{
  display: flex;
  align-items: center;
  width: 100%;
  align-items: center;
  position: relative;
  border: 1px solid rgba(70, 85, 95, 0.2);
  box-sizing: border-box;
  border-radius: .3rem;
  height: 3.4rem;
  min-height: 3.4rem;
  margin-bottom: 1.2rem;
  &.focused{
    &:after{
      transform: rotate(-180deg);
      transition: .2s;
    }
  }
  &__value{
    font-family: Arial;
    font-style: normal;
    font-weight: normal;
    font-size: .8rem;
    height: 100%;
    width: 100%;
    color: #46555F;
    outline: none;
    background: transparent;
    border: none;
    padding: 0 1rem;
    z-index: 1;
    cursor: initial;
  }
  &:after{
    content: '';
    width: 1rem;
    height: 1rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/cityHallWeb/arr-drop.svg');
    position: absolute;
    right: 1rem;
    transform-origin: 50% 50%;
    z-index: 0;
    transition: .2s;
  }
  &__focused{
    display: flex;
    flex-flow: column;
    width: 100%;
    position: absolute;
    bottom: -.5rem;
    left: 0;
    transform: translateY(100%);
    background-color: #fff;
    box-shadow: 0 0 1rem rgba(123, 135, 150, 0.2);
    border-radius: .3rem;
    z-index: 2;
    .item{
      display: flex;
      align-items: center;
      padding: 0 1rem;
      text-align: left;
      height: 100%;
      width: 100%;
      font-family: Arial;
      font-style: normal;
      font-weight: normal;
      font-size: .8rem;
      color: rgba(70, 85, 95, 0.5);
      height: 2.5rem;
      min-height: 2.5rem;
      z-index: 1;
      &:hover, &.active{
        background: #F3F5F7;
      }
      &.active{
        color: #E00B29;
      }
    }
  }
}
</style>
