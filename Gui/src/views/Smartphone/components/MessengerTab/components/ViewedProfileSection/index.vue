<template>
  <div class="viewed-profile-section">
    <preloader-circle v-if="messengerData.viewedProfile.isLoading" />
    <div v-else class="viewed-profile-section__content">
      <div class="header">
        <back-button 
          :isText="true"
          :location="{ key: 'MessengerTab', section: 'GeneralSection', subsection: 'GeneralSubsection', data: null }"
          :styles="{ position: 'absolute', left: '0.75rem', top: '2.5rem' }"
        />
        <div
          class="viewed-profile-section__avatar"
          v-bind:class="{online: messengerData.viewedProfile.IsOnline}"
        ></div>
        <div class="viewed-profile-section__nickname">{{messengerData.viewedProfile.DisplayedName}}</div>
        <div
          class="viewed-profile-section__entry-time"
          v-if="messengerData.viewedProfile.IsOnline"
        >Online</div>
        <div 
          class="viewed-profile-section__entry-time"
          v-else
        >{{getLastVisit(this.messengerData.viewedProfile.LastVisitTime)}}</div>
        <div class="viewed-profile-section__interaction-buttons">
          <div 
            v-if="!messengerData.viewedProfile.IsNumberHided"
            class="btn" 
            @click="call"
          >
            <div class="icon icon-phone"></div>
            <div class="desc">{{loc('phone:cont:sub:12')}}</div>
          </div>
          <div 
            class="btn"
            @click="msg_openPrivateChat(messengerData.viewedProfile.Id)"
          >
            <div class="icon icon-comment"></div>
            <div class="desc">{{loc('phone:cont:sub:13')}}</div>
          </div>
          <div 
            class="btn"
            v-if="this.messengerData.viewedProfile.IsMuted"
            @click="muteAccount(false)"
          >
            <div class="icon icon-mute muted"></div>
            <div class="desc">{{loc('phone:cont:sub:14')}}</div>
          </div>
          <div 
            class="btn"
            v-else
            @click="muteAccount(true)"
          >
            <div class="icon icon-mute"></div>
            <div class="desc">{{loc('phone:cont:sub:14')}}</div>
          </div>
        </div>
      </div>
      <div class="body-section">
        <div class="viewed-profile-section__info">
          <div class="title">{{loc('phone:profile:9')}}</div>
          <div class="value">@{{messengerData.viewedProfile.Username}}</div>
        </div>
        <div class="viewed-profile-section__info"
          v-if="!messengerData.viewedProfile.IsNumberHided">
          <div class="title">{{loc('phone:back0')}}</div>
          <div class="value">{{messengerData.viewedProfile.Number}}</div>
        </div>
        <div
            v-if="!messengerData.viewedProfile.IsInContacts"
            class="viewed-profile-section__body-interaction" 
            @click="isAddPopup = true"
        >
          <div class="btn">{{loc('phone:cont:sub:8')}}</div>
        </div>
        <div
            v-else
            class="viewed-profile-section__body-interaction" 
            @click="deleteContact"
        >
          <div class="btn block">{{loc('phone:cont:sub:7')}}</div>
        </div>
        <div class="viewed-profile-section__body-interaction">
          <div 
            class="btn block"
            v-if="!this.messengerData.viewedProfile.IsBlocked"
            @click="blockAccount(true)"
          >{{loc('phone:call:btn:5')}}</div>
          <div 
            class="btn block"
            v-else
            @click="blockAccount(false)"
          >{{loc('phone:call:btn:7')}}</div>
        </div>
      </div>
      <transition name="smartphone-fade">
        <div class="layer" v-if="isAddPopup"></div>
      </transition>
      <transition name="smartphone-bottom-to-top-slide">
        <add-popup v-if="isAddPopup" @close="isAddPopup = false" />
      </transition>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapActions, mapGetters } from 'vuex'

import BackButton from '../ChatSection/common/BackButton'
import AddPopup from './AddPopup'
import PreloaderCircle from '../PreloaderCircle'

