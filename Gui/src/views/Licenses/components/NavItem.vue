<template>
  <div 
    :class="[{'active': item === currentNav}, 'nav-item']"
    @click="setCurrentNav(item)"
  >
    <div
      class="nav-item__icon"
      :style="{backgroundImage: 'url(/img/licenses/'+ currentNavIcon +'.svg)'}"
    ></div>
  </div>
</template>

<script>
export default {
  name: 'NavItem',

  props: {
    item: String,
    setCurrentNav: Function,
    currentNav: String,
  },

  computed: {
    currentNavIcon: function() {
      let icon = this.item
      if (this.item === this.currentNav) {
        return 'active-'+ icon
      } else {
        return icon
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .nav-item{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 2.2rem;
    height: 2.2rem;
    border: 1px solid #DBDBDB;
    background-color: #fff;
    transform-origin: 50% 50%;
    transform: rotate(45deg);
    transition: .3s;
    &:hover{
      background-color: #DBDBDB;
    }
    &__icon{
      transform-origin: 50% 50%;
      transform: rotate(-45deg);
      width: 1.65rem;
      height: 1.65rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
    &.active{
      background-color: #FC5A15;
    }
  }
</style>
