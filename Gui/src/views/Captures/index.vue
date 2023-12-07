<template>
  <div
    :class="[
      { attack: capturing.attackStatus === 'we' },
      { protect: capturing.attackStatus === 'us' },
      'captures',
    ]"
  >
    <div 
      :class="{ 
        'attack': capturing.attackStatus === 'we',
        'protect': capturing.attackStatus === 'us' 
      }"
      class="captures-body">
      <div class="captures-group-container">
        <div class="header">
          <div class="title">{{ loc('captmenu_1') }}</div>
          <div class="gang-name">{{ capturing.myGangName }}</div>
        </div>
        <GroupList />
      </div>
      <div class="captures-team-container">
        <div class="header">
          <div v-if="capturing.attackStatus === 'us'" class="title">
            {{ loc('captmenu_2') }}
          </div>
          <div v-else-if="capturing.attackStatus === 'we'" class="title">
            {{ loc('captmenu_3') }}
          </div>
          <div v-if="capturing.attackStatus === 'us'" class="gang-name">
            {{ `${capturing.enemy}` }}
          </div>
          <div v-else-if="capturing.enemy !== ''" class="gang-name">
            {{ `${loc('captmenu_4')} ${capturing.enemy}` }}
          </div>
          <div v-else class="gang-name">{{loc('captmenu_5')}}</div>
        </div>
        <div class="desc">{{loc('captmenu_6')}}</div>
        <TeamList />
      </div>
    </div>
    
  </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";

import GroupList from "./components/GroupList";
import TeamList from "./components/TeamList";

export default {
  name: "Captures",

  data: function () {
    return {};
  },

  components: {
    GroupList,
    TeamList,
  },

  computed: {
    ...mapState("captures", ["capturing"]),
    ...mapGetters("localization", ["loc"])
  },

  methods: {},
};
</script>

<style lang="scss" scoped>
.captures {
  position: absolute;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.9);
  &-body{
    max-width: 1920px;
    height: 100vh;
    margin: auto;
    position: relative;
    display: flex;
    justify-content: center;
  }
  &:before,
  &:after {
    position: absolute;
    right: 0;
  }
  &:after {
    top: 0;
    content: "";
    height: 100%;
    width: 30rem;
    background-size: cover;
  }
  &:before {
    top: 6.75rem;
    content: "";
    width: 28.5rem;
    height: 36rem;
    background: #10cd45;
    opacity: 0.4;
    filter: blur(250px);
  }
  &.attack {
    &:after {
      background-image: url("/img/captures/attack.png");
    }
  }
  &.protect {
    background-color: rgba(33, 15, 15, 0.9);
    .captures-group-container {
      background-color: rgba(0, 0, 0, 0.6);
      .header {
        &:after {
          content: "VS";
          color: #db4733;
          text-transform: uppercase;
          font-style: normal;
          font-weight: bold;
          font-size: 5rem;
          line-height: 5rem;
          text-shadow: 0 1rem 2rem rgba(219, 71, 51, 0.3);
          position: absolute;
          right: 0;
          bottom: 0;
          transform: translateX(50%);
        }
      }
    }
    &:after {
      background-image: url("/img/captures/protect.png");
    }
    &:before {
      background: #e1523f;
    }
  }
  .header {
    padding: 1.75rem 0 0 2rem;
    width: 100%;
    height: 9.2rem;
    min-height: 9.2rem;
    .title,
    .gang-name {
      text-transform: uppercase;
      font-style: normal;
      color: #ffffff;
    }
    .title {
      font-weight: 300;
      font-size: 1.5rem;
      line-height: 1.5rem;
      margin-bottom: 0.2rem;
    }
    .gang-name {
      font-weight: bold;
      font-size: 5rem;
      line-height: 5rem;
    }
  }  
  &-group-container {
    background-color: rgba(8, 40, 17, 0.7);
    width: 29.75rem;
    min-width: 29.75rem;
    height: 100%;
    overflow: hidden;
    position: relative;
    .header {
      margin-bottom: 0.75rem;
    }
  }
  &-team-container {
    height: 100%;
    overflow: hidden;
    position: relative;
    &:after {
      content: "";
      height: 100%;
    }
    .header {
      padding-left: 5rem;
      padding-bottom: 0.7rem;
      &:after {
        content: "";
        width: 100%;
        height: 1px;
        background: rgba(255, 255, 255, 0.2);
        position: absolute;
        bottom: 0;
        left: 0;
      }
    }
    .desc {
      text-transform: uppercase;
      font-style: normal;
      font-weight: 300;
      font-size: 1.5rem;
      line-height: 1.5rem;
      color: #ffffff;
      margin: 2rem 0 0 5rem;
    }
  }
}
</style>
