<template>
  <div class="report-menu">
    <Header
      :setCurrentPage="setCurrentPage"
      :currentPage="currentPage"
    />
    <div class="report-menu__body">
      <components :is="currentPage"/>
    </div>
  </div>
</template>

<script>
import Header from './components/Header'
import ReportsList from './ReportsList'
import AnswersList from './AnswersList'
import ReportsLogs from './ReportsLogs'
import { mapMutations, mapState } from 'vuex'
export default {
  name: 'ReportMenu',
  components: {
    Header,
    ReportsList,
    AnswersList,
    ReportsLogs
  },
  computed: {
    ...mapState('reportMenu', ['currentPage'])
  },
  methods: {
    ...mapMutations('reportMenu', ['refreshCurrentChat', 'setCurrentPage']),
  }
}
</script>

<style lang="scss" scoped>
.report-menu{
  display: flex;
  flex-flow: column;
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  color: #fff;
  text-transform: uppercase;
  background: rgba(2, 4, 11, 0.95);
  z-index: 1;
  &__body{
    display: flex;
    background-size: contain;
    background-position: right bottom;
    background-repeat: no-repeat;
    background-image: url('/img/reportMenu/bg-face.svg');
    height: calc(100vh - (9.8rem));
    max-height: calc(100vh - (9.8rem));
  }
}
</style>

<style lang="scss">
.report-menu{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
  .scroll{
    &::-webkit-scrollbar{
      background: transparent;
      width: .3rem;
    }
    &::-webkit-scrollbar-thumb{
      background: #4C4F56;
      border-radius: 1.1rem;
    }
  }
}
</style>
