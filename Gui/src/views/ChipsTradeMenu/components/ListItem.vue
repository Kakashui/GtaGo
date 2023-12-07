<template>
  <div 
    @click="setCurrentProduct(item.id)" 
    :class="[{opacity: item.id !== curProductId && curProductId !== null}, 'list__item']"
  >
    <div class="item__general">
      <span class="list__item-info">{{ toLocal(item.price) }}$</span>
      <img
        class="general__img"
        :src="item.icon"
        alt="chip"
      >
      <div
        v-if="item.id === curProductId"
        class="item__actions"
      >
        <div
          class="actions__btn"
          @mousedown="decChipCount(item.id)"
        >-</div>
        <span class="general__quantity general__quantity--current">{{ toLocal(item.quantity) }}</span>
        <div
          @mousedown="incChipCount(item.id)"
          class="actions__btn"
        >+</div>
      </div>
      <span v-else class="general__quantity">x{{ toLocal(item.quantity) }}</span>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'listitem',

  props: ['item', 'toLocal'],

  data: function () {
    return {
      timer: null,
      timerInteval: 150
    }
  },

  computed: {
    ...mapState('chipsTradeMenu', ['curProductId']),

    priceLocal: function () {
      return this.item.price.toLocaleString()
    }
  },

  methods: {
    ...mapMutations('chipsTradeMenu', [
      'addChip',
      'removeChip',
      'setCurrentProduct'
    ]),

    incChipCount: function (id) {
      this.addChip(id)
      if(this.timer == null){
        this.timer = setInterval(() => {
          this.addChip(id)
        }, this.timerInteval)        
      }
    },

    decChipCount: function (id) {
      this.removeChip(id)
      this.timer = setInterval(() => {
        this.removeChip(id)
      }, this.timerInteval)
    },

    mouseUp(){
      this.stopTimer();
    },

    stopTimer: function () {
      clearInterval(this.timer);
      this.timer = null;
    }
  },
  mounted(){
    addEventListener("mouseup", this.mouseUp)
  },
  beforeDestroy(){
    removeEventListener("mouseup", this.mouseUp);
  }
}
</script>

<style lang="scss" scoped>
.list__item {
  display: flex;
  align-items: center;
  flex-direction: column;
  position: relative;
  &.opacity{
    transition: .3s;
    opacity: .5;  
  }
  &-info{
    font-weight: bold;
    font-size: 1rem;
    text-align: center;
    letter-spacing: 0.12em;
    color: #FFFFFF;
    margin-bottom: .5rem;
  }
  .item__general {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    width: 100%;
    position: relative;
    .general__quantity {
      font-weight: bold;
      font-size: 1rem;
      height: 1.4rem;
      line-height: 130%;
      letter-spacing: 0.12em;
      color: rgba(255, 255, 255, 0.6);
      text-align: center;
      &--current{
        color: #fff;
      }
    }
    .general__img {
      width: 5rem;
      height: 5rem;
      -webkit-transition: -webkit-transform .8s ease-in-out;
      transition: transform .8s ease-in-out;
      margin-bottom: .7rem;
    }
    &:hover .general__img{
      transform: rotateY(360deg);
    }
  }
  .item__actions {
    display: flex;
    align-items: center;
    width: 100%;
    justify-content: space-around;
    .actions__btn {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 1.4rem;
      min-width: 1.4rem;
      height: 1.4rem;
      text-align: center;
      background: rgba(255, 255, 255, 0.2);
      border-radius: .2rem;
      font-weight: bold;
      font-size: 1rem;
      letter-spacing: 0.12em;
      color: #fff;
      &:hover {
        background: rgba(255, 255, 255, 0.3);
      }
    }
  }
}
</style>
