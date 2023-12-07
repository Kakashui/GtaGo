<template>
  <div class="actions__slider">
    <span class="slider__header">{{ currentTitle }}</span>
    <div class="slider__main">
      <button @click="slide(-1)" class="main__button prev">
        <img
          src="/img/barbershop/back.svg"
          alt="prev"
          class="button__icon"
        >
      </button>
      <transition-group
        tag="div"
        class="main__group"
        :name="transitionName"
      >
        <div :key="value.style" class="group__slide">
          <span class="slide__title">{{ `${currentTitle} ${loc('barber_2')} ${value.style}` }}</span>
        </div>
      </transition-group>
      <button @click="slide(1)" class="main__button next">
        <img
          src="/img/barbershop/back.svg"
          alt="next"
          class="button__icon"
        >
      </button>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'TypeSlider',

  props: ['currentTitle', 'currentTypes', 'value'],

  data: function() {
    return {
      direction: 1,
      transitionName: ''
    }
  },
  watch: {
    currentTypes(){
      this.value.style = 0;
      this.value.color = 0;
    }
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    slide(dir) {
      this.value.style += dir;
      if(this.currentTypes.hair){
        if(this.value.style == this.currentTypes.hair + 1) this.value.style = 500;
        else if(this.value.style == 499) this.value.style = this.currentTypes.hair;       
      }
      
      if(this.currentTypes.exclude > 0 && this.currentTypes.exclude.indexOf(this.value.style) !== -1) {
        this.slide(dir);
        return;
      } 
      
      if(this.value.style > this.currentTypes.max) this.value.style = this.currentTypes.min;
      else if(this.value.style < this.currentTypes.min) this.value.style = this.currentTypes.max;
      
      this.$emit("onChange");
    }
  }
}
</script>

<style lang="scss" scoped>
.actions__slider {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: .8rem;
  background: linear-gradient(90deg, rgba(26, 26, 26, 0.46) 0%, #1E1E1E 49.48%, rgba(26, 26, 26, 0.43) 100%);
  border-radius: .6rem;
  .slider__header {
    color:#ADADAD;
    font-weight: bold;
    font-size: .8rem;
    letter-spacing: .05rem;
    text-transform: uppercase;
  }
  .slider__main {
    display: flex;
    margin: .5rem 0 0 0;
    .main__button {
      width: 2rem;
      height: 2rem;
      background-color: #FFFFFF;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      &.next {
        .button__icon {
          transform: rotate(180deg);
        }
      }
      &:hover {
        box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.6);
        transition: all .15s;
        background: center / cover no-repeat url('/img/common/textures/texture_button_go_now.jpg');
        .button__icon {
          filter: invert(1);
        }
      }
      .button__icon {
        width: 55%;
        height: auto;
      }
    }
    .main__group {
      position: relative;
      width: 12rem;
      height: 3rem;
      .group__slide {
        position: absolute;
        left: 0;
        right: 0;
        bottom: 0;
        top: 18%;
        color: #fff;
        display: flex;
        flex-direction: column;
        align-items: center;
        .slide__price {
          font-size: .9rem;
          margin: .4rem 0 0 0;
        }
      }
    }
  }
  
}

.slide-next-enter-active,
.slide-next-leave-active {
  transition: all 0.2s;
}
.slide-next-enter {
  transform: translate(25%);
  opacity: 0;
}
.slide-next-leave-to {
  transform: translate(-25%);
  opacity: 0;
}

.slide-prev-enter-active,
.slide-prev-leave-active {
  transition: all 0.2s;
}
.slide-prev-enter {
  transform: translate(-25%);
  opacity: 0;
}
.slide-prev-leave-to {
  transform: translate(25%);
  opacity: 0;
}
</style>
