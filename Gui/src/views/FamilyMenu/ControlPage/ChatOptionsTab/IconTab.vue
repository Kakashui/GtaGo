<template>
  <div class="icon-tab">
    <div 
      :class="[{active: item === currentIcon}, 'item']"
      v-for="item in 20"
      :key="item.id"
      @click="setCurrentIcon(item)"
    >
      <div 
        class="icon"
        :style="{backgroundImage: 'url(/img/familyMenu/chatOptions/icon-' + item + '.svg)'}"
      ></div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'IconTab',

  props: {
    currentIcon: Number
  },
  
  methods: {
    setCurrentIcon: function(value) {
      this.$emit('setCurrentIcon', value)
    }
  },

  computed: {
    ...mapState('familyMenu/controlPage', ['chatOptions'])
  },
}
</script>

<style lang="scss" scoped>
.icon-tab{
  display: grid;
  width: 100%;
  grid-template-columns: repeat(4, 1fr);
  grid-auto-rows: 2.5rem;
  grid-column-gap: .6rem;
  grid-row-gap: .8rem;
  .item{
    border: .1rem solid rgba(255, 255, 255, 0.6);
    border-radius: .5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: .2s;
    &:hover, &.active{
      border-color: #DCFF00;
      transition: .2s;
    }
    .icon{
      width: 1.2rem;
      height: 1.2rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
  }
}
</style>
