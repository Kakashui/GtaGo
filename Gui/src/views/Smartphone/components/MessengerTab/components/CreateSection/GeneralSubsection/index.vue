<template>
  <div class="general-subsection">
    <div class="general-subsection__header">
      <back-button 
        :location="{key: currentTab.key, section: 'GeneralSection', subsection: 'GeneralSubsection'}"
        :isText="true"
      />
    </div>
    <div class="general-subsection__title">{{loc('phone:channel:1')}}</div>
    <div class="general-subsection__btns">
      <div class="btn-change btn-change__group" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: 'GroupFirstSubsection' })">
        <div class="btn-change__title">{{loc('phone:channel:2')}}</div>
        <div class="btn-change__icon"></div>
      </div>
      <div class="btn-change btn-change__channel" @click="isCreatePopupVisible = true">
        <div class="btn-change__title">{{loc('phone:channel:3')}}</div>
        <div class="btn-change__icon"></div>
      </div>
    </div>
    <div v-bar>
      <blocks-list :items="messengerData.Contacts" />
    </div>
    <transition name="smartphone-fade">
      <div class="general-subsection__layer" v-if="isCreatePopupVisible"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <create-popup v-if="isCreatePopupVisible" @close="isCreatePopupVisible = false" />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import BlocksList from '../../GeneralSection/ContactsSubsection/BlocksList'
import BackButton from '../../ChatSection/common/BackButton'
import CreatePopup from './CanalCreatePopup'

export default {
  name: 'GeneralSubbsection',

  components: {
    BlocksList,
    BackButton,
    CreatePopup,
  },

  data: function() {
    return {
      isCreatePopupVisible: false,
      currentGradient: 'linear-gradient(45deg, rgb(161, 140, 209) 0%, rgb(251, 194, 235) 100%)',
      triggerPallette: false
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),
  }
}
</script>

<style lang="scss" scoped>
@keyframes scale-anim {
  from {
    transform: scale(0);
  }
  to {
    transform: scale(1);
  }
}
@keyframes opacity-anim {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
@keyframes transform-anim {
  from {
    transform: translateY(200%);
  }
  to {
    transform: translateY(0%);
  }
}
.general-subsection::v-deep .blocks-list {
  height: 20.5rem !important;
}
.general-subsection{
  display: flex;
  flex-flow: column;
  padding-top: 2.5rem;
  height: 100%;
  &__header {
    display: flex;
    padding: 0 0.75rem;
  }
  &__title{
    font-weight: bold;
    font-size: 1.3rem;
    line-height: 1.5rem;
    margin: .5rem 0 1rem 0;
    color: #020202;
    margin-left: .75rem;
  }
  &__btns{
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    height: 5rem;
    padding: 0 .5rem;
    margin: 0 0 1rem 0;
    .btn-change{
      width: 6.9rem;
      height: 100%;
      padding: .75rem;
      overflow: hidden;
      border-radius: .5rem;
      position: relative;
      &:hover{
        .btn-change__icon{
          transform: scale(1.2);
          transition: .3s;
        }
      }
      &:after{
        content: '';
        width: 2.5rem;
        height: 2.5rem;
        position: absolute;
        right: 0;
        bottom: 0;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: right bottom;
      }
      &__group{
        background: #6A65FF;
        &:after{
          background-image: url('/img/hud/smartphone/messengerTab/img-group.svg');
        }
      }
      &__channel{
        background: #7ED3B2;
        &:after{
          background-image: url('/img/hud/smartphone/messengerTab/img-channel.svg');
        }
      }
      &__title{
        font-size: .7rem;
        font-weight: 500;
        line-height: .8rem;
        color: #FFFFFF;
      }
      &__icon{
        width: 1.2rem;
        height: 1.2rem;      
        background-color: rgba(255, 255, 255, 0.3);
        border-radius: 50%;
        margin-top: .5rem;
        background-image: url('/img/hud/smartphone/messengerTab/arr-change.svg');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        transition: .3s;
      }
    }
  }
  &__layer {
    position: absolute;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7);
    top: 0;
    left: 0;
  }
}
</style>
