<template>
  <div :class="[{ active: item.id === this.currentId }, 'sound-item']" @click="setCurrentId(item.id)">
    <img
      src="img/hud/smartphone/check.svg"
      alt="check"
      class="sound-item__icon"
      v-if="item.id === this.currentId"
    >
    <span class="sound-item__text">{{ item.title }}</span>
  </div>
</template>

<script>
export default {
  name: 'SoundItem',

  props: {
    item: Object,
    currentId: Number,
    setCurrentId: Function
  }
}
</script>

<style lang="scss" scoped>
.sound-item {
  padding: .8rem 0 .8rem 2.8rem;
  position: relative;
  &::after {
    position: absolute;
    content: '';
    bottom: 0;
    right: 0;
    height: 1px;
    width: 12.35rem;
    background: rgba(195, 195, 195, 0.35);
  }
  &:last-child{
    &::after {
      display: none;
    }
  }
  &.active {
    .sound-item__text {
      font-weight: bold;
    }
  }
  &:hover {
    .sound-item__text {
      transform: translateY(-.1rem);
    }
  }
  &__icon {
    position: absolute;
    transform: translate(-50%, -50%);
    top: 50%;
    left: 1.4rem;
    width: 1.35rem;
    height: 1.35rem;
  }
  &__text {
    font-size: .8rem;
    font-weight: 300;
    display: inline-block;
    transition: all .15s;
  }
}
</style>
