<template>
  <div class="list-item" @click="setCurrentTab({ key: currentTab.key, section: 'ThirdSection', data: { currentItem: item } })">
    <div class="list-item__icon">
      <img
        :src="`img/hud/smartphone/mapTab/categories/${item.icon}.svg`"
        :alt="item.icon"
        class="img"
      >
    </div>
    <div class="list-item__desc">
      <p class="title">{{ loc(item.title) }}</p>
      <div class="distance">
        <span class="distance__desc">{{loc('phone:map:4')}}</span>
        <span class="distance__value">{{ item.distance }} m</span>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'ListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab'])
  }
}
</script>

<style lang="scss" scoped>
.list-item {
  display: flex;
  align-items: center;
  padding: 0.5rem;
  border-radius: 0.5rem;
  background: #fff;
  position: relative;
  &:not(:last-child) {
    margin: 0 0 0.4rem 0;
  }
  &::after {
    visibility: hidden;
    content: '';
    top: 0;
    left: 0;
    position: absolute;
    width: 100%;
    height: 100%;
    border-radius: 0.5rem;
    box-shadow: 0px 0.5rem 1.5rem rgba(0, 0, 0, 0.1);
  }
  &:hover {
    &::after {
      visibility: visible;
    }
    z-index: 2;
  }
  &__icon {
    width: 2rem;
    height: 2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 0.5rem;
    border: 1px solid #EBEBEB;
    .img {
      width: 1rem;
      height: 1rem;
    }
  }

  &__desc {
    margin: 0 0 0 0.6rem;
    font-size: 0.65rem;
    .title {
      color: #000;
    }
    .distance {
      color: #888888;
      margin: 0.15rem 0 0 0;
    }
  }
}
</style>
