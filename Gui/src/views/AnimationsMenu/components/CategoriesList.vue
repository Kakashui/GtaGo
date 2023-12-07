<template>
  <div class="categories-list">
    <div 
      v-for="item in categories"
      :key="item.id"
      :class="[{active: item.key === currentAnimations.key}, 'categories-list__item']"
      @click="setCurrentAnimationsList(item.key)"
    >
      <span 
        class="icon" 
        :style="{ backgroundImage: 'url(/img/animationsMenu/nav-icon-' + item.key + '.svg)' }"
      ></span>
      <span class="text">{{loc(item.name)}}</span>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'

export default {
  name: 'CategoriesList',

  computed: {
    ...mapState('animationsMenu', ['categories', 'currentAnimations']),
    ...mapGetters('localization', ['loc']),
    // ...mapGetters('localization',['loc']),
  },

  methods: {
    ...mapMutations('animationsMenu', ['setCurrentAnimationsList']),
    getAnimationsList: function(array, key) {
      this.setCurrentAnimationsList({array: array, key: key})
    }
  }
}
</script>

<style lang="scss" scoped>
.categories-list{
  display: flex;
  align-items: center;
  justify-content: flex-start;
  min-height: 3.9rem;
  &__item{
    display: flex;
    align-items: center;
    padding: .35rem .75rem 2.2rem 1.8rem;
    position: relative;
    opacity: .65;
    transition: .2s;
    &:last-child{
      margin-right: 0;
    }
    &.active, &:hover{
      opacity: 1;
      transition: .2s;
      &:after{
        content: '';
        width: 100%;
        height: .1rem;
        position: absolute;
        bottom: 0;
        left: 0;
        background: linear-gradient(90deg, #C1E704 0%, rgba(193, 231, 4, 0) 89.45%);
        filter: drop-shadow(0px 4px 6px rgba(193, 231, 4, 0.45));
        transform: matrix(-1, 0, 0, 1, 0, 0);
      }
    }
    .icon{
      width: 1.3rem;
      height: 1.3rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      margin-right: .1rem;
    }
    .text{
      white-space: pre;
    }
  }
}
</style>
