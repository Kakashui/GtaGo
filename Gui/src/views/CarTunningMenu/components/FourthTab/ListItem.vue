<template>
  <div class="list-item">
    <div
      @click="clcItem(parent, item.key)"
      @mouseenter="play({ name: 'hover' , volume: .4 })"
      :class="[{ active: item.key === currentSelectItem }, 'list-item__icon']"
    >
      <img :src="`/img/carTunningMenu/${item.image}`" class="inner">
      <div class="rank" v-if="item.rank">{{ item.rank }}</div>
    </div>
    <span class="list-item__title">{{ loc(item.title) }}</span>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters  } from 'vuex'

export default {
  name: 'ListItem',

  props: ['item', 'parent'],

  computed: {
    ...mapState('carTunningMenu', [
      'currentSelectItem'
    ]),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('carTunningMenu', ['setCurrentTab']),
    ...mapMutations('sounds', ['play']),

    clcItem: function(parent, key) {
        window.mp.trigger('lsCustom:clickTun', parent, key);
      // SOME EVENT (SET FOUR TAB DATA)
    }
  }
}
</script>

<style lang="scss" scoped>
.list-item {
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
    .rank {
      position: absolute;
      bottom: 0.4rem;
      right: 0;
      width: 2.05rem;
      height: 2.05rem;
      background: center / contain no-repeat url("/img/carTunningMenu/itemRankBg.png");
      display: flex;
      align-items: center;
      justify-content: center;
      color: rgba(255, 255, 255, 0.65);
      font-size: 1.2rem;
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
