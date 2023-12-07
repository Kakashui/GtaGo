<template>
  <div class="edit-popup">
    <button class="back-button" @click="$emit('close')">
      <img
        src="img/hud/smartphone/messengerTab/chatSection/chevronLeft.svg"
        alt="chevron"
        class="back-button__icon"
      >
      <span class="back-button__text">{{loc('phone:back')}}</span>
    </button>
    <div class="title">{{loc('phone:group:edit:2')}}</div>
    <!-- <div class="img">
      <div class="img__current-gradient" v-bind:style="{ background: messengerData.CurrentChat.IconColors }"></div>
      <div :class="[{ active: isPalette }, 'btn-options']" @click="isPalette = !isPalette"></div>
    </div>
    <GradientsList 
      v-if="isPalette"
      @set-gradient="setColor"
      :styles="{ top: '7.3rem' }"
    /> -->
    <div class="colors">
        <div class="colors__current" :style="{ background: `center / 2rem no-repeat url(/img/hud/smartphone/messengerTab/createSection/secondGroupSection/users.svg), ${messengerData.CurrentChat.IconColors}` }">
          <button :class="[{ active: isPalette }, 'btn']" @click="isPalette = !isPalette"></button>
        </div>
        <gradients-list
          v-if="isPalette"
          @set-gradient="setColor"
          :styles="{ top: '3rem' }"
        />
      </div>
    <div class="info-wrap">
      <input
        type="text"
        :placeholder="loc('phone:group:edit:plh:1')"
        v-model="title"
        maxlength="25"
        class="info-wrap__input"
      >
      <textarea
        :placeholder="loc('phone:group:edit:plh:2')"
        v-model="description"
        @input="resize"
        maxlength="150"
        class="info-wrap__textarea"
        ref="textarea"
      ></textarea>
    </div>
    <div class="btn-save" @click="save">{{loc('phone:group:edit:3')}}</div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import GradientsList from '../../../GradientsList'

export default {
  name: 'EditPopup',

  components: {
    GradientsList
  },

  data: function() {
    return {
      title: '',
      description: '',
      isPalette: false,
    }
  },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
    textareaStyle: function() {
      return { 'min-height': this.textareaHeight }
    }
  },

  methods: {
    ...mapMutations('smartphone/messagePage', ['setMessengerCurrentChatIconColors']),

    save: function() {
      window.mp.triggerServer('phone::msg::updateChat', this.messengerData.CurrentChat.Id, this.title, this.description, this.messengerData.CurrentChat.IconColors);
      this.$emit('close');
    },

    setColor: function(color) {
      this.setMessengerCurrentChatIconColors(color)
      this.isPalette = false
    },

    resize: function() {
      this.$refs.textarea.style.height = 'auto'
      this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
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
.edit-popup {
  width: 100%;
  height: 30rem;
  border-top-right-radius: 0.6rem;
  border-top-left-radius: 0.6rem;
  background: #FFFFFF;
  border-radius: .6rem;
  bottom: 0;
  left: 0;
  display: flex;
  align-items: center;
  flex-flow: column;
  padding: .75rem 0 2.25rem 0;
  position: absolute;
  z-index: 2;
  .back-button {
    background: transparent;
    display: flex;
    align-items: center;
    align-self: flex-start;
    margin: 0 0 0 0.75rem;
    &:hover {
      .back-button__icon {
        animation: shake 0.4s;
      }
    }
    &__icon {
      width: 0.6rem;
      height: 1rem;
    }
    &__text{
      font-size: .7rem;
      line-height: 1rem;
      color: #007AFF;
      margin-left: .4rem;
    }
  }
  .colors {
    height: 7.15rem;
    width: 100%;
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
  .title{
    font-weight: bold;
    margin: 1.25rem 0;
    font-size: 1.3rem;
    line-height: 1.15rem;
    color: #020202;
  }
  .desc{
    font-size: .7rem;
    line-height: .9rem;
    max-width: 12.2rem;
    text-align: center;
    color: #020202;
    margin-bottom: 1.5rem;
  }
  .btn-save{
    display: flex;
    justify-content: center;
    align-items: center;
    background: #4E6ED9;
    border-radius: 5rem;
    font-size: .7rem;
    line-height: .8rem;
    height: 2rem;
    min-height: 2rem;
    width: 13.2rem;
    color: #FFFFFF;
    margin: auto 0 0 0;
    &:hover{
      box-shadow: 0 .5rem 1.5rem rgba(78, 110, 216, 0.3);
      transition: .3s;
    }
  }
  .pallette{
    display: grid;
    position: absolute;
    top: 7.65rem;
    background: #fff;
    box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.5);
    border-radius: .5rem;
    padding: .75rem;
    grid-template-columns: repeat(7, 1.4rem);
    grid-auto-rows: 1.4rem;
    grid-gap: .4rem;
    z-index: 1;
    &__color{
      border-radius: 50%;
    }
  }
  .info-wrap {
    background: rgba(195, 195, 195, 0.1);
    width: 100%;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0 0 0 1rem;
    display: flex;
    flex-direction: column;
    &__input {
      background: transparent;
      width: 100%;
      height: 2.35rem;
      font-size: 0.7rem;
      color: rgba(2, 2, 2, 0.6);
      border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      &::placeholder {
        color: rgba(2, 2, 2, 0.4);
      }
    }
    &__textarea {
      resize: none;
      width: 13.7rem;
      background: transparent;
      border: none;
      outline: none;
      font-size: 0.7rem;
      line-height: 1rem;
      overflow: hidden;
      max-height: 6.5rem;
      font-family: "Roboto";
      color: rgba(2, 2, 2, 0.7);
      padding: 1rem 0.5rem 1rem 0;
      &::placeholder {
        color: rgba(2, 2, 2, 0.4);
      }
    }
  }
}
</style>
