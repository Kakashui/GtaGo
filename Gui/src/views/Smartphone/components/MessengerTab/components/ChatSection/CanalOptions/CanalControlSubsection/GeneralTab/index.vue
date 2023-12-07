<template>
  <div class="general-tab">
    <div class="general-tab__header">
      <back-button :location="{ key: currentTab.key, section: currentTab.section, subsection: 'GroupOptionsSubsection', data: currentTab.data }" :isText="true" />
      <span class="title">{{loc('phone:chat:gen:1')}}</span>
    </div>
    <button class="general-tab__add" @click="isAddAdmin = true">
      <div class="icon"></div>
      <span class="text">{{loc('phone:chat:gen:2')}}</span>
    </button>
    <!-- change -->
    <div v-bar>
      <div class="general-tab__list">
        <list-item
          v-for="item in items"
          :key="item.Id"
          :item="item"
          @event="setTab({ key: 'ProfileTab', id: item.Id })"
        />
      </div>
    </div>
    <p class="general-tab__desc">{{loc('phone:chat:gen:3')}}</p>
    <transition name="smartphone-fade">
      <div class="general-tab__layer" v-if="isAddAdmin"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <popup-add
        v-if="isAddAdmin"
        @close="isAddAdmin = false"
        :setTab="setTab"
      />
    </transition>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import BackButton from '../../../common/BackButton'
import ListItem from './ListItem'
import PopupAdd from './PopupAdd'

export default {
  name: 'CanalControlSubsection',

  components: {
    BackButton,
    ListItem,
    PopupAdd
  },

  watch: {
    messengerData: {
      handler: function() {
        this.items = this.messengerData.CurrentChat.Admins;
      },
      deep: true
    } 
  },

  props: { setTab: Function },

  data: function() {
    return {
      isAddAdmin: false,
      items: [
        // { Id: 0, IsOnline: true, DisplayedName: 'Username', AdminLvl: 2,  },
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  mounted: function() {
    const chat = this.messengerData.CurrentChat;

    if (!chat.Admins) {
      window.mp.triggerServer('phone::msg::requestAdmins', chat.Id);
    }

    this.items = chat.Admins;
  }
}
</script>

<style lang="scss" scoped>
.general-tab {
  padding: 2.5rem 0 0 0;
  width: 100%;
  height: 100%;
  background: #F3F6FA;
  display: flex;
  flex-direction: column;
  position: relative;
  &__header {
    display: flex;
    align-items: center;
    position: relative;
    padding: 0 0.75rem;
    .title {
      position: absolute;
      transform: translate(-50%, 0);
      font-size: 0.7rem;
      left: 50%;
    }
  }
  &__add {
    display: flex;
    align-items: center;
    background: #FFFFFF;
    height: 2.2rem;
    padding: 0 0 0 1.25rem;
    margin: 1.1rem 0 0.5rem 0;
    .icon {
      mask: url("/img/hud/smartphone/messengerTab/chatSection/canalOptions/userAdd.svg") no-repeat center / contain;
      background: #4E6ED9;
      width: 1rem;
      height: 1rem;
    }
    .text {
      color: #4E6ED9;
      font-size: 0.7rem;
      margin: 0 0 0 1rem;
    }
  }
  &__list {
    height: 21rem !important;
    background: #fff;
    border-top: solid 1px rgba(195, 195, 195, 0.35);
    border-bottom: solid 1px rgba(195, 195, 195, 0.35);
  }
  &__desc {
    margin: 0.5rem 0 0 0;
    padding: 0 0.75rem;
    font-size: 0.65rem;
    line-height: 0.8rem;
    color: #8A8A8E;
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
.fade-enter-active, .fade-leave-active {
  transition: opacity .4s;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
}
.slide-enter-active, .slide-leave-active {
  transition: transform .4s;
}
.slide-enter, .slide-leave-to {
  transform: translate(0, 150%);
}
</style>
