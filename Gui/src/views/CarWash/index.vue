<template>
  <div class="car-wash">
    <div class="car-wash__board">
      <div class="car-wash__title">Car Wash №{{washId}}</div>
      <div class="car-wash__desc">{{loc('car_wash_0')}}</div>
      <div class="car-wash__list">
        <ServiceItem
          v-for="item in servicesList"
          :key="item.key"
          :item="item"
          :basketList="basketList"
          @interactionWidthBasketList="interactionWidthBasketList(item)"
        />
      </div>
      <div class="car-wash__payment">
        <div class="car-wash__payment-desc">{{loc('car_wash_1')}}</div>
        <div class="car-wash__payment__list">
          <div 
            :class="[{active: item.key === currentPayment}, 'car-wash__payment__list-item']"
            v-for="item in paymentList"
            :key="item.key"
            @click="setCurrentPayment(item.key)"
          ><svg 
              xmlns="http://www.w3.org/2000/svg" 
              viewBox="0 0 24 24"
              v-html="item.svg"
            ></svg>
            {{loc(item.text)}}
          </div>
        </div>
      </div>
      <div class="car-wash__outcome">
        <div class="car-wash__outcome-info">
          <div class="car-wash__outcome-desc">{{loc('car_wash_2')}}</div>
          <div class="car-wash__outcome-value">$ {{totalAmount}}</div>
        </div>
        <div 
          class="car-wash__outcome__btn"
          @click="payForWash"
        >{{loc('car_wash_3')}}</div>
      </div>
    </div>
    <div class="car-wash__decorate car-wash__decorate--spray"></div>
    <div class="car-wash__decorate car-wash__decorate--sponge"></div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ServiceItem from './ServiceItem.vue'
export default {
  name: 'CarWash',
  components: {
    ServiceItem,
  },
  data: function() {
    return {
      currentPayment: 0,
      paymentList: [
        {
          key: 0,
          text: 'car_wash_5',
          svg: `<g><path d="M2.22656 4.87506H20.25V2.41506C20.25 2.30408 20.2254 2.19449 20.1779 2.09418C20.1304 1.99387 20.0613 1.90534 19.9755 1.83499C19.8896 1.76463 19.7893 1.7142 19.6816 1.68733C19.5739 1.66046 19.4616 1.65781 19.3528 1.67959L2.10281 4.51459C1.97436 4.54016 1.85485 4.59892 1.75616 4.68503C1.65748 4.77113 1.58306 4.88158 1.54031 5.00537C1.75878 4.919 1.99164 4.87478 2.22656 4.87506ZM21.7266 6.00006H2.22656C2.02765 6.00006 1.83688 6.07908 1.69623 6.21973C1.55558 6.36038 1.47656 6.55115 1.47656 6.75006V20.2501C1.47656 20.449 1.55558 20.6397 1.69623 20.7804C1.83688 20.921 2.02765 21.0001 2.22656 21.0001H21.7266C21.9255 21.0001 22.1162 20.921 22.2569 20.7804C22.3975 20.6397 22.4766 20.449 22.4766 20.2501V6.75006C22.4766 6.55115 22.3975 6.36038 22.2569 6.21973C22.1162 6.07908 21.9255 6.00006 21.7266 6.00006ZM17.25 15.0001C16.9533 15.0001 16.6633 14.9121 16.4166 14.7473C16.17 14.5824 15.9777 14.3482 15.8642 14.0741C15.7506 13.8 15.7209 13.4984 15.7788 13.2074C15.8367 12.9165 15.9796 12.6492 16.1893 12.4394C16.3991 12.2296 16.6664 12.0868 16.9574 12.0289C17.2483 11.971 17.5499 12.0007 17.824 12.1142C18.0981 12.2278 18.3324 12.42 18.4972 12.6667C18.662 12.9134 18.75 13.2034 18.75 13.5001C18.75 13.8979 18.592 14.2794 18.3107 14.5607C18.0294 14.842 17.6478 15.0001 17.25 15.0001Z"/><path d="M1.46857 12.1641V5.4375C1.46857 4.85953 1.73669 4.57125 2.19138 4.5C3.84138 4.24078 7.28107 4.10156 7.28107 4.10156C7.28107 4.10156 6.8906 4.85156 5.99997 4.85156V6C6.86716 6 7.46857 7.10156 7.46857 7.10156L3.97638 11.0625L1.46857 12.1641Z"/></g>`
        },
        {
          key: 1,
          text: 'car_wash_4',
          svg: `<path d="M1.5 19.5C1.5 19.6989 1.57902 19.8897 1.71967 20.0303C1.86032 20.171 2.05109 20.25 2.25 20.25H21.75C21.9489 20.25 22.1397 20.171 22.2803 20.0303C22.421 19.8897 22.5 19.6989 22.5 19.5V10.4062H1.5V19.5ZM4.59375 13.0312C4.59375 12.9318 4.63326 12.8364 4.70359 12.7661C4.77391 12.6958 4.86929 12.6562 4.96875 12.6562H9.28125C9.38071 12.6562 9.47609 12.6958 9.54642 12.7661C9.61674 12.8364 9.65625 12.9318 9.65625 13.0312V16.0312C9.65625 16.1307 9.61674 16.2261 9.54642 16.2964C9.47609 16.3667 9.38071 16.4062 9.28125 16.4062H4.96875C4.86929 16.4062 4.77391 16.3667 4.70359 16.2964C4.63326 16.2261 4.59375 16.1307 4.59375 16.0312V13.0312ZM21.75 3.75H2.25C2.05109 3.75 1.86032 3.82902 1.71967 3.96967C1.57902 4.11032 1.5 4.30109 1.5 4.5V7.59375H22.5V4.5C22.5 4.30109 22.421 4.11032 22.2803 3.96967C22.1397 3.82902 21.9489 3.75 21.75 3.75Z"/>`
        }
      ],
      basketList: []
    }
  },
  computed: {
    ...mapState('carWash', ['washId', 'servicesList']),
    ...mapGetters('localization', ['loc']),
    totalAmount() {
      let amount = 0
      this.basketList.forEach(function (item) {
        amount = amount + Number(item.cost)
      })
      return amount.toLocaleString('ru')
    }
  },
  methods: {
    setCurrentPayment: function(value) {
      this.currentPayment = value
    },
    interactionWidthBasketList: function(item) {
      if (!this.basketList.includes(item)) {
        this.basketList.push(item)
      } else {
        this.basketList = this.basketList.filter(element => element !== item)
      }
    },
    payForWash: function() {
      if (this.basketList.length > 0) {
        window.mp.triggerServer('carwash:buy', this.currentPayment, JSON.stringify(this.basketList.map(item => item.key)))
        window.mp.trigger('carwash::exit')
        
        this.basketList = []
      }
    }
  },
  mounted() {
    this.currentPayment = this.paymentList[0].key
  }
}
</script>

