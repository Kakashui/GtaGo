<template>
  <div class="modal-wrap">
    <div class="modal">
      <div
        class="btn-close btn"
        @click="closeModal"
      ></div>
      <div class="modal__header">{{loc('cityHallWeb_77')}}</div>
      <div class="modal__body">
        <div class="body-desc">{{loc('cityHallWeb_78')}}</div>
        <div class="body-switch">
          <div 
            :class="[{active: item.key === currentCategory}, 'body-switch__item']"
            v-for="item in categoriesList"
            :key="item.key"
            @click="setCurrentCategory(item.key)"
          >{{loc(item.text)}}</div>
        </div>
        <div class="body-desc">{{loc('cityHallWeb_79')}}</div>
        <input 
          type="text"
          :placeholder="loc('chall:modal:plh:1')"
          v-model="currentPhoto"
        >
        <div class="body-desc">{{loc('cityHallWeb_80')}}</div>
        <input 
          type="text"
          :placeholder="loc('chall:modal:plh:2')"
          v-model="currentTitle"
        >
        <div class="body-desc">{{loc('cityHallWeb_81')}}</div>
        <textarea
          :placeholder="loc('chall:modal:plh:2')"
          v-model="currentText"
        ></textarea>
        <div 
          v-if="currentCategory && currentPhoto && currentTitle && currentText"
          class="body-btn btn"
          @click="sendNewsForm"
        >{{loc('cityHallWeb_82')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'Modal',
  
  props: {
    categoriesList: Array
  },

  data: function() {
    return{
      currentCategory: null,
      currentPhoto: null,
      currentTitle: null,
      currentText: null,
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    setCurrentCategory: function(value) {
      this.currentCategory = value
    },
    sendNewsForm: function() {
      window.mp.trigger('cityHallWeb:sendNewsForm', this.currentCategory, this.currentPhoto, this.currentTitle, this.currentText)
      this.closeModal()
    },
    closeModal: function() {
      this.$emit('closeModal')
    }
  },

  created() {
    this.currentCategory = this.categoriesList[0].key
  }
}
</script>

<style lang="scss" scoped>
.modal-wrap{
  width: 100%;
  height: 100%;
  position: fixed;
  left: 0;
  top: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0, 0, 0, 0.5);
  .modal{
    background-color: #FFFFFF;
    background-image: url('/img/cityHallWeb/present-news-bg.png');
    background-size: contain;
    background-position: left bottom;
    background-repeat: no-repeat;
    border-radius: 1rem;
    position: relative;
    width: 33.4rem;
    display: flex;
    flex-flow: column;
    align-items: center;
    min-height: 34.5rem;
    .btn-close{
      position: absolute;
      right: -.5rem;
      top: 0;
      transform: translateX(100%);
      width: 2rem;
      height: 2rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/cityHallWeb/btn-close.svg');
      transition: .2s;
      &:hover{
        transform: translateX(100%) scale(1.1);
        transition: .2s;
      }
    }
    &__header{
      width: 100%;
      padding: 1.45rem 0;
      text-align: center;
      font-family: Roboto;
      font-style: normal;
      font-weight: normal;
      font-size: 1.2rem;
      color: #46555F;
      border-bottom: 1px solid rgba(70, 85, 95, 0.2);
    }
    &__body{
      display: flex;
      flex-flow: column;
      padding: 1rem 1.7rem 1.5rem 11.5rem;
      .body-desc{
        font-family: Roboto;
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: .95rem;
        color: #46555F;
        margin-bottom: .7rem;
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
        padding-left: 1rem;
        width: fit-content;
        &:before{
          content: '';
          width: .4rem;
          height: .4rem;
          position: absolute;
          left: 0;
          background: linear-gradient(0deg, #E00B29, #E00B29), #5F6AC4;
          border-radius: .3rem;
        }
      }
      .body-switch{
        display: flex;
        align-items: center;
        margin-bottom: 1rem;
        &__item{
          display: flex;
          align-items: center;
          justify-content: center;
          padding: .5rem .8rem;
          font-family: Roboto;
          font-style: normal;
          font-weight: 500;
          font-size: .8rem;
          color: rgba(70, 85, 95, 0.5);
          border-radius: .2rem;
          margin-right: .2rem;
          transition: .2s;
          &:last-child{
            margin-right: 0;
          }
          &:hover, &.active{
            color: #E00B29;
            transition: .2s;
          }
          &.active{
            background: #F3F5F7;
          }
        }
      }
      input, textarea{
        border: 1px solid rgba(70, 85, 95, 0.2);
        box-sizing: border-box;
        border-radius: .3rem;
        background-color: transparent;
        font-family: Roboto;
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: .9rem;
        margin-bottom: 1rem;
        width: 100%;
        outline: none;
        &::placeholder{     
          color: rgba(70, 85, 95, 0.7);
        }
      }
      input{
        padding: 0 1rem;
        min-height: 2.5rem;
        height: 2.5rem;
      }
      textarea{
        padding: 1.25rem 1rem;
        resize: none;
        height: 6.85rem;
      }
      .body-btn{
        min-height: 2.8rem;
        height: 2.8rem;
        background-color: #E00B29;
        border-radius: .3rem;
        width: 100%;
        font-family: Roboto;
        font-style: normal;
        font-weight: 500;
        font-size: .9rem;
        text-align: center;
        color: #FFFFFF;
        &:hover{
          background-color: #a51024;
        }
      }
    }
  }
}
</style>
