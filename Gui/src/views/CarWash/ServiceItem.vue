<template>
  <div 
    :class="[{active: basketList.includes(item)}, 'service-item']"
    @click="$emit('interactionWidthBasketList', item)"
  >
    <div 
      class="service-item__img"
      :style="{backgroundImage: `url(/img/carWash/services/${item.img})`}"
    ></div>
    <div class="service-item__text">{{loc(item.text)}}</div>
    <div class="service-item__cost">$ {{item.cost.toLocaleString('ru')}}</div>
    <div class="service-item__label">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
       <path 
          v-if="basketList.includes(item)"
          d="M19.5 6L9 18L4.5 13.5"
          stroke-width="2" 
          stroke-linecap="round" 
          stroke-linejoin="round"
        />
        <path 
          v-else
          d="M18.75 12H5.25M12 5.25V18.75V5.25Z"
          stroke-width="2" 
          stroke-linecap="round" 
          stroke-linejoin="round"
        />
      </svg>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ServiceItem',
  props: {
    item: Object,
    basketList: Array
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
.service-item{
  display: flex;
  flex-direction: column;
  position: relative;
  align-items: flex-start;
  justify-content: space-between;
  &.active{
    .service-item__label{
      background-color:  #AFCC3B;
      svg{
        fill: #AFCC3B;
      }
    }
  }
  &__img{
    margin-bottom: .6rem;
    width: 100%;
    height: 8.4rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  &__text{
    font-size: 1rem;
    line-height: 1.15rem;
    letter-spacing: 0.04em;
    color: rgba(255,255,255, .6);
  }
  &__cost{
    font-size: 1.5rem;
    line-height: 1.7rem;
  }
  &__label{
    position: absolute;
    right: 0;
    bottom: 1rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 50%;
    width: 2rem;
    height: 2rem;
    background-color: rgba(255, 255, 255, 0.1);
    svg{
      fill: rgba(255, 255, 255, 0.1);
      width: 1.2rem;
      height: 1.2rem;
      stroke: #fff;
    }
  }
}
</style>
