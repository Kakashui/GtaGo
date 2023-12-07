<template>
  <div class="chat-options-tab">
    <div class="title">{{loc('familyMenu_79')}}</div>
    <div class="desc">{{loc('familyMenu_81')}}.</div>
    <div class="options-wrap">
      <div class="col">
        <div class="options-nav">
          <div 
            v-for="item in navList"
            :key="item.id"
            :class="[{active: item.key === currentTab}, 'item']" 
            @click="setCurrentTab(item.key)"
          >{{loc(item.text)}}</div>
        </div>
        <component 
          :is="currentTab"
          :currentIcon="currentIcon"
          :currentColor="currentColor"
          :colorList="colorList"
          @setCurrentColor="setCurrentColor"
          @setCurrentIcon="setCurrentIcon"
        />
      </div>
      <div class="col">
        <div class="prompt">{{loc('familyMenu_82')}}</div>
        <div class="chat-sample">
          <div class="message" :style="{color: currentColor}"><span class="icon-wrap" :style="{backgroundColor: currentColor}"><span class="icon" :style="{backgroundImage: 'url(/img/familyMenu/chatOptions/icon-'+ currentIcon +'.svg)'}"></span></span><span class="nickname">[F] S_Wilson [16]: </span> <span class="message-text">Hi!</span></div>
          <div class="message" :style="{color: currentColor}"><span class="icon-wrap" :style="{backgroundColor: currentColor}"><span class="icon" :style="{backgroundImage: 'url(/img/familyMenu/chatOptions/icon-'+ currentIcon +'.svg)'}"></span></span><span class="nickname">[F] A_Smith [66]: </span> <span class="message-text">Hello!</span></div>
          <div class="message" :style="{color: currentColor}"><span class="icon-wrap" :style="{backgroundColor: currentColor}"><span class="icon" :style="{backgroundImage: 'url(/img/familyMenu/chatOptions/icon-'+ currentIcon +'.svg)'}"></span></span><span class="nickname">[F] N_Family [3]: </span> <span class="message-text">Welcome!</span></div>
        </div>
      </div>
    </div>
    <div 
      v-if="currentColor !== chatOptions.currentColor || currentIcon !== chatOptions.currentIcon"
      class="btns-wrap"
    >
      <div 
        class="btn btn-save"
        @click="saveChatOptions"
      >{{loc('familyMenu_83')}}</div>
      <div 
        class="btn"
        @click="loadOptions"
      >{{loc('familyMenu_84')}}</div>
    </div>
  </div>
</template>

<script>
import IconTab from './IconTab'
import ColorTab from './ColorTab'
import { mapGetters, mapState } from 'vuex'