<style lang="scss" scoped>
.car-wash{
  width: 100%;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  background-image: url('/img/carWash/raindrops.png');
  background-size: cover;
  background-position: center;
  animation: showCarWash .7s;
  font-family: Bebas Neue Pro;
  color: #FFFFFF;
  text-transform: uppercase;
  font-weight: bold;
  display: flex;
  align-items: center;
  justify-content: center;
  &__decorate{
    pointer-events: none;
    position: absolute;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    &--sponge{
      width: 15.15rem;
      height: 10.15rem;
      right: 18.65rem;
      bottom: -2.5rem;
      background-image: url('/img/carWash/sponge.png');
      z-index: 1;
      animation: showSponge 1s;
    }
    &--spray{
      width: 13.45rem;
      height: 17.7rem;
      right: 12.85rem;
      bottom: -3.5rem;
      background-image: url('/img/carWash/spray.png');
      z-index: 2;
      animation: showSpray .7s;
    }
  }
  &__board{
    width: 34.2rem;
    height: 54rem;
    max-height: 100vh;
    background: rgba(17, 17, 17, .9);
    padding: 2rem 3rem 4rem 3rem;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
  &__title{
    font-size: 4rem;
    line-height: 5.7rem;
    margin-bottom: 1rem;
    position: relative;
    &:before{
      content: '';
      width: 5.3rem;
      height: 5.3rem;
      position: absolute;
      left: -.85rem;
      top: -1rem;
      transform: translateX(-100%);
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/carWash/drop.png');
    }
  }
  &__desc{
    font-size: 2rem;
    line-height: 2.3rem;
    text-shadow: 0 .5rem 1rem rgba(255, 255, 255, 0.25);
    margin-bottom: 1.1rem;
    letter-spacing: 0.1em;
    color: rgba(255,255,255, .6);
    position: relative;
    &:before{
      content: '';
      width: 3.65rem;
      height: 3.65rem;
      position: absolute;
      left: -.5rem;
      top: -2.9rem;
      transform: translateX(-100%);
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/carWash/drop.png');
    }
  }
  &__list{
    margin-bottom: 1.5rem;
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-auto-rows: 12.15rem;
    grid-row-gap: 1.4rem;
    grid-column-gap: 2rem;
    max-height: 25.75rem;
    overflow-y: auto;
    &::-webkit-scrollbar{
      width: 0;
      background: transparent;
      &-thumb{
        background: transparent;
      }
    }
  }
  &__payment{
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: .8rem 0;
    border-top: .1rem solid rgba(255, 255, 255, 0.1);
    border-bottom: .1rem solid rgba(255, 255, 255, 0.1);
    margin-bottom: .8rem;
    &-desc{
      font-size: 2rem;
      line-height: 2.3rem;
    }
    &__list{
      display: flex;
      align-items: center;
      justify-content: center;
      background: rgba(255, 255, 255, 0.06);
      padding: .2rem;
      &-item{
        display: flex;
        align-items: center;
        letter-spacing: 0.04em;
        opacity: 0.4;
        color: #fff;
        padding: .7rem 1.5rem;
        transition: .3s;
        border: .1rem solid rgba(255, 255, 255, 0);
        svg{
          width: 1.2rem;
          height: 1.2rem;
          fill: #fff;
          margin-right: .6rem;
          transition: .3s;
        }
        &.active, &:hover{
          color: #AFCC3B;
          opacity: 1;
          svg{
            fill: #AFCC3B;
          }
        }
        &.active{
          background: rgba(255, 255, 255, 0.06);
          border-color: rgba(255, 255, 255, 0.1);
        }
      }
    }
  }
  &__outcome{
    display: flex;
    align-items: center;
    justify-content: space-between;
    &-info{
      display: flex;
      flex-direction: column;
    }
    &-desc{
      font-size: .8rem;
      line-height: .9rem;
      letter-spacing: 0.1em;
      color: rgba(255, 255, 255, 0.6);
      margin-bottom: .15rem;
    }
    &-value{
      font-size: 3rem;
      line-height: 3.45rem;
    }
    &__btn{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 10.9rem;
      height: 3.1rem;
      background: #AFCC3B;
      font-size: 1rem;
      transition: .3s;
      &:hover{
        transform: scale(1.1);
      }
      &:active{
        transform: scale(1.05);
      }
    }
  }
}
@keyframes showCarWash {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
@keyframes showSponge {
  from {
    transform: translateY(100%);
  }
  to {
    transform: translateY(0);
  }
}
@keyframes showSpray {
  from {
    transform: translateX(300%);
  }
  to {
    transform: translateX(0);
  }
}
</style>
