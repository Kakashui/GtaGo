<template>
  <div :class="[{locked: item.requiredLevel > userLevel.level}, 'level-item']">
    <div class="level-item__board">{{item.requiredLevel}} level</div>
    <div class="level-item__plants">
      <div 
        class="level-item__plants-item"
        v-for="(plant, index) in item.plantHoles"
        :key="index"
      >
        <div 
          class="level-item__plants-item-img"
          :style="{backgroundImage: `url(/img/farmHouse/${plant.img}.png)`}"
        ></div>
        <div class="level-item__plants-item-title">{{loc(plant.title)}}</div>
        <div class="level-item__plants-item-time">
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
          <span>{{plant.time}}</span>
        </div>
        <div class="level-item__plants-item-exp">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 22 22" fill="none">
            <path d="M16.1744 18.9983C16.0481 18.9988 15.9249 18.9613 15.8223 18.8912L10.9991 15.5616L6.17585 18.8912C6.0728 18.9623 5.94866 19.0004 5.82136 19C5.69407 18.9996 5.57022 18.9606 5.46772 18.8887C5.36522 18.8168 5.28938 18.7158 5.25117 18.6002C5.21296 18.4845 5.21436 18.3603 5.25515 18.2455L7.13629 12.9402L2.26094 9.75673C2.15534 9.68785 2.07564 9.58862 2.03348 9.47354C1.99132 9.35846 1.98892 9.23356 2.02662 9.11708C2.06432 9.00061 2.14016 8.89866 2.24304 8.82615C2.34592 8.75363 2.47046 8.71434 2.59847 8.71402H8.61314L10.4283 3.39513C10.4674 3.2803 10.5437 3.18021 10.6462 3.10922C10.7488 3.03824 10.8723 3 10.9991 3C11.1258 3 11.2494 3.03824 11.3519 3.10922C11.4545 3.18021 11.5308 3.2803 11.5699 3.39513L13.385 8.71581H19.3997C19.5278 8.71575 19.6526 8.75476 19.7558 8.82714C19.859 8.89952 19.9352 9.00145 19.9731 9.11801C20.011 9.23458 20.0088 9.35965 19.9667 9.47491C19.9246 9.59016 19.8448 9.68955 19.7391 9.75852L14.8618 12.9402L16.7418 18.2441C16.7723 18.33 16.7809 18.4215 16.7668 18.5112C16.7528 18.6008 16.7166 18.686 16.6611 18.7597C16.6057 18.8333 16.5327 18.8934 16.448 18.9349C16.3634 18.9764 16.2696 18.9981 16.1744 18.9983Z" fill="#ECB21C"/>
          </svg>
          <span>+ {{plant.exp}} exp</span>
        </div>
        <div class="level-item__plants-item-fetus">
          <svg id="Layer_1" enable-background="new 0 0 512.002 512.002" height="512" viewBox="0 0 512.002 512.002" width="512" xmlns="http://www.w3.org/2000/svg"><g>
            <path d="m285.709 114.051c-.125-.685-.248-1.372-.379-2.049h67.671c49.981 0 91.203-38.264 95.61-87.289.421-4.679-3.278-8.711-7.976-8.711h-71.634c-43.185 0-79.691 28.508-91.761 67.708-10.846-27.929-27.47-46.268-42.376-57.938-27.015-21.15-54.353-25.423-55.503-25.595-8.738-1.3-16.925 4.719-18.229 13.458-1.303 8.74 4.682 16.875 13.421 18.178 3.229.519 76.099 13.362 82.049 107.699-1.612 7.199-8.061 12.566-15.766 12.489-8.844-.088-15.818-7.595-15.837-16.439-.012-5.517-.241-10.785-.661-15.815-.244-2.918-2.97-4.985-5.854-4.478-96.846 17.022-170.483 98.771-170.483 196.733 0 110.28 93.31 200 208 200s208-89.72 208-200c0-100.582-77.625-184.047-178.292-197.951z" fill="#fe7700"/></g>
            </svg>
          <span>+ {{plant.fetus}} {{loc(' к урожаю')}}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'LevelItem',
  props: {
    item: Object
  },
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapGetters('farmHouse', ['userLevel'])
  }
}
</script>

<style lang="scss" scoped>
.level-item{
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  border-right: 1px solid rgba(255, 255, 255, 0.1);
  $item: &;
  &:nth-child(3n+4){
    &:before{
      content: '';
      width: calc(300% + 2*(2.5rem));
      height: 1px;
      background: rgba(255, 255, 255, 0.1);
      position: absolute;
      left: -2.5rem;
      top: 0;
    }
  }
  &:nth-child(3n){
    border-right: 0;
  }
  &.locked{
    opacity: .4;
    &:after{
      content: '';
      width: 4rem;
      height: 4rem;
      position: absolute;
      z-index: 2;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/farmHouse/lock.png');
    }
    & #{$item}__board{
      background-image: url('/img/farmHouse/plate-locked.png');
    }  
  }
  &__board{
    position: absolute;
    display: flex;
    align-items: center;
    justify-content: center;
    top: .85rem;
    width: 9.1rem;
    height: 3.2rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/farmHouse/plate.png');
    font-style: normal;
    font-weight: bold;
    font-size: 1.6rem;
    text-align: center;
    letter-spacing: 0.04em;
    text-transform: uppercase;
    transform: rotate(-8deg);
    z-index: 0;
  }
  &__plants{
    z-index: 1;
    display: flex;
    align-items: center;
    justify-content: center;
    font-style: normal;
    font-size: .8rem;
    &-item{
      margin-right: .5rem;
      &:last-child{
        margin-right: 0;
      }
      svg{
        width: 1.1rem;
        height: 1.1rem;
        margin-right: .4rem;
      }
      &-img{
        height: 3.7rem;
        width: 3rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        margin-bottom: .6rem;
      }
      &-title{
        font-weight: bold;
        margin-bottom: .6rem;
        max-width: 5rem;
      }
      &-time, &-exp, &-fetus{
        display: flex;
        align-items: center;
        font-weight: normal;
        color: rgba(255, 255, 255, 0.6);
      }
      &-time, &-exp{
        margin-bottom: .45rem;
      }
    }
  }
}
</style>
