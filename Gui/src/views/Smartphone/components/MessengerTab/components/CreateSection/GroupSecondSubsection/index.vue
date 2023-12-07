<template>
  <div class="second-subsection">
    <div class="second-subsection__header">
      <button class="btn-back" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: 'GroupFirstSubsection' })">
        <img src="img/hud/smartphone/messengerTab/chatSection/chevronLeftDark.svg" class="btn-back__icon">
        <span class="btn-back__text">{{loc('phone:back')}}</span>
      </button>
      <div class="title">{{loc('phone:group:second:1')}}</div>
    </div>
    <div class="second-subsection__options">
      <div class="colors">
        <div class="colors__current" :style="{ background: `center / 2rem no-repeat url(/img/hud/smartphone/messengerTab/createSection/secondGroupSection/users.svg), ${currentColor}` }">
          <button :class="[{ active: isPalette }, 'btn']" @click="isPalette = !isPalette"></button>
        </div>
        <gradients-list
          v-if="isPalette"
          @set-gradient="setCurrentColor"
          :styles="{ top: '3rem' }"
        />
      </div>
      <div class="actions">
        <input
          type="text"
          class="actions__input"
          :placeholder="loc('phone:group:second:plh:1')"
          v-model="name"
        >
        <textarea
          class="actions__textarea"
          :placeholder="loc('phone:group:second:plh:2')"
          v-model="description"
          @input="resize"
          ref="textarea"
        ></textarea>
      </div>
    </div>
    <div v-bar v-if="currentTab.data.users.length">
      <div class="second-subsection__list">
        <list-item
          v-for="(item, index) in currentTab.data.users"
          :key="index"
          :item="item"
        />
      </div>
    </div>
    <button class="second-subsection__btn" @click="create">{{loc('phone:group:second:2')}}</button>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import GradientsList from '../../GradientsList'
import ListItem from './ListItem'

export default {
  name: 'GroupSecondSubsection',

  components: {
    GradientsList,
    ListItem
  },

  data: function() {
    return {
      currentColor: 'linear-gradient(90deg, #43E97B 0%, #38F9D7 100%)',
      isPalette: false,
      description: '',
      name: '',
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    setCurrentColor: function(color) {
      this.currentColor = color
      this.isPalette = false
    },

    resize: function() {
      this.$refs.textarea.style.height = 'auto'
      this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
    },

    create: function() {
      if (!this.name || !this.description) return;
      const usersToInvite = this.currentTab.data.users;
      const usersIds = usersToInvite.map(u => u.AccountId);
      
      this.$store.dispatch('smartphone/messagePage/msg_createChat', {
          ChatType: 1,
          Name: this.name,
          Description: this.description,
          IconColors: this.currentColor,
          UsersToInvite: usersIds
        });
    }
  },

  mounted: function() {
    this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
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
.second-subsection {
  padding: 2.5rem 0 0 0;
  display: flex;
  flex-direction: column;
  background: #F3F6FA;
  width: 100%;
  height: 100%;
  position: relative;
  &__header {
    padding: 0 1rem;
    margin: 0 0 0.5rem 0;
    display: flex;
    align-items: center;
    position: relative;
    .btn-back {
      display: flex;
      align-items: center;
      background: transparent;
      &__icon {
        width: 0.6rem;
        height: auto;
        margin: 0 0.4rem 0 0;
      }
      &__text {
        color: #4E6ED9;
        font-size: 0.7rem;
      }
      &:hover .btn-back__icon {
        animation: shake 0.4s;
      }
    }
    .title {
      position: absolute;
      transform: translate(-50%, 0);
      left: 50%;
      font-size: 0.7rem;
      font-weight: 500;
      color: #020202;
    }
  }
  &__options {
    display: flex;
    flex-direction: column;
    background: #fff;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    margin: 0 0 0.75rem 0;
    .colors {
      height: 7.15rem;
      display: flex;
      align-items: center;
      justify-content: center;
      background: center / contain no-repeat url('/img/hud/smartphone/messengerTab/createSection/secondGroupSection/colorsBg.png');
      position: relative;
      &__current {
        width: 4.7rem;
        height: 4.7rem;
        border-radius: 50%;
        position: relative;
        .btn {
          position: absolute;
          top: 0;
          right: 0;
          width: 1.4rem;
          height: 1.4rem;
          border-radius: 50%;
          background: #FFFFFF;
          box-shadow: 0px 0.5rem 1.5rem rgba(0, 0, 0, 0.3);
          background-image: url('/img/hud/smartphone/messengerTab/options.svg');
          transition: all 0.4s;
          &.active {
            transform: rotate(90deg);
          }
          &:hover {
            box-shadow: none;
          }
        }
      }
    }
    .actions {
      width: 100%;
      border-top: 1px solid rgba(195, 195, 195, 0.35);
      padding: 0 0 0 1rem;
      display: flex;
      flex-direction: column;
      &__input {
        background: transparent;
        width: 100%;
        height: 2.35rem;
        font-size: 0.7rem;
        color: #020202;
        border-bottom: 1px solid rgba(195, 195, 195, 0.35);
        &::placeholder {
          color: rgba(2, 2, 2, 0.4);
        }
      }
      &__textarea {
        resize: none;
        background: transparent;
        border: none;
        outline: none;
        font-family: "Roboto";
        width: 13.7rem;
        font-size: 0.7rem;
        line-height: 1rem;
        overflow-y: auto;
        max-height: 6.5rem;
        color: #020202;
        padding: 1rem 0.5rem 1rem 0;
        &::placeholder {
          color: rgba(2, 2, 2, 0.4);
        }
        &::-webkit-scrollbar-track {
          background-color: transparent;
          margin: 1rem 0;
        }
        &::-webkit-scrollbar {
          width: 0.1rem;
          background-color: transparent;
        }
        &::-webkit-scrollbar-thumb {
          background: rgba(0, 0, 0, 0.3);
          border-radius: 0.1rem;
        }
      }
    }
  }
  &__list {
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    background: #fff;
    padding: 0 0 0 0.75rem;
    max-height: 15.55rem !important;
    min-height: fit-content !important;
  }
  &__btn {
    position: absolute;
    transform: translate(-50%, 0);
    bottom: 1.5rem;
    left: 50%;
    width: 13.2rem;
    height: 2rem;
    border-radius: 5rem;
    background: #4E6ED9;
    transition: all 0.2s;
    font-size: 0.7rem;
    color: #FFFFFF;
    box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
    &:hover {
      box-shadow: none;
    }
  }
}
</style>
