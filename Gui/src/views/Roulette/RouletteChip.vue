<template>
  <div 
    class="roulette-chip"
    @click="setChip"
  >
    <div
      class="roulette-chip-img"
      :style="{ backgroundImage: `url(/img/chipsTradeMenu/chip-${color}.png)` }"
    ></div>
    <div>x{{count}}</div>
  </div>
</template>

<script>
export default {
  name: "RouletteChip",
  props: {
    color: String,
    count: Number
  },
  methods: {
    setChip() {
      window.mp.trigger('roulette:shipset:update', this.color)
    }    
  }
};
</script>

<style lang="scss" scoped>
.roulette-chip {
  display: flex;
  flex-direction: column;
  align-items: center;
  font-size: 0.8rem;
  line-height: 0.9rem;
  letter-spacing: 0.04em;
  color: rgba(255, 255, 255, 0.8);
  margin-right: 1.3rem;
  &:last-child {
    margin-right: 0;
  }
  &-img {
    width: 2rem;
    height: 2rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    margin-bottom: 0.6rem;
    border-radius: 50%;
    transition: .2s;
  }
  &:hover{
    .roulette-chip-img{
      box-shadow: 0 .5rem 1.5rem rgba(255, 255, 255, 0.9);
      transform: scale(1.15);
    }
  }
  &:active{
    .roulette-chip-img{
      transform: scale(1.1);
    }
  }
}
</style>