export default {
  name: 'ViewedProfileSection',

  components: {
    BackButton,
    AddPopup,
    PreloaderCircle
  },

  data: function() {
    return {
      isAddPopup: false
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),
    ...mapActions('smartphone/messagePage', ['msg_openPrivateChat']),

    muteAccount: function(toggle) {
      window.mp.triggerServer('phone::msg::muteAccount', this.messengerData.viewedProfile.Id, toggle);
    },

    blockAccount: function(toggle) {
      window.mp.triggerServer('phone::msg::blockAccount', this.messengerData.viewedProfile.Id, toggle);
    },
    
    deleteContact: function() {
      window.mp.triggerServer('phone::msg::removeContact', this.messengerData.viewedProfile.Id);
    },

    call: function() {
      window.setData('smartphone/setCallData', { destination: this.messengerData.viewedProfile.Number, status: 'outcome' })
      window.setData('smartphone/setCurrentTab', { key: 'CallTab' })
    },

    getLastVisit: function(LastVisitTime) {
        const date = new Date(LastVisitTime);

        const dayMonth = `${this.getDoubleDigit(date.getUTCDate())}.${this.getDoubleDigit(date.getUTCMonth() + 1)}`;
        const hoursMinutes = `${this.getDoubleDigit(date.getUTCHours())}:${this.getDoubleDigit(date.getUTCMinutes())}`;
        
        return this.loc(`phone:contact:2@${dayMonth}@${hoursMinutes}`);    
    },

    getDoubleDigit: function(number) {
      return number < 10 ? `0${number}` : number;
    }
  }
}
</script>

<style lang="scss" scoped>
.viewed-profile-section{
  width: 100%;
  height: 100%;
  background: #F3F6FA;
  position: relative;
  &__content {
    display: flex;
    flex-flow: column;
    width: 100%;
    height: 100%;
  }
  .header, .body-section{
    display: flex;
    flex-flow: column;
    width: 100%;
    background: #FFFFFF;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
  }
  .header{
    padding: 3rem 0 1rem 0;
    position: relative;
    align-items: center;
  }
  .body-section{
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    box-shadow: 0px 4px 10px rgba(236, 234, 244, 0.5);
    margin-top: .75rem;
  }
  &__avatar{
    width: 4rem;
    height: 4rem;
    position: relative;
    margin-bottom: .75rem;
    background-image: url('/img/hud/smartphone/messengerTab/avatar.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    &.online{
      &:before{
        position: absolute;
        content: '';
        border-radius: 50%;
        width: .8rem;
        height: .8rem;
        right: .15rem;
        top: .15rem;
        background: #4BDB4B;
        border: .1rem solid #FFFFFF;
        box-sizing: border-box;
      }
    }
  }
  &__nickname{
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.15rem;
    color: #151515;
  }
  &__entry-time{
    font-size: .7rem;
    line-height: .8rem;
    color: #AAAAAA;
    margin: .25rem 0 1.2rem 0;
  }
  &__interaction-buttons{
    display: flex;
    align-items: center;
    justify-content: center;
    .btn{
      margin-right: 2.1rem;
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: center;
      &:hover .desc {
        text-decoration: underline;
      }
      .icon{
        width: 2rem;
        height: 2rem;
        background: #4E6ED9;
        box-shadow: 0 .2rem .5rem rgba(236, 234, 244, 0.5);
        border-radius: 5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        &:before{
          content: '';
          width: .8rem;
          height: .8rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
        }
        &.icon-phone{
          &:before{
            background-image: url('/img/hud/smartphone/messengerTab/icon-phone.svg');
          }
        }
          &.icon-comment{
          &:before{
            background-image: url('/img/hud/smartphone/messengerTab/icon-comment.svg');
          }
        }
          &.icon-mute{
          &.muted{
            &:before{
            background-image: url('/img/hud/smartphone/messengerTab/icon-mute.svg');
          }
          }
          &:before{
            background-image: url('/img/hud/smartphone/messengerTab/icon-unmute.svg');
          }
        }
      }
      .desc{
        display: flex;
        justify-content: center;
        font-weight: normal;
        font-size: .55rem;
        line-height: .65rem;
        margin-top: .3rem;
        width: 1px;
        text-align: center;
        color: #4E6ED9;
      }
      &:last-child{
        margin-right: 0;
      }
    }
  }
  &__info, &__body-interaction{
    margin-left: 1rem;
    padding: .6rem 0;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    &:hover .btn {
      text-decoration: underline;
    }
    &:last-child{
      border-bottom: 0;
    }
  }
  &__info{
    display: flex;
    flex-flow: column;
    align-content: flex-start;
    .title{
      font-weight: normal;
      font-size: .65rem;
      line-height: .75rem;
      color: #020202;
      margin-bottom: .15rem;
    }
    .value{
      font-weight: normal;
      font-size: .7rem;
      line-height: .8rem;
      color: #4E6ED9;
    }
  }
  &__body-interaction{
    .btn{
      font-weight: normal;
      font-size: .7rem;
      line-height: .8rem;
      color: #4E6ED9;
      &.block{
        color: #FF3B30;
      }
    }
  }
  .layer {
    position: absolute;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7);
    top: 0;
    left: 0;
  }
}
</style>