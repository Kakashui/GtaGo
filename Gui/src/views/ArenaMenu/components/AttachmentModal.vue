<template>
  <div class="attachment-modal">
    <div class="attachment-modal__box">
      <label class="box__input">
        <input
          v-focus
          v-model="roomId"
          type="text"
          maxlength="12"
        >
      </label>
      <div class="box__notify">
        <span v-if="!isAttachmentResult" class="notify__text">ID {{ room.id }} not found</span>
        <span v-else class="notify__text">{{ loc('arena_dm_17') }}</span>
      </div>
      <div class="box__actions">
        <button class="actions__btn" @click="searchLobby">
          <span>{{ loc('arena_dm_18') }}</span>
        </button>
        <button class="actions__btn" @click="setIsAttachment(false)">
          <span>{{ loc('arena_dm_19') }}</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex'

export default {
  name: 'AttachmentModal',

  data: function () {
    return {
      roomId: ''
    }
  },

  computed: {
    ...mapState('arenaMenu', ['isAttachmentResult']),

    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('arenaMenu', ['setIsAttachment']),

    searchLobby: function () {
      // CALL EVENT
      window.mp.trigger('ARENA::JOIN::LOBBY::BY::ID::VUE', this.roomId)
      this.setIsAttachment(false)
      //console.log(this.roomId)
    }
  },
  
  directives: {
    focus: {
      inserted: function (el) {
        el.focus()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.attachment-modal {
  position: fixed;
  width: 100vw;
  height: 100vh;
  text-transform: uppercase;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba($color: #000000, $alpha: .6);
  z-index: 9;
  display: flex;
  align-items: center;
  justify-content: center;
  animation: fade .4s;
  @keyframes fade {
    0% {
      opacity: 0;
    }
    100% {
      opacity: 1;
    }
  }
  &__box {
    display: flex;
    flex-direction: column;
    align-items: center;
    .box__input {
      border-radius: .5rem;
      background: #fff;
      width: 19rem;
      height: 3rem;
      border: 1px solid #B6D300;
      cursor: text;
      & input {
        width: 100%;
        height: 100%;
        background: transparent;
        outline: none;
        border: none;
        text-align: center;
      }
    }
    .box__notify {
      height: 3rem;
      color: #fff;
      display: flex;
      font-size: 1.1rem;
      align-items: center;
      justify-content: center;
    }
    .box__actions {
      width: 100%;
      display: flex;
      justify-content: space-between;
      .actions__btn {
        width: 8.5rem;
        height: 2.9rem;
        position: relative;

        text-transform: uppercase;
        padding: 1.4rem;
        //margin: .5rem 0 0 0;
        line-height: .1rem;
        border-radius: .2rem;
        color: #fff;
        //transition: all .2s;
        background: #B6D300;
        box-shadow: 0px 10px 50px rgba(168, 195, 2, 0.6);
        &:hover {
          box-shadow: 0 0 0 0;
        }
        & span {
          display: inline-block;
          text-transform: uppercase;
          font-size: .8rem;
          font-weight: bold;
          letter-spacing: .05rem;
        }
      }
    }
  }
}
</style>
