<template>
  <div class="canal-subscribers-subsection">
    <div class="canal-subscribers-subsection__header">
      <back-button :isText="true" :location="{ key: currentTab.key, section: 'ChatSection', subsection: 'CanalSubsection' }"/>
      <div class="btn-edit" @click="isEdit = !isEdit">{{ isEdit ? 'phone:chat:subscr:1' : 'phone:chat:subscr:2' }}</div>
      <div class="title">{{loc('phone:chat:subscr:3')}}</div>
    </div>
    <div class="btn-add" @click="isInvite = true">{{loc('phone:chat:subscr:4')}}</div>
    <div v-bar>
      <div class="canal-subscribers-subsection__list">
        <list-item
          v-for="item in this.list"
          :key="item.id"
          :item="item"
          :edit="isEdit"
          @delete="setDeletedId"
          @get-profile="getProfile"
        />
      </div>
    </div>
    <transition name="smartphone-fade">
      <div class="canal-subscribers-subsection__layer" v-if="isInvite || (deletedId || deletedId === 0) || isPopupUserCard"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <popup-invite v-if="isInvite" @close="isInvite = false" />
    </transition>
    <transition name="smartphone-scale-in">
      <popup-remove
        v-if="deletedId || deletedId === 0"
        @close="deletedId = null"
        @confirm="deleteConfirm"
      />
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <popup-user-card
        v-if="isPopupUserCard"
        @close="isPopupUserCard = false"
        :currentProfile="currentProfile"
      />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import BackButton from '../../common/BackButton'
import ListItem from './ListItem'
import PopupInvite from './PopupInvite'
import PopupRemove from './PopupRemove'
import PopupUserCard from './PopupUserCard'

export default {
  name: 'CanalSubscribersSubsection',

  components: {
    BackButton,
    ListItem,
    PopupInvite,
    PopupRemove,
    PopupUserCard
  },

  data: function() {
    return {
      isEdit: false,
      isInvite: false,
      deletedId: null,
      isPopupUserCard: false,
      currentProfileId: null,
      list: []
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc']),
    currentProfile: function() {
      return this.list.find(item => item.id === this.currentProfileId)
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    setDeletedId: function(id) {
      this.deletedId = id
    },

    deleteConfirm: function() {
      console.log(this.deletedId)
    },

    getProfile: function(id) {
      this.isPopupUserCard = true
      this.currentProfileId = id
    }
  }
}
</script>

<style lang="scss" scoped>
.canal-subscribers-subsection{
  display: flex;
  flex-flow: column;
  background: #F3F6FA;
  padding-top: 2.6rem;
  position: relative;
  height: 100%;
  &__header {
    display: flex;
    justify-content: space-between;
    padding: 0 1rem 0 0.75rem;
    align-items: center;
    .btn-edit{
      font-size: .7rem;
      color: #4E6ED9;
      &:hover {
        text-decoration: underline;
      }
    }
    .title{
      position: absolute;
      transform: translate(-50%, 0);
      left: 50%;
      font-size: .7rem;
      color: #000000;
    }
  }
  .btn-add{
    display: flex;
    align-items: center;
    justify-content: flex-start;
    padding: .6rem 0 .6rem 1.25rem;
    font-size: .7rem;
    line-height: .8rem;
    background: #FFFFFF;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    margin: 1rem 0 .5rem 0;
    color: #4E6ED9;
    &:before{
      content: '';
      width: 1rem;
      height: 1rem;
      margin-right: 1rem;
      background-image: url('/img/hud/smartphone/messengerTab/chatSection/canalOptions/userAdd.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
  }
  &__list{
    height: 25.75rem !important;
    background: #fff;
    padding: .25rem 0 .25rem .75rem;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
  }
  &__layer {
    background: rgba(0, 0, 0, 0.7);
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
  }
}
</style>
