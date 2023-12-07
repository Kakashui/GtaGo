<template>
  <div class="apps-item" :style="{ background: 'center / cover no-repeat url(' + `img/hud/smartphone/appStoreTab/${item.img}.png` + ')' }">
    <p class="apps-item__tagline" v-html="loc(item.tagline)"></p>
    <div class="apps-item__footer">
      <img
        :src="`img/hud/smartphone/desktopTab/apps/${item.icon}.svg`"
        :alt="item.icon"
        class="icon"
      >
      <div class="desc">
        <p class="desc__name">{{ loc(item.title) }}</p>
        <p class="desc__title">{{ loc(item.desc) }}</p>
      </div>
      <div class="actions">
        <transition name="actions">
          <svg
            :height="appWidthUnit * 1.3"
            :width="appWidthUnit * 1.3"
            class="actions__circle"
            v-if="isDownloading"
          >
            <circle :cx="appWidthUnit * 0.65" :cy="appWidthUnit * 0.65" :r="appWidthUnit * 0.575" stroke="#E8E8E8" :stroke-width="appWidthUnit * 0.15" fill="none" />
            <circle :cx="appWidthUnit * 0.65" :cy="appWidthUnit * 0.65" :r="appWidthUnit * 0.15" stroke="#007AFF" :stroke-width="appWidthUnit * 0.1" fill="#007AFF" />
            <circle class="dynamic" :cx="appWidthUnit * 0.65" :cy="appWidthUnit * 0.65" :r="appWidthUnit * 0.575" stroke="#007AFF" :stroke-width="appWidthUnit * 0.15" fill="none" />
          </svg>
          <button class="actions__btn actions__btn-download" v-else-if="!item.isInstalled" @click="startDownload">{{loc('phone:store:1')}}</button>
          <button class="actions__btn actions__btn-uninstall" v-else-if="item.isInstalled" @click="removeApp">{{loc('phone:store:2')}}</button>
        </transition>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'AppsItem',

  props: { item: Object },

  data: function() {
    return {
      isDownloading: false,
      downloadingTimer: null
    }
  },

  computed: {
    appWidthUnit: function () {
      const appWidth = document.getElementById('app').offsetWidth / 100
      return Math.ceil(appWidth)
    },
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone/appPage', ['setAppStoreItemIsInstalled']),

    startDownload: function() {
      this.isDownloading = true
      window.mp.trigger('phone::appStore::installApp', this.item.id)
      setTimeout(() => {
        this.isDownloading = false
      }, 2000)
    },

    removeApp: function() {
      this.isDownloading = true
      window.mp.trigger('phone::appStore::removeApp', this.item.id)
      setTimeout(() => {
        this.isDownloading = false
      }, 2000)
    }
  },

  beforeDestroy: function() {
    clearTimeout(this.downloadingTimer)
  }
}
</script>

<style lang="scss" scoped>
@keyframes dasharray {
  from {
    stroke-dasharray: 0 290%;
  }
  to {
    stroke-dasharray: 290% 290%;
  }
}
.apps-item {
  height: 13rem;
  width: 13.7rem;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  border-radius: 0.6rem;
  overflow: hidden;
  margin: 0.75rem 0 0 0;
  &:first-child {
    margin: 0;
  }
  &__tagline {
    font-size: 1.2rem;
    font-weight: bold;
    color: #fff;
    margin: 0 1rem;
    padding: 0.75rem 0;
  }
  &__footer {
    display: flex;
    align-items: center;
    background: #fff;
    padding: 0.75rem 0.5rem;
    .icon {
      width: 2rem;
      height: 2rem;
    }
    .desc {
      margin: 0 0 0 0.5rem;
      &__name {
        color: #020202;
        font-size: 0.7rem;
      }
      &__title {
        font-size: 0.6rem;
        color: #8A8A8E;
        line-height: 0.7rem;
        margin: 0.1rem 0 0 0;
      }
    }
    .actions {
      width: 3.5rem;
      height: 1.3rem;
      display: flex;
      justify-content: center;
      margin: 0 0 0 auto;
      position: relative;
      &__btn {
        width: 100%;
        height: 1.2rem;
        font-weight: 500;
        font-size: 0.6rem;
        border-radius: 5rem;
        z-index: 3;
      }
      &__btn-download {
        color: #fff;
        background: #007AFF;
        &:hover {
          background: #0066EB;
        }
      }
      &__btn-uninstall {
        color: #869CB1;
        background: #F1F8FF;
        &:hover {
          background: #E4EBF2;
        }
      }
      &__circle {
        .dynamic {
          animation: dasharray 2s linear;
        }
      }
    }
  }
}
.actions-enter-active, .actions-leave-active {
  transition: all .5s;
  position: absolute;
}
.actions-enter, .actions-leave-to {
  opacity: 0;
  transform: scale(0.2);
}
</style>
