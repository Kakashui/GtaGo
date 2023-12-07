<template>
  <div class="first-tab">
    <span class="first-tab__title">{{loc('phone:channel:info:1')}}</span>
    <p class="first-tab__desc">{{loc('phone:channel:info:2')}}</p>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'FirstTab',
  computed: {
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
.first-tab {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
  height: fit-content;
  &::before {
    content: '';
    background: center / contain no-repeat url("/img/hud/smartphone/messengerTab/create-channel-img.png");
    width: 13.35rem;
    height: 11.55rem;
    margin: 0 0 1.65rem 0;
  }
  &__title {
    line-height: 1.15rem;
    color: #020202;
    font-weight: bold;
    margin: 0 0 0.5rem 0;
  }
  &__desc {
    font-size: 0.7rem;
    line-height: 0.9rem;
    color: #020202;
    text-align: center;
    padding: 0 1.5rem;
  }
}
</style>
