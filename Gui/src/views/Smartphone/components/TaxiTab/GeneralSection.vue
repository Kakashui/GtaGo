<template>
    <div class="general-tab">
    <transition name="img" appear>
      <img
        src="img/hud/smartphone/taxiTab/logo.png"
        alt="logo"
        class="general-tab__logo"
      >
    </transition>
    <transition name="slide-in" appear>
      <div class="general-tab__main">
        <div class="location">
          <img
            src="img/hud/smartphone/taxiTab/beacon.svg"
            alt="beacon"
            class="location__icon"
          >
          <div class="location__info">
            <p class="desc">{{loc('phone:taxi:gps:1')}}</p>
            <p class="data">{{ geolocation.zoneName }} - {{ geolocation.streetName }}</p>
          </div>
        </div>
        <div class="actions">
          <button class="actions__btn-first" @click="setTab">{{loc('phone:taxi:gps:1')}}</button>
          <button class="actions__btn-second" @click="setTab">
            <img
              src="img/hud/smartphone/bankTab/arrow.svg"
              alt="arrow"
              class="icon"
            >
          </button>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'GeneralSection',

  computed: {
    ...mapState('smartphone', ['currentTab', 'geolocation']),
    ...mapState('smartphone/taxiPage', ['taxiData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme']),
    ...mapMutations('smartphone/taxiPage', ['taxi_setSpecifyLocation']),

    
    setTab: function(){
      this.setCurrentTab({ key: this.currentTab.key, section: 'SecondarySection' })
      this.taxi_setSpecifyLocation({name:'', position: null, type: 'unknown'})
    }
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade-in {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
@keyframes slide-in {
  from {
    transform: translateY(100%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
.general-tab {
  height: 100%;
  padding: 2.35rem 0 0 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  &__logo {
    width: 100%;
    height: 19.25rem;
  }
  &__main {
    background: #fff;
    border-radius: 0.7rem;
    margin: 2.3rem 0 0 0;
    width: 14.2rem;
    padding: 0.75rem;
    box-shadow: 0px 0.5rem 1.5rem rgba(177, 188, 203, 0.5);
    .location {
      display: flex;
      align-items: center;
      padding: 0 0 0.6rem 0;
      position: relative;
      &::after {
        content: '';
        position: absolute;
        bottom: 0;
        right: 0;
        height: 1px;
        width: 11.75rem;
        background: #EBEBEB;
      }
      &__icon {
        width: 0.65rem;
        height: 0.65rem;
      }
      &__info {
        margin: 0 0 0 0.25rem;
        width: 100%;
        .desc {
          font-size: 0.65rem;
          color: rgba(0, 0, 0, 0.4);
        }
        .data {
          margin: 0.25rem 0 0 0;
          font-size: 0.65rem;
          color: #000;
        }
      }
    }
    .actions {
      display: flex;
      justify-content: space-between;
      margin: 0.75rem 0 0 0;
      &__btn-first {
        width: 9.45rem;
        height: 2.5rem;
        background: #F1C115;
        border-radius: 0.5rem;
        box-shadow: 0px 0.5rem 1.5rem rgba(244, 193, 11, 0.3);
        color: #fff;
        font-size: 0.7rem;
      }
      &__btn-second {
        height: 2.5rem;
        width: 2.5rem;
        background: #fff;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0px 0.5rem 2rem rgba(0, 0, 0, 0.15);
        border-radius: 0.5rem;
        .icon {
          width: 0.7rem;
          height: auto;
        }
      }
    }
  }
}
.img-enter-active {
  animation: fade-in 0.4s;
}
.slide-in-enter-active {
  animation: slide-in 0.4s;
}
// .slide-in-leave-active  {
//   animation: slide-in 0.4s reverse;
// }
</style>