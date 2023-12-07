<template>
  <div class="item">
    <img
      src="img/hud/smartphone/contactsTab/phone.svg"
      alt="phone"
      class="item__icon"
      v-if="item.type === 'outgoing'"
    >
    <div class="item__content">
      <div class="left">
        <span :class="[{ failed: item.type === 'income' && !item.isSuccessful }, 'left__name']">{{ getContactName(item.number) }}</span>
        <p class="left__text">{{loc('phone:recent:list')}}</p>
      </div>
      <div class="right">
        <span class="right__date">{{ getCorrectDate(item.date) }}</span>
        <button class="right__btn" @click="openUser(item.number)">
          <img
            src="img/hud/smartphone/info.svg"
            alt="info"
            class="icon"
          >
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'ListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('smartphone', ['currentTab', 'contacts']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setOpenedContactByNumber']),

    openUser: function(number) {
      this.setOpenedContactByNumber(number);
      this.setCurrentTab({ key: this.currentTab.key, section: this.currentTab.section, subsection: 'UserSubsection' });
    },

    getContactName: function(number) {
      const contact = this.contacts.find(c => c.number === number);
      return contact ? contact.name : number;
    },

    getCorrectDate: function(dateInSeconds) {
      const date = new Date(dateInSeconds);
      const currentDate = new Date();

      if (date.getUTCFullYear() === currentDate.getUTCFullYear() && date.getUTCMonth() == currentDate.getUTCMonth() && date.getUTCDate() == currentDate.getUTCDate())
      {
        return `${this.getDoubleDigit(date.getUTCHours())}:${this.getDoubleDigit(date.getUTCMinutes())}`
      }
      else if (date.getUTCFullYear() == currentDate.getUTCFullYear()) {
        return `${this.getDoubleDigit(date.getUTCDate())}.${this.getDoubleDigit(date.getUTCMonth() + 1)}`;
      }
      else {
        return `${this.getDoubleDigit(date.getUTCDate())}.${this.getDoubleDigit(date.getUTCMonth() + 1)}.${date.getUTCFullYear()}`;
      }
    },

    getDoubleDigit: function(number) {
      return (number < 10) ? `0${number}` : number;
    }
  }
}
</script>

<style lang="scss" scoped>
.item {
  padding: 0 0 0 2rem;
  position: relative;
  &__icon {
    position: absolute;
    transform: translate(-50%, -50%);
    top: 50%;
    left: 1rem;
    width: 0.7rem;
    height: 0.7rem;
  }
  &__content {
    display: flex;
    justify-content: space-between;
    padding: .5rem .75rem .5rem 0;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    .left {
      &__name {
        font-size: .8rem;
        white-space: nowrap;
        overflow: hidden;
        position: relative;
        display: inline-block;
        width: 8.3rem;
        text-overflow: ellipsis;
        &.failed {
          color: #FF3B30;
        }
      }
      &__text {
        font-size: .6rem;
        font-weight: 300;
        color: #8A8A8E;
      }
    }
    .right {
      display: flex;
      align-items: center;
      &__date {
        font-size: .7rem;
        color: #8A8A8E;
      }
      &__btn {
        background: transparent;
        width: 1.25rem;
        height: 1.25rem;
        margin: 0 0 0 0.5rem;
        &:hover {
          transform: scale(1.05);
        }
        .icon {
          width: 100%;
          height: 100%;
        }
      }
    }
  }
}
</style>
