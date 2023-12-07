<template>
  <div class="popup">
    <div class="popup__layer"></div>
    <div class="popup__block">
      <button class="close" @click="close"></button>
      <img
        src="img/hud/smartphone/contactsTab/popupImg.png"
        alt="promo"
        class="image"
      >
      <div class="form">
        <div class="form__fields">
          <label class="label">
            <span class="label__prepend">{{loc('phone:cont:pop:1')}}</span>
            <input
              type="text"
              class="label__input"
              v-model.trim="name"
              maxlength="16"
            >
          </label>
          <div class="separator"></div>
          <label class="label">
            <span class="label__prepend">{{loc('phone:cont:pop:2')}}</span>
            <input
              type="text"
              class="label__input"
              v-model.trim="number"
              maxlength="10"
            >
          </label>
        </div>
        <button 
          v-if="data.type === 'add'"
          class="form__btn"
          @click="add"
        >
          <img
            src="img/hud/smartphone/plus1.svg"
            alt="plus"
            class="icon"
          >
          <span class="text">{{loc('phone:cont:pop:3')}}</span>
        </button>
        <button
          v-else-if="data.type === 'edit'"
          class="form__btn"
          @click="edit"
        >
          {{loc('phone:cont:pop:4')}}
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'AddContactTab',

  props: {
    setContactPopupData: Function,
    data: Object
  },

  data: function() {
    return {
      id: 0,
      number: '',
      name: ''
    }
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    setNumber: function(value) {
      this.number = value
    },
    setName: function(value) {
      this.name = value
    },
    setId: function(value) {
      this.id = value
    },
    add: function() {
      if (!this.number || !this.name)
        return;
      window.mp.triggerServer('phone::contacts::add', JSON.stringify({
        number: this.number,
        name: this.name
      }));
      
      this.close();
    },
    edit: function() {
      if (!this.number || !this.name)
        return;
      window.mp.triggerServer('phone::contacts::edit', JSON.stringify({
        id: this.id,
        number: this.number,
        name: this.name
      }));
      
      this.close();
    },
    close: function() {
      this.setContactPopupData(false, null);
    }
  },

  mounted: function() {
    this.setNumber(this.data.number)
    this.setName(this.data.name)
    this.setId(this.data.id)
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade {
  0% {
    opacity: 0;
  }
  100% {
    opacity: 1;
  }
}
@keyframes slide {
  0% {
    transform: translateY(100%);
  }
  100% {
    transform: translateY(0);
  }
}
.popup {
  position: absolute;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  border-radius: 1.6rem;
  z-index: 4;
  &__layer {
    background: rgba(0, 0, 0, 0.9);
    z-index: 1;
    width: 100%;
    height: 100%;
    animation: fade 0.3s;
  }
  &__block {
    position: absolute;
    z-index: 2;
    color: #fff;
    bottom: 0;
    left: 0;
    height: 17.95rem;
    width: 100%;
    border-radius: 1.6rem;
    overflow: hidden;
    background: #F3F2F8;
    animation: slide 0.2s;
    .close {
      position: absolute;
      top: 1rem;
      right: 1rem;
      mask: url("/img/hud/smartphone/plus1.svg") no-repeat center / contain;
      background: rgba($color: #252525, $alpha: 0.5);
      transform: rotate(45deg);
      width: 1.25rem;
      height: 1.25rem;
      &:hover {
        transform: rotate(45deg) scale(1.08);
      }
    }
    .image {
      width: 100%;
      height: 8rem;
    }
    .form {
      display: flex;
      flex-direction: column;
      align-items: center;
      &__fields {
        width: 14.2rem;
        height: 5.2rem;
        background: #fff;
        border-radius: .4rem;
        overflow: hidden;
        position: relative;
        display: flex;
        flex-direction: column;
        justify-content: space-around;
        margin: 0.5rem 0 0 0;
        .separator {
          position: absolute;
          transform: translate(0, -50%);
          top: 50%;
          right: 0;
          background: rgba(195, 195, 195, 0.35);
          width: 13.2rem;
          height: 1px;
        }
        .label {
          width: 100%;
          position: relative;
          height: 50%;
          display: flex;
          align-items: center;
          justify-content: flex-end;
          &__prepend {
            color: #000;
            font-size: 0.7rem;
            color: #8A8A8E;
            text-align: left;
            width: 100%;
            padding: 0 0 0 1rem;
          }
          &__input {
            width: 9.5rem;
            padding: 0 0.75rem;
            font-size: 0.7rem;
            border-left: 1px solid transparent;
            height: 1.6rem;
            &:focus {
              border-left: 1px solid rgba(195, 195, 195, 0.35);
            }
          }
        }
      }
      &__btn {
        margin: 0.75rem 0 0 0;
        width: 13.2rem;
        height: 2rem;
        background: #0A84FF;
        border-radius: 0.7rem;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: all 0.15s;
        font-size: 0.7rem;
        font-weight: 500;
        color: #fff;
        &:hover {
          transform: translateY(-0.2rem);
          box-shadow: 0 0.1rem 0.15rem 0 rgba(0, 0, 0, 0.8);
        }
        .icon {
          width: 0.8rem;
          height: 0.8rem;
        }
        .text {
          margin: 0 0 0 0.25rem;
        }
      }
    }
  }
}
</style>
