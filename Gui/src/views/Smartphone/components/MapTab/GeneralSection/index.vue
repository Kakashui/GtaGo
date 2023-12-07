<template>
  <transition name="slide-bottom-to-top">
    <div class="general-section">
      <transition name="general-section__header">
        <div class="general-section__header" v-if="!isFullCategories">
          <img
            src="img/hud/smartphone/mapTab/mark.svg"
            alt="mark"
            class="icon"
          >
          <div class="address">
            <p class="address__area">{{ geolocation.zoneName }}</p>
            <p class="address__location">{{ geolocation.streetName }}</p>
          </div>
        </div>
      </transition>
      <div class="general-section__main">
        <div v-bar>
          <div :class="[{ full: isFullCategories }, 'categories']" ref="categories">
            <categories-item
              v-for="(category, index) in gpsData.categories"
              :key="index"
              :category="category"
            />
          </div>
        </div>
        <button class="btn" @click="isFullCategories = !isFullCategories">{{ loc(isFullCategories ? 'phone:map:1' : 'phone:map:2') }}</button>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import CategoriesItem from './CategoriesItem'

export default {
  name: 'GeneralSection',

  components: { CategoriesItem },

  data: function() {
    return {
      isFullCategories: false
    }
  },

  computed: {
    ...mapState('smartphone', ['gpsData', 'geolocation']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
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
.general-section::v-deep .vb > .vb-dragger {
  margin: 0 0.2rem 0 0;
}
.general-section {
  position: relative;
  height: 100%;
  background: linear-gradient(180deg, rgba(0, 0, 0, 0) 0%, rgba(0, 0, 0, 0.9) 50%);
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  &__header {
    display: flex;
    align-items: center;
    padding: 1rem 0.75rem;
    .icon {
      width: 1rem;
      height: 1rem;
    }
    .address {
      margin: 0 0 0 0.75rem;
      color: #fff;
      text-transform: uppercase;
      &__area {
        font-size: 0.9rem;
        font-weight: bold;
      }
      &__location {
        font-size: 0.65rem;
      }
    }
  }
  &__main {
    padding: 1.7rem 0 1.5rem 0;
    background: linear-gradient(0deg, #F5F5F5, #F5F5F5), #EAFBEA;
    border-top-left-radius: 0.6rem;
    border-top-right-radius: 0.6rem;
    display: flex;
    flex-direction: column;
    .categories {
      height: 9.05rem !important;
      width: 15.2rem;
      padding: 0 0 0 0.75rem;
      transition: all 0.4s;
      display: grid !important;
      grid-template-columns: repeat(3, 4.4rem);
      grid-gap: 0.25rem;
      overflow: unset !important;
      &.full {
        height: 21rem !important;
        overflow: hidden scroll !important;
      }
    }
    .btn {
      height: 2.3rem;
      width: 13.7rem;
      background: linear-gradient(268.27deg, #1BBD1B 4.17%, #2ADF5D 97.17%);
      border-radius: 1.6rem;
      color: #fff;
      text-transform: uppercase;
      font-size: 0.65rem;
      font-weight: 500;
      margin: 1rem auto 0 auto;
    }
  }
}
.general-section__header-enter-active, .general-section__header-leave-active {
  transition: all .4s;
}
.general-section__header-enter, .general-section__header-leave-to {
  opacity: 0;
  transform: translate(0, 3rem);
}
.slide-bottom-to-top-enter-active {
  animation: slide-from 0.4s;
}
.slide-bottom-to-top-leave-active  {
  animation: slide-from 0.4s reverse;
}
</style>
