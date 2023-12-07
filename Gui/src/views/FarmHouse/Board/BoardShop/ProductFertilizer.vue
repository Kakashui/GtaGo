<template>
  <div class="product-fertilizer">
    <div class="product-fertilizer__exp">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 22 22" fill="none">
        <path d="M16.1744 18.9983C16.0481 18.9988 15.9249 18.9613 15.8223 18.8912L10.9991 15.5616L6.17585 18.8912C6.0728 18.9623 5.94866 19.0004 5.82136 19C5.69407 18.9996 5.57022 18.9606 5.46772 18.8887C5.36522 18.8168 5.28938 18.7158 5.25117 18.6002C5.21296 18.4845 5.21436 18.3603 5.25515 18.2455L7.13629 12.9402L2.26094 9.75673C2.15534 9.68785 2.07564 9.58862 2.03348 9.47354C1.99132 9.35846 1.98892 9.23356 2.02662 9.11708C2.06432 9.00061 2.14016 8.89866 2.24304 8.82615C2.34592 8.75363 2.47046 8.71434 2.59847 8.71402H8.61314L10.4283 3.39513C10.4674 3.2803 10.5437 3.18021 10.6462 3.10922C10.7488 3.03824 10.8723 3 10.9991 3C11.1258 3 11.2494 3.03824 11.3519 3.10922C11.4545 3.18021 11.5308 3.2803 11.5699 3.39513L13.385 8.71581H19.3997C19.5278 8.71575 19.6526 8.75476 19.7558 8.82714C19.859 8.89952 19.9352 9.00145 19.9731 9.11801C20.011 9.23458 20.0088 9.35965 19.9667 9.47491C19.9246 9.59016 19.8448 9.68955 19.7391 9.75852L14.8618 12.9402L16.7418 18.2441C16.7723 18.33 16.7809 18.4215 16.7668 18.5112C16.7528 18.6008 16.7166 18.686 16.6611 18.7597C16.6057 18.8333 16.5327 18.8934 16.448 18.9349C16.3634 18.9764 16.2696 18.9981 16.1744 18.9983Z" fill="white"/>
      </svg>
      <span>+ {{fertilizer.exp}} EXP</span>
    </div>
    <div class="product-fertilizer__param">
      <svg 
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 22 22" 
        fill="none"
      >
        <path d="M11 9.96875V6.53125" stroke="#B6D300" stroke-linecap="round" stroke-linejoin="round"/>
        <path d="M5.67188 5.67188L5.15625 5.15626M11 3.78125V3.09375V3.78125Z" stroke="#B6D300" stroke-linecap="round" stroke-linejoin="round"/>
        <path d="M11 13.0625C11.7594 13.0625 12.375 12.4469 12.375 11.6875C12.375 10.9281 11.7594 10.3125 11 10.3125C10.2406 10.3125 9.625 10.9281 9.625 11.6875C9.625 12.4469 10.2406 13.0625 11 13.0625Z" stroke="#B6D300" stroke-miterlimit="10"/>
        <path d="M11 4.125C9.50426 4.125 8.04213 4.56853 6.79848 5.39951C5.55484 6.23049 4.58553 7.41159 4.01315 8.79346C3.44076 10.1753 3.291 11.6959 3.5828 13.1629C3.8746 14.6299 4.59486 15.9774 5.65249 17.035C6.71012 18.0926 8.05763 18.8129 9.52461 19.1047C10.9916 19.3965 12.5122 19.2467 13.894 18.6743C15.2759 18.102 16.457 17.1327 17.288 15.889C18.119 14.6454 18.5625 13.1832 18.5625 11.6875C18.5625 9.6818 17.7657 7.75825 16.3475 6.34001C14.9292 4.92176 13.0057 4.125 11 4.125Z" stroke="#B6D300" stroke-miterlimit="10"/>
      </svg>
      <span>- {{fertilizer.minTime}} {{loc('farmHouse_5')}}</span>
    </div>
    <div class="product-fertilizer__block">
      <div 
        class="product-fertilizer__block__image"
        :style="{backgroundImage: `url(/img/farmHouse/fertilizers/${fertilizer.img}.png)`}"
      ></div>
      <div class="product-fertilizer__count">x10</div>
      <div 
        class="product-fertilizer__block-btn"
        @click="buyProduct"
      >
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
          <path d="M23.0139 5.00645H4.24886L3.96992 1.89901C3.9243 1.39084 3.49841 1.00146 2.98823 1.00146H0.98564C0.441285 1.00146 0 1.44275 0 1.9871C0 2.53146 0.441285 2.97274 0.98564 2.97274H2.08712C2.68906 9.67894 1.13335 -7.6541 3.22074 15.6025C3.30119 16.5128 3.79302 17.5005 4.6403 18.1717C3.1127 20.1225 4.50846 22.9985 6.99335 22.9985C9.05578 22.9985 10.5104 20.9415 9.80317 18.9933H15.1977C14.4913 20.939 15.9429 22.9985 18.0075 22.9985C19.6553 22.9985 20.9958 21.6579 20.9958 20.0102C20.9958 18.3625 19.6553 17.022 18.0075 17.022H7.00001C6.2513 17.022 5.59886 16.5703 5.31725 15.9122L21.0691 14.9864C21.4991 14.9612 21.8629 14.6595 21.9675 14.2416L23.9702 6.23113C24.1255 5.60999 23.6553 5.00645 23.0139 5.00645ZM6.99335 21.0272C6.43266 21.0272 5.97645 20.571 5.97645 20.0102C5.97645 19.4495 6.43266 18.9933 6.99335 18.9933C7.55408 18.9933 8.01029 19.4495 8.01029 20.0102C8.01029 20.571 7.55408 21.0272 6.99335 21.0272ZM18.0075 21.0272C17.4468 21.0272 16.9906 20.571 16.9906 20.0102C16.9906 19.4495 17.4468 18.9933 18.0075 18.9933C18.5682 18.9933 19.0244 19.4495 19.0244 20.0102C19.0244 20.571 18.5682 21.0272 18.0075 21.0272ZM20.2306 13.061L5.05192 13.9531L4.4258 6.97768H21.7516L20.2306 13.061Z" fill="white"/>
        </svg>
      </div>
    </div>
    <div class="product-fertilizer__info">
      <div class="product-fertilizer__info-title">{{loc(fertilizer.title)}}</div>
      <div class="product-fertilizer__info-cost">{{fertilizer.cost}}$</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ProductFertilizer',
  props: {
    fertilizer: Object
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    buyProduct: function() {
      this.$emit('buyProduct', this.fertilizer.id)
    }
  }
}
</script>

