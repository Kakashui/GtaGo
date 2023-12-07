<template>
  <div class="answers-list">
    <div class="answers-list__requests">
      <requests-stats/>
      <requests-list
        :reports="myReports"
      />
    </div>
    <Chat 
      :dynamic="false" 
    />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import RequestsStats from '../components/RequestsStats'
import RequestsList from '../components/RequestsList'
import Chat from '../components/Chat'
export default {
  name: "AnswersList",

  components: {
    RequestsStats,
    RequestsList,
    Chat
  },

  computed: {
    ...mapState('reportMenu', ['myReports'])
  }
}
</script>

<style lang="scss" scoped>
  .answers-list{
    width: 100%;
    height: 100%;
    display: flex;
    &__requests{
      display: flex;
      flex-flow: column;
      width: 21rem;
      min-width: 21rem;
      height: 100%;
    }
  }
</style>
