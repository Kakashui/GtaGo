<template>
  <div class="interaction-menu">
    <div class="interaction-menu__items">
      <div
        v-for="(item, index) in items"
        :key="index"
        class="item"
        :style="{ '--itemArea': 'item' + (index + 1) }"
        @click="someEvent(item.key)"
      >
        <div class="item__main">
          <div :style="{ 'webkit-mask': `center / cover no-repeat url(img/interactionMenu/icons/${item.key}.svg)` }" class="icon"></div>
          <span class="text">{{ loc(item.title) }}</span>
          <!-- <span class="text">{{ loc(item.title) }}</span> -->
        </div>
      </div>
      <div class="item item-empty"></div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import { mapGetters } from 'vuex'

export default {
  name: 'InteractionMenu',

  computed: {
    ...mapState('interactionMenu', ['items']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    someEvent: function(key) {
      window.mp.trigger('intMenu:selected', key)
    }
  }
}
</script>

<style lang="scss" scoped>
.interaction-menu {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  &__items {
    display: grid;
    grid-template-columns: repeat(5, 6.6rem);
    grid-auto-rows: 6.1rem;
    transform: skew(-5deg, 0);
    grid-template-areas: 
      "item21 item13 item9 item14 item22"
      "item20 item5 item1 item6 item15"
      "item12 item4 item0 item2 item10"
      "item19 item8 item3 item7 item16"
      "item24 item18 item11 item17 item23"
    ;
    .item {
      background: center / cover no-repeat url("/img/interactionMenu/itemBg.png");
      grid-area: var(--itemArea);
      &:hover {
        background: center / cover no-repeat url("/img/interactionMenu/itemBgActive.png");
      }
      &-empty {
        pointer-events: none;
        background: none;
        grid-area: item0;
      }
      &__main {
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        flex-direction: column;
        padding: 0.95rem 0 0 0;
        transform: skew(5deg, 0);
        .icon {
          width: 2.1rem;
          height: 2.1rem;
          margin: 0 0 0.5rem 0;
          background: #C0E604;
        }
        .text {
         text-transform: uppercase;
          font-size: 0.8rem;
          line-height: 0.8rem;
          text-align: center;
          color: #fff;
          width: 3rem;
          text-transform: uppercase;
          font-weight: bold;
        }
      }
    }
  }
}
</style>
