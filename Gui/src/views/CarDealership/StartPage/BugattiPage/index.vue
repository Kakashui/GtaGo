<template>
  <div class="bugatti-page" @click="setCurrentPage('ViewPage')">
    <div class="info">
      <div class="title">Bugatti</div>
      <div class="text">there is no other automotive brand like it in the world: bugatti is unique. it was already unique during ettore bugatti’s lifetime, and it has retained this unique status ever since the brand’s renaissance in 1998 as a subsidiary of the volkswagen group – a renaissance that is unparalleled in the history of motoring.</div>
    </div>
    <div class="block-link">
      <div class="text">discover</div>
    </div>
  </div>
</template>

<script>
import { mapMutations } from 'vuex'
export default {
  name: 'BugattiPage',

  methods: {
    ...mapMutations('carDealership', ['setCurrentPage'])
  },
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
    transform: translateY(-100%);
  }
  to {
    transform: translateY(0%);
  }
}
@keyframes view-mirror {
  from {
    transform: translateX(-100%);
  }
  to {
    transform: translateX(0%);
  }
}
.bugatti-page{
  width: 100%;
  height: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/bugatti.png');
  display: flex;
  align-items: flex-start;
  justify-content: flex-start;
  padding-left: 2rem;
  font-family: Arial;
  color: #FFFFFF;
  .info{
    display: flex;
    align-items: center;
    .title{
      font-style: normal;
      font-weight: bold;
      font-size: 12.5rem;
      line-height: 13.85rem;
      text-transform: uppercase;
      animation: view 1s;
    }
    .text{
      margin-left: 1rem;
      width: 34.5rem;
      min-width: 34.5rem;
      font-style: normal;
      font-weight: normal;
      font-size: 1.1rem;
      line-height: 1.25rem;
      text-transform: uppercase;
      animation: identifier 1s;
    }
  }
  .block-link{
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    left: 0;
    align-self: center;
    width: 32.9rem;
    height: 10.9rem;
    background-image: url('/img/carDealership/link-bugatti.svg');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center left;
    animation: view-mirror .5s;
    transition: .2s;
    &:hover{
      transform: translateX(-5%);
      transition: .2s;
    }
    .text{
      position: absolute;
      right: 0;
      transform: translateX(50%);
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.85rem;
      text-transform: lowercase;
    }
  }
}
</style>
