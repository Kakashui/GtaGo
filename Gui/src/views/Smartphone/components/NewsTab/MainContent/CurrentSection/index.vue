<template>
  <transition name="news-slide">
    <div class="current-section">
      <div class="current-section__header">
        <button class="btn-back" @click="setCurrentTab({ key: currentTab.key, section: 'GeneralSection' })"></button>
        <span class="title">{{loc('phone:advert:1')}}</span>
      </div>
      <div v-bar>
        <div class="current-section__main">
          <img
            :src="currentAd.Picture"
            class="img"
            @error="$event.target.src='/img/hud/smartphone/newsTab/main/default.png'"
          >
          <p class="title">{{ currentAd.Title }}</p>
          <div class="actions">
            <button class="actions__btn actions__btn-call" @click="call">{{loc('phone:advert:2')}}</button>
            <button class="actions__btn actions__btn-msg" @click="message">{{loc('phone:advert:3')}}</button>
          </div>
          <div class="info">
            <div class="info__part info__part-left">
              <span class="desc">{{loc('phone:advert:4')}}</span>
              <span class="value">{{ currentAd.Sender }}</span>
            </div>
            <div class="info__part info__part-right">
              <span class="desc">{{loc('phone:advert:5')}}</span>
              <span class="value">{{ currentAd.Simnum }}</span>
            </div>
          </div>
          <div class="footer">
            <img src="img/hud/smartphone/newsTab/main/logoSmall.png" class="img">
            <div class="footer__info">
              <span class="desc">{{loc('phone:advert:6')}}</span>
              <span class="value">{{ currentAd.Editor }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'CurrentSection',

  data: function() {
    return {
      currentAd: null
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/newsPage', ['newsData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme', 'setCurrentTab']),

    getCurrentAd: function() {
      this.currentAd = this.newsData.find(item => item.Id === this.currentTab.data.id)
    },

    call: function() {
      
      window.setData('smartphone/setCallData', { destination: this.currentAd.Simnum, status: 'outcome' })
      window.setData('smartphone/setCurrentTab', { key: 'CallTab', data: {theme: 'dark'} })
    },

    message: function() {
      window.mp.triggerServer('phone::contacts::goToMsg', this.currentAd.Simnum);
    }
  },

  created: function() {
    this.setColorTheme({ header: 'light', button: 'none' })
    this.getCurrentAd()
  }
}
</script>

<style lang="scss" scoped>
@keyframes slide {
  from {
    transform: translateX(-100%);
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
@keyframes shake {
  25% {
    transform: translateX(0.5rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.25rem);
  }
}
.current-section {
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  background: #fff;
  &__header {
    padding: 2.5rem 1rem 1rem 1rem;
    display: flex;
    align-items: center;
    position: relative;
    background: center / cover url("/img/hud/smartphone/newsTab/main/texture1.png"), #EE443A;
    .btn-back {
      width: 2rem;
      height: 2rem;
      background: rgba(255, 255, 255, 0.3);
      border-radius: 0.4rem;
      display: flex;
      align-items: center;
      justify-content: center;
      &:hover {
        &::after {
          animation: shake 0.4s;
        }
      }
      &::after {
        content: '';
        background: center / cover url("/img/hud/smartphone/newsTab/main/icons/chevronRight.svg");
        width: 0.35rem;
        height: 0.6rem;
        margin: 0 0.2rem 0 0;
      }
    }
    .title {
      position: absolute;
      left: 50%;
      transform: translate(-50%, 0);
      font-size: 0.65rem;
      color: #fff;
    }
  }
  &__main {
    padding: 0.1rem 0 0 0;
    height: 27.5rem;
    width: 100%;
    position: relative;
    .img {
      width: 100%;
      height: auto;
      margin: 0 0 0.75rem 0;
    }
    .title {
      padding: 0 1rem;
      line-height: 1.1rem;
      font-weight: 500;
      color: #020202;
      margin: 0 0 1rem 0;
    }
    .actions {
      display: flex;
      justify-content: space-between;
      padding: 0 0.5rem;
      margin: 0 0 1rem 0;
      &__btn {
        width: 6.9rem;
        height: 2.5rem;
        transition: all 0.15s;
        border-radius: 5rem;
        font-size: 0.65rem;
        display: flex;
        align-items: center;
        justify-content: center;
        &::before {
          content: "";
          width: 0.9rem;
          height: 0.9rem;
          margin: 0 0.5rem 0 0;
          display: block;
        }
      }
      &__btn-call {
        background: #EE443A;
        border: 0.15rem solid rgba(255, 255, 255, 0.3);
        box-shadow: 0px 0.35rem 1rem rgba(238, 68, 58, 0.4);
        color: #fff;
        &:hover {
          box-shadow: none;
        }
        &::before  {
          background: center / cover url("/img/hud/smartphone/newsTab/main/icons/phone.svg");
        }
      }
      &__btn-msg  {
        background: transparent;
        border: 0.1rem solid rgba(66, 79, 97, 0.3);
        color: #020202;
        &::before {
          background: center / cover url("/img/hud/smartphone/newsTab/main/icons/envelope1.svg");
        }
      }
    }
    .info {
      display: flex;
      justify-content: space-between;
      padding: 0.75rem 0;
      border-top: 1px solid #EBEBEB;
      border-bottom: 1px solid #EBEBEB;
      margin: 0 1rem 4.3rem 1rem;
      &__part {
        font-size: 0.65rem;
        line-height: 0.65rem;
        display: flex;
        flex-direction: column;
        .desc {
          color: #9E9E9E;
          margin: 0 0 0.1rem 0;
        }
        .value {
          color: #020202;
        }
      }
      &__part-right {
        align-items: flex-end;
      }
    }
    .footer {
      display: flex;
      align-items: center;
      padding: 0 0 1.75rem 1rem;
      .img {
        width: 2.5rem;
        height: auto;
        margin: 0 0.75rem 0 0;
      }
      &__info {
        font-size: 0.65rem;
        line-height: 0.65rem;
        display: flex;
        flex-direction: column;
        .desc {
          color: #9E9E9E;
          margin: 0 0 0.1rem 0;
        }
        .value {
          color: #020202;
        }
      }
    }
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
