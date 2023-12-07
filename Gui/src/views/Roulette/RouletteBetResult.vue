<template>
  <div :class="[{'black' : betResult.color === 'black'}, {'red' : betResult.color === 'red'}, 'roulette-bet-result']">
    <div class="roulette-bet-result__desc">{{(betResult.isWin ? 'win' : 'lose')}}</div>
    <div :class="[{'win' : betResult.isWin}, 'roulette-bet-result__cash']">$ {{betResult.cash}}</div>
    <div class="roulette-bet-result__values">
      <div 
        class="roulette-bet-result__values-item"
        v-for="item in betResult.values"
        :key="item"
      >{{item}}</div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'RouletteBetResult',
  computed: {
    ...mapState('roulette', ['betResult'])
  }
}
</script>

<style lang="scss" scoped>
.roulette-bet-result{
  position: absolute;
  top: 2rem;
  left: 50%;
  width: 19.55rem;
  height: 19.55rem;
  transform: translateX(-50%);
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 3.35rem;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center top;
  background-image: url('/img/roulette/result-null.png');
  &.black{
    background-image: url('/img/roulette/result-black.png');
  }
  &.red{
    background-image: url('/img/roulette/result-red.png');
  }
  &__desc{
    font-size: 1.5rem;
    line-height: 1.7rem;
    letter-spacing: 0.04em;
    text-transform: uppercase;
    margin-bottom: .2rem;
  }
  &__cash{
    font-weight: 800;
    font-size: 2rem;
    line-height: 2.3rem;
    color: #aa1616;
    &:before{
      content: '- ';
    }
    &.win{
      color: #D6FF00;
      &:before{
        content: '+ ';
      }
    }
  }
  &__values{
    display: flex;
    align-items: flex-end;
    height: 2.1rem;
    position: absolute;
    top: .45rem;
    &-item{
      width: 1rem;
      text-align: center; 
      font-size: 1.5rem;
      line-height: 1.7rem;
      margin-right: 1.3rem;
      &:last-child{
        margin-right: 0;
      }
      &:nth-child(2) {
        align-self: flex-start;
      }
    }
  }
}
</style>