<style lang="scss" scoped>
.product-fertilizer{
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  position: relative;
  $fertilizer: &;
  &:hover{
    & #{$fertilizer}__block{
      &-btn{
        opacity: 1;
      }
    }
  }
  &__exp, &__param{
    position: absolute;
    right: -.6rem;
    border-radius: 1rem;
    padding: .3rem .4rem;
    font-size: .8rem;
    letter-spacing: 0.04em;
    display: flex;
    align-items: center;
    font-weight: bold;
    color: #FFFFFF;
    z-index: 1;
  }
  &__exp{
    top: -1.3rem;
    background: #B48819;
    svg{
      width: 1.1rem;
      height: 1.1rem;
      fill: #fff;
      margin-right: .3rem;
    }
  }
  &__param{
    top: .55rem;  
    background: #383838;
    svg{
      width: 1.1rem;
      height: 1.1rem;
      margin-right: .3rem;
    }
  }
  &__block{
    display: flex;
    justify-content: center;
    align-items: center;
    position: relative;
    background: rgba(255, 255, 255, 0.1);
    border: 1px solid rgba(255, 255, 255, 0.16);
    width: 6.5rem;
    height: 6.5rem;
    margin-bottom: .5rem;
    &-btn{
      width: 2rem;
      height: 2rem;
      border-radius: 50%;
      background: #A2BC05;
      display: flex;
      align-items: center;
      justify-content: center;
      position: absolute;
      right: -.5rem;
      bottom: -.5rem;
      opacity: 0;
      transition: .3s;
      svg{
        width: 1.2rem;
        height: 1.2rem;
      }
      &:active{
        transform: scale(.9);
      }
    }
    &__image{
      width: 4.3rem;
      height: 5.7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
  }
  &__count{
    font-size: 1rem;
    position: absolute;
    left: .3rem;
    bottom: .1rem;
  }
  &__info{
    display: flex;
    align-items: flex-end;
    justify-content: space-between;
    margin-bottom: .4rem;
    width: 100%;
    &-title{
      font-size: .9rem;
      line-height: 1.05rem;
      letter-spacing: 0.02em;
      width: 75%;
    }
    &-cost{
      font-weight: bold;
      font-size: 1.2rem;
      line-height: 1.2rem;
      letter-spacing: 0.04em;
    }
  }
}
</style>
