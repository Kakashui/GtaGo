<template>
  <div class="color-tab">
    <div 
      :class="[{active: item === currentColor}, 'color-tab__item']"
      v-for="item in colorList"
      :key="item.key"
      :style="{backgroundColor: item}"
      @click="setCurrentColor(item)"
    ></div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'ColorTab',

  props: {
    colorList: Array,
    currentColor: String
  },

  computed: {
    ...mapState('familyMenu/controlPage', ['chatOptions'])
  },

  methods: {
    setCurrentColor: function(value) {
      this.$emit('setCurrentColor', value)
    }
  }
}
</script>

<style lang="scss" scoped>
.color-tab{
  display: grid;
  grid-template-columns: repeat(11, 2rem);
  grid-gap: .6rem;
  grid-auto-rows: 2rem;
  &__item{
    border: .1rem solid rgba(255, 255, 255, 0.3);
    border-radius: .5rem;
    transition: .2s;
    &:hover{
      box-shadow: 0 0 1rem rgba(255, 255, 255, 0.5);
    }
    &.active, &:hover{
      border-color: #fff;
      transition: .2s;
    }
  }
}
</style>
