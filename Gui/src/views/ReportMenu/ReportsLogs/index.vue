<template>
  <div class="reports-logs">
    <div class="reports-logs__requests">
      <requests-stats/>
      <requests-list
        :reports="filterReportsLogs"
      />
    </div>
    <Chat 
      :dynamic="false" 
    />
    <div class="reports-logs__admins">
      <div class="title">{{loc('ReportMenu_1')}}</div>
      <div class="input-wrap">
        <input v-model="currentAdmin" type="text" :placeholder="loc('ReportMenu_23')">
      </div>
      <div class="prompt">{{loc('ReportMenu_2')}} - {{reportsLogAdmins.length}}</div>
      <div class="admins-list scroll">
        <div
          class="admins-list__item"
          v-for="item in filterReportsLogAdmins"
          :key="item.key"
          @click="setAdmin(item)"
        >{{item}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import RequestsStats from '../components/RequestsStats'
import RequestsList from '../components/RequestsList'
import Chat from '../components/Chat'
export default {
  name: "ReportsLogs",

  components: {
    RequestsStats,
    RequestsList,
    Chat
  },

  data: function() {
    return {
      currentAdmin: null,
      adminName: null
    }
  },

  computed: {
    ...mapState('reportMenu', ['reportsLogs', 'reportsLogAdmins']),
    ...mapGetters('localization', ['loc']),
    filterReportsLogs: function() {
      const array = []
      if (this.adminName) {
        this.reportsLogs.forEach(element => {
          const isCoincidence = element.messages.some(item => this.formatString(item.sender).includes(this.formatString(this.adminName)))
          if (isCoincidence) array.push(element) 
        })
        return array
      } else {
        return this.reportsLogs
      }
    },
    filterReportsLogAdmins: function() {
      if (this.currentAdmin) {
        return this.reportsLogAdmins.filter(item => {
          return this.formatString(item).includes(this.formatString(this.currentAdmin))
        })
      } else {
        return this.reportsLogAdmins
      }
    },
  },

  methods: {
    setAdmin: function(item) {
      this.adminName = item
    },
    formatString(item) {
      return item.trim().toLowerCase()
    }
  }
}
</script>

<style lang="scss" scoped>
  .reports-logs{
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
    &__admins{
      display: flex;
      flex-flow: column;
      padding: 1.8rem 0 0 1.8rem;
      width: 100%;
      .title, .input-wrap, .prompt{
        padding-right: 1.8rem;
      }
      .title{
        font-size: 1.55rem;
        line-height: 1.55rem;
        color: #FFFFFF;
        text-transform: uppercase;
        margin-bottom: .8rem;
      }
      .input-wrap{
        position: relative;
        display: flex;
        align-items: center;
        width: 100%;
        &:before{
          content: '';
          z-index: 1;
          position: absolute;
          left: 1.4rem;
          height: 1.15rem;
          width: 1.4rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/reportMenu/search-icon.svg');
        }
        input{
          width: 100%;
          height: 100%;
          padding: 1.3rem .5rem 1.3rem 3.95rem;
          outline: none;
          box-sizing: border-box;
          background: rgba(38, 40, 46, 0.35);
          border-bottom: .2rem solid #26282E;
          font-family: Roboto;
          color: #fff;
          font-size: .9rem;
          line-height: 1.05rem;
          z-index: 2;
          box-sizing: border-box;
          &:focus, &:active{
            border-color:  #C1E704;
            transition: .2s;
          }
        }
        
      }
      .prompt, .admins-list .admins-list__item{
        font-weight: bold;
        font-size: 1.05rem;
        line-height: 1.05rem;
        text-transform: uppercase;
      }
      .prompt{
        color: rgba(255, 255, 255, 0.15);
        margin: 1.5rem 0;
      }
      .admins-list{
        display: flex;
        flex-flow: column;
        margin-right: .65rem;
        padding-right: .8rem;
        overflow-y: auto;
        &__item{
          margin-bottom: 1.5rem;
          color: rgba(255, 255, 255, 0.45);
          &:hover{
            color: rgba(255, 255, 255, 1);
            transition: .2s;
          }
        }
      }
    }
  }
</style>
