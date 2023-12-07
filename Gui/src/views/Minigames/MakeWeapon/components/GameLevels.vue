<template>
  <div class="game-levels">
    <div class="level">
      <div class="level__num">1</div>
      <div class="level__text">LEVEL</div>
      <div class="level__info">
        <div class="level__title">{{loc('mw:levels:tit')}}</div>
        <div class="level__info-val">0 {{loc('mw:levels:weapons')}}</div>
      </div>
      <div class="level__info">
        <div class="level__title">{{loc('mw:levels:parts')}}</div>
        <div class="level__info-val">x2</div>
      </div>
    </div>
    <div class="level">
      <div class="level__num">2</div>
      <div class="level__text">LEVEL</div>
      <div class="level__info">
        <div class="level__title">{{loc('mw:levels:tit')}}</div>
        <div class="level__info-val">2000 {{loc('mw:levels:weapons')}}</div>
      </div>
      <div class="level__info">
        <div class="level__title">{{loc('mw:levels:parts')}}</div>
        <div class="level__info-val">x3</div>
      </div>
    </div>
    <div class="level">
      <div class="level__num">3</div>
      <div class="level__text">LEVEL</div>
      <div class="level__info">
        <div class="level__title">{{loc('mw:levels:tit')}}</div>
        <div class="level__info-val">6000 {{loc('mw:levels:weapons')}}</div>
      </div>
      <div class="level__info">
        <div class="level__title">{{loc('mw:levels:parts')}}</div>
        <div class="level__info-val">x4</div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapGetters } from 'vuex';
export default {
  name: 'GameLevels',
  computed:{
    ...mapGetters('localization', ['loc'])
  }
};
</script>
<style scoped lang="scss">
.game-levels {
  width: 100%;
  .level {
    margin-top: 1.5rem;
    display: flex;
    flex-wrap: nowrap;
    justify-content: space-between;
    height: 5.7rem;
    align-items: flex-end;
    &__num {
      width: 2.4rem;
      margin-right: 0.3rem;
      //font-weight: 700;
      font-size: 5rem;
      color: #fff;
      line-height: 1.15;
    }
    &__text {
      width: 1.2rem;
      height: 100%;
      padding: 1rem 0;
      writing-mode: vertical-lr;
      transform: rotate(180deg);
      
      //font-weight: 400;
      font-size: 1rem;
      color: rgba(#fff, .6);
      line-height: 1.15;
      letter-spacing: 0.35rem;
    }
    &__info {
      margin-left: .9rem;
      padding-bottom: 1rem;
    }
    &__title {
      margin-bottom: 0.5rem;
      
      font-size: 1rem;
      //font-weight: 700;
      color: #fff;
    }
    &__info-val {
      
      color: rgba(#fff, .6);
    }
  }
}
</style>
