<template>
  <div :class="[{ active: category.type === currentType }, 'categories__item']" @click="set(category)">
    <img
      :src="`/img/barbershop/${category.type}.svg`"
      :alt="category.type"
      class="item__icon"
    >
    <span class="item__title">{{ loc(category.title) }}</span>
  </div>
</template>

<script>
import { mapGetters} from 'vuex'
export default {
  name: 'CategoriesItem',

  props: {
    category: Object,
    currentType: String
  },
  computed:{
    ...mapGetters('localization', ['loc']),
  },
  methods: {
    set: function(item) {
      this.$emit('setCurrentCategory', item)
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes slide {
  0% {
    opacity: 0;
    transform: scale(.5);
  }
  80% {
    opacity: 0.8;
    transform: scale(1.1);
  }
  100% {
    opacity: 1;
    transform: scale(1);
  }
}
.categories__item {
  width: 5.4rem;
  height: 5.4rem;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: rgba(255, 255, 255, 0.7);
  border-radius: 50%;
  margin: .5rem 0 0 0;
  position: relative;
  &.active {
    box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.6);
    // transition: all .15s;
    background: center / cover no-repeat url('/img/common/textures/texture_button_go_now.jpg');
    .item__icon {
      filter: invert(1);
    }
    .item__title {
      display: block;
      box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.6);
    }
  }
  &:hover {
    box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.6);
    transition: all .15s;
    background: center / cover no-repeat url('/img/common/textures/texture_button_go_now.jpg');
    .item__icon {
      filter: invert(1);
    }
    .item__title {
      display: block;
      box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.6);
    }
  }
  .item__icon {
    width: 3rem;
    height: 3rem;
  }
  &:first-child {
    margin: 0;
    transform: translateX(-7rem);
  }
  &:nth-child(2), &:nth-child(7) {
    transform: translateX(-3.5rem);
    margin: .1rem 0 0 0;
  }
  &:nth-child(4) {
    margin: .5rem 0;
  }
  &:nth-child(3), &:nth-child(6) {
    transform: translateX(-1rem);
    margin: .5rem 0 .5rem 0;
  }
  &:last-child {
    transform: translateX(-7rem);
    margin: 0;
  }
  .item__title {
    position: absolute;
    right: 110%;
    top: 10%;
    background: rgba(255, 255, 255, 0.49);
    border-radius: .25rem;
    font-size: .8rem;
    color: #151515;
    padding: .4rem .7rem;
    font-weight: bold;
    display: none;
    animation: slide .15s;
  }
}
</style>
