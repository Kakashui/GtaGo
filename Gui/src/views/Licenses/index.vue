<template>
  <div class="licenses">
    <div 
      class="btn-close"
      @click="closeLicenses"
    ></div>
    <div class="licenses__header">
      <div class="info info-name">
        <div class="desc">{{loc('Licenses_0')}}</div>
        <div class="value">{{nickname}}</div>
      </div>
      <div class="info">
        <div class="desc">{{loc('Licenses_1')}}</div>
        <div class="value">{{bithday}}</div>
      </div>
      <div class="info info-sex">
        <div class="desc">{{loc('Licenses_2')}}</div>
        <div class="value">{{getSex}}</div>
      </div>
    </div>
    <div class="licenses__body">
      <div class="body-title">{{loc('Licenses_3')}}</div>
      <div class="body-navigation">
        <nav-item
          v-for="(nav, index) in list"
          :key="index"
          :item="nav"
          :currentNav="currentNav"
          :setCurrentNav="setCurrentNav"
        />
      </div>
      <component :is="currentNav"/>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

import NavItem from './components/NavItem'
import Weapon from './tabs/Weapon'
import Vehicle from './tabs/Vehicle'
import Medical from './tabs/Medical'
import Job from './tabs/Job'
import Military from './tabs/Military'

export default {
  name: 'Licenses',

  components: {
    NavItem,
    Weapon,
    Vehicle,
    Medical,
    Job,
    Military
  },

  data: function() {
    return{
      list: [
        'vehicle',
        'weapon',
        'medical',
        'job',
        'military'
      ],
      currentNav: '',
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('licenses', [
      'nickname',
      'gender',
      'bithday',
    ]),
    ...mapGetters('localization',['loc']),
    getSex: function() {
      if (this.gender) {
        return 'M'
      } else {
        return 'F'
      }
    }
  },
  
  methods:{
    closeLicenses: function() {
      window.mp.trigger('Licenses:closeLicenses')
    },
    setCurrentNav: function(value) {
      this.currentNav = value
    }
  },

  mounted() {
    this.currentNav = this.list[0]
  }
}
</script>

<style lang="scss" scoped>
.licenses {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  position: relative;
  height: 28.85rem;
  width: 26.45rem;
  background: #fff;
  font-style: normal;
  .btn-close{
    width: 1.2rem;
    height: 1.2rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/licenses/close.svg');
    position: absolute;
    top: 0;
    right: -.5rem;
    transform: translateX(100%);
    transition: .2s;
    opacity: .7;
    &:hover{
      opacity: 1;
    }
  }
  &__header{
    display: flex;
    align-items: center;
    padding: 1.5rem 1.5rem .95rem 1.5rem;
    .info{
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      margin-right: 1rem;
      white-space: pre;
      font-weight: 600;
      &:last-child{
        margin-right: 0;
      }
      .desc{
        font-size: .7rem;
        line-height: .8rem;
        color: rgba(51, 61, 71, 0.4);
        margin-bottom: .3rem;
        letter-spacing: 0.07em;
      }
      .value{
        font-weight: 600;
        font-size: .8rem;
        line-height: .9rem;
        color: #333D47;
      }
      &.info-name{
        margin-right: 2.8rem;
        width: 100%;
        .desc{
          margin-bottom: 1px;
        }
        .value{
          font-size: 1.7rem;
          line-height: 1.95rem;
        }
      }
      &.info-sex{
        .value{
          width: 100%;
          text-align: center;
        }
      }
    }
  }
  &__body{
    background: rgba(196, 196, 196, 0.2);
    padding-top: 1.2rem;
    height: 100%;
    .body-title{
      font-weight: 600;
      font-size: 1.2rem;
      line-height: 1.35rem;
      margin-bottom: 1.8rem;
      color: #333D47;
      width: 100%;
      text-align: center;
    }
    .body-navigation{
      display: flex;
      width: 100%;
      justify-content: space-between;
      align-items: center;
      padding: 0 2.5rem;
      position: relative;
      margin-bottom: 1.3rem;
      &:before{
        content: '';
        width: 100%;
        height: 1px;
        background-color: rgba(196, 196, 196, 0.5);
        position: absolute;
        left: 0;
      }
    }
  }
}
</style>

<style lang="scss">
.licenses {
  &__body{
    .body-tab{
      height: 100%;
      padding: 0 .5rem;
      position: relative;
      overflow: hidden;
      &__title{
        padding: 0 1rem;
        margin-bottom: .4rem;
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        .desc{
          margin-bottom: .2rem;
          font-weight: 600;
          font-size: .7rem;
          line-height: .8rem;
          letter-spacing: 0.04em;
          color: rgba(51, 61, 71, 0.4);
        }
        .value{
          font-weight: 800;
          font-size: 2.5rem;
          line-height: 2.85rem;
          letter-spacing: 0.04em;
          text-transform: uppercase;
          color: #FC5A15;
        }
      }
      .block-locked, .block-open{
        width: 100%;
        height: 100%;
        display: flex;
        flex-flow: column;
        align-items: center;
        justify-content: center;
        font-weight: 600;
        font-size: .7rem;
        line-height: .8rem;
        text-align: center;
        letter-spacing: 0.04em;
        color: rgba(255, 255, 255, 0.8);
        &:before{
          content: '';
          width: 5rem;
          height: 5rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-bottom: .5rem;
        }
      }
      .block-locked{
        &:before{
          background-image: url('/img/licenses/lock.svg');
        }
      }
      .block-open{
        &:before{
          background-image: url('/img/licenses/open.svg');
        }
      }
    }
  }
}
</style>
