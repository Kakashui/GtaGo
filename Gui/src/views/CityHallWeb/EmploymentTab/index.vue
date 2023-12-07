<template>
  <div class="body-tab employment-tab">
    <div>
      <div class="body-tab__title">{{loc('cityHallWeb_25')}}</div>
      <div class="body-tab__desc">{{loc('cityHallWeb_26')}}</div>
      <menu-nav
        :list="jobs"
        :currentPage="currentJob"
        :setCurrentPage="setCurrentJob"
      />
    </div>
    <div>
      <job-wrap
        :selectedJob="selectedJob"
      />
    </div>
  </div>
</template>

<script>
//import { mapState } from 'vuex'
import MenuNav from '../components/MenuNav'
import JobWrap from './JobWrap'
import worksList from '../../OptionsMenu/Tabs/InformationTab/sections/WorksSection/worksList'
import { mapGetters } from 'vuex'
export default {
  name: 'EmploymentTab',

  components: {
    MenuNav,
    JobWrap
  },
  
  data: function() {
    return{
      currentJob: null,
      workList: worksList,
      jobs: [],
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),

    selectedJob: function() {
      return this.jobs.find(element => element.key === this.currentJob)
    }
  },

  methods: {
    setCurrentJob: function(value) {
      this.currentJob = value
    }
  },

  created() {
    this.jobs = this.workList.filter(item => !item.locked && item.point != 5);
    this.jobs.forEach(job => {
      job.key = job.title;
      job.text = job.title
    });
    this.currentJob = this.jobs[0].key
  }
}
</script>


<style lang="scss" scoped>
.body-tab{
  &__title{
    line-height: 3.5rem;
    max-width: 25rem;
    margin-top: 2rem;
  }
  &__desc{
    font-family: Roboto;
    font-style: normal;
    font-weight: normal;
    font-size: .8rem;
    line-height: 1.2rem;
    color: #46555F;
    max-width: 22.6rem;
    margin-bottom: 1.2rem;
  }
}
</style>

<style lang="scss">
.city-hall-web{
  .body-tab{
    display: flex;
    justify-content: space-between;
    &.employment-tab{
      padding: 1rem 1rem 1rem 3rem;
      .menu-nav{
        padding-right: 2.1rem;
        margin-left: -1.1rem;   
        padding-left: 1rem;
        transform: rotateY(180deg);
        max-height: 24.3rem;
        height: 24.3rem; //fix
        overflow-y: auto;
        &::-webkit-scrollbar{
          background-color: transparent;
          width: .1rem;
          &-thumb{
            background: #E9EBED;
            border-radius: .1rem;
          }
        }
        &>div{
          transform: rotateY(-180deg);
        }
      }
    }
  }
}
</style>
