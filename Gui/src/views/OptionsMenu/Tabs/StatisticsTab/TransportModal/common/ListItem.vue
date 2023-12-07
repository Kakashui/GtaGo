<template>
  <div :class="[{ active: item.id === currentId }, 'item']">
    <div class="item__info">
      <div class="title">{{ loc(item.name) }}</div>
      <div class="numbers">{{loc('mmain:stats:veh:number')}}: {{ item.numbers }}</div>
    </div>
    <button class="item__btn" @click="$emit('set-current-car', item)"></button>
  </div>
</template>

<script>
import { mapGetters} from 'vuex'
export default {
  name: 'ListItem',

  props: {
    item: Object,
    currentId: Number
  },
  computed: {
    ...mapGetters('localization', ['loc']),
  }
}
</script>

<style lang="scss" scoped>
.item {
  height: 4.7rem;
  width: 22rem;
  display: flex;
  justify-content: space-between;
  padding: 1.1rem 0 1.1rem 1rem;
  position: relative;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  &:first-child {
    border-top: 1px solid rgba(255, 255, 255, 0.1);
  }
  &.active {
    .item__btn {
      background: #B6D300;
      box-shadow: 0px 0.5rem 2rem 0.2rem rgba(162, 188, 31, 0.3);
      &::after {
        background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/transportModal/arrow1.svg");
      }
    }
  }
  &::before {
    content: "";
    position: absolute;
    left: 0;
    top: 50%;
    transform: translate(0, -50%);
    height: 2.5rem;
    width: 0.1rem;
    border-radius: 0.1rem;
    background: #B6D300;
    box-shadow: 0px 0px 1rem rgba(168, 195, 2, 0.7);
  }
  &__info {
    display: flex;
    flex-direction: column;
    letter-spacing: 0.03em;
    .title {
      font-size: 1.5rem;
      line-height: 1.5rem;
      margin: 0 0 0.2rem 0;
      color: #FFFFFF;
    }
    .numbers {
      color: rgba(255, 255, 255, 0.5);
      font-size: 0.9rem;
    }
  }
  &__btn {
    width: 2.5rem;
    height: 2.5rem;
    border-radius: 50%;
    border: 0.1rem solid rgba(255, 255, 255, 0.1);
    background: transparent;
    display: flex;
    align-items: center;
    justify-content: center;
    &::after {
      content: "";
      width: 0.85rem;
      height: 0.5rem;
      background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/transportModal/arrow2.svg");
    }
  }
}
</style>
