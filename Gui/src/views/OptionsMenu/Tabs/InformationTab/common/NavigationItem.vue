<template>
  <div class="item">
    <div class="item-exist" v-if="Object.keys(item).length">
      <div class="item-exist__desc">
        <div class="title">{{ loc(item.title) }}</div>
        <div class="subtitle">{{ loc(item.subtitle) }}</div>
      </div>
      <button class="item-exist__btn"></button>
    </div>
    <div class="item-empty" v-else>Soon</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'NavigationItem',

  props: {
    item: Object
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.item {
  width: 22rem;
  height: 5rem;
  background: center / cover no-repeat url("/img/optionsMenu/informationTab/navItemBg.png");
  &:not(:last-child) {
    margin: 0 0 0.5rem 0;
  }
  &-exist {
    width: 100%;
    height: 100%;
    background: rgba(255, 255, 255, 0.05);
    border: 1px solid rgba(255, 255, 255, 0.7);
    box-shadow: inset 0px 0px 1.5rem rgba(255, 255, 255, 0.15);
    padding: 0 1.25rem 0 1.75rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
    &__desc {
      font-weight: 300;
      display: flex;
      flex-direction: column;
      .title {
        font-size: 2rem;
        letter-spacing: 0.03em;
        color: #fff;
      }
      .subtitle {
        font-size: 0.9rem;
        line-height: 1.1rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.5);
      }
    }
    &__btn {
      width: 2.5rem;
      height: 2.5rem;
      border-radius: 50%;
      border: 0.1rem solid rgba(255, 255, 255, 0.1);
      display: flex;
      align-items: center;
      justify-content: center;
      background: transparent;
      &::after {
        content: "";
        width: 0.85rem;
        height: 0.5rem;
        background: center / cover no-repeat url("/img/optionsMenu/informationTab/icons/arrow.svg");
      }
    }
  }
  &-empty {
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 2rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.3);
    border: 1px solid rgba(255, 255, 255, 0.5);
  }
}
</style>
