<template>
  <div class="city-hall-web">
    <Header/>
    <template v-if="isRegistered">
      <div class="title-wrap">
        <div class="title-wrap__value">Government</div>
        <div class="title-wrap__decorate">
          <div 
            class="star"
            v-for="item in 3"
            :key="item.key"
          ></div>
        </div>
      </div>
      <div class="body">
        <div class="nav-list">
          <nav-item
            v-for="item in navList"
            :key="item.key"
            :item="item"
            :currentTab="currentTab"
            :setCurrentTab="setCurrentTab"
          />
        </div>
        <component :is="currentTab"/>
        <simple-modal v-show="modalData.show"/>
      </div>
    </template> 
    <Registration
      v-else
    />
  </div>
</template>

<script>
import Registration from './Registration'
import Header from './components/Header'
import NavItem from './components/NavItem'
import LegalAidTab from './LegalAidTab'
import EmploymentTab from './EmploymentTab'
import VoteTab from './VoteTab'
import NewsTab from './NewsTab'
import SocialTab from './SocialTab'
import RelationshipsTab from './RelationshipsTab'
import HealthCareTab from './HealthCareTab'
import VehicleTaxTab from './VehicleTaxTab'
import CultureTab from './CultureTab'
import CommunityTab from './CommunityTab'
import DonationsTab from './DonationsTab'
import SimpleModal from './components/SimpleModal'

