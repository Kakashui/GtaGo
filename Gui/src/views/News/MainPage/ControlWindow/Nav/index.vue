<template>
  <div class="nav">
    <div 
      class="nav__item nav__item--tv img-contain-style"
      @click="setStream"
    >
      <svg 
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 30 30" 
        fill="none"
        v-if="isStream"
      >
        <path d="M26 7.5H16.5125L20.625 3.3875L19.75 2.5L14.75 7.5L9.75 2.5L8.8625 3.3875L12.9875 7.5H8.24375H3.5C2.125 7.5 1 8.6125 1 10V25C1 26.375 2.125 27.5 3.5 27.5H26C27.375 27.5 28.5 26.375 28.5 25V10C28.5 8.6125 27.375 7.5 26 7.5ZM26 25H3.5V10H26V25Z"/>
        <path d="M14 13H12V22H14V13Z"/>
        <path d="M18 13H16V22H18V13Z"/>
      </svg>
      <svg 
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 30 30" 
        fill="none"
        v-else
      >
        <path d="M26.25 7.5H16.7625L20.875 3.3875L20 2.5L15 7.5L10 2.5L9.1125 3.3875L13.2375 7.5H3.75C2.375 7.5 1.25 8.6125 1.25 10V25C1.25 26.375 2.375 27.5 3.75 27.5H26.25C27.625 27.5 28.75 26.375 28.75 25V10C28.75 8.6125 27.625 7.5 26.25 7.5ZM26.25 25H3.75V10H26.25V25ZM11.25 12.5V22.5L20 17.5L11.25 12.5Z"/>
      </svg>
      <div class="nav__item-prompt">{{loc('news:nav:1')}}</div>
    </div>
    <div 
      class="nav__item img-contain-style"
      @click="setVolume"
    >
      <svg 
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24" 
        fill="none"
        v-if="isVolume"
      >
        <path d="M3 8.99998V15H7L12 20V3.99998L7 8.99998H3ZM16.5 12C16.5 10.23 15.48 8.70998 14 7.96998V16.02C15.48 15.29 16.5 13.77 16.5 12ZM14 3.22998V5.28998C16.89 6.14998 19 8.82998 19 12C19 15.17 16.89 17.85 14 18.71V20.77C18.01 19.86 21 16.28 21 12C21 7.71998 18.01 4.13998 14 3.22998Z"/>
      </svg>
      <svg
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24" 
        fill="none"
        v-else
      >
        <rect x="4" y="3.30078" width="2.20121" height="23.6652" transform="rotate(-45 4 3.30078)"/>
        <path fill-rule="evenodd" clip-rule="evenodd" d="M7.72168 9H3V15H7L12 20V13.2783L7.72168 9ZM16.2453 17.5236C15.5765 18.0419 14.8281 18.4506 14 18.7V20.76C15.3756 20.451 16.622 19.8141 17.6799 18.9582L16.2453 17.5236ZM4.27416 3.00416L4.27 3L3 4.27L3.00416 4.27416L4.27416 3.00416ZM14 7.97C15.48 8.71 16.5 10.23 16.5 12C16.5 12.22 16.48 12.43 16.45 12.63L14 10.18V7.97ZM18.46 14.64C18.8 13.82 19 12.94 19 12C19 8.83 16.89 6.15 14 5.29V3.23C18.01 4.14 21 7.72 21 12C21 13.5 20.63 14.91 19.97 16.15L18.46 14.64ZM9.91 6.09L12 4V8.18L9.91 6.09Z"/>
      </svg>
    </div>
    <div 
      class="nav__item img-contain-style"
      @click="setPage('StatsPage')"      
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24" 
        fill="none"
      >
        <path d="M10 20H14V4H10V20ZM4 20H8V12H4V20ZM16 9V20H20V9H16Z"/>
      </svg>
    </div>
    <div class="nav__item nav__item--toggle img-contain-style">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24" 
        fill="none"
      >
        <path d="M19.14 12.9399C19.18 12.6399 19.2 12.3299 19.2 11.9999C19.2 11.6799 19.18 11.3599 19.13 11.0599L21.16 9.4799C21.34 9.3399 21.39 9.0699 21.28 8.8699L19.36 5.5499C19.24 5.3299 18.99 5.2599 18.77 5.3299L16.38 6.2899C15.88 5.9099 15.35 5.5899 14.76 5.3499L14.4 2.8099C14.36 2.5699 14.16 2.3999 13.92 2.3999H10.08C9.83999 2.3999 9.64999 2.5699 9.60999 2.8099L9.24999 5.3499C8.65999 5.5899 8.11999 5.9199 7.62999 6.2899L5.23999 5.3299C5.01999 5.2499 4.76999 5.3299 4.64999 5.5499L2.73999 8.8699C2.61999 9.0799 2.65999 9.3399 2.85999 9.4799L4.88999 11.0599C4.83999 11.3599 4.79999 11.6899 4.79999 11.9999C4.79999 12.3099 4.81999 12.6399 4.86999 12.9399L2.83999 14.5199C2.65999 14.6599 2.60999 14.9299 2.71999 15.1299L4.63999 18.4499C4.75999 18.6699 5.00999 18.7399 5.22999 18.6699L7.61999 17.7099C8.11999 18.0899 8.64999 18.4099 9.23999 18.6499L9.59999 21.1899C9.64999 21.4299 9.83999 21.5999 10.08 21.5999H13.92C14.16 21.5999 14.36 21.4299 14.39 21.1899L14.75 18.6499C15.34 18.4099 15.88 18.0899 16.37 17.7099L18.76 18.6699C18.98 18.7499 19.23 18.6699 19.35 18.4499L21.27 15.1299C21.39 14.9099 21.34 14.6599 21.15 14.5199L19.14 12.9399ZM12 15.5999C10.02 15.5999 8.39999 13.9799 8.39999 11.9999C8.39999 10.0199 10.02 8.3999 12 8.3999C13.98 8.3999 15.6 10.0199 15.6 11.9999C15.6 13.9799 13.98 15.5999 12 15.5999Z"/>
      </svg>
      <input 
        type="text"
        readonly
        @focus="toggleBlock = false"
        @blur="setToggleBlock"
      >
      <div 
        class="toggle-block"
        v-if="toggleBlock"
      >
        <div 
          class="toggle-block__item"
          @click="setPage('ManagementPage')"
        >{{loc('news:nav:2')}}</div>
        <div 
          class="toggle-block__item"
          @click="setModal({show: true, currentModal: 'PriceModal'})"
        >{{loc('news:nav:3')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'Nav',
  
  data: function() {
    return {
      toggleBlock: false
    }
  },

  computed: {
    ...mapState('news', ['isVolume', 'isStream']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('news', ['setModal', 'setCurrentPage']),
    setVolume: function() {
      window.mp.trigger('news:setVolume', !this.isVolume)
    },
    setStream: function() {
      if (this.isStream) {
        window.mp.trigger('news:setOffStream')
      } else {
        //this.setModal({show: true, currentModal: 'ConfirmationStreamModal'})
      }
    },
    setToggleBlock: function() {
      setTimeout(() => {
        this.toggleBlock = false
      }, 200);
    },
    setPage: function(page) {
      console.log(page)
      // this.setCurrentPage(page)
    },

  },
}
</script>

<style lang="scss" scoped>
.nav{
  display: flex;
  align-items: flex-end;
  &__item{
    display: flex;
    justify-content: center;
    align-items: center;
    margin-right: 1rem;
    margin-bottom: .3rem;
    position: relative;
    overflow: hidden;
    &:hover{
      overflow: initial;
    }
    &-prompt{
      position: absolute;
      display: flex;
      align-items: center;
      justify-content: center;
      bottom: -.1rem;
      transform: translateY(100%);
      padding: 0 .5rem;
      white-space: pre;
      font-weight: normal;
      font-size: .6rem;
      color: #F3F5F7;
      height: 1.5rem;
      background: rgba(0, 0, 0, 0.7);
      border-radius: .2rem;
    }
    svg{
      width: 1.2rem;
      height: 1.2rem;
      fill: #AFB9C5;
      transition: .2s;
    }
    &:hover{
      svg{
        fill: #D92C2C;
      }
    }
    &:last-child{
      margin-right: 0;
    }
    &--tv{
      margin-bottom: 0;
      svg{
        width: 2.2rem;
        height: 2.2rem;
      } 
    }
    &--toggle{
      overflow: initial;
      input{
        width: 100%;
        height: 100%;
        position: absolute;
        z-index: 1;
        background-color: transparent;
      }
      svg{
        z-index: 0;
      }
      .toggle-block{
        position: absolute;
        left: 0;
        bottom: -.4rem;
        transform: translateY(100%);
        padding: .7rem 1rem;
        background: #FFFFFF;
        box-shadow: 0 0 .5rem rgba(0, 0, 0, 0.08);
        border-radius: .5rem;
        &__item{
          min-width: 8.1rem;
          border-bottom: 1px solid rgba(0, 0, 0, 0.1);
          padding: .7rem 0;
          font-weight: 600;
          font-size: .8rem;
          line-height: .9rem;
          color: #46555F;
          transition: .2s;
          &:hover{
            color: #E30000;
          }
          &:first-child{
            padding-top: 0;
          }
          &:last-child{
            border-bottom: 0;
            padding-bottom: 0;
          }
        }
      }
    }
  }
}
</style>
