<template>
  <div class="user" v-if="contactsUserData">
    <div class="user__header">
      <div class="actions">
        <button class="btn btn-back" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: 'GeneralSubsection' })">
          <img
            src="img/hud/smartphone/arrowLeft.svg"
            alt="arrow"
            class="btn-back__icon"
          >
          <span class="btn-back__text">{{loc('phone:back')}}</span>
        </button>
        <button
          class="btn btn-change"
          v-if="contactsUserData.isKnown"
          @click="setContactPopupData(true, 'edit', contactsUserData.number, contactsUserData.name, contactsUserData.id)"
        >{{loc('phone:cont:sub:2')}}</button>
      </div>
      <div class="general">
        <div class="avatar">
          <img
            src="img/hud/smartphone/contactsTab/avatar.png" 
            alt="avatar"
            class="avatar__img"
          >
          <div class="avatar__icon" v-if="contactsUserData.isBlocked">
            <img
              src="img/hud/smartphone/cancel.svg"
              alt="cancel"
              class="img"
            >
          </div>
        </div>
        <div class="title">
          <span class="title__name" v-if="contactsUserData.isKnown">{{ contactsUserData.name }}</span>
          <div class="title__info" v-else>
            <span class="number">{{ contactsUserData.number }}</span>
            <p class="desc">{{loc('phone:cont:sub:3')}}</p>
          </div>
        </div>
        <div class="general__actions">
          <button
            class="actions__btn"
            v-for="item in buttons"
            :key="item.id"
            @click="item.event"
          >
            <div class="icon" :style="{ '-webkit-mask': 'url(' + `img/hud/smartphone/contactsTab/${item.icon}.svg` + ') no-repeat center / contain' }"></div>
            <span class="text">{{ loc(item.text) }}</span>
          </button>
        </div>
      </div>
    </div>
    <div v-bar>
      <div class="user__main">
        <div class="number" v-if="contactsUserData.isKnown">
          <p class="number__title">{{loc('phone:cont:sub:4')}}</p>
          <p class="number__value">{{ contactsUserData.number }}</p>
        </div>
        <div class="sub-actions" v-if="contactsUserData.isKnown">
          <div class="sub-actions__btn red" @click="blockContact" v-if="!contactsUserData.isBlocked">{{loc('phone:cont:sub:5')}}</div>
          <div class="sub-actions__btn red" @click="unblockContact" v-else>{{loc('phone:cont:sub:6')}}</div>
          <div class="sub-actions__separator"></div>
          <div class="sub-actions__btn red" @click="removeContact(contactsUserData.id)">{{loc('phone:cont:sub:7')}}</div>
        </div>
        <div class="sub-actions" v-else>
          <div class="sub-actions__btn blue" @click="setContactPopupData(true, 'add', contactsUserData.number)">{{loc('phone:cont:sub:8')}}</div>
          <div class="sub-actions__separator"></div>
          <div class="sub-actions__btn red" @click="blockContact" v-if="!contactsUserData.isBlocked">{{loc('phone:cont:sub:5')}}</div>
          <div class="sub-actions__btn red" @click="unblockContact" v-else>{{loc('phone:cont:sub:6')}}</div>
        </div>
        <div class="history">
          <span class="history__title">{{loc('phone:cont:sub:9')}}</span>
          <div
            class="history__item"
            v-for="item in contactsUserData.callHistory"
            :key="item.id"
          >
          <span class="date">{{ getCorrectDate(item.date) }}</span>
          <div class="info">
            <p class="info__type" v-if="item.type === 'outgoing'">{{loc('phone:cont:sub:10')}}</p>
            <p class="info__type" v-else-if="item.type === 'income'">{{loc('phone:cont:sub:11')}}</p>
            <p class="info__duration">{{ item.duration }} s</p>
          </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'UserSubsection',

  props: {
    setContactPopupData: Function
  },

  data: function() {
    return {
      buttons: [
        { id: 0, text: 'phone:cont:sub:12', icon: 'phone1', event: this.call },
        { id: 1, text: 'phone:cont:sub:13', icon: 'message', event: this.message }
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['contactsUserData', 'currentTab']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    call: function() {
      window.setData('smartphone/setCallData', { destination: this.contactsUserData.number, status: 'outcome' })
      window.setData('smartphone/setCurrentTab', { key: 'CallTab', data: {theme: 'dark'} })
    },

    message: function() {
      window.mp.triggerServer('phone::contacts::goToMsg', this.contactsUserData.number);
    },

    removeContact: function(contactId) {
      window.mp.triggerServer('phone::contacts::remove', contactId);
    },

    blockContact: function() {
      window.mp.triggerServer('phone::contacts::block', this.contactsUserData.number);
    },

    unblockContact: function() {
      window.mp.triggerServer('phone::contacts::unblock', this.contactsUserData.number);
    },

    getCorrectDate: function(dateInSeconds) {
      const date = new Date(dateInSeconds);
      return `${this.getDoubleDigit(date.getUTCDate())}.${this.getDoubleDigit(date.getUTCMonth() + 1)}.${date.getUTCFullYear()}`;
    },

    getDoubleDigit: function(number) {
      return (number < 10) ? `0${number}` : number;
    }
  },

  mounted: function() {
    // window.setData('smartphone/setContactsUserData', {
    //   isKnown: false,
    //   name: null,
    //   isBlocked: true,
    //   number: 879436,
    //   callHistory: [
    //     { id: 0, type: 'income', date: '11.09.2020', duration: 160 },
    //     { id: 1, type: 'outgoing', date: '21.09.2020', duration: 160 },
    //     { id: 2, type: 'outgoing', date: '21.09.2020', duration: 160 },
    //   ]
    // })
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateX(0.5rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.25rem);
  }
}
.user::v-deep .vb > .vb-dragger > .vb-dragger-styler {
  height: calc(100% - 0.75rem);
}
.user::v-deep .vb > .vb-dragger {
  margin: 0 0.125rem 0 0 !important;
}
.user {
  height: 100%;
  width: 100%;
  &__header {
    padding: 0 0.5rem;
    .actions {
      display: flex;
      justify-content: space-between;
      padding: 0 0.25rem;
      .btn {
        font-size: 0.7rem;
        color: #007AFF;
        background: transparent;
        &.btn-back {
          display: flex;
          align-items: center;
          &:hover {
            .btn-back__icon {
              animation: shake 0.4s;
            }
          }
          .btn-back__icon {
            width: .6rem;
            height: 1rem;
          }
          .btn-back__text {
            margin: 0 0 0 .4rem;
          }
        }
        &.btn-change {
          &:hover {
            text-decoration: underline;
          }
        }
      }
    }
    .general {
      display: flex;
      flex-direction: column;
      align-items: center;
      .avatar {
        width: 4rem;
        height: 4rem;
        margin: 0.35rem 0 0 0;
        position: relative;
        &__img {
          width: 100%;
          height: 100%;
        }
        &__icon {
          width: 1.5rem;
          height: 1.5rem;
          display: flex;
          align-items: center;
          justify-content: center;
          background: #FF3B30;
          border-radius: 50%;
          position: absolute;
          top: 20%;
          left: 90%;
          transform: translate(-50%, -50%);
          .img {
            width: 1rem;
            height: 1rem;
          }
        }
      }
      .title {
        margin: 0.5rem 0 0 0;
        text-align: center;
        font-size: 1.1rem;
        &__info {
          .desc {
            font-size: 0.8rem;
            color: #8A8A8E;
          }
        }
      }
      &__actions {
        display: flex;
        justify-content: space-between;
        width: 100%;
        margin: 1rem 0 0.75rem 0;
        .actions__btn {
          width: 6.9rem;
          height: 2.75rem;
          background: #fff;
          border-radius: 0.4rem;
          display: flex;
          align-items: center;
          justify-content: center;
          flex-direction: column;
          box-shadow: 0px 0.2rem 0.5rem #ECEAF4;
          &:hover {
            .text {
              text-decoration: underline;
            }
          }
          .icon {
            width: 1rem;
            height: 1rem;
            background: #007AFF;
          }
          .text {
            color: #007AFF;
            font-size: 0.55rem;
            margin: 0.25rem 0 0 0;
          }
        }
      }
    }
  }
  &__main {
    padding: 0 0.5rem 0.75rem 0.5rem;
    height: 14.3rem !important;
    .number {
      box-shadow: 0px 0.2rem 0.5rem #ECEAF4;
      background: #fff;
      width: 100%;
      padding: 0.6rem 1rem;
      border-radius: 0.4rem;
      &__title {
        font-size: 0.7rem;
      }
      &__value {
        color: #007AFF;
        font-size: 0.7rem;
        letter-spacing: 0.05rem;
        margin: 0.25rem 0 0 0;
      }
    }
    .sub-actions {
      width: 100%;
      box-shadow: 0px 0.2rem 0.5rem #ECEAF4;
      background: #fff;
      margin: 0.75rem 0 0 0;
      padding: 0.6rem 0 0.6rem 1rem;
      border-radius: 0.4rem;
      &__separator {
        margin: 0.55rem 0;
        width: 100%;
        height: 1px;
        background: rgba(195, 195, 195, 0.35);
      }
      &__btn {
        font-size: 0.7rem;
        &:hover {
          text-decoration: underline;
        }
        &.blue {
          color: #007AFF;
        }
        &.red {
          color: #FF3B30;
        }
      }
    }
  }
  .history {
    background: #fff;
    border-radius: 0.4rem;
    box-shadow: 0px 0.2rem 0.5rem #ECEAF4;
    padding: 1rem;
    width: 100%;
    margin: 0.75rem 0 0 0;
    overflow: hidden;
    &__title {
      font-size: 0.7rem;
    }
    &__item {
      display: flex;
      font-size: 0.6rem;
      margin: 0.6rem 0 0 0;
      .date {
        font-weight: 300;
        width: 26%;
        text-align: right;
      }
      .info {
        margin: 0 0 0 0.35rem;
        &__duration {
          color: #8A8A8E;
          font-weight: 300;
        }
      }
    }
  }
}
</style>
