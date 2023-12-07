<template>
  <div class="avatar-option">
    <div class="avatar-option__modal">
      <div 
        class="close-modal"
        @click="$emit('setAvatarOption', false)"
      >
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 40 40" fill="none">
          <path d="M28.75 11.25L11.25 28.75M28.75 28.75L11.25 11.25L28.75 28.75Z" stroke="white" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
      </div>
      <div class="avatar-option__img-wrap">
        <div 
          v-if="currentPlayer.avatar"
          class="avatar-option__img-wrap__value"
          :style="{backgroundImage: `url(${currentPlayer.avatar})`}"
        ></div>
        <div 
          v-else
          class="avatar-option__img-wrap__value"
          :style="{backgroundImage: `url(/img/poker/avatar.png)`}"
        ></div>
      </div>
      <div class="avatar-option__info">
        <div class="avatar-option__title">{{loc('poker_1')}}</div>
        <div class="avatar-option__desc">{{loc('poker_2')}}</div>
        <input 
          class="avatar-option__input"
          placeholder="Поле для ссылки"
          v-model="avatarUrl"
        >
        <div 
          class="avatar-option__btn"
          @click="addAvatar"
        >{{loc('poker_3')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'AvatarOption',
  data: function() {
    return {
      avatarUrl: "",
    }
  },
  computed: {
    ...mapGetters('localization',['loc']),
    ...mapState('poker', ['currentPlayer'])
  },
  methods: {
    addAvatar: function() {
      window.mp.triggerServer('player::changeimage', this.avatarUrl)
      this.$emit('setAvatarOption', false)
    }
  }
}
</script>

<style lang="scss" scoped>
.avatar-option{
  position: fixed;
  width: 100%;
  height: 100vh;
  left: 0;
  top: 0;
  background-color: rgba(34, 34, 34, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3;
  &__modal{
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    width: 35rem;
    height: 14.55rem;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: left top;
    background-image: url('/img/poker/modal-bg.png');
    position: relative;
    padding: 2rem;
    .close-modal{
      width: 2rem;
      height: 2rem;
      position: absolute;
      top: 0;
      right: -.5rem;
      transform: translateX(100%);
      svg{
        width: 100%;
        height: 100%;
      }
    }
  }
  &__img-wrap{
    width: 9.5rem;
    height: 9.5rem;
    border: .2rem solid rgba(255, 255, 255, 0.2);
    border-radius: 50%;
    margin-right: 2.2rem;
    overflow: hidden;
    &__value{
      background-size: cover;
      background-repeat: no-repeat;
      background-position: center;
      width: 100%;
      height: 100%;
    }
  }
  &__info{
    display: flex;
    flex-direction: column;
  }
  &__title{
    font-weight: bold;
    font-size: 1.6rem;
    line-height: 1.85rem;
    letter-spacing: 0.04em;
    text-transform: uppercase;
    color: #FFFFFF;
    margin-bottom: .8rem;
  }
  &__desc{
    font-weight: bold;
    font-size: .8rem;
    line-height: .9rem;
    color: #FFFFFF;
    opacity: 0.7;
    margin-bottom: .6rem;
  }
  &__input{
    position: relative;
    background: rgba(255, 255, 255, 0.1);
    border-radius: .1rem;
    width: 18.5rem;
    height: 2.8rem;
    margin-bottom: .8rem;
    font-weight: bold;
    font-size: .8rem;
    color: rgba(255, 255, 255, 0.3);
    padding: 0 1rem;
  }
  &__btn{
    height: 3.2rem;
    width: fit-content;
    padding: 0 1.6rem;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: .8rem;
    text-align: center;
    letter-spacing: 0.1em;
    text-transform: uppercase;
    color: #FFFFFF;
    background: #B5D60D;
    transition: .3s;
    border-radius: .1rem;
    &:hover{
      box-shadow: 0 .5rem 1.5rem rgba(182, 209, 42, 0.5);
    }

  }
  &__balance{
    position: absolute;
    left: 2rem;
    top: -1rem;
    transform: translateY(-100%);
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    font-weight: bold;
    color: #FFFFFF;
    &-desc{
      margin-bottom: .8rem;
      font-size: .8rem;
      letter-spacing: 0.35em;
      text-transform: uppercase;
    }
    &-value{
      font-size: 2.4rem;
      letter-spacing: 0.05em;
      display: flex;
      align-items: center;
      &:after{
        content: '';
        width: 1.8rem;
        min-width: 1.8rem;
        height: 1.8rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/poker/chip_black.png');
        margin-left: .4rem;
      }
    }
  }
}
</style>