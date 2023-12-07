<template>
  <div class="general-subsection">
    <div class="general-subsection__header">
      <span class="title">{{loc('phone:recent:gen:1')}}</span>
      <button class="btn">{{loc('phone:recent:gen:2')}}</button>
    </div>
    <div v-bar>
      <div class="general-subsection__list">
        <list-item
          v-for="item in calls"
          :key="item.id"
          :item="item"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ListItem from './ListItem'

export default {
  name: 'GeneralSubsection',

  components: { ListItem },

  computed: {
    ...mapState('smartphone', ['calls']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.general-subsection {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  &__header {
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    padding: 0 .75rem .75rem 2rem;
    .title {
      font-size: 1.5rem;
      font-weight: bold;
    }
    .btn {
      background: transparent;
      color: #007AFF;
      font-size: .7rem;
      height: 1.3rem;
      &:hover {
        text-decoration: underline;
      }
    }
  }
  &__list {
    height: 23.4rem;
  }
}
</style>
