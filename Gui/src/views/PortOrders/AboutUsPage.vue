<template>
  <div class="about-us-page">
    <div class="about-us-page__header">{{loc('PortOrders_17')}}</div>
    <div class="about-us-page__info">
      <div class="text">{{loc('PortOrders_18')}}<br/><br/>{{loc('PortOrders_22')}}<br/><br/>{{loc('PortOrders_23')}}</div>
      <div class="stats">
        <div class="stats__info">
          <div class="value">40 000+ </div>
          <div class="desc">{{loc('PortOrders_19')}}</div>
        </div>
        <div class="stats__info">
          <div class="value">20</div>
          <div class="desc">{{loc('PortOrders_20')}}</div>
        </div>
        <div class="stats__info">
          <div class="value">56</div>
          <div class="desc">{{loc('PortOrders_21')}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'AboutUsPage',

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.about-us-page{
  width: 100%;
  height: 100%;
  display: flex;
  flex-flow: column;
  justify-content: flex-start;
  color: #fff;
  padding: 0 20.4rem;
  min-height: 39.6rem;
  position: relative;
  &:after{
    content: '';
    width: 42.9rem;
    height: 26rem;
    position: absolute;
    right: 0;
    bottom: 0;
    background-image: url('/img/portOrders/ship.png');
    background-size: contain;
    background-position: right bottom;
    background-repeat: no-repeat;
  }
  &__header{
    font-weight: bold;
    font-size: 2.5rem;
    line-height: 2.95rem;
    margin-top: 2rem;
    text-transform: uppercase;
  }
  &__info{
    margin-top: 2rem;
    display: flex;
    align-items: flex-start;
    justify-content: space-between;
    width: 100%;
    .text{
      max-width: 22.8rem;
      font-size: 1.2rem;
      line-height: 1.4rem;
      color: rgba(255, 255, 255, 0.8);
    }
    .stats{
      display: flex;
      align-items: flex-start;
      &__info{
        display: flex;
        flex-flow: column;
        align-items: center;
        margin-left: 2.85rem;
        &:first-child{
          margin-left: 0;
        }
        .value{
          font-weight: bold;
          font-size: 2.5rem;
          line-height: 2.95rem;
          color: #48B3EF;
          margin-bottom: .3rem;
          white-space: pre;
        }
        .desc{
          font-size: 1rem;
          line-height: 1.15rem;
          color: rgba(255, 255, 255, 0.8);
          max-width: 9.7rem;
          text-align: center;
          min-width: 6.25rem;
        }
      }
    }
  }
}
</style>
