<template>
  <div class="content-item">
    <span class="content-item__time">{{ item.Time }}</span>
    <img
      :src="`img/hud/smartphone/weatherTab/icons/${item.Weather}.svg`"
      class="content-item__icon"
    >
    <span class="content-item__degrees">{{ item.Degrees }}</span>
  </div>
</template>

<script>
export default {
  name: 'MainContentItem',

  props: {
    item: Object
  }
}
</script>

<style lang="scss" scoped>
.content-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-between;
  height: 4rem;
  width: 2.75rem;
  &:not(:last-child) {
    border-right: 1px solid #EBEBEB;
  }
  &__time {
    font-size: 0.7rem;
    line-height: 0.8rem;
    color: #8C8C8C;
    margin: 0 0 0.5rem 0;
  }
  &__icon {
    width: 1.2rem;
    height: 1.2rem;
    margin: 0 0 0.5rem 0;
  }
  &__degrees {
    font-size: 0.9rem;
    line-height: 1.05rem;
  }
}
</style>