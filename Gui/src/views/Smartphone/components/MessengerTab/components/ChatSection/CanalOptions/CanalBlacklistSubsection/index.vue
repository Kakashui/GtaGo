<template>
  <div class="canal-blacklist">
    <div class="canal-blacklist__header">
      <back-button :isText="true" :location="{ key: currentTab.key, section: currentTab.section, subsection: 'GroupOptionsSubsection', data: currentTab.data }"  />
      <span class="title">{{loc('phone:msg:blist:5')}}</span>
    </div>
    <button class="canal-blacklist__btn-add" @click="isAddUser = true">{{loc('phone:msg:blist:6')}}</button>
    <p class="canal-blacklist__desc">{{loc('phone:msg:blist:7')}}</p>
    <div class="canal-blacklist__main">
      <span class="title">{{loc('phone:msg:blist:8')}}</span>
      <div v-bar>
        <div class="list">
          <!-- <list-item
            v-for="item in users"
            :key="item.Id"
            :item="item"
            @event="setCurrentTab({ key: currentTab.key, section: 'ViewedProfileSection', data: { location: { key: currentTab.key, section: currentTab.section, subsection: currentTab.subsection }} })"
          /> -->
          <list-item
            v-for="item in users"
            :key="item.AccountId"
            :item="item"
            @event="selectUser(item.AccountId)"
          />
        </div>
      </div>
    </div>
    <transition name="smartphone-fade">
      <div class="canal-blacklist__layer" v-if="isAddUser || isConfirmPopup"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <popup-add v-if="isAddUser" @close="isAddUser = false" />
    </transition>
    <transition name="smartphone-scale-in">
      <confirm-popup
        v-if="isConfirmPopup"
        @close="isConfirmPopup = false"
        :selectedUserId="selectedUserId"
      />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import BackButton from '../../common/BackButton'
import PopupAdd from './PopupAdd'
import ListItem from './ListItem'
import ConfirmPopup from './ConfirmPopup'

export default {
  name: 'CanalBlacklistSubsection',
  
  components: {
    BackButton,
    PopupAdd,
    ListItem,
    ConfirmPopup
  },

  data: function() {
    return {
      isAddUser: false,
      isConfirmPopup: false,
      selectedUserId: null,
      users: [
        // { Id: 1, Name: 'USERUSERUSERUSERUSERUSERW', IsOnline: true, whoAdded: 'USER_USERUSER_USERUSER_USERUSER_USER' },
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  watch: {
    messengerData: {
      handler: function() {
        this.users = this.messengerData.CurrentChat.BlockItems;
      },
      deep: true
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    selectUser: function(id) {
      this.isConfirmPopup = true
      this.selectedUserId = id
    }
  },

  mounted: function() {
    this.users = this.messengerData.CurrentChat.BlockItems;

    if (!this.users) {
      window.mp.triggerServer('phone::msg::manage::requestBlockedUsers', this.messengerData.CurrentChat.Id);
    }
  }
}
</script>

<style lang="scss" scoped>
.canal-blacklist {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  padding: 2.5rem 0 0 0;
  &__header {
    padding: 0 0.75rem;
    display: flex;
    align-items: center;
    position: relative;
    margin: 0 0 1rem 0;
    .title {
      position: absolute;
      transform: translate(-50%, 0);
      left: 50%;
      font-size: 0.7rem;
      line-height: 0.8rem;
    }
  }
  &__btn-add {
    height: 2.2rem;
    background: #fff;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    color: #4E6ED9;
    background: 1.25rem 50% / 1rem no-repeat url("/img/hud/smartphone/messengerTab/chatSection/canalOptions/userAdd.svg"), #fff;
    font-size: 0.7rem;
    margin: 0 0 0.5rem 0;
  }
  &__desc {
    font-size: 0.65rem;
    line-height: 0.8rem;
    color: #8A8A8E;
    padding: 0 0.75rem;
  }
  &__main {
    margin: 1.25rem 0 0 0;
    display: flex;
    flex-direction: column;
    .title {
      font-size: 0.7rem;
      font-weight: 300;
      line-height: 0.8rem;
      color: #8A8A8E;
      padding: 0 0 0.5rem 0.75rem;
    }
    .list {
      height: 20.2rem !important;
      background: #fff;
      border-top: solid 1px rgba(195, 195, 195, 0.35);
      border-bottom: solid 1px rgba(195, 195, 195, 0.35);
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

