<template>
  <div class="pay-page">
    <div class="btn btn-back" @click="setCurrentPage({page: 'FuelPage'})"></div>
    <div class="container">
      <div class="container__payment">
        <div class="title">{{loc('gasStation_8')}}</div>
        <div class="btns-wrap">
          <div 
            :class="[{ active: item.key === paymentType }, 'btn']"
            v-for="item in pays"
            :key="item.key"
            @click="setPaymentType(item.key)"
          >{{loc(item.title)}}</div>
        </div>
      </div>
      <div class="container__info">
        <div class="title">{{loc('gasStation_9')}}</div>
        <div class="info-row">
          <div class="desc">{{loc('gasStation_10')}}:</div>
          <div class="value">{{currentFuel.title}}</div>
        </div>
        <div class="info-row">
          <div class="desc">{{loc('gasStation_11')}}:</div>
          <div class="value">{{currentPage.data}}</div>
        </div>
        <div class="info-total">
          <div class="desc">{{loc('gasStation_12')}}</div>
          <div class="value">$ {{currentPage.data*currentFuel.cost}}</div>
        </div>
        <div 
          class="btn btn-pay"
          v-if="paymentType >= 0"
          @click="buyFuel"
        >{{loc('gasStation_13')}}</div>
        <div 
          class="btn btn-locked"
          v-else
        >{{loc('gasStation_13')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: "PayPage",
  
  data: function() {
    return {
      pays: [
        { key: 0, title: "Biz_173"},
        { key: 1, title: "Biz_172"},
        { key: 2, title: "Biz_174"},
      ],
      paymentType: -1
    }
  },

  computed: {
    ...mapState('gasStation', ['currentFuel', 'currentPage']),
    ...mapGetters('localization', ['loc'])
  },

  methods:{
    ...mapMutations('gasStation', ['setCurrentPage']),
    buyFuel: function() {
      window.mp.trigger('gasStation:buyFuel', this.currentFuel.key, this.currentPage.data, this.paymentType)
    },
    setPaymentType: function(value) {
      this.paymentType = value
    }
  }
}
</script>

<style lang="scss" scoped>
.pay-page{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  width: 100%;
  .btn-back{
    width: 1.4rem;
    height: 1.4rem;
    background-image: url('/img/gasStation/btn-back.svg');
    margin-bottom: 1rem;
    &:hover{
      transform: scale(1.1);
    }
  }
  .container{
    padding: 0;
    margin: 0;
    display: flex;
    width: 100%;
    &__payment{
      padding-right: 2rem;
      margin-right: 2rem;
      border-right: 1px solid rgba(255, 255, 255, 0.3);
      width: fit-content;
      min-width: fit-content;
      .title{
        font-weight: bold;
        font-size: 2rem;
        line-height: 2rem;
        margin-bottom: 1rem;
      }
      .btns-wrap{
        display: flex;
        align-items: center;
        width: 100%;
        justify-content: space-between;
        align-items: center;
        .btn{
          font-size: 1.5rem;
          line-height: 1.5rem;
          padding-bottom: .15rem;
          border-bottom: .1rem solid transparent;
          &:hover, &.active{
            color: #DCFF00;
            border-color: #DCFF00;
          }
        }
      }
    }
    &__info{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      width: 100%;
      .title{
        font-weight: bold;
        font-size: 1.1rem;
        line-height: 1.1rem;
        color: rgba(255, 255, 255, 0.7);
        margin-bottom: 1rem;
      }
      .info-row, .info-total{
        display: flex;
        align-items: center;
        width: 100%;
        justify-content: space-between;
      }
      .info-row{
        color: rgba(255, 255, 255, 0.7);
        margin-bottom: .5rem;
        .desc{
          font-weight: 300;
        }
        .value{
          font-weight: bold;
        }
      }
      .info-total{
        padding-top: .5rem;
        margin-top: .5rem;
        margin-bottom: 1rem;
        font-size: 1.1rem;
        line-height: 1.1rem;
        border-top: 1px solid rgba(255, 255, 255, 0.3);
        .desc{
          font-weight: bold;
        }
        .value{
          color: #DCFF00;
        }
      }
      .btn-pay, .btn-locked{
        padding: .6rem 2.2rem;
        font-weight: bold;
        font-size: 1.1rem;
        line-height: 1.1rem;
        border-radius: .1rem;
      }
      .btn-pay{      
        background: #C1E704;
        color: #fff;
        &:hover{
          box-shadow: 0 .1rem .8rem #DCFF00;
        }
      }
      .btn-locked{
        color: rgba(199, 199, 201, 0.3);
        background: rgba(255, 255, 255, 0.1);
      }
    }
  }
}
</style>
