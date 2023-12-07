<template>
  <div class="list__item">
    <div :class="[{ in: item.directFrom == false, out: item.directFrom == true }, 'icon']">
      <img
        src="img/hud/smartphone/forbesTab/arrowDown.svg"
        alt="down"
        class="icon__img"
        v-if="item.directFrom == true"
      >
      <div class="icon__img" v-else-if="item.directFrom == false" ></div>
    </div>
    <div class="purpose" v-if="item.directFrom == true">
      <p class="purpose__desc" >{{loc('phone:histi:1')}}</p>
      <p class="purpose__recipient">{{ loc(recipientNumber) }}</p>
    </div>
    <div class="purpose" v-else-if="item.directFrom == false">
      <p class="purpose__desc" >{{loc('phone:histi:2')}}</p>
      <p class="purpose__recipient">{{ loc(recipientNumber) }}</p>
    </div>
    <div class="info">
      <p class="info__value">{{getSign}}${{ item.value.toLocaleString() }}</p>
      <p class="info__date">{{ currentDate }}</p>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'HistoryItem',

  props: {
    item: Object
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    currentDate: function() {
      const date = new Date(this.item.date * 1000).toLocaleDateString()
      return date
    },
    recipientNumber: function() {
      switch (this.item.recipientType) {
        case 1:
          return `${this.item.directFrom ? 'phone:histi:3' : 'phone:histi:4'}@${this.item.recipient}` 
        case 2:
          return 'phone:histi:5'
        case 3:
          return 'phone:histi:6'
      }
      return ''
    },
    getSign: function() {
      return this.item.directFrom ? '-' : '+';
    },
    getAmount: function() {
      return this.item.directFrom ? this.item.value : this.item.value - this.item.tax;
    }
  }
}
</script>

<style lang="scss" scoped>
.list__item {
  display: flex;
  align-items: center;
  width: 13.7rem;
  &:not(:first-child) {
    margin: 0.75rem 0 0 0;
  }
  .icon {
    width: 2rem;
    height: 2rem;
    border-radius: 0.4rem;
    display: flex;
    align-items: center;
    justify-content: center;
    &.in {
      background: rgba(64, 193, 49, 0.2);
      .icon__img {
        mask: url('/img/hud/smartphone/forbesTab/arrowUp.svg') no-repeat center / contain;
        background: #40C131;
      }
    }
    &.out {
      background: rgba(238, 68, 58, 0.2);
    }
    &__img {
      width: 0.5rem;
      height: 0.5rem;
    }
  }
  .purpose {
    margin: 0 0 0 0.5rem;
    &__desc {
      font-size: 0.7rem;
      font-weight: 500;
    }
    &__recipient {
      font-size: 0.65rem;
      color: rgba(39, 59, 64, 0.5);
    }
  }
  .info {
    margin: 0 0 0 auto;
    text-align: right;
    &__value {
      font-size: 0.7rem;
      font-weight: 500;
    }
    &__date {
      font-size: 0.65rem;
      color: rgba(39, 59, 64, 0.5);
    }
  }
}
</style>
