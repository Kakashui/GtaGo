<template>
    <div class="dressing-up-menu">
        <div class="dressing-up-menu_wrap">
          <div class="title">{{loc('dress:up:menu:1')}}</div>
          <div class="descr">{{loc('dress:up:menu:2')}}</div>
          <div class="list">
            <Dress 
              v-for="dress in dresses"
              :key="dress.id"
              :dress="dress"
              :currentDressId="currentDressId"
              :setCurrentDressId="setCurrentDressId"
            />
          </div>
          <div class="btns-wrap">
            <div class="btn" @click="toApply">{{loc('dress:up:menu:3')}}</div>
            <div class="btn" @click="toCancel">{{loc('dress:up:menu:4')}}</div>
          </div>
        </div>
    </div>
</template>

<script>
import Dress from './components/Dress'
import {mapGetters, mapState} from 'vuex'

export default {
  name: 'DressingUpMenu',

  data: function() {
    return {
      currentDressId: null
    }
  },

  components: {
    Dress
  },

  computed: {
    ...mapState('dressingUpMenu', ['dresses']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentDressId: function(value) {
      this.currentDressId = value;
      window.mp.trigger('frac:setCurrDress', value);
    },
    toApply: function() {
      window.mp.trigger('frac:applyDress', this.currentDressId);
    },
    toCancel: function() {
      window.mp.trigger('frac:cancelDress');
    }
  }

}
</script>

<style lang="scss" scoped>
  .dressing-up-menu{
    display: flex;
    align-items: flex-end;
    justify-content: flex-start;
    padding: 3rem;
    width: 71%;
    height: 100%;
    position: absolute;
    left: 0;
    top: 0;
    background: linear-gradient(86.86deg, #000000 2.01%, rgba(0, 0, 0, 0) 97.7%);
    overflow: hidden;
    &:before{
      content: '';
      width: 41.85rem;
      height: 42.35rem;
      position: absolute;
      left: -19.5rem;
      top: -18.5rem;
      background-image: url('../../../public/img/dressingUpMenu/logo.png');
      background-size: cover;
      background-repeat: no-repeat;
      background-position: center;
    }
    .dressing-up-menu_wrap{
      display: flex;
      flex-flow: column;
      justify-content: flex-end;
      position: relative;
      .title{
        text-transform: uppercase;
        font-size: 3rem;
        line-height: 3rem;
        color: #fff;
        max-width: 15rem;
        position: relative;
        &:before{
          content: '';
          width: 7rem;
          height: .1rem;
          background: linear-gradient(90.01deg, #C1E704 0%, #1C1C1C 85.03%);
          position: absolute;
          top: -1.35rem;
          left: 0;
        }
      }
      .descr{
        font-size: .8rem;
        line-height: 1rem;
        color: #FFFFFF;
      }
      .list{
        height: 16rem;
        overflow-y: auto;
        margin: 1.75rem 0 1.5rem 0;
        display: flex;
        flex-flow: column;
        padding-right: .75rem;
        margin-right: -0.75rem;
        &::-webkit-scrollbar{
          width: .2rem;
        }
        &::-webkit-scrollbar-thumb{
          background: rgba(255, 255, 255, 0.1);
        }
      }
      .btns-wrap{
        width: 100%;
        display: flex;
        justify-content: space-between;
        align-items: center;
        .btn{
          cursor: pointer;
          display: flex;
          align-items: center;
          justify-content: center;
          height: 2.7rem;
          min-height: 2.7rem;
          color: rgba(255, 255, 255, 0.4);
          background-color: transparent;
          font-weight: 300;
          font-size: .75rem;
          width: 100%;
          margin-right: .65rem;
          box-sizing: border-box;
          border: 1px solid rgba(255, 255, 255, .2);
          transition: .3s;
          &:last-child{
            margin-right: 0;
          }
          &:hover{
            background: #A2BC1F;
            box-shadow: 0 1rem 2rem rgba(193, 231, 4, 0.3);
            color: rgba(255, 255, 255, 1);
            font-weight: 500;
          }
        }
      }
    }
  }
</style>
