<template>
  <transition name="smartphone-bottom-to-top-slide">
    <div class="popup">
      <div class="popup__header">
        <button
          class="btn-close"
          @click="$emit('close')"
        >{{loc('phone:cancel')}}</button>
        <span class="title">{{loc('phone:advert:7')}}</span>
        <p class="desc">{{loc('phone:advert:8')}}</p>
      </div>
      <div class="popup__fields">
        <input
          type="text"
          :placeholder="loc('phone:advert:plh:1')"
          v-model="titleModel"
          maxlength="100"
          class="input"
        >
        <textarea
          :placeholder="loc('phone:advert:plh:2')"
          v-model="descriptionModel"
          @input="resize"
          maxlength="150"
          class="textarea"
          ref="textarea"
        ></textarea>
      </div>
      <div class="popup__info">
        <span class="desc">{{loc('phone:advert:9')}}</span>
        <span class="value">$ {{getPriceAdvert}}</span>
      </div>
      <button class="popup__btn-submit" @click="createAdvert">{{loc('phone:advert:10')}}</button>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'PopupCreateAd',

  data: function() {
    return {
      titleModel: '',
      descriptionModel: ''
    }
  },
  computed: {
    ...mapState('smartphone/newsPage', ['priceSymbol', 'priceImage']),
    ...mapGetters('localization', ['loc']),
    getPriceAdvert() {
      return this.priceSymbol * this.descriptionModel.length + ((this.titleModel && this.titleModel.length > 0) ? this.priceImage : 0);
    }
  },


  methods: {
    resize: function() {
      this.$refs.textarea.style.height = 'auto'
      this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
    },
    createAdvert: function() {
      if (!this.descriptionModel || this.descriptionModel.length == 0)
        return;
        window.mp.triggerServer('phone::news::createAdvert', this.descriptionModel, this.titleModel)
        this.titleModel='';
        this.descriptionModel='';
        this.$emit('close')
    }
  },

  mounted: function() {
    this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
  }
}
</script>

<style lang="scss" scoped>
.popup {
  position: absolute;
  bottom: 0;
  left: 0;
  background: 100% 0 / 3.35rem 4rem no-repeat url("/img/hud/smartphone/newsTab/main/megaphone.png"), #FAFAFA;
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 24rem;
  border-radius: 0.6rem 0.6rem 0 0;
  z-index: 5;
  &__header {
    padding: 1rem;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    .btn-close {
      background: transparent;
      font-size: 0.7rem;
      line-height: 0.8rem;
      color: #EE443A;
      margin: 0 0 0.7rem 0;
      &:hover {
        text-decoration: underline;
      }
    }
    .title {
      font-size: 1.3rem;
      line-height: 1.5rem;
      font-weight: bold;
      color: #020202;
      margin: 0 0 0.25rem 0;
    }
    .desc {
      font-size: 0.7rem;
      line-height: 0.9rem;
      color: rgba(2, 2, 2, 0.4);
    }
  }
  &__fields {
    background: rgba(195, 195, 195, 0.1);
    width: 100%;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0 0 0 1rem;
    display: flex;
    flex-direction: column;
    margin: 0 0 auto 0;
    .input {
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
    .textarea {
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
      &::placeholder {
        color: rgba(2, 2, 2, 0.4);
      }
    }
  }
  &__info {
    display: flex;
    align-items: center;
    margin: 0 0 0.8rem 0.8rem;
    .desc {
      width: 4.5rem;
      font-size: 0.65rem;
      line-height: 0.75rem;
      color: #020202;
    }
    .value {
      font-size: 1.3rem;
      line-height: 1.5rem;
      color: #EE443A;
      text-shadow: 0px 0.45rem 1.5rem rgba(238, 68, 58, 0.4);
      font-weight: 500;
    }
  }
  &__btn-submit {
    align-self: center;
    width: 13.2rem;
    height: 2.5rem;
    color: #fff;
    background: #EE443A;
    border: 0.15rem solid rgba(255, 255, 255, 0.3);
    box-shadow: 0px 0.35rem 1rem rgba(238, 68, 58, 0.4);
    border-radius: 5rem;
    transition: all 0.15s;
    margin: 0 0 0.7rem 0;
    font-size: 0.7rem;
    &:hover {
      box-shadow: none;
    }
  }
}
</style>
