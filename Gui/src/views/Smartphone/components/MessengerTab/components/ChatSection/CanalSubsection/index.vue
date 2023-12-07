<template>
  <div class="canal-subsection">
    <preloader-circle v-if="messengerData.CurrentChat.IsLoading" />
    <div v-else class="canal-subsection__content">
      <div class="canal-subsection__header">
        <back-button :location="{ key: currentTab.key, section: 'GeneralSection' }" @extraClick="closeCurrentChat" />
        <div class="desc">
          <span class="desc__title">{{ messengerData.CurrentChat.Name }}</span>
          <span class="desc__subscribers">{{loc(`phone:chat:post:1@${messengerData.CurrentChat.SubscribersCount}`)}}</span>
        </div>
        <div 
          :style="{ background: messengerData.CurrentChat.IconColors }" 
          class="avatar"
          @click="setCurrentTab({key: currentTab.key, section: currentTab.section, subsection: 'CanalOptionsSubsection' })"
        ></div>
      </div>
      <div v-bar>
        <div class="canal-subsection__list">
          <list-item
            v-for="item in items"
            :key="item.Id"
            :item="item"
            @edit="edit"
          />
        </div>
      </div>
      <div class="canal-subsection__footer">
        <button v-if="messengerData.CurrentChat.CanCreatingPosts" class="btn-create" @click="isCreation = true">
          <div class="icon"></div>
          <span class="text">{{loc('phone:chat:post:4')}}</span>
        </button>
        <button v-else-if="messengerData.CurrentChat.IsMuted" class="btn-create" @click="toggleMuted(false)">
          <span class="text">{{loc('phone:chat:post:2')}}</span>
        </button>
        <button v-else class="btn-create" @click="toggleMuted(true)">
          <span class="text">{{loc('phone:chat:post:3')}}</span>
        </button>
      </div>
      <transition name="smartphone-fade">
        <div class="canal-subsection__layer" v-if="isCreation || isEdit"></div>
      </transition>
      <transition name="smartphone-bottom-to-top-slide">
        <create-popup v-if="isCreation" @closeCreation="isCreation = false" @create="create" />
      </transition>
      <transition name="smartphone-bottom-to-top-slide">
        <edit-popup
          v-if="isEdit"
          :currentId="currentPostEditedId"
          @close="isEdit = false"
          @confirm="editConfirm"
          @delete="deleteEditPost"
        />
      </transition>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapActions, mapGetters } from 'vuex'
import BackButton from '../common/BackButton'
import CreatePopup from './CreatePopup'
import EditPopup from './EditPopup'
import ListItem from './LIstItem'
import PreloaderCircle from '../../PreloaderCircle'

export default {
  name: 'CanalSection',

  components: {
    BackButton,
    CreatePopup,
    EditPopup,
    ListItem,
    PreloaderCircle
  },

  data: function() {
    return {
      isCreation: false,
      isEdit: false,
      currentPostEditedId: null,
      items: []
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
  },

  watch: {
    messengerData: {
      handler: function() {
        this.setCurrentPosts();
      },
      deep: true
    } 
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),
    ...mapActions('smartphone/messagePage', ['msg_getIsHavePermission', 'msg_dropCurrentChat']),
    setCurrentPosts: function() {
      const nonSortedItems = this.messengerData.CurrentChat.Items;
      this.items = nonSortedItems.sort((a, b) => b.Time - a.Time);
    },

    closeCurrentChat: function() {
      this.msg_dropCurrentChat();
    },

    edit: function(id) {
      this.isEdit = true
      this.currentPostEditedId = id
    },

    deleteEditPost: function() {
      window.mp.triggerServer('phone::msg::deletePost', this.currentPostEditedId);
      this.isEdit = false;
    },

    editConfirm: function(imgUrl, title, text) {
      window.mp.triggerServer('phone::msg::editPost', this.currentPostEditedId, imgUrl, title, text);
      this.isEdit = false;
    },

    create: function(imgUrl, title, text) {
      window.mp.triggerServer('phone::msg::createPost', this.messengerData.CurrentChat.Id, imgUrl, title, text);  
      this.isCreation = false;
    }
  },

  mounted: function() {
    this.setCurrentPosts();
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.canal-subsection {
  height: 100%;
  width: 100%;
  position: relative;
  &__main {
    height: 100%;
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
  &__header {
    flex: 0 0 4.5rem;
    background: #fff;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 2rem 0.5rem 0.5rem 0.75rem;
    position: relative;
    .desc {
      text-align: center;
      position: absolute;
      transform: translate(-50%, 0);
      left: 50%;
      display: flex;
      flex-direction: column;
      &__title {
        font-size: 0.7rem;
        font-weight: 500;
        line-height: 0.8rem;
        color: #020202;
        width: 8.85rem;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
      }
      &__subscribers {
        margin: 0.1rem 0 0 0;
        font-size: 0.65rem;
        line-height: 0.75rem;
        color: #9E9EA2;
      }
    }
    .avatar {
      width: 2rem;
      height: 2rem;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      &::after {
        content: '';
        background: center /contain no-repeat url("/img/hud/smartphone/messengerTab/megaphone.svg");
        width: 0.9rem;
        height: 0.9rem;
      }
    }
  }
  &__list {
    height: 25rem !important;
  }
  &__footer {
    height: 3.5rem;
    display: flex;
    justify-content: center;
    background: #fff;
    padding: 1rem 0 0 0;
    .btn-create {
      display: flex;
      align-items: center;
      height: fit-content;
      background: transparent;
      &:hover {
        .icon {
          opacity: 1;
        }
        .text::after {
          transform: scale(1);
        }
      }
      .icon {
        mask: url("/img/hud/smartphone/messengerTab/edit.svg") no-repeat center / contain;
        background: #4E6ED9;
        opacity: 0.5;
        width: 0.9rem;
        height: 0.9rem;
        transition: opacity 0.2s;
      }
      .text {
        font-size: 0.7rem;
        line-height: 0.8rem;
        color: #4E6ED9;
        margin: 0 0 0 0.5rem;
        position: relative;
        &::after {
          position: absolute;
          transform: scale(0);
          transition: transform 0.2s;
          top: 100%;
          left: 0;
          width: 100%;
          content: '';
          height: 1px;
          background: rgba(78, 110, 217, 0.3);
        }
      }
    }
  }
  &__layer {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7);
  }
}
.canal-subsection__layer-enter-active {
  animation: fade 0.4s;
}
.canal-subsection__layer-leave-active {
  animation: fade 0.4s reverse;
}
</style>
