<template>
  <div class="general-subsection">
    <div class="general-subsection__header">
      <span class="title">{{loc('phone:gen:3')}}</span>
      <button class="btn btn-new" @click="setCurrentTab({key: currentTab.key, section: 'CreateSection', subsection: 'GeneralSubsection' })"></button>
    </div>
    <label class="general-subsection__label">
      <input
        class="input"
        :placeholder="loc('phone:serch')"
      />
      <div class="icon"></div>
    </label>
    <div class="general-subsection__main">
      <div class="navigation">
        <navigation-item
          v-for="item in navigationItems"
          :key="item.key"
          :item="item"
          :currentFilter="currentFilter"
          @setCurrentFilter="setCurrentFilter"
        />
      </div>
      <main-list :items="currentChats" />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import NavigationItem from './NavigationItem'
import MainList from './MainLIst'

export default {
  name: 'GeneralSubsection',

  components: { 
    NavigationItem,
    MainList
  },

  data: function() {
    return {
      currentFilter: 'all',
      navigationItems: [
        { text: 'phone:gen:navitem:1', key: 'all' },
        { text: 'phone:gen:navitem:2', key: 'new' },
        { text: 'phone:gen:navitem:3', key: 'channels' }
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
    currentChats: function() {
      const chats = this.messengerData.Chats
        .filter(c => c.Type !== 0 || c.LastMessage)
        .sort((a, b) => b.ActionTime - a.ActionTime)
      let rightChats = []
      
      if (this.currentFilter === 'new') {
        rightChats = this.sortByNewParameter(chats)
      } else if (this.currentFilter === 'channels') {
        const rightItems = chats.filter(item => item.Type === 2)
        rightChats = rightItems
      } else {
        rightChats = chats;
      }
      
      return rightChats
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    setCurrentFilter: function(value) {
      this.currentFilter = value
    },

    sortByNewParameter: function(items) {
      const nonReadItems =  items.filter(item => item.NonReadMessagesCount > 0)
      const sortByTimeItems = nonReadItems.sort((a, b) => a.ActionTime - b.ActionTime)

      return sortByTimeItems
    }
  }
}
</script>

<style lang="scss" scoped>
.general-subsection {
  height: 100%;
  width: 100%;
  padding: 2.25rem 0 0 0;
  &__header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 0.75rem;
    position: relative;
    .btn {
      background: transparent;
      &-change {
        color: #007AFF;
        font-size: 0.7rem;
        line-height: 0.8rem;
      }
      &-new {
        background: #007AFF;
        width: 1rem;
        height: 1rem;
        mask: url("/img/hud/smartphone/messengerTab/edit.svg") no-repeat center / contain;
      }
    }
    .title {
      position: absolute;
      transform: translate(-50%, -50%);
      top: 50%;
      left: 50%;
      font-weight: 500;
      font-size: 0.7rem;
      line-height: 0.8rem;
      color: #020202;
    }
  }
  &__label {
    display: block;
    width: 13.7rem;
    height: 1.8rem;
    position: relative;
    border-radius: 0.5rem;
    overflow: hidden;
    margin: 0.9rem auto 0 auto;
    .input {
      width: 100%;
      height: 100%;
      font-size: 0.7rem;
      padding: 0 0.5rem 0 1.75rem;
      background: rgba(118, 118, 128, 0.12);
      color: #020202;
      &::placeholder {
        color: rgba(60, 60, 67, 0.6);
      }
    }
    .icon {
      mask: url("/img/hud/smartphone/messengerTab/magnifier.svg") no-repeat center / contain;
      width: 0.7rem;
      height: 0.7rem;
      background: #8E8E93;
      position: absolute;
      transform: translate(0, -50%);
      top: 50%;
      left: 0.55rem;
    }
  }
  &__main {
    .navigation {
      display: flex;
      padding: 0 0.75rem;
      margin: 0.75rem 0 0 0;
      position: relative;
      &::after {
        content: '';
        width: 100%;
        height: 1px;
        position: absolute;
        bottom: 0;
        left: 0;
        background: rgba(195, 195, 195, 0.35);
      }
    }
  }
}
</style>
