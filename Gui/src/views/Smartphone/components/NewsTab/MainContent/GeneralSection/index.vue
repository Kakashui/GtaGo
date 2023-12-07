<template>
  <transition name="news-slide">
    <div class="general-section">
      <div class="general-section__header">
        <button
          class="btn"
          @click="isCreateAd = true"
        >{{loc('phone:advert:7')}}</button>
      </div>
      <!-- <simplebar class="general-section__list simplebar__list-dark"> -->
        <!-- <div v-bar> -->
          <div class="general-section__list">
            <list-item
              v-for="(item, index) in newsData"
              :key="index"
              :item="item"
            />
          </div>
        <!-- </div> -->
      <!-- </simplebar> -->
      <transition name="smartphone-fade">
        <div
          class="general-section__layer"
          v-if="isCreateAd"
        ></div>
      </transition>
      <popup-create-ad
        v-if="isCreateAd"
        @close="isCreateAd = false"
      />
    </div>
  </transition>
</template>

<script>
// import Simplebar from 'simplebar-vue'
// import 'simplebar-vue/dist/simplebar.min.css'

import { mapState, mapMutations, mapGetters } from 'vuex'
import ListItem from './ListItem'
import PopupCreateAd from './PopupCreateAd'

export default {
  name: 'ListSection',

  components: {
    ListItem,
    // Simplebar,
    PopupCreateAd
  },

  data: function() {
    return {
      isCreateAd: false
    }
  },

  computed: {
    ...mapState('smartphone/newsPage', ['newsData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes slide {
  from {
    transform: translateX(100%);
  }
  to {
    transform: translateX(0);
  }
}
@keyframes slideSecond {
  from {
    transform: translateX(0);
  }
  to {
    transform: translateX(0);
  }
}
.general-section {
  width: 100%;
  height: 100%;
  // display: flex;
  // flex-direction: column;
  background: #F3F2F7;
  padding: 2.25rem 0 0 0;
  position: relative;
  &::after {
    content: '';
    position: absolute;
    width: 100%;
    height: 4rem;
    bottom: 0;
    left: 0;
    z-index: 0;
    background: linear-gradient(180deg, rgba(243, 242, 247, 0) 0%, #F3F2F7 100%);
  } 
  &__header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 0.75rem 0.5rem 1rem;
    position: relative;
    &::before {
      content: '';
      width: 3.5rem;
      height: 1.55rem;
      background: center / contain no-repeat url("/img/hud/smartphone/newsTab/main/logo.png");
    }
    &::after {
      content: '';
      position: absolute;
      width: 13.7rem;
      height: 1px;
      background: #EDEDED;
      bottom: 0;
      left: 50%;
      transform: translate(-50%, 0);
    }
    .btn {
      width: 8rem;
      height: 2.2rem;
      border: 0.15rem solid rgba(255, 255, 255, 0.3);
      background: #EE443A;
      box-shadow: 0px 0.35rem 1rem rgba(238, 68, 58, 0.4);
      border-radius: 5rem;
      transition: all 0.15s;
      font-size: 0.65rem;
      color: #fff;
      text-shadow: 0px 0.35rem 1rem rgba(238, 68, 58, 0.4);
      &:hover {
        box-shadow: none;
      }
    }
  }
  &__list {
    padding: 0 0 0 0.5rem;
    height: 26.1rem;
    position: relative;
    overflow-y: scroll;
    &::-webkit-scrollbar {
      width: 0;
    }
    &::-webkit-scrollbar-track {
      border: solid 0 transparent;
      margin: .1rem;
    }
    &::-webkit-scrollbar-thumb {
      border: solid 0 transparent;      
      background-color: rgba(#fff,.1);
    }
  }
  &__layer {
    position: absolute;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
    background: rgba(0, 0, 0, 0.7);
  }
}
.news-slide-enter-active {
  animation: slide 0.3s linear;
  position: absolute;
  z-index: 2;
}
.news-slide-leave-active  {
  animation: slideSecond 0.3s;
  z-index: 0;
}
</style>
