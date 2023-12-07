<template>
  <span :class="[{ active: isActive }, 'main__select']">
    <div
      v-if="this.type === 'Color'"
      class="select__current"
      @click="toggleIsActive"
    >
      <span class="current__title">{{ loc(thirdTabData.currentMainColorType.title) }}</span>
      <img
        src="/img/carTunningMenu/chevronDown.svg"
        alt="arrow"
        class="current__icon"
      >
    </div>
    <div
      v-else-if="this.type === 'SecColor'"
      class="select__current"
      @click="toggleIsActive"
    >
      <span class="current__title">{{ loc(thirdTabData.currentAdditionalColorType.title) }}</span>
      <img
        src="/img/carTunningMenu/chevronDown.svg"
        alt="arrow"
        class="current__icon"
      >  
    </div>
    <div class="select__list" v-if="isActive">
      <div
        class="list__item"
        v-for="item in currentItems"
        :key="item.id"
        @click="setCurrentItem(item)"
      >{{ loc(item.title) }}</div>
    </div>
  </span>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex'

export default {
  name: 'Select',

  props: ['type'],

  data: function() {
    return {
      isActive: false
    }
  },

  computed: {
    ...mapState('carTunningMenu', ['thirdTabData']),
    ...mapGetters('localization', ['loc']),

    currentItems: function() {
      return this.thirdTabData.colorTypeItems
    }
  },

  methods: {
    ...mapMutations('carTunningMenu', ['setThirdCurrentMainColorType', 'setThirdCurrentAdditionalColorType']),

    toggleIsActive: function() {
      this.isActive = !this.isActive
    },

    setCurrentItem: function(item) {
      this.toggleIsActive()
      console.log(item)
      window.mp.trigger('lsCustom:changePaintType', item.id, this.type);
      if (this.type === 'Color') {
        this.setThirdCurrentMainColorType(item)
      } else if (this.type === 'SecColor') {
        this.setThirdCurrentAdditionalColorType(item)
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.main__select {
  width: 10rem;
  display: block;
  text-align: center;
  background: rgb(190, 189, 189);
  position: relative;
  &.active {
    .select__current {
      .current__icon {
        transform: rotate(180deg);
      }
    }
  }
  .select__current {
    height: 2.4rem !important;
    display: flex;
    align-items: center;
    justify-content: center;
    background: rgba(71, 71, 71, 0.95);
    .current__title {
      color: #fff;
      font-weight: bold;
      margin: 0 0.55rem 0 0;
    }
    .current__icon {
      width: 1rem;
      height: auto;
    }
  }
  .select__list {
    background: rgb(190, 189, 189);
    position: absolute;
    display: flex;
    flex-direction: column;
    width: 100%;
    z-index: 9;
    top: 100%;
    left: 0;
    overflow: hidden;
    .list__item {
      height: 2.4rem;
      background: rgba(53, 53, 53, 0.95);
      font-size: 0.9rem;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: bold;
      color: rgba(255, 255, 255, 0.45);
      border: 1px solid transparent;
      transition: all 0.15s;
      &:nth-child(2n) {
        background: rgba(43, 43, 43, 0.95);
      }
      &:hover {
        color: #fff;
        border-color: #C1E804;
      }
    }
  }
}
</style>
