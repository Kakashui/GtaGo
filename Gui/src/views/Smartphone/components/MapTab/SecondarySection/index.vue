<template>
  <div class="secondary-section">
    <transition name="slide-bottom-to-top" appear>
      <div class="secondary-section__content">
        <div class="header">
          <div class="header__btn" @click="setCurrentTab({ key: currentTab.key, section: 'GeneralSection' })">
            <div class="icon"></div>
            <span class="text">{{loc('phone:back')}}</span>
          </div>
          <span class="header__title">{{ loc(currentTab.data.title) }}</span>
        </div>
        <div v-bar>
          <div class="list">
            <list-item
              v-for="(item, index) in gpsData.categoryItems"
              :key="index"
              :item="item"
            />
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import ListItem from './ListItem'

export default {
  name: 'SecondarySection',

  components: { ListItem },

  computed: {
    ...mapState('smartphone', [
      'currentTab',
      'gpsData',
      'colorTheme'
    ]),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: this.colorTheme.header, button: 'none' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateX(0.3rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.15rem);
  }
}
@keyframes slide-from {
  from {
    transform: translateY(100%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
.secondary-section::v-deep .vb > .vb-dragger {
  margin: 0 0.05rem 0 0;
}
.secondary-section {
  height: 100%;
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  &__content {
    background: linear-gradient(0deg, #F5F5F5, #F5F5F5), #EAFBEA;
    border-top-right-radius: 0.6rem;
    border-top-left-radius: 0.6rem;
    padding: 1.5rem 0.1rem 1.8rem 0.1rem;
    height: 29.5rem;
    .header {
      padding: 0 0.7rem;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      margin: 0 0 1rem 0;
      &__btn {
        display: flex;
        align-items: center;
        &:hover {
          .icon {
            animation: shake 0.4s;
          }
        }
        .icon {
          background: #1BBD1B;
          mask: url('/img/hud/smartphone/arrowLeft.svg') no-repeat center / contain;
          width: 0.35rem;
          height: 0.6rem;
        }
        .text {
          color: #1BBD1B;
          font-size: 0.65rem;
          text-transform: uppercase;
          margin: 0 0 0 0.35rem;
          font-weight: 400;
        }
      }
      &__title {
        font-size: 0.9rem;
        text-transform: uppercase;
        font-weight: bold;
        margin: 0.25rem 0 0 0;
        display: inline-block;
      }
    }
    .list {
      height: 23.4rem !important;
      padding: 0 0.5rem;
    }
  }
}
.slide-bottom-to-top-enter-active {
  animation: slide-from 0.4s;
}
.slide-bottom-to-top-leave-active  {
  animation: slide-from 0.4s reverse;
}
</style>