import { mapState } from 'vuex'
export default {
  name: 'CityHallWeb',

  components: {
    Registration,
    Header,
    NavItem,
    LegalAidTab,
    EmploymentTab,
    VoteTab,
    NewsTab,
    SocialTab,
    RelationshipsTab,
    HealthCareTab,
    VehicleTaxTab,
    CultureTab,
    CommunityTab,
    DonationsTab,
    SimpleModal
  },
  data: function() {
    return {
      currentTab: '',
      navList: [
        {
          icon: 'law',
          text: 'chall:cat:1',
          key: 'LegalAidTab'
        },
        {
          icon: 'portfolio',
          text: 'chall:cat:2',
          key: 'EmploymentTab'
        },
        {
          icon: 'vote',
          text: 'chall:cat:3',
          key: 'VoteTab'
        },
        {
          icon: 'world-grid',
          text: 'chall:cat:4',
          key: 'NewsTab'
        },
        {
          icon: 'verified',
          text: 'chall:cat:5',
          key: 'SocialTab'
        },
        {
          icon: 'engagement-rings',
          text: 'chall:cat:6',
          key: 'RelationshipsTab'
        },
        {
          icon: 'pharmacy',
          text: 'chall:cat:7',
          key: 'HealthCareTab'
        },
        {
          icon: 'sedan-car-front',
          text: 'chall:cat:8',
          key: 'VehicleTaxTab'
        },
        {
          icon: 'theater',
          text: 'chall:cat:9',
          key: 'CultureTab'
        },
        {
          icon: 'users',
          text: 'chall:cat:10',
          key: 'CommunityTab'
        },
        {
          icon: 'savings',
          text: 'chall:cat:11',
          key: 'DonationsTab'
        },
      ],
    }
  },

  methods: {
    setCurrentTab: function(key) {
      this.currentTab = key
    }
  },

  computed: {
    ...mapState('cityHallWeb', ['isRegistered', 'modalData'])
  },

  mounted(){
    this.currentTab = this.navList[0].key
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  display: flex;
  flex-flow: column;
  width: 100%;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  z-index: 1;
  background-color: #F3F5F7;
  .title-wrap{
    width: 100%;
    display: flex;
    flex-flow: column;
    align-items: center;
    font-family: Merriweather;
    margin: .9rem 0;
    &__value{
      font-size: 2.05rem;
      line-height: 2.6rem;
      font-weight: bold;
      color: #293340;
    }
    &__decorate{
      display: flex;
      align-items: center;
      .star{
        width: .9rem;
        height: .9rem;
        margin-right: .95rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/cityHallWeb/star-decore.svg');
        &:first-child, &:last-child{
          position: relative;
          &:before, &:after{
            content: '';
            position: absolute;
            background: #758795;
            height: 1px;
          }
          &:before{
            top: .4rem;
            width: 4.7rem;
          }
          &:after{
            top: .6rem;
            width: 2.6rem;
          }
        }
        &:first-child{
          &:before, &:after{
            left: -.3rem;
            transform: translateX(-100%);
          }
        }
        &:last-child{
          margin-right: 0;
          &:before, &:after{
            right: -.3rem;
            transform: translateX(100%);
          }
        }
      }
    }
  }
  .body{
    font-family: Merriweather;
    width: 100%;
    display: flex;
    justify-content: space-between;
    padding: 0 2.6rem;
    height: calc(100vh - (14.6rem));
    .nav-list{
      display: flex;
      flex-flow: column;
      width: 14.4rem;
      min-width: 14.4rem;
      padding: 1.5rem 2.4rem;
      background: #FFFFFF;
      border-radius: 1rem;
      margin-right: 2.6rem;
      height: 100%;
      justify-content: space-between;
    }
  }
}
</style>

<style lang="scss">
.city-hall-web{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    position: relative;
    transition: .3s;
    &:hover{
      transition: .3s;
    }
  }
  .body-tab{
    display: flex;
    justify-content: space-between;
    height: 100%;
    width: 100%;
    background: #FFFFFF;
    border-radius: 1rem;
    position: relative;
    padding: 3rem 12.5rem 3rem 3rem;
    &__title{
      font-family: Merriweather;
      font-style: normal;
      font-weight: bold;
      font-size: 2.4rem;
      line-height: 3rem;
      color: #000000;
      margin-bottom: 1.2rem;
    }
    &>div{
      display: flex;
      flex-flow: column;
    }
    .page{
      display: flex;
      flex-flow: column;
      width: 20rem;
      &__title{
        font-family: Merriweather;
        font-style: normal;
        font-weight: bold;
        font-size: 1.7rem;
        line-height: 2.15rem;
        text-transform: uppercase;
        color: #46555F;
        margin-bottom: .5rem;
      }
      &__prompt{
        font-family: Roboto;
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: .95rem;
        color: rgba(70, 85, 95, 0.6);
        margin-bottom: 1.2rem;
      }
      &__desc{
        display: flex;
        align-items: center;
        position: relative;
        font-family: Roboto;
        font-style: normal;
        font-weight: bold;
        font-size: .9rem;
        line-height: 1.05rem;
        text-transform: uppercase;
        color: #46555F;
        margin-bottom: .7rem;
        padding-left: 1rem;
        &:before{
          content: '';
          width: .4rem;
          height: .4rem;
          border-radius: 50%;
          position: absolute;
          left: 0;
          background: #E00B29;
        }
      }
      &__btn{
        background: #E00B29;
        border-radius: .3rem;
        text-transform: uppercase;
        font-style: normal;
        font-weight: bold;
        font-size: 1.1rem;
        color: #FFFFFF;
        height: 3.5rem;
        min-height: 3.5rem;
        &:hover{
          background: #C00621;
        }
      }
      input, textarea{
        border: 1px solid rgba(70, 85, 95, 0.2);
        box-sizing: border-box;
        border-radius: .3rem;
        background-color: transparent;
        outline: none;
        text-align: left;
        font-family: Roboto;
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: .9rem;
        color: #46555F;
        margin-bottom: 1.2rem;
        &::placeholder{
          color: rgba(70, 85, 95, 0.7);
        }
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button{
          -webkit-appearance: none;
        }
      }
      input{
        padding: 0 1rem;
        height: 3.4rem;
        min-height: 3.4rem;
      }
      textarea{
        height: 9rem;
        min-height: 9rem;
        padding: .9rem 1rem;
        resize: none;
      }
      .menu-drop{
        input{
          border: none;
          margin: 0;
        }
      }
    }
  }
}
</style>
