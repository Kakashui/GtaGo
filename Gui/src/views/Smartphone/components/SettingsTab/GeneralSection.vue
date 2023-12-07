<template>
  <div class="general-tab">
    <span class="general-tab__title">{{loc('phone:setts:1')}}</span>
    <div class="general-tab__info">
      <img
        src="img/hud/smartphone/settingsTab/user.png"
        alt="user"
        class="icon"
      >
      <div class="text">
        <p class="text__name">{{ configuration.Username }}</p>
        <p class="text__number">{{ configuration.Number }}</p>
      </div>
    </div>
    <div class="general-tab__list">
      <div class="item">
        <img
          src="img/hud/smartphone/settingsTab/airplane.svg"
          alt="airplane"
          class="item__icon"
        >
        <span class="item__title">{{loc('phone:setts:4')}}</span>
        <label :style="{ 'background-color': configuration.IsAirplaneMode ? '#32D74B' : ' rgba(120, 120, 128, 0.32)' }" class="item__label">
          <input
            type="checkbox"
            class="checkbox"
            v-model="airplaneModeModel"
          >
          <span class="span"></span>
        </label>
      </div>
      <list-item
        v-for="item in items"
        :key="item.id"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import ListItem from './ListItem'

export default {
  name: 'GeneralTab',

  components: { ListItem },

  props: { configuration: Object },

  data: function() {
    return {
      items: [
        { id: 0, key: 'NotificationSoundSubsection', icon: 'notificationSound', section: 'SecondarySection', title: 'phone:setts:5' },
        { id: 1, key: 'RingtoneSubsection', icon: 'ringtone', section: 'SecondarySection', title: 'phone:setts:6' },
        { id: 2, key: 'WallpaperSubsection', icon: 'wallpaper', section: 'SecondarySection', title: 'phone:setts:7' }
      ]
    }
  },

  computed: {
    airplaneModeModel: {
      get: function() {
        return this.configuration.IsAirplaneMode
      },
      set: function(value) {
        window.setData('smartphone/setConfigIsAirplaneMode', value)

        window.mp.trigger('phone::settings::setConfigAirplaneMode', value);
      }
    },
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
.general-tab {
  height: 100%;
  width: 100%;
  &__title {
    font-size: 1.5rem;
    font-weight: bold;
    margin: 0 0 0 0.75rem;
    letter-spacing: .025rem;
  }
  &__info {
    display: flex;
    align-items: center;
    background: #fff;
    padding: .5rem .75rem;
    margin: 1.75rem 0 0 0;
    .icon {
      width: 2.5rem;
      height: 2.5rem;
    }
    .text {
      margin: 0 0 0 .75rem;
      &__name {
        font-size: .9rem;
      }
      &__number {
        font-size: .7rem;
      }
    }
  }
  &__list {
    display: flex;
    flex-direction: column;
    background: #fff;
    margin: 1rem 0 0 0;
    .item {
      padding: .45rem .75rem;
      display: flex;
      align-items: center;
      &__icon {
        width: 1.5rem;
        height: 1.5rem;
      }
      &__title {
        margin: 0 0 0 .6rem;
        font-size: .8rem;
        font-weight: 300;
        letter-spacing: .025rem;
      }
      &__label {
        margin: 0 0 0 auto;
        width: 2.5rem;
        height: 1.5rem;
        border-radius: 5rem;
        background: rgba(120, 120, 128, 0.32);
        position: relative;
        transition: all .15s;
        .checkbox {
          position: absolute;
          opacity: 0;
          z-index: -1;
          appearance: none;
        }
        .span {
          width: 1.3rem;
          height: 1.3rem;
          box-shadow: 0px 3px 8px rgba(0, 0, 0, 0.15), 0px 3px 1px rgba(0, 0, 0, 0.06);
          background-color: #fff;
          border-radius: 50%;
          transform: translate(-50%, -50%);
          position: absolute;
          top: 50%;
          left: 30%;
          transition: left .15s;
        }
        .checkbox:checked + .span {
          left: 70%;
        }
      }
    }
  }
}
</style>
