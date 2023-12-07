<template>
  <div class="section">
    <div class="section__header">
      <div class="text">
        <div class="text__title">{{loc("mm_info_allj_tit")}}</div>
        <div class="text__subtitle">{{loc("mm_info_allj_sub")}}</div>
      </div>
    </div>
    <div class="section__main">
      <works-item
        v-for="(item, index) in worksList"
        :key="index"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import WorksItem from './common/WorksItem'
import worksList from './worksList'
import { mapGetters } from 'vuex'

export default {
  name: 'WorksSection',

  components: {
    WorksItem
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },

  data: function() {
    return {
      worksList: worksList
    }
  }
}
</script>

<style lang="scss" scoped>
.section {
  display: flex;
  flex-direction: column;
  &__header {
    display: flex;
    margin: 0 0 1.75rem 0;
    &::before {
      content: "";
      width: 4.5rem;
      height: 4.5rem;
      background: center / cover no-repeat url("/img/optionsMenu/informationTab/ticket.png");
      margin: 0 1.25rem 0 0;
    }
    .text {
      letter-spacing: 0.03em;
      &__title {
        font-size: 2.5rem;
        line-height: 2rem;
        color: #fff;
        margin: 0 0 0.5rem 0;
      }
      &__subtitle {
        font-size: 0.9rem;
        line-height: 1.2rem;
        color: rgba(255, 255, 255, 0.5);
        width: 25rem;
      }
    }
  }
  &__main {
    display: flex;
    flex-direction: column;
    height: 35.8rem;
    overflow: scroll;
    &::-webkit-scrollbar {
      display: none;
    }
  }
}
</style>
