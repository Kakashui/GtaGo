<template>
  <div class="bmw-page">
    <div class="title">BMW</div>
    <div class="text">{{loc('carDealership_28')}}</div>
    <div class="slogan">Bayerische Motoren Werke GmbH</div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">discover.</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'BmwPage',

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
.bmw-page{
  width: 100%;
  height: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/bmw.png');
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: flex-start;
  font-family: Helvetica;
  padding: 2rem;
  color: #fff;
  .title{
    font-size: 10rem;
    line-height: 11.5rem;
    text-transform: uppercase;
    animation: view-mirror .5s;
  }
  .text{
    margin-left: .7rem;
    width: 35.9rem;
    font-style: normal;
    font-weight: normal;
    font-size: 1.2rem;
    line-height: 1.4rem;
    animation: identifier 1s;
  }
  .slogan{
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    top: 5.25rem;
    right: 2rem;
    height: 5rem;
    padding-right: 6rem;
    font-size: 1.2rem;
    line-height: 1.4rem;
    animation: view 1s;
    &:after{
      content: '';
      width: 5rem;
      height: 5rem;
      position: absolute;
      right: 0;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/carDealership/bmw-logo.png');
    }
  }
  .block-link{
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    left: 0;
    bottom: 14.8rem;
    width: 12.9rem;
    height: 10.9rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/carDealership/link-bmw.svg');
    transform: translateX(-5%);
    animation: identifier 1.5s;
    transition: .2s;
    &:hover{
      transform: translateX(0%);
      transition: .2s;
    }
    .text{
      position: absolute;
      right: 0;
      transform: translateX(50%);
      font-family: Arial;
      font-style: normal;
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.85rem;
      text-transform: lowercase;
      width: fit-content;
    }
  }
}
</style>
