<template>
  <div class="contracts-tab">
    <div class="contracts-tab__board">
      <div class="contracts-tab__header">
        <div class="contracts-tab__title">{{loc('options_program_2')}}</div>
        <div class="contracts-tab__filter">
          <div 
            :class="[{active: currentFilter === item.filter}, 'contracts-tab__filter-item']"
            v-for="item in filtersList"
            :key="item.filter"
            @click="setCurrentFilter(item.filter)"
          >{{loc(item.desc)}}</div>
        </div>
      </div>
      <div class="contracts-tab__list">
        <ContractItem
          v-for="(item, index) in currentList"
          :key="index"
          :item="contracts[item]"
          :currentContract="currentContract"
          @setCurrentContract="setCurrentContract"
        />
      </div>
    </div>
    <transition name="current-contract">
      <CurrentContract 
        v-if="currentContract"
        :contract="currentContract"

      />
    </transition>
  </div>
</template>

<script>
import ContractItem from './components/ContractItem'
import CurrentContract from './components/CurrentContract'
import contractConfig from '../../../../../configs/personalEvents/contractConfig'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ContractsTab',
  components: {
    ContractItem,
    CurrentContract
  },
  data: function() {
    return {
      currentContract: null,
      currentFilter: null,
      filtersList: [
        {
          desc: 'options_program_5',
          filter: 'Single'
        },
        {
          desc: 'options_program_6',
          filter: 'Family'
        },
        {
          desc: 'options_program_7',
          filter: 'current'
        },
      ]
    }
  },
  computed: {
    ...mapState('optionsMenu', ['myContracts', 'familyContracts']),
    ...mapState('familyMenu', ['inFamily']),
    ...mapGetters('localization', ['loc']),
    currentList: function() {
      let keys = Object.keys(contractConfig).filter(item => contractConfig[item].ContractType == 'Family' && this.inFamily || contractConfig[item].ContractType != 'Family' );
      if (this.currentFilter === 'current') {
        return keys.filter(item => this.myContracts.find(c => c.ContractName == item) && this.myContracts.find(c => c.ContractName == item).InProgress || this.familyContracts.find(c => c.ContractName == item) && this.familyContracts.find(c => c.ContractName == item).InProgress)
      }
      else if (this.currentFilter) {
        return keys.filter(item => contractConfig[item].ContractType == this.currentFilter)
      }
      return keys
    },
    contracts() {
      return contractConfig
    }
  },
  methods: {
    setCurrentFilter: function(value) {
      if (this.currentFilter !== value) {        
        this.currentContract = null
        this.currentFilter = value
      }
      else {
        this.currentFilter = null
      }
    },
    setCurrentContract: function(obj) {
      if (this.currentContract != obj)
        this.currentContract = obj
      else 
        this.currentContract = null
    }
  }
}
</script>

<style lang="scss" scoped>
.contracts-tab{
  width: 100%;
  display: flex;
  justify-content: space-between;
  &__board{
    margin-top: 2rem;
    min-width: 40rem;
    margin-right: 1.2rem;
  }
  &__header{
    display: flex;
    align-items: center;
    margin-bottom: 1.5rem;
  }
  &__title{
    font-style: normal;
    font-weight: normal;
    font-size: 4rem;
    margin-right: 2rem;
  }
  &__filter{
    display: flex;
    align-items: center;
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    width: 100%;
    &-item{
      position: relative;
      font-weight: normal;
      padding-bottom: .5rem;
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.03em;
      margin-right: 1.5rem;
      transition: .3s;
      &:before{
        transition: .3s;
      }
      &:last-child{
        margin-right: 0;
      }
      &.active, &:hover{
        color: #B6D300;
        &:before{
          content: '';
          width: 100%;
          height: .1rem;
          background-color: #B6D300;
          position: absolute;
          left: 0;
          bottom: -1px;
        }
      }
      &:hover{
        text-shadow: 0 1rem 4.5rem rgba(182, 211, 0, 0.6);
        &:before{
          box-shadow: 0 0 1.5rem #B6D300;
        }
      }
    }
  }
  &__list{
    display: flex;
    height: 16.5rem;
    width: 100%;
    overflow-y: auto;
    height: calc(100% - (10.2rem));
    //border: 1px solid #000;
    flex-wrap: wrap;
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: #29FFF2;
        border-radius: .5rem;
      }
    }
  }
}
.current-contract-enter-active, .current-contract-leave-active {
  transition: transform .5s;
}
.current-contract-enter, .current-contract-leave-to /* .current-contract-leave-active below version 2.1.8 */ {
  transform: translateX(100%)
}
</style>
