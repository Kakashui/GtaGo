<template>
  <div class="quests-tab">
    <div class="quests-tab__title">{{loc('options_program_3')}}</div>
    <div class="quests-tab__list">
      <div 
        class="quests-tab__list-item"
        v-for="item in 7"
        :key="item"
      ></div>
    </div>
    <div class="coming-soon">В разработке</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'QuestsTab',
  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.quests-tab{
  position: relative;
  &__title{
    margin-top: 2rem;
    margin-bottom: 1.5rem;
    font-weight: normal;
    font-size: 4rem;
    line-height: 4.8rem;
    color: rgba(255, 255, 255, 0.2);
  }
  &__list{
    display: flex;
    flex-direction: column;
    width: 45rem;
    &-item{
      width: 100%;
      height: 5rem;
      margin-bottom: .75rem;
      background: rgba(0, 0, 0, 0.4);
    }
  }
  .coming-soon{
    width: 21.9rem;
    height: 16.2rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/optionsMenu/programTab/coming-soon.png');
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.03em;
    padding-top: 8.5rem;
    text-align: center;
    position: absolute;
    top: 17rem;
    left: 50%;
    transform: translateX(-50%);
  }
}
</style>
