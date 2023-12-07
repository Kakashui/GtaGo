<template>
  <div class="luxe-page">
    <div class="info">
      <div class="title">Luxe cars</div>
      <div class="btn" @click="setCurrentPage('ViewPage')">{{loc('carDealership_0')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'LuxePage',

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
  .luxe-page{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/carDealership/startPage/luxe.png');
    color: #FFFFFF;
    text-transform: uppercase;
    font-style: normal;
    display: flex;
    align-items: center;
    justify-content: center;
    padding-bottom: 10.8rem;
    .info{
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: center;
      animation: identifier linear 1s;
      .title{
        font-weight: bold;
        font-size: 3rem;
        line-height: 3rem;
        letter-spacing: 0.8em;
        margin-bottom: 2.5rem;
      }
      .btn{
        padding: 1.55rem;
        background: rgba(0, 0, 0, 0.8);
        font-size: 1rem;
        line-height: 1.2rem;
        position: relative;
        transition: .2s;
        &:hover{
          transform: scale(1.1);
          transition: .2s;
        }
        &:before, &:after{
          content: '';
          width: .75rem;
          height: .75rem;
          position: absolute;
        }
        &:before{
          top: .8rem;
          left: .8rem;
          border-top: 1px solid #FFFFFF;
          border-left: 1px solid #FFFFFF;
        }
        &:afteR{
          right: .8rem;
          bottom: .8rem;
          border-right: 1px solid #FFFFFF;
          border-bottom: 1px solid #FFFFFF;
        }
      }
    }
  }
</style>
