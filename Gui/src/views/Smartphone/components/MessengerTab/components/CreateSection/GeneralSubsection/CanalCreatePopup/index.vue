<template>
  <div class="create-popup">
    <div class="create-popup__header">
      <button class="back-button" @click="back">
        <img
          src="img/hud/smartphone/messengerTab/chatSection/chevronLeft.svg"
          alt="chevron"
          class="back-button__icon"
        >
        <span class="back-button__text">{{loc('phone:back')}}</span>
      </button>
    </div>
    <div class="create-popup__main">
      <transition name="three-d-unfold">
        <component
          :is="currentTab"
          :title="title"
          :description="description"
          @setTitle="(newTitle) => { title = newTitle }"
          @setDescription="(newDescription) => { description = newDescription }"
        />
      </transition>
    </div>
    <button class="create-popup__btn" @click="createCanal">{{loc('phone:channel:3')}}</button>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import InfoTab from './InfoTab'
import ActionTab from './ActionTab'

export default {
  name: 'CreatePopup',

  components: {
    ActionTab,
    InfoTab
  },

  data: function() {
    return {
      currentTab: 'InfoTab',
      title: '',
      description: '',
    }
  },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    createCanal: function() {
      if (this.currentTab === 'InfoTab') {
        this.currentTab = 'ActionTab'
      } else {
        this.$store.dispatch('smartphone/messagePage/msg_createChat', {
          ChatType: 2,
          Name: this.title,
          Description: this.description,
          IconColors: this.messengerData.CurrentChat.IconColors
        });
      }
    },

    back: function() {
      if (this.currentTab === 'ActionTab') {
        this.currentTab = 'InfoTab'
      } else {
        this.$emit('close')
      }
    }
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
.create-popup {
  background: #fff;
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 30rem;
  border-top-right-radius: 0.6rem;
  border-top-left-radius: 0.6rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 0.75rem 0 2.25rem 0;
  &__header {
    padding: 0 0.75rem;
    align-self: flex-start;
    .back-button {
      background: transparent;
      display: flex;
      align-items: center;
      color: #4E6ED9;
      margin: 0 0 1.5rem 0;
      &:hover {
        .back-button__icon {
          animation: shake 0.4s;
        }
      }
      &__icon {
        margin: 0 0.4rem 0 0;
      }
      &__text {
        font-size: 0.7rem;
      }
    }
  }
  &__main {
    margin: 0 0 auto 0;
    perspective: 100rem;
    position: relative;
    width: 100%;
  }
  &__btn {
    height: 2rem;
    width: 13.2rem;
    background: #4E6ED9;
    border-radius: 5rem;
    transition: all 0.15s;
    font-size: 0.7rem;
    color: #fff;
    &:hover {
      box-shadow: 0px 10px 30px rgba(78, 110, 216, 0.3);
    }
  }
}
.three-d-unfold-enter, .three-d-unfold-leave {
  opacity: 0;
  transform: rotateY(-180deg) scale(0.8);
}
.three-d-unfold-enter-active, .three-d-unfold-leave-active {
  transition: all 0.4s ease-in-out;
  transform-style: preserve-3d;
  position: absolute;
  opacity: 0;
}
.three-d-unfold-enter-to {
  transform: rotateY(0deg);
  opacity: 1;
}
.three-d-unfold-leave-to {
  transform: rotateY(180deg);
}
</style>
