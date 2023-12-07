<template>
  <div class="bentley-page">
    <div class="logo"></div>
    <div class="title">Bentley motors</div>
    <div class="desc">{{loc('carDealership_29')}}</div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">discover.</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'BentleyPage',

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
    transform: translateX(-100%);
  }
  to {
    transform: translateX(0%);
  }
}
@keyframes scaling {
  from {
    transform: scale(0);
  }
  to {
    transform: scale(1);
  }
}
.bentley-page{
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/bentley.png');
  font-family: Cantarell;
  font-style: normal;
  text-transform: uppercase;
  text-align: center;
  color: #FFFFFF;
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: flex-start;
  padding-top: 2rem;
  .logo{
    height: 4.2rem;
    width: 13.5rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/carDealership/bentley-logo.png');
    animation: identifier 1s;
  }
  .title{
    font-weight: bold;
    font-size: 5rem;
    line-height: 7.2rem;
    animation: view 1s;
  }
  .desc{
    font-weight: 500;
    font-size: 1rem;
    line-height: 1.4rem;
    max-width: 26.6rem; 
  }
  .block-link{
    width: 10.9rem;
    height: 17.54rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/carDealership/link.svg');
    position: absolute;
    bottom: 0;
    animation: scaling 1s;
    transition: .2s;
    &:hover{
      transform: scale(1.1);
      transition: .2s;
    }
    .text{
      position: absolute;
      top: 4rem;
      right: 0;
      transform: translateX(50%);
      font-family: Arial;
      font-style: normal;
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.85rem;
      text-transform: lowercase;
    }
  }
}
</style>
