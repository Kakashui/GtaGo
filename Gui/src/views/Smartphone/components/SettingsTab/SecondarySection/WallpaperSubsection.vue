<template>
  <div v-bar>
    <div class="wallpaper-subsection">
      <div
        :class="[{ active: item === configuration.Wallpaper }, 'wallpaper-subsection__item']"
        :style="{ background: `center / cover no-repeat url(img/hud/smartphone/settingsTab/wallpaper/${item}.png)` }"
        v-for="item in items"
        :key="item"
        @click="setWallpaper(item)"
      >
        <img
          v-if="item === configuration.Wallpaper"
          src="img/hud/smartphone/check.svg"
          alt="check"
          class="icon"
        >
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  name: 'WallpaperSubsection',
  
  data: function() {
    return {
      items: Array.from({ length: 9 }, (index, number) => number)
    }
  },

  computed: {
    ...mapState('smartphone', ['configuration'])
  },

  methods: {
    setWallpaper: function(value) {
      window.setData('smartphone/setConfigWallpaper', value)
      window.mp.trigger('phone::settings::setConfigWallpaper', value);
    }
  }
}
</script>

<style lang="scss" scoped>
.wallpaper-subsection {
  display: grid !important;
  grid-template-columns: repeat(3, 4.3rem);
  grid-gap: .4rem;
  padding: 0 .75rem;
  height: 26.6rem !important;
  &__item {
    height: 9rem;
    position: relative;
    &.active {
      border: .1rem solid #007AFF;
    }
    .icon {
      position: absolute;
      top: .25rem;
      left: .25rem;
      width: 1.35rem;
      height: 1.35rem;
    }
  }
}
</style>