export default {
  name: 'ChatOptionsTab',

  components: {
    IconTab,
    ColorTab
  },

  data: function() {
    return {
      currentTab: null,
      currentColor: null,
      currentIcon: null,
      navList: [
        {
          text: 'familyMenu_85',
          key: 'IconTab'
        },
        {
          text: 'familyMenu_86',
          key: 'ColorTab'
        }
      ],
      colorList: [
        '#FF41E0',
        '#BD31FF',
        '#FF9AEF',
        '#C40000',
        '#D9D000',
        '#A8DC7F',
        '#5FD900',
        '#3FB5E8',
        '#2127BA',
        '#6C72FF',
        '#1A4E65',
        '#9D278A',
        '#7D1EAA',
        '#D680FF',
        '#8D0101',
        '#DEB448',
        '#D0CC61',
        '#3A8302',
        '#07719F',
        '#3139FF',
        '#B4E8FF',
        '#065B80',
        '#C55151',
        '#BE5E28',
        '#FF9052',
        '#FF5D02',
        '#AD800A',
        '#DEA000'
      ]
    }
  },

  computed: {
    ...mapState('familyMenu/controlPage', ['chatOptions']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentTab: function(value) {
      this.currentTab = value
    },
    loadOptions: function() {
      this.currentColor = this.chatOptions.currentColor
      this.currentIcon = this.chatOptions.currentIcon
    },
    saveChatOptions: function() {
      window.mp.trigger('familyMenu:saveChatOptions', this.currentIcon, this.currentColor)
    },
    setCurrentColor: function(value) {
      this.currentColor = value
    },
    setCurrentIcon: function(value) {
      this.currentIcon = value
    }
  },

  mounted(){
    this.setCurrentTab('IconTab'),
    this.loadOptions()
  }
}
</script>

<style lang="scss" scoped>
.chat-options-tab{
  display: flex;
  flex-flow: column;
  .title{
    font-weight: bold;
    font-size: 3.5rem;
    line-height: 3.5rem;
    margin-bottom: .25rem;
  }
  .desc{
    font-weight: bold;
    font-size: 1.2rem;
    line-height: 1.3rem;
    text-transform: uppercase;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: 3.3rem;
    max-width: 18.8rem;
  }
  .options-wrap{
    min-height: 22.75rem;
    margin-bottom: 1.2rem;
    position: relative;
    display: flex;
    &:after{
      content: '';
      width: 100%;
      height: 1px;
      background: linear-gradient(270deg, rgba(255, 255, 255, 0) 0%, #FFFFFF 100%);
      position: absolute;
      bottom: 0;
      left: 0;
    }
    .col{
      display: flex;
      flex-flow: column;
      &:first-child{
        width: 28.6rem;
        margin-right: 2rem;
      }
      &:last-child{
        width: 11.65rem;
      }
      .options-nav{
        display: flex;
        width: 100%;
        position: relative;
        margin-bottom: 1.2rem;
        &:after{
          content: '';
          width: 100%;
          height: 1px;
          background: linear-gradient(90deg, rgba(255, 255, 255, 0.2) 51.84%, rgba(255, 255, 255, 0) 99.91%);
          position: absolute;
          bottom: 0;
          left: 0;
        }
        .item{
          margin-right: 2rem;
          font-weight: bold;
          font-size: 1.2rem;
          line-height: 1.2rem;
          padding-bottom: .5rem;
          position: relative;
          &:hover, &.active{
            &:after{
              opacity: 1;
              transition: .2s;
            }
          }
          &:after{
            content: '';
            width: 100%;
            height: .1rem;
            background: #DCFF00;
            position: absolute;
            bottom: 0;
            left: 0;
            opacity: 0;
            transition: .2s;
          }
          &:last-child{
            margin-right: 0;
          }
        }
      }
      .prompt{
        font-weight: bold;
        font-size: 1.2rem;
        line-height: 1.2rem;
        padding-bottom: .5rem;
        width: 100%;
        position: relative;
        margin-bottom: 1.5rem;
        &:after{
          content: '';
          width: 100%;
          height: 1px;
          background: linear-gradient(90deg, #FFFFFF 0%, rgba(255, 255, 255, 0) 93.13%);
          position: absolute;
          left: 0;
          bottom: 0;
        }
      }
      .chat-sample{
        display: flex;
        flex-flow: column;
        padding: .8rem .8rem .8rem .95rem;
        width: 15.36rem;
        background: #252A2D;
        border-radius: .5rem;
        .message{
          display: flex;
          align-items: center;
          justify-content: flex-start;
          white-space: pre;
          font-family: "Arial";
          font-style: normal;
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.15rem;
          margin-bottom: .75rem;
          &:last-child{
            margin-bottom: 0;
          }
          .icon-wrap{
            width: 1.2rem;
            min-width: 1.2rem;
            height: 1.2rem;
            min-height: 1.2rem; 
            border-radius: .2rem;
            margin-right: .5rem;
            display: flex;
            align-items: center;
            justify-content: center;
            .icon{
              background-size: contain;
              background-position: center;
              background-repeat: no-repeat;
              width: .8rem;
              height: .8rem;
            }
          }
          .message-text{
            color: #fff;
            white-space: pre;
          }
        }
      }
    }
  }
  .btns-wrap{
    display: flex;
    align-items: center;
    .btn{
      font-weight: bold;
      font-size: 1.2rem;
      color: rgba(255, 255, 255, 0.7);
      padding: 1rem 1.2rem;
      margin-right: 1.2rem;
      &:last-child{
        margin-right: 0;
      }
      &.btn-save{
        color: #fff;
        background: #C1E704;
        border-radius: .1rem;
        &:hover{
          box-shadow: 0 0 1rem #DCFF00;
        }
      }
    }
  }
}
</style>
