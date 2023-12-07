<template>
  <div class="action-tab">
    <div class="title">{{loc('phone:channel:3')}}</div>
    <div class="img">
      <div class="img__current-gradient" v-bind:style="{ background: messengerData.CurrentChat.IconColors }"></div>
      <div :class="[{ active: isPalette }, 'btn-options']" @click="isPalette = !isPalette"></div>
    </div>
    <gradients-list
      v-if="isPalette"
      @set-gradient="setCurrentColor"
      :styles="gradientsListStyles"
    />
    <div class="info-wrap">
      <input
        type="text"
        :placeholder="loc('phone:channel:plh:1')"
        v-model="localTitleModel"
        maxlength="25"
        class="info-wrap__input"
      >
      <textarea
        :placeholder="loc('phone:channel:plh:2')"
        v-model="localDescriptionModel"
        @input="resize"
        maxlength="150"
        class="info-wrap__textarea"
        ref="textarea"
      ></textarea>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import GradientsList from '../../../GradientsList'

export default {
  name: 'ActionTab',

  components: { GradientsList },

  props: {
    description: String,
    title: String
  },

  data: function() {
    return {
      isPalette: false,
      gradientsListStyles: { top: '5.65rem' },
      descriptionLocal: this.description,
      titleLocal: this.title
    }
  },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),

    textareaStyle: function() {
      return { 'min-height': this.textareaHeight }
    },

    localTitleModel: {
      get: function() {
        return this.title
      },
      set: function(value) {
        this.$emit('setTitle', value)
      }
    },

    localDescriptionModel: {
      get: function() {
        return this.description
      },
      set: function(value) {
        this.$emit('setDescription', value)
      }
    }
  },

  methods: {
    ...mapMutations('smartphone/messagePage', ['setMessengerCurrentChatIconColors']),

    setCurrentColor: function(color) {
      this.setMessengerCurrentChatIconColors(color);
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
.action-tab {
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  width: 100%;
  height: fit-content;
  .img{
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-image: url('/img/hud/smartphone/messengerTab/creating-channel-img.png');
    width: 12.3rem;
    height: 4.7rem;
    margin-bottom: 1.5rem; 
    animation: opacity-anim .3s linear;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    &__current-gradient{
      width: 4.7rem;
      height: 4.7rem;
      position: absolute;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      &:after{
        content: '';
        position: absolute;
        width: 1.8rem;
        height: 1.8rem;
        background-image: url('/img/hud/smartphone/messengerTab/channel-gradient-icon.svg');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
      }
    }
    .btn-options{
      position: absolute;
      right: 3.9rem;
      top: 0;
      width: 1.4rem;
      height: 1.4rem;
      border-radius: 50%;
      background-color: #fff;
      background-image: url('/img/hud/smartphone/messengerTab/options.svg');
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      box-shadow: 0 .5rem 1.5rem rgba(0, 0, 0, 0.1);
      transition: all 0.4s;
      &:hover{
        box-shadow: 0 .5rem 1.5rem rgba(0, 0, 0, 0.3);
      }
      &.active {
        transform: rotate(90deg);
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
    margin: 1rem 0 0 0;
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
      color: rgba(2, 2, 2, 0.7);
      padding: 1rem 0.5rem 1rem 0;
      font-family: "Roboto";
      &::placeholder {
        color: rgba(2, 2, 2, 0.4);
      }
    }
  }
}
</style>
