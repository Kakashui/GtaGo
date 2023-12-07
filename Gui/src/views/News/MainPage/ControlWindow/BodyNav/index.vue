<template>
  <div class="body-nav">
    <div class="body-nav__option">
      <div 
        class="body-nav__option-checkbox"
        v-if="true"
      >
        <input 
          type="checkbox" 
          name="checkbox" 
          id="checkbox-nav"
          value="true"
        >
        <label for="checkbox-nav">{{loc('news:bnav:hide')}}</label>
      </div>
      <div v-else>
        {{loc('news:bnav:hided')}}
      </div>
    </div>

    <div 
      v-for="(item, index) in navList"
      :key="index"
      :class="[{ active: statusAds === item.key }, 'body-nav__item']"
      @click="setStatusAds(item.key)"
    >
      <svg 
        xmlns="http://www.w3.org/2000/svg" 
        viewBox="0 0 24 24" 
        class="body-nav__item-svg"
      >
        <path :d="item.svg"/>
      </svg>
      <div class="body-nav__item-text">{{loc(item.text)}}</div>
    </div>
    <div 
      class="body-nav__btn-power"
      @click="exitFromNews"
    >
      <div class="body-nav__btn-power-img"></div>
      <div class="body-nav__btn-power-text">{{loc('news:bnav:show')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'BodyNav',

  props: {
    setStatusAds: Function,
    statusAds: String
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  data: function() {
    return {
      navList: [
        {
          text: 'news:bnav:check',
          key: 'Created',
          svg: 'M6 15.4437V17.6389C6 17.8411 6.15887 18 6.36106 18H8.55631C8.65019 18 8.74406 17.9639 8.80905 17.8917L16.6946 10.0133L13.9867 7.30537L6.10832 15.1837C6.03611 15.2559 6 15.3426 6 15.4437ZM18.7888 7.91918C19.0704 7.63755 19.0704 7.18261 18.7888 6.90099L17.099 5.21122C16.8174 4.92959 16.3624 4.92959 16.0808 5.21122L14.7593 6.5327L17.4673 9.24066L18.7888 7.91918Z'
        },
        {
          text: 'news:bnav:hold',
          key: 'Compleate',
          svg: 'M9.64281 15.2999L6.89281 12.5499C6.58638 12.2435 6.09924 12.2435 5.79281 12.5499C5.48638 12.8563 5.48638 13.3435 5.79281 13.6499L9.08495 16.942C9.39138 17.2485 9.88638 17.2485 10.1928 16.942L18.5214 8.62133C18.8278 8.3149 18.8278 7.82775 18.5214 7.52133C18.215 7.2149 17.7278 7.2149 17.4214 7.52133L9.64281 15.2999Z'
        },
        {
          text: 'news:bnav:cancel',
          key: 'Canceled',
          svg: 'M17.4 6.60841C17.0657 6.27412 16.5257 6.27412 16.1914 6.60841L12 10.7913L7.80853 6.59984C7.47424 6.26555 6.93424 6.26555 6.59996 6.59984C6.26567 6.93412 6.26567 7.47412 6.59996 7.80841L10.7914 11.9998L6.59996 16.1913C6.26567 16.5255 6.26567 17.0655 6.59996 17.3998C6.93424 17.7341 7.47424 17.7341 7.80853 17.3998L12 13.2084L16.1914 17.3998C16.5257 17.7341 17.0657 17.7341 17.4 17.3998C17.7342 17.0655 17.7342 16.5255 17.4 16.1913L13.2085 11.9998L17.4 7.80841C17.7257 7.48269 17.7257 6.93412 17.4 6.60841Z'
        }
      ]
    }
  },

  methods: {
    exitFromNews: function() {
      window.mp.trigger('lsnews::exit')
    }
  }
}
</script>

<style lang="scss" scoped>
.body-nav{
  border-right: 1px solid rgba(51, 51, 51, 0.1);
  width: 12.3rem;
  min-width: 12.3rem;
  padding: 2rem 1rem 1rem;
  position: relative;
  &__option{
    padding-bottom: 1.5rem;
    border-bottom: 1px solid rgba(51, 51, 51, 0.1);
    margin-bottom: 1.5rem;
    font-weight: 600;
    font-size: .9rem;
    color: #46555F;
    &>div{
      margin-left: .6rem;
    }
    &-checkbox{
      display: flex;
      align-items: center;
      input{
        opacity: 0;
        position: absolute;
        &+label{
          position: relative;
          display: flex;
          align-items: center;
          padding-left: 1.7rem;
          &:before, &:after{
            content: '';
            position: absolute;
            border-radius: 50%;
            box-sizing: border-box;
            width: 1rem;
            height: 1rem;
            left: 0;
            border: .1rem solid #AFB9C5;
          }
          &:before{
            opacity: 1;
            z-index: 2;
          }
          &:after{
            opacity: 0;
            z-index: 1;
            background-color: #E30000;
            border: .25rem solid #F3F5F7;
          }
        }
        &:checked{
          &+label{
            &:after{
              opacity: 1;
            }
          }
        }
      }
    }
  }
  &__item{
    display: flex;
    align-items: center;
    justify-content: flex-start;
    position: relative;
    padding: 0 .8rem 0 .6rem;
    height: 2.4rem;
    background-color: transparent;
    border-radius: .3rem;
    color: #46555F;
    transition: .2s;
    margin-bottom: .15rem;
    fill: #AFB9C5;
    &:hover, &.active{
      background-color: #DFE3E8;
      color: #D92C2C;
      fill: #D92C2C; 
    }
    &-svg{
      width: 1.2rem;
      height: 1.2rem;
      margin-right: .4rem;
    }
    &-text{
      font-weight: 600;
      font-size: .9rem;
    }
  }
  &__btn-power{
    display: flex;
    align-items: center;
    position: absolute;
    bottom: 1rem;
    left: 1rem;
    &-img{
      width: 1.9rem;
      height: 1.9rem;
      margin-right: .4rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/news/power.png');
    }
    &-text{
      font-weight: 600;
      font-size: 1rem;
      color: #46555F;
    }
  }
}
</style>
