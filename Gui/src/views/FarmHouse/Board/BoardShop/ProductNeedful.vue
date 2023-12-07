<template>
  <!-- <div 
    :class="[{'watering-can': needful.id === 'watering-can'}, {'box': needful.id === 'box'}, 'product-needful']"
    :style="{backgroundImage: `url(/img/farmHouse/${needful.img}-bg.png)`}"
  > -->
  <div 
    class="product-needful"
    :style="{backgroundImage: `url(/img/farmHouse/${needful.img}.png)`}"
  >
    <div class="product-needful-title">{{loc(needful.title)}}</div>
    <div class="product-needful-cost">${{needful.cost}}</div>
    <div 
      class="product-needful-param"
      v-if="needful.paramType" 
    ><span :style="{backgroundImage: `url(/img/farmHouse/${needful.paramType})`}"></span>{{loc(needful.param)}}</div>
    <div 
      class="product-needful__btn"
      @click="buyProduct"
    >
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
        <path d="M23.0139 5.00645H4.24886L3.96992 1.89901C3.9243 1.39084 3.49841 1.00146 2.98823 1.00146H0.98564C0.441285 1.00146 0 1.44275 0 1.9871C0 2.53146 0.441285 2.97274 0.98564 2.97274H2.08712C2.68906 9.67894 1.13335 -7.6541 3.22074 15.6025C3.30119 16.5128 3.79302 17.5005 4.6403 18.1717C3.1127 20.1225 4.50846 22.9985 6.99335 22.9985C9.05578 22.9985 10.5104 20.9415 9.80317 18.9933H15.1977C14.4913 20.939 15.9429 22.9985 18.0075 22.9985C19.6553 22.9985 20.9958 21.6579 20.9958 20.0102C20.9958 18.3625 19.6553 17.022 18.0075 17.022H7.00001C6.2513 17.022 5.59886 16.5703 5.31725 15.9122L21.0691 14.9864C21.4991 14.9612 21.8629 14.6595 21.9675 14.2416L23.9702 6.23113C24.1255 5.60999 23.6553 5.00645 23.0139 5.00645ZM6.99335 21.0272C6.43266 21.0272 5.97645 20.571 5.97645 20.0102C5.97645 19.4495 6.43266 18.9933 6.99335 18.9933C7.55408 18.9933 8.01029 19.4495 8.01029 20.0102C8.01029 20.571 7.55408 21.0272 6.99335 21.0272ZM18.0075 21.0272C17.4468 21.0272 16.9906 20.571 16.9906 20.0102C16.9906 19.4495 17.4468 18.9933 18.0075 18.9933C18.5682 18.9933 19.0244 19.4495 19.0244 20.0102C19.0244 20.571 18.5682 21.0272 18.0075 21.0272ZM20.2306 13.061L5.05192 13.9531L4.4258 6.97768H21.7516L20.2306 13.061Z" fill="white"/>
      </svg>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ProductNeedful',
  props: {
    needful: Object
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    buyProduct: function() {
      this.$emit('buyProduct', this.needful.id)
    }
  }
}
</script>

<style lang="scss" scoped>
.product-needful{
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  justify-content: space-between;
  background-size: contain;
  background-position: center;
  background-repeat: no-repeat;
  position: relative;
  padding: 1rem 1.5rem .4rem 2rem;
  font-weight: bold;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  $needful: &;
  &:hover{
    & #{$needful}__btn{
      opacity: 1;
    }
  }
  // &.watering-can, &.box{
  //   &:before{
  //     content: '';
  //     background-size: contain;
  //     background-repeat: no-repeat;
  //     background-position: center;
  //     z-index: 1;
  //     position: absolute;
  //   }
  // }
  // &.watering-can{
  //   background-image: url('/img/farmHouse/watering-can-bg.png');
  //   &:before{
  //     left: -1.35rem;
  //     top: .15rem;
  //     width: 5.35rem;
  //     height: 4.25rem;
  //     background-image: url('/img/farmHouse/watering-can.png');
  //   }
  // }
  // &.box{
  //   background-image: url('/img/farmHouse/box-bg.png');
  //   &:before{
  //     left: .5rem;
  //     top: .9rem;
  //     width: 4.8rem;
  //     height: 3.7rem;
  //     background-image: url('/img/farmHouse/box.png');
  //   }
  // }
  &-title{
    text-transform: uppercase;
    font-size: 1rem;
  }
  &-cost{
    font-size: 1.1rem;
    color: rgba(255, 255, 255, 0.7);
    margin-bottom: .6rem;
  }
  &-param{
    width: 100%;
    font-size: .9rem;
    text-align: left;
    letter-spacing: 0.04em;
    display: flex;
    align-items: center;
    span{
      width: 1rem;
      height: 1rem;
      margin-right: .3rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
  }
  &__btn{
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
}
</style>
