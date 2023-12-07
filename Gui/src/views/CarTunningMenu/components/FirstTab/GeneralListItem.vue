<template>
  <div class="first-item">
    <div
      class="first-item__icon"
      @mouseenter="play({ name: 'hover' , volume: .4 })"
      @click="setTab('SecondTab', item.key)"
    >
      <img :src="`/img/carTunningMenu/firstTab/${item.key}.png`" class="inner">
    </div>
    <span class="first-item__title">{{ loc(item.title) }}</span>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'GeneralListItem',

  props: ['item'],

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('carTunningMenu', ['setCurrentTab']),
    ...mapMutations('sounds', ['play']),

    setTab: function(value, key) {
      this.setCurrentTab(value);
      window.mp.trigger('lsCustom:openListCat', key);
    }
  }
}
</script>

<style lang="scss" scoped>
.first-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  transform: rotate(90deg) scale(1, -1);
  width: 7rem;
  height: 7rem;
  &__icon:hover + &__title {
    color: #fff;
  }
  &__icon {
    min-width: 6rem;
    min-height: 6rem;
    margin: 0 0 0.5rem 0;
    background: center / contain no-repeat url("/img/carTunningMenu/itemIconBg.png");
    position: relative;
    &:hover {
      background: center / contain no-repeat url("/img/carTunningMenu/itemIconBg1.png");
      &::after {
        transform: scale(1);
        opacity: 1;
      }
    }
    &::after {
      content: "";
      width: 100%;
      height: 100%;
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      left: 0;
      margin: auto;
      background: center / contain no-repeat url("/img/carTunningMenu/itemIconBg2.png");
      opacity: 0;
      transform: scale(0.9);
      transition: all 0.2s;
    }
    .inner {
      position: absolute;
      transform: translate(-50%, -50%);
      left: 50%;
      top: 50%;
      width: 3.5rem;
      height: 3.5rem;
    }
  }
  &__title {
    color: rgba(255, 255, 255, 0.45);
    width: 6rem;
    text-transform: uppercase;
    text-align: center;
    font-size: 1.2rem;
    transition: all 0.2s;
  }
}
</style>
