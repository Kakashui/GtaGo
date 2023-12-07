<template>
  <div class="help-page">
    <div class="help-page__head">
      <div
        class="head-item"
        v-for="item in headNav"
        :key="item.id"
      >{{loc(item)}}</div>
    </div>
    <div class="help-page__body scroll">
      <div 
        class="body-item"
        v-for="item in helpList"
        :key="item.id"
      >
        <div>{{isReportDate(item.time * 1000)}}</div>
        <div v-if="item.code != 999">{{loc('Pda_1')}}-{{item.code}}</div>
        <div v-else>{{loc('Pda_49')}}</div>
        <div>{{item.name}}</div>
        <div>{{item.distance}}m</div>
        <div 
          v-if="item.helpers.length > 0"
          @click="toWatchHelpers(item.helpers)"
        >{{item.helpers.length}} <div class="icon"></div> </div>
        <div v-else>
          <div class="space"></div>
        </div>
        <div>
          <div 
            :class="[{army: type === 2}, {fbi: type === 1},'btn', 'btn-help']"
            @click="toHelp(item.id)"
          >{{loc('Pda_3')}}</div>
        </div>
      </div>
    </div>
    <modal-helpers 
      v-if="modalShow"
      :currentHelpers="currentHelpers"
      @closeModal="closeModal"
    />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import ModalHelpers from '../components/ModalHelpers'

export default {
  name: 'HelpPage',

  components: {
    ModalHelpers
  },

  data: function() {
    return {
      headNav: [
        'Pda_22',
        'Pda_1',
        'Pda_14',
        'Pda_23',
        'Pda_5'
      ],
      currentHelpers: null,
      modalShow: false
    }
  },

  computed: {
    ...mapState('personalDigitalAssistant', ['helpList', 'type']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    toWatchHelpers: function(array) {
      this.currentHelpers = array
      this.modalShow = true
    },
    closeModal: function() {
      this.modalShow = false
      this.currentHelpers = null
    },
    toHelp: function(id) {
      window.mp.trigger('pda:toHelp', id)
    },
    isReportDate: function(value) {
      let time = new Date(value).toLocaleTimeString('ru-RU')
      let date = new Date(value).toLocaleDateString('ru-RU')
      return (date + ' ' + time)
    }
  }
}
</script>

<style lang="scss" scoped>
  .help-page{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column;
    font-size: 1rem;
    line-height: 1.15rem;
    &__head{
      display: flex;
      align-items: center;
      width: 100%;
      padding: 0 1rem;
      margin-bottom: .5rem;
      color: rgba(255, 255, 255, 0.7);
    }
    &__head>div, &__body .body-item>div{
      &:nth-child(1){
        width: 7.55rem;
        min-width: 7.55rem;
      }
      &:nth-child(2){
        width: 7.9rem;
        min-width: 7.9rem;
      }
      &:nth-child(3){
        width: 11.95rem;
        min-width: 11.95rem;
      }
      &:nth-child(4){
        width: 10.55rem;
        min-width: 10.55rem;
      }
      &:nth-child(5){
        width: 4.65rem;
        min-width: 4.65rem;
        justify-content: center;
      }
    }
    &__body{
      display: flex;
      flex-flow: column;
      width: 100%;
      padding-right: .6rem;
      overflow-y: auto;
      height: 100%;
      max-height: 100%;
      .body-item{
        width: 100%;
        padding: 0 1rem;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        background: rgba(255, 255, 255, 0.2);
        border-radius: .4rem;
        font-size: 1rem;
        height: 4.5rem;
        min-height: 4.5rem;
        margin-bottom: 1rem;
        .icon{
          margin-left: .5rem;
          width: 1.2rem;
          height: 1.2rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/personalDigitalAssistant/info.svg');
        }
        .space{
          width: 1.2rem;
          height: .1rem;
          background: #FFFFFF;
        }
        &>div{
          display: flex;
          align-items: center;
          &:last-child{
            width: 100%;
            justify-content: flex-end;
          }
        }
        .btn-help{
          background: #FFD130;
          border-radius: .4rem;
          padding: .6rem;
          width: fit-content;
          color: #000;
          font-weight: bold;
          font-size: 1rem;
          &.fbi{
            color: #fff;
            background: #FB7712;
          }
          &.army{
            color: #fff;
            background: #6C8073;
          }
          &:hover{
            transform: scale(1.1);
          }
          &:active{
            transform: scale(1.05);
          }
        }
      }
    }
  }
</style>
