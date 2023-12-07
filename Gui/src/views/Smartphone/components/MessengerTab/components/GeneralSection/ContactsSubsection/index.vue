<template>
  <div class="contacts-subsection">
    <div class="contacts-subsection__header">
      <span class="title">{{loc('phone:contact:1')}}</span>
      <button class="btn" @click="isAddPopup = true"></button>
    </div>
    <search-label :value="search" @input="search = $event" :styles="{ margin: '0 auto 0.75rem auto' }" />
    <div v-bar v-show="search.length > 0">
      <div class="contacts-subsection__list" >
        <list-item
          v-for="(item, index) in filterContacts"
          :key="index"
          :item="item"
        />
      </div>
    </div>
    <div v-bar v-show="search.length <= 0">
      <blocks-list :items="messengerData.Contacts"/>
    </div>
    <transition name="smartphone-fade">
      <div class="layer" v-if="isAddPopup"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <add-popup v-if="isAddPopup" @close="isAddPopup = false" />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import SearchLabel from './SearchLabel'
import BlocksList from './BlocksList'
import AddPopup from './AddPopup'

import ListItem from './ListItem'

export default {
  name: 'ContactsSubsection',

  components: {
    SearchLabel,
    ListItem,
    BlocksList,
    AddPopup
  },

  data: function() {
    return {
      search: '',
      items: [],
      isAddPopup: false
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),

    filterContacts: function() {
      return this.messengerData.Contacts.filter(item => {
        return item.Name.toLowerCase().includes(this.search.toLowerCase())
      })
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),
  }
}
</script>

<style lang="scss" scoped>
.contacts-subsection::v-deep .blocks-list {
  height: 22.25rem !important;
}
.contacts-subsection {
  padding: 2.25rem 0 0 0;
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  position: relative;
  &__header {
    position: relative;
    padding: 0 0.75rem;
    display: flex;
    justify-content: flex-end;
    margin: 0 0 0.9rem 0;
    .title {
      display: block;
      position: absolute;
      transform: translate(-50%, -50%);
      top: 50%;
      left: 50%;
      font-size: 0.7rem;
      font-weight: 500;
      line-height: 0.8rem;
      color: #020202;
    }
    .btn {
      width: 1rem;
      height: 1rem;
      background: #007AFF;
      mask: url("/img/hud/smartphone/messengerTab/plus.svg") no-repeat center / contain;
    }
  }
  &__list {
    height: 22.25rem !important;
    display: flex;
    flex-direction: column;
  }
  .layer {
    position: absolute;
    width: 100%;
    height: 33rem;
    background: rgba(0, 0, 0, 0.7);
    top: 0;
    left: 0;
    z-index: 43;
  }
}
</style>


