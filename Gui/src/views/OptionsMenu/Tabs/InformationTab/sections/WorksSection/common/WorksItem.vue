<template>
  <div class="item">
    <div class="item__main">
      <div class="info">
        <div class="info__level">{{ item.entryLevel }} {{loc("mm_info_work_lvl")}}</div>
        <div class="info__title">{{ loc(item.title) }}</div>
      </div>
      <button
        :class="[{ active: isAdditional }, 'btn']"
        :disabled="item.locked"
        @click="isAdditional = !isAdditional"
      >
        <img :src="`img/optionsMenu/informationTab/icons/${ !item.locked ? 'arrowDown' : 'padlock' }.svg`" class="btn__icon">
      </button>
    </div>
    <div class="item__additional" v-if="isAdditional">
      <img :src="`img/optionsMenu/informationTab/works/${item.img}.png`" class="img">
      <div class="main">
        <div class="main__desc">{{ loc(item.description) }}</div>
        <button v-if="item.point !== 9" class="main__btn" @click="getDirections">{{loc("mm_info_work_rout")}}</button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'WorksItem',

  props: {
    item: Object
  },

  data: function() {
    return {
      isAdditional: false
    }
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    getDirections: function() {
      if(this.item.locked)return;
      window.mp.triggerServer("mmenu:job:waypoint", this.item.point)
    },
    select(){

    }
  }
}
</script>

<style lang="scss" scoped>
.item {
  width: 40rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  &:not(:last-child) {
    margin: 0 0 0.5rem 0;
  }
  &__main {
    height: 5rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 1.25rem 0 2rem;
    background: rgba(255, 255, 255, 0.05);
    letter-spacing: 0.03em;
    .info {
      display: flex;
      flex-direction: column;
      &__level {
        font-size: 0.9rem;
        color: rgba(255, 255, 255, 0.5);
      }
      &__title {
        font-size: 2rem;
        color: #fff;
      }
    }
    .btn {
      width: 2.5rem;
      height: 2.5rem;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      border: 0.1rem solid rgba(255, 255, 255, 0.1);
      background: transparent;
      &.active {
        transform: rotate(180deg);
      }
      &__icon {
        width: 1.1rem;
        height: 1.1rem;
      }
    }
  }
  &__additional {
    .img {
      width: 100%;
      min-height: 12rem;
      height: auto;
    }
    .main {
      padding: 1.25rem 1.5rem 1.5rem 2rem;
      display: flex;
      justify-content: space-between;
      align-items: flex-end;
      &__desc {
        font-size: 0.9rem;
        line-height: 1.3rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
        width: 25.5rem;
      }
      &__btn {
        min-width: 9.25rem;
       text-transform: uppercase;
        font-size: 0.9rem;
        letter-spacing: 0.03em;
        color: #fff;
        height: 2rem;
        background: #B6D300;
        box-shadow: 0px 0.5rem 4rem rgba(168, 195, 2, 0.5);
        border-radius: 0.1rem;
        transition: all 0.15s;
        &:hover {
          box-shadow: none;
        }
      }
    }
  }
}
</style>
