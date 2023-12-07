<template>
  <div class="win-notification">
    <div 
      class="win-notification__img"
    >
      <img 
        :src="`/img/newDonateShop/items/${item.image}.png`"
        alt="item"
      >
      <div 
        class="block-shadow"
        :style="{backgroundColor: item.color}"
      ></div>
    </div>
    <div class="win-notification__info">
      <div 
        class="decorate"
        :style="{backgroundColor: item.color}"
      ></div>
      <div class="desc">{{winNotification.nickname}} {{loc('WinNotification_0')}}</div>
      <div class="win-item">{{item.name}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import { itemById } from '../../configs/newDonateShop'
export default {
  name: 'WinNotification',

  data: function() {
    return{
      item: null
    }
  },


  computed: {
    ...mapState('hud', ['winNotification']),
    ...mapGetters('localization', ['loc'])
  },

  created(){
    this.item = itemById(this.winNotification.itemId)
  }
}
</script>

<style lang="scss" scoped>
@keyframes showInterface {
  from {
    opacity: 0;
    transform: translateX(-50%) scale(.3);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) scale(1);
  }
}
@keyframes rotateDecorate {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
.win-notification{
  display: flex;
  align-items: center;
  position: absolute;
  left: 50%;
  top: 3rem;
  transform: translateX(-50%);
  text-transform: uppercase;
  font-weight: bold;
  color: #FFFFFF;
  text-shadow: 0 .1rem .35rem rgba(0, 0, 0, 0.2);
  animation: showInterface .7s;
  &__img{
    width: 5rem;
    height: 5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    margin-right: 1.5rem;
    img{
      width: 100%;
      height: 100%;
      opacity: .8;
    }
    .block-shadow{
      position: absolute;
      z-index: -1;
      width: 6.5rem;
      height: 6.5rem;
      border-radius: 50%;
      filter: blur(2.5rem);
    }
    &:before{
      content: '';
      width: 11rem;
      height: 11rem;
      position: absolute;
      z-index: -1;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/winNotification/decorate.svg');
      transform-origin: 50% 50%;
      animation: rotateDecorate 12s linear infinite;
    }
  }
  &__info{
    display: flex;
    justify-content: center;
    align-items: flex-start;
    flex-flow: column;
    padding-left: .65rem;
    position: relative;
    .decorate{
      width: .15rem;
      height: 2.5rem;
      position: absolute;
      left: 0;
      align-self: center;
    }
    .desc{
      font-size: 1.1rem;
      line-height: 1.25rem;
    }
    .win-item{
      font-size: 1.5rem;
      line-height: 1.7rem;
    }
  }
}
</style>
