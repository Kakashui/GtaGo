<template>
  <div class="item" @click="someEvent">
    <img :src="`img/optionsMenu/statisticsTab/transportModal/${item.icon}.png`" class="item__icon">
    <div v-if="item.type === 1" class="item__title">{{ loc(item.title) }}</div>
    <div v-else class="item__info">
      <div class="desc">{{loc('mmain:stats:veh:act:cell')}}</div>
      <div class="price">{{ item.price.toLocaleString() }}$</div>
    </div>
  </div>
</template>

<script>
import { mapGetters} from 'vuex'
export default {
  name: 'ActionsItem',

  props: {
    item: Object,
    currentCarId: Number
  },
  computed: {
    ...mapGetters('localization', ['loc']),
  },
  methods: {
    someEvent: function() {
      this.$emit("onClick", this.item.event);
    }
  }
}
</script>

<style lang="scss" scoped>
.item {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
  padding: 2rem 0 1.75rem 0;
  background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/transportModal/actionItemBg1.png");
  &:hover {
    background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/transportModal/actionItemBg2.png");
  }
  &__icon {
    width: 2.5rem;
    height: 2.5rem;
  }
  &__title {
    font-size: 1rem;
    letter-spacing: 0.03em;
    color: #FFFFFF;
  }
  &__info {
    display: flex;
    flex-direction: column;
    align-items: center;
    letter-spacing: 0.03em;
    .desc {
      font-size: 1rem;
      color: rgba(255, 255, 255, 0.4);
      font-weight: 300;
      margin: 0 0 0.1rem 0;
    }
    .price {
      font-size: 1.5rem;
      line-height: 1.5rem;
      color: #FFFFFF;
    }
  }
}
</style>
