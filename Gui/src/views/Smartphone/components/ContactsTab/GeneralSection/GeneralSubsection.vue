<template>
  <div class="general-subsection">
    <div class="general-subsection__header">
      <div class="title">{{loc('phone:gen:1')}}</div>
      <button class="btn" @click="setContactPopupData(true, 'add')">
        <img
          src="img/hud/smartphone/plus.svg"
          alt="plus"
          class="btn__icon"
        >
      </button>
      <input
        type="text"
        :placeholder="loc('phone:serch')"
        class="input"
        v-model="search"
      >
    </div>
    <div class="general-subsection__list" v-show="search.length > 0">
      <div v-bar>
        <div class="contacts">
          <div
            class="contacts__item"
            v-for="item in filterContacts"
            :key="item.id"
            @click="openContact(item.id)"
          >{{ item.name }}</div>
        </div>
      </div>
    </div>
    <div class="general-subsection__main" v-show="!search.length">
      <div class="owner">
        <img
          src="img/hud/smartphone/contactsTab/avatar.png"
          alt="avatar"
          class="owner__icon"
        >
        <div class="owner__info">
          <span class="title">{{loc('phone:gen:2')}}</span>
          <p class="number">{{ configuration.Number }}</p>
        </div>
      </div>
      <div v-bar>
        <div class="contacts">
          <list-item
            v-for="item in items"
            :key="item.category"
            :item="item"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

import ListItem from './ListItem'

export default {
  name: 'GeneralSubsection',

  components: { ListItem },

  props: {
    setContactPopupData: Function
  },

  data: function() {
    return {
      search: ''
    }
  },

  computed: {
    ...mapState('smartphone', [
      'configuration',
      'contacts',
      'currentTab'
    ]),
    ...mapGetters('localization', ['loc']),
    items: function() {
      return this.formingItems(this.contacts);
    },

    filterContacts: function() {
      return this.contacts.filter(item => {
        return item.name.toLowerCase().includes(this.search.toLowerCase())
      })
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setOpenedContactData']),

    formingItems: function(items) {
      const letters = items.map(item => item.name.charAt(0).toUpperCase())
      const filterLetters = letters.filter((value, index) => letters.indexOf(value) === index)
      const sortFilterLetters = filterLetters.sort((a, b) => a.localeCompare(b))
      const result = sortFilterLetters.map(letter => {
        const contacts = this.contacts.filter(object => object.name.charAt(0).toUpperCase() === letter)
        return {
          category: letter,
          contacts
        }
      })

      return result
    },

    openContact: function(contactId) {
      this.setOpenedContactData(contactId);
      this.setCurrentTab({ key: this.currentTab.key, section: this.currentTab.section, subsection: 'UserSubsection' });
    }
  }
}
</script>

<style lang="scss" scoped>
.general-subsection {
  width: 100%;
  height: 100%;
  &__header {
    position: relative;
    padding: 0 0.75rem 0.5rem 0.75rem;
    &::after {
      content: '';
      position: absolute;
      width: 14.45rem;
      height: 1px;
      background: rgba(195, 195, 195, 0.35);
      bottom: 0;
      right: 0;
    }
    .title {
      font-size: 1.5rem;
      font-weight: 700;
    }
    .btn {
      position: absolute;
      top: 0;
      right: 1rem;
      background: transparent;
      width: 1rem;
      height: 1rem;
      &__icon {
        width: 100%;
        height: 100%;
      }
    }
    .input {
      background: left 50% / 12% 0.8rem no-repeat url("/img/hud/smartphone/contactsTab/search.svg"), rgba(118, 118, 128, 0.12);
      border-radius: 0.5rem;
      height: 1.8rem;
      width: 100%;
      margin: 0.5rem 0 0 0;
      padding: 0 0.5rem 0 1.65rem;
      font-size: .8rem;
    }
  }
  &__main {
    display: flex;
    flex-direction: column;
    
    .owner {
      display: flex;
      align-items: center;
      padding: 0.5rem 0.75rem;
      &__icon {
        width: 2.5rem;
        height: 2.5rem;
      }
      &__info {
        margin: 0 0 0 .75rem;
        .title {
          font-size: .7rem;
          color: #8A8A8E;
        }
        .number {
          font-size: .9rem;
        }
      }
    }
    .contacts {
      height: 17.85rem !important;
      width: 100%;
    }
  }
  .general-subsection__list {
    height: 21.4rem;
    width: 100%;
    .contacts {
      height: 21.4rem !important;
      position: relative;
      width: 100%;
      &__item {
        padding: .6rem .6rem .6rem 0;
        margin: 0 0 0 1rem;
        font-size: .8rem;
        border-bottom: 1px solid rgba(195, 195, 195, 0.35);
        &:hover {
          transform: translateY(-.1rem);
        }
        &:last-child {
          border: none;
        }
      }
    }
  }
}
</style>
