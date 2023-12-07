<template>
  <div class="setting-amount-car-sales">
    <div class="setting-amount-car-sales__wrap">
      <div class="setting-amount-car-sales__model">{{carModel}}</div>
      <div class="setting-amount-car-sales-desc">{{loc('settingAmountCarSales_0')}}</div>
      <input 
        type="number"
        min="1"
        step="1"
        :placeholder="loc('settingAmountCarSales_3')"
        class="setting-amount-car-sales-input"
        v-model="currentPrice"
      >
      <div class="setting-amount-car-sales__btns">
        <div 
          class="setting-amount-car-sales-btn"
          @click="acceptCurrentPrice"
        >
          <svg 
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 26 26" 
            fill="none"
          >
            <path d="M19.4918 8.72615C19.0916 8.30416 18.4244 8.28557 18.0018 8.68679L11.2727 15.0684L8.06311 11.7731C7.6566 11.356 6.9901 11.3469 6.57268 11.7534C6.15561 12.1596 6.14683 12.8268 6.55297 13.2439L10.4881 17.2841C10.6943 17.4959 10.9684 17.6027 11.2432 17.6027C11.5039 17.6027 11.7646 17.5064 11.9687 17.3136L19.4525 10.2163C19.8748 9.81571 19.8927 9.1485 19.4918 8.72615Z"/>
            <path d="M13 0C5.83172 0 0 5.83172 0 13C0 20.1683 5.83172 26 13 26C20.1683 26 26 20.1683 26 13C26 5.83172 20.1683 0 13 0ZM13 23.8919C6.99436 23.8919 2.10808 19.0061 2.10808 13C2.10808 6.99436 6.9943 2.10808 13 2.10808C19.006 2.10808 23.8919 6.9943 23.8919 13C23.8919 19.006 19.006 23.8919 13 23.8919Z"/>
          </svg>
          <div class="setting-amount-car-sales-btn-text">{{loc('settingAmountCarSales_1')}}</div>
        </div>
        <div 
          class="setting-amount-car-sales-btn"
          @click="cancelCurrentPrice"
        >
          <svg 
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 26 26" 
            fill="none"
          >
            <path d="M22.198 3.80608C17.1287 -1.26326 8.87761 -1.26326 3.80828 3.80608C1.35267 6.26265 0 9.52807 0 13.0009C0 16.4738 1.35267 19.7392 3.80828 22.1948C6.34345 24.73 9.67331 25.9971 13.0031 25.9971C16.3329 25.9971 19.6628 24.73 22.198 22.1948C27.2673 17.1255 27.2673 8.87639 22.198 3.80608ZM20.7738 20.7706C16.4891 25.0553 9.51716 25.0553 5.23245 20.7706C3.15761 18.6958 2.01441 15.936 2.01441 13.0009C2.01441 10.0659 3.15761 7.30611 5.23245 5.23025C9.51716 0.945543 16.4891 0.946558 20.7738 5.23025C25.0575 9.51497 25.0575 16.4869 20.7738 20.7706Z"/>
            <path d="M17.3432 15.7843L14.4918 12.937L17.3432 10.0896C17.736 9.69677 17.736 9.05921 17.3443 8.66534C16.9504 8.27052 16.3129 8.27153 15.9191 8.66433L13.0656 11.5137L10.2122 8.66433C9.8184 8.27153 9.18084 8.27052 8.78702 8.66534C8.39423 9.05916 8.39423 9.69672 8.78804 10.0896L11.6395 12.937L8.78804 15.7843C8.39423 16.1771 8.39423 16.8147 8.78702 17.2086C8.98345 17.4059 9.24228 17.5036 9.50015 17.5036C9.75802 17.5036 10.0158 17.4049 10.2123 17.2095L13.0657 14.3601L15.9191 17.2095C16.1155 17.4059 16.3734 17.5036 16.6312 17.5036C16.8891 17.5036 17.1479 17.4049 17.3444 17.2086C17.7371 16.8147 17.7371 16.1771 17.3432 15.7843Z"/>
          </svg>
          <div class="setting-amount-car-sales-btn-text">{{loc('settingAmountCarSales_2')}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'SettingAmountCarSales',

  data: function() {
    return {
      currentPrice: null
    }
  },

  computed: {
    ...mapState('settingAmountCarSales', ['carModel']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    acceptCurrentPrice: function() {
      if (this.currentPrice) {
        window.mp.trigger('vehTrade:acceptSell', this.currentPrice)
      }
    },

    cancelCurrentPrice: function() {
      window.mp.trigger('vehTrade:cancelSell')
    }
    
  }
}
</script>

<style lang="scss" scoped>
.setting-amount-car-sales{
  width: 100%;
  height: 100vh;
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  left: 0;
  top: 0;
  background: radial-gradient(44.58% 79.26% at 50% 50%, rgba(21, 23, 28, 0) 0%, rgba(11, 13, 18, 0.8) 100%);
  &__wrap{
    display: flex;
    flex-direction: column;
    align-items: center;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/settingAmountCarSales/bg.png');
    border-radius: .3rem;
    padding: 3rem 3.75rem;
    width: 26.5rem;
    text-transform: uppercase;
    text-align: center;
    letter-spacing: 0.1em;
    color: #FFFFFF;
  }
  &__model{
    font-weight: bold;
    font-size: .8rem;
    line-height: .8rem;
    color: #B6D300;
    text-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
    margin-bottom: .5rem;
  }
  &-desc{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.6rem;
    margin-bottom: 1.25rem;
  }
  &-input, &-btn{
    width: 100%;
    height: 3.5rem;
    border-radius: .2rem;
    background: rgba(255, 255, 255, 0.1);
  }
  &-input{
    outline: none;
    box-sizing: border-box;
    padding: 0 1.5rem;
    border: 1px solid rgba(255, 255, 255, 0.1);
    box-sizing: border-box;
    box-shadow: 0 .5rem 1.5rem rgba(0, 0, 0, 0.2);
    margin-bottom: 1rem;
    text-transform: uppercase;
    font-style: normal;
    font-weight: 800;
    font-size: .8rem;
    text-transform: uppercase;
    color: rgba(255, 255, 255, 1);
    &::placeholder{
      color: rgba(255, 255, 255, 0.3);
    }
    &::-webkit-outer-spin-button,
    &::-webkit-inner-spin-button {
      -webkit-appearance: none;
    }
  }
  &-btn{
    display: flex;
    align-items: center;
    justify-content: center;
    transition: .3s;
    svg{
      width: 1.3rem;
      height: 1.3rem;
      margin-right: .75rem;
      fill: white;
      opacity: .3;
      transition: .3s;
    }
    &:hover{
      background: #B6D300;
      box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
      svg{
        opacity: 1;
      }
    }
  }
  &__btns{
    width: 100%;
    display: flex;
    align-items: center;
    &>div{
      margin-right: 1rem;
      &:last-child{
        margin-right: 0;
      }
    }
  }
}
</style>
