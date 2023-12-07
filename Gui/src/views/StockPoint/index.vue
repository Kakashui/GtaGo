<template>
  <div v-if="isMed"  class="stock-point">
    <div class="stock-point__box">
      <img
        src="/img/stockPoint/medicine.png"
        alt="icon"
        class="box__img"
      >
      <span class="box__title">{{loc('gui_769')}}</span>
      <div class="box__actions">
        <button class="actions__btn" @click="load(2)">
          <div class="btn__inner">
            <svg class="inner__icon" width="30" height="25" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_735')}}</span>
          </div>
        </button>
        <button class="actions__btn" @click="unload(2)">
          <div class="btn__inner">
            <svg class="inner__icon" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_736')}}</span>
          </div>
        </button>
      </div>
    </div>
  </div>
  <div v-else  class="stock-point">

      <div class="stock-point__box" v-if="!isArmy">
        <img
          src="/img/stockPoint/drugs.png"
          alt="icon"
          class="box__img"
        >
        <span class="box__title">{{loc('gui_770')}}</span>
        <div class="box__actions">
        <button class="actions__btn" @click="load(1)">
          <div class="btn__inner">
            <svg class="inner__icon" width="30" height="25" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_735')}}</span>
          </div>
        </button>
        <button class="actions__btn" @click="unload(1)">
          <div class="btn__inner">
            <svg class="inner__icon" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_736')}}</span>
          </div>
        </button>
      </div>
    </div> 
    
    <div class="stock-point__box" v-if="!isArmy">
      <img
      src="/img/stockPoint/bands.png"
      alt="icon"
      class="box__img"
      >
      <span class="box__title">{{loc('gui_771')}}</span>
      <div class="box__actions">
        <button class="actions__btn" @click="load(0)">
          <div class="btn__inner">
            <svg class="inner__icon" width="30" height="25" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_735')}}</span>
          </div>
        </button>
        <button class="actions__btn" @click="unload(0)">
          <div class="btn__inner">
            <svg class="inner__icon" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_736')}}</span>
          </div>
        </button>
      </div>
    </div> 

    <div class="stock-point__box" v-if="isArmy">
      <img
      src="/img/stockPoint/army.png"
      alt="icon"
      class="box__img"
      >
      <span class="box__title">{{loc('gui_771')}}</span>
      <div class="box__actions">
        <button class="actions__btn" @click="load(0)">
          <div class="btn__inner">
            <svg class="inner__icon" width="30" height="25" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_735')}}</span>
          </div>
        </button>
        <button class="actions__btn" @click="unload(0)">
          <div class="btn__inner">
            <svg class="inner__icon" viewBox="0 0 30 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.929 0H8.64328C8.09099 0 7.64328 0.447717 7.64328 1V8.92857H3.25182C2.89546 8.92857 2.71699 9.35943 2.96897 9.61141L14.079 20.7215C14.4696 21.112 15.1027 21.112 15.4932 20.7215L26.6033 9.61141C26.8553 9.35943 26.6768 8.92857 26.3204 8.92857H21.929V1C21.929 0.447715 21.4813 0 20.929 0Z" fill="#262626"/>
              <path d="M0.5 10.7142L13.3715 23.5857C14.1525 24.3668 15.4189 24.3668 16.1999 23.5857L29.0714 10.7142" stroke="#262626" stroke-width="0.4" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            <span class="inner__text">{{loc('gui_736')}}</span>
          </div>
        </button>
      </div>
    </div>  
  </div>
  
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  computed: {
    ...mapState('stockPoint', ['isMed', 'isArmy']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    load: function (id){
      window.mp.trigger('matsL', id)
    },
    unload: function (id){
      window.mp.trigger('matsU', id)
    }
  }
}
</script>

<style lang="scss" scoped>
.stock-point {
  display: flex;
  width: 80%;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  
  &__box {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin: 2rem;
    .box__img {
      width: 5.5rem;
    }
    .box__title {
      margin: 1rem 0 0 0;
      font-size: 1.5rem;
      color: #fff;
      text-shadow: 0 0 6px rgba(0, 0, 0, 0.7  );
    }
    .box__actions {
      display: flex;
      margin: .5rem 0 0 0;
      .actions__btn {
        width: 9rem;
        height: 2.5rem;
        transform: skew(20deg);
        border-radius: .6rem;
        position: relative;
        text-transform: uppercase;
        display: flex;
        align-items: center;
        justify-content: center;
        &:hover{
          transform: scale(1.05) skew(20deg);
        }
        &:first-child {
          background-color: #ABE95D;
          .inner__icon {
            transform: rotate(180deg);
          }
        }
        &:last-child {
          margin:  0 0 0 3rem;
          background-color: #fff;
        }
        &::after {
          content: '';
          width: 9.4rem;
          height: 3rem;
          border: 1px solid #ABE95D;
          position: absolute;
          transform: translate(-50%, -50%);
          top: 50%;
          left: 50%;
          border-radius: .6rem;
        }
        .btn__inner {
          display: flex;
          align-items: center;
          transform: skew(-20deg);
          .inner__icon {
            width: 1.5rem;
          }
          .inner__text {
            margin: 0 0 0 .2rem;
            font-size: .8rem;
          }
        }
      }
    }
  }
}
</style>
