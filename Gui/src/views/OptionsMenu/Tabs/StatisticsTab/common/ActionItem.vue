<template>
  <div :style="{ background: `center / cover no-repeat url(/img/optionsMenu/statisticsTab/backs/${item.bg}.png), rgba(255, 255, 255, 0.05)` }" class="item">
    <div class="item__info">
      <div class="title">{{ loc(item.title) }}</div>
      <div v-if="item.key === 'house'" class="desc">{{ property[item.key] ? loc('optmenu:stat:act:1') : loc('mmain:stats:act:miss') }}</div>
      <div v-else-if="item.key === 'business'" class="desc">{{ property[item.key] ? `№ ${property[item.key].number}`: loc('optmenu:stat:act:2') }}</div>
      <div v-else-if="item.key === 'transport'" class="desc">
        <div class="desc__text">{{loc('mmain:stats:act:count')}}:</div>
        <div class="desc__value">{{ property[item.key] ? property[item.key].length : 0 }}</div>
      </div>
    </div>
    <button class="item__btn" :disabled="!property[item.key]" @click="item.event"> 
      <div v-if="item.key === 'transport'" class="text">+</div>
      <div v-else class="icon"></div>
    </button>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'ActionsItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('optionsMenu', ['property']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.item {
  width: 20rem;
  height: 8rem;
  display: flex;
  justify-content: flex-end;
  align-items: center;
  padding: 0 1rem 0 0;
  border: 1px solid rgba(255, 255, 255, 0.1);
  &:not(:last-child) {
    margin: 0 0 0.75rem 0;
  }
  &__info {
    letter-spacing: 0.03em;
    margin: 0 1.25rem 0 0;
    .title {
      font-size: 2rem;
      line-height: 2rem;
      color: #fff;
      margin: 0 0 0.25rem 0;
      text-align: right;
      font-weight: 300;
    }
    .desc {
      color: rgba(255, 255, 255, 0.5);
      font-size: 0.9rem;
      line-height: 1.2rem;
      display: flex;
      justify-content: flex-end;
      &__value {
        color: #fff;
        margin: 0 0 0 0.15rem;
      }
    }
  }
  &__btn {
    width: 3.5rem;
    height: 3.5rem;
    border: 0.1rem solid rgba(255, 255, 255, 0.1);
    display: flex;
    align-items: center;
    justify-content: center;
    background: transparent;
    border-radius: 50%;
    &:disabled {
      .text {
        color: rgba(255, 255, 255, 0.1);
      }
      .icon {
        background: rgba(255, 255, 255, 0.1);
      }
    }
    .text {
      color: #B6D300;
      font-size: 2rem;
      line-height: 2rem;
    }
    .icon {
      mask: center / cover no-repeat url("/img/optionsMenu/statisticsTab/icons/levels.svg");
      width: 1.2rem;
      height: 1.2rem;
      background: #B6D300;
    }
  }
}
</style>
