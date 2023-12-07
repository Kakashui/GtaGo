<template>
  <div class="rolls-page">
    <div class="logo">Rolls-royce</div>
    <div class="info">{{loc('carDealership_13')}} </div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">discover.</div>
    </div>
    <div class="icon"></div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'RollsPage',

  methods: {
    ...mapMutations('carDealership', ['setCurrentPage'])
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes identifier {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
@keyframes view {
  from {
    transform: translateY(100%);
  }
  to {
    transform: translateY(10%);
  }
}
.rolls-page{
  width: 100%;
  height: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/rolls.png');
  font-family: Montserrat;
  color: #FFFFFF;
  display: flex;
  align-items: center;
  flex-flow: column;
  padding-top: 2rem;
  .logo{
    position: relative;
    font-weight: 300;
    font-size: 7.5rem;
    line-height: 9.15rem;
    text-transform: uppercase;
    margin-bottom: .5rem;
    animation: identifier linear 1.5s;
    &:before{
      content: '';
      position: absolute;
      top: 0;
      left: -.5rem;
      transform: translateX(-100%);
      width: 5.75rem;
      height: 7.7rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/carDealership/rolls.png');
    }
  }
  .info{
    font-size: 1.5rem;
    line-height: 1.85rem;
    max-width: 48.5rem;
    animation: identifier linear 1.5s;
  }
  .block-link{
    height: 14.85rem;
    width: 10.85rem;
    position: absolute;
    display: flex;
    align-items: center;
    justify-content: center;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-image: url('/img/carDealership/link-rolls.svg');
    bottom: 0;
    left: 2rem;
    animation: view linear .5s;
    transform: translateY(10%);
    transition: .2s;
    &:hover{
      transform: translateY(5%);
      transition: .2s;
    }
    .text{
      font-size: 2.5rem;
      line-height: 3.05rem;
      text-transform: lowercase;
      position: absolute;
      right: 0;
      top: 4rem;
      transform: translateX(50%);
    }
  }
  .icon{
    width: 5rem;
    height: 5.6rem;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-image: url('/img/carDealership/rr.png');
    position: absolute;
    right: 2rem;
    bottom: 2rem;
    animation: identifier linear 2.5s;
  }
}
</style>
