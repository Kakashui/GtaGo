<template>
  <div class="about-game">
    <div class="about-game__tabs">
      <button
        class="about-game__tab"
        v-bind:key="'Rules'"
        v-bind:class="{ active: currentTab === 'Rules' }"
        v-on:click="currentTab = 'Rules'"
      >
        {{loc('mw:about:rules')}}
      </button>
      <button
        class="about-game__tab"
        v-bind:key="'Reference'"
        v-bind:class="{ active: currentTab === 'Reference' }"
        v-on:click="currentTab = 'Reference'"
      >
        {{loc('mw:about:info')}}
      </button>
      <button
        class="about-game__tab"
        v-bind:key="'Levels'"
        v-bind:class="{ active: currentTab === 'Levels' }"
        v-on:click="currentTab = 'Levels'"
      >
        {{loc('mw:about:levels')}}
      </button>
    </div>

    <component v-bind:is="currentTabComponent" class="tab"></component>
  </div>
</template>
<script>
import GameRules from './GameRules';
import GameReference from './GameReference';
import GameLevels from './GameLevels';
import { mapGetters } from 'vuex';
export default {
  name: 'AboutGame',
  computed: {
    currentTabComponent: function() {
      return 'Game' + this.currentTab;
    },
    ...mapGetters('localization', ['loc'])
  },
  data() {
    return {
      currentTab: 'Rules',
    };
  },
  components: {
    GameRules,
    GameReference,
    GameLevels,
  },
};
</script>
<style scoped lang="scss">
.about-game {
  width: 100%;
  height: 36rem;
  margin-top: 2rem;
  overflow: hidden;
  &__tabs {
    display: flex;
    flex-wrap: nowrap;
    justify-content: space-between;
    height: 3.2rem;
    border-bottom: 1px solid rgba(#fff, 0.2);
  }
  &__tab {
    color: #fff;
    background: transparent;
    line-height: 3.2rem;
    text-transform: uppercase;
    border-bottom: 0.1rem solid transparent;
    cursor: pointer;
    text-transform: none;
    font-weight: normal;
    transition: border 0.2s ease;
    &.active {
      border-bottom: 0.1rem solid #dcff00;
    }
  }
}
</style>
