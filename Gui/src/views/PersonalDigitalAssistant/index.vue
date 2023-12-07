<template>
  <div class="personal-digital-assistant">
    <div 
      class="personal-digital-assistant__display" 
      :style="{backgroundImage: 'url(/img/personalDigitalAssistant/bg-' + type + '.png)'}"
    >
      <div :class="[{army: type === 2}, {fbi: type === 1},'head']">
        <div class="icon" :style="{backgroundImage: 'url(/img/personalDigitalAssistant/logo-' + type + '.svg)'}"></div>
        <div class="head__info" v-if="type === 0">
          <div class="title">Los santos police dept.</div>
          <div class="desc">National office of security enforcement</div>
        </div>
        <div class="head__info" v-else-if=" type === 1">
          <div class="title">Los santos dept. of justice</div>
          <div class="desc">federal investigation bereau</div>
        </div>
        <div class="head__info" v-else>
          <div class="title">Los santos army.</div>
          <div class="desc">National office of security enforcement</div>
        </div>
      </div>
      <div class="header">
        <div 
          class="block-nav"
          v-if="type !== 2"
        >
          <nav-item 
            v-for="item in navItems"
            :item="item"
            :key="item.id"
          />
        </div>
        <div 
          class="block-nav"
          v-else
        >
          <nav-item 
            v-for="item in navItemsArmy"
            :item="item"
            :key="item.id"
          />
        </div>
        <div 
          class="btn btn-exit"
          @click="exit"
        >{{loc('Pda_0')}}</div>
      </div>
      <div class="body">
        <component :is="currentPage" />
      </div>
      <div :class="[{army: type === 2}, {fbi: type === 1},'footer']">
        <footer-item
          v-for="item in 5"
          :item="item"
          :key="item.key"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import FooterItem from './components/FooterItem'
import NavItem from './components/NavItem'
import HelpPage from './HelpPage'
import FindHumanPage from './FindHumanPage'
import FindCarPage from './FindCarPage'
import WantedListPage from './WantedListPage'
import DataBasePage from './DataBasePage'

export default {
  name: 'PersonalDigitalAssistant',

  components: {
    FooterItem,
    NavItem,
    HelpPage,
    FindHumanPage,
    FindCarPage,
    WantedListPage,
    DataBasePage
  },

  data: function() {
    return {
      navItems: [
        {
          text: 'Pda_3',
          key: 'HelpPage'
        },
        {
          text: 'Pda_6',
          key: 'FindHumanPage'
        },
        {
          text: 'Pda_8',
          key: 'FindCarPage'
        },
        {
          text: 'Pda_11',
          key: 'WantedListPage'
        },
        {
          text: 'Data Base',
          key: 'DataBasePage'
        }
      ]
    }
  },

  methods: {
    exit: function() {
      window.mp.trigger('pda:exit')
    }
  },

  computed: {
    ...mapState('personalDigitalAssistant', ['currentPage', 'type']),
    ...mapGetters('localization', ['loc']),
    navItemsArmy: function() {
      let array = this.navItems
      return array.filter(element => element.key !== 'DataBasePage')
    }
  }
}
</script>

<style lang="scss" scoped>
.personal-digital-assistant{
  display: flex;
  width: 74.4rem;
  height: 51.6rem;
  padding: 3rem;
  background-size: contain;
  background-position: center;
  background-repeat: no-repeat;
  background-image: url('/img/personalDigitalAssistant/bg.png');
  font-family: Arial;
  color: #fff;
  z-index: 1;
  &__display{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column;
    background-size: cover;
    background-position: center;
    border-radius: 1.5rem;
    overflow: hidden;
    .head, .header, .footer, .body{
      padding: 0 6.5rem;
      display: flex;
      align-items: center;
    }
    .head{
      background: #333333;
      min-height: 4.5rem;
      text-transform: uppercase;
      font-style: normal;
      font-size: 1.2rem;
      line-height: 1.2rem;
      color: #FFFFFF;
      &.fbi{
        background: #222222;
      }
      &.army{
        background: #46544B;
      }
      .icon{
        width: 2.5rem;
        height: 2.5rem;
        margin-right: .5rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
      }
      &__info{
        display: flex;
        flex-flow: column;
      }
    }
    .header{
      background: #FFFFFF;
      min-height: 3.75rem;
      justify-content: space-between;
      margin-bottom: 2rem;
      .block-nav{
        display: flex;
        align-items: center;
        height: 100%;
      }
      .btn-exit{
        color: #000;
        font-weight: bold;
        font-size: 1rem;
        &:hover{
          &:after{
            transform: scale(1.1);
            transition: .2s;
          }
        }
        &:after{
          content: '';
          width: 1.7rem;
          height: 1.7rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/personalDigitalAssistant/btn-exit.svg');
          margin-left: .5rem;
          transition: .2s;
        }
      }
    }
    .body{
      height: 30.25rem;
    }
    .footer{
      display: flex;
      background: rgba(51, 51, 51, 0.8);
      backdrop-filter: blur(2rem);
      height: 5.1rem;
      min-height: 5.1rem;
      justify-content: space-between;
      &.fbi{
        background: rgba(51, 51, 51, 0.8);
      }
      &.army{
        background: rgba(51, 51, 51, 0.8);
      }
    }
  }
}
</style>

<style lang="scss">
.personal-digital-assistant{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
  .scroll{
    &::-webkit-scrollbar{
      background: rgba(255, 255, 255, 0.3);
      border-radius: .1rem;
      width: .1rem;
      &-thumb{
        background: #FFFFFF;
        border-radius: .1rem;
      }
    }
  }
  .modal-wrap{
    position: fixed;
    top: 0;
    left: 0;
    z-index: 2;
    width: 100%;
    height: 100vh;
    background: rgba(0, 0, 0, 0.7);
    display: flex;
    align-items: center;
    justify-content: center;
  }
}
</style>