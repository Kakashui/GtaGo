<template>
  <div class="game-rules">
    <div class="rule">
      <div class="rule__title">{{loc('mw:rules:tit:1')}}</div>
      <div class="rule__text">
        {{loc('mw:rules:text:1')}}
      </div>
    </div>
    <div class="rule">
      <div class="rule__title">{{loc('mw:rules:tit:2')}}</div>
      <div class="rule__text">
        {{loc('mw:rules:text:2')}}
      </div>
    </div>
    <div class="rule">
      <div class="rule__title">{{loc('mw:rules:tit:3')}}</div>
      <div class="rule__text">
        {{loc('mw:rules:text:3')}}
      </div>
    </div>
    <div class="rule">
      <div class="rule__title">{{loc('mw:rules:tit:4')}}</div>
      <div class="rule__text">
        {{loc('mw:rules:text:4')}}
      </div>
    </div>
  </div>
</template>
<script>
import { mapGetters } from 'vuex';
export default {
  name: 'GameRules',
  computed: {
    ...mapGetters('localization', ['loc'])
  },
};
</script>
<style scoped lang="scss">
.game-rules {
  padding-top: 1.6rem;
  .rule {
    margin-bottom: 1.2rem;
    &__title {
      margin-bottom: 0.4rem;
      
      font-size: 1rem;
      //font-weight: bold;
      line-height: 1.2rem;
    }
    &__text {
      
      font-size: 1rem;
      //font-weight: normal;
      color: rgba(#fff, 0.6);
    }
  }
}
</style>
