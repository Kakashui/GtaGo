<template>
  <div class="secondary-section">
    <div class="secondary-section__header">
      <button class="btn" @click="setCurrentTab({ key: currentTab.key, section: 'GeneralSection' })">
        <img
          src="img/hud/smartphone/arrowLeft.svg"
          alt="arrow"
          class="btn__icon"
        >
        <span class="btn__text">{{loc('phone:back')}}</span>
      </button>
      <span class="title">{{ loc(currentTab.data.title) }}</span>
    </div>
    <div class="secondary-section__main">
      <component :is="currentTab.subsection" />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

import WallpaperSubsection from './WallpaperSubsection'
import NotificationSoundSubsection from './NotificationSoundSubsection'
import RingtoneSubsection from './RingtoneSubsection'

export default {
  name: 'SecondarySection',

  components: {
    WallpaperSubsection,
    NotificationSoundSubsection,
    RingtoneSubsection
  },

  props: { configuration: Object },

  data: function() {
    return {
      subsections: [
        { title: 'phone:setts:5', key: 'NotificationSubsection' },
        { title: 'phone:setts:6', key: 'RingtoneSubsection' },
        { title: 'phone:setts:7', key: 'WallpaperSubsection' }
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab'])
  }
}
</script>

<style lang="scss" scoped>
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
.secondary-section {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  &__header {
    text-align: center;
    position: relative;
    padding: 0 0 1rem 0;
    .btn {
      position: absolute;
      bottom: 46%;
      left: .75rem;
      display: flex;
      align-items: center;
      &:hover {
        .btn__icon {
          animation: shake 0.4s;
        }
      }
      &__icon {
        width: .6rem;
        height: 1rem;
      }
      &__text {
        font-size: .7rem;
        color: #007AFF;
        margin: 0 0 0 .4rem;
      }
    }
    .title {
      font-weight: 500;
      font-size: .75rem;
    }
  }
  &__main {
    background: #fff;
    width: 100%;
    height: 28.2rem;
    &::v-deep .vb > .vb-dragger {
      margin: 0 0.1rem 0 0;
    }
  }
}
</style>
