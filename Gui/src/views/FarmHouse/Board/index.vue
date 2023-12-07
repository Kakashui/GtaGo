<template>
  <div class="board">
    <div class="board__nav">
      <div 
        :class="[{active: currentPage === 'BoardShop'}, 'board__nav-item']"
        @click="setCurrentPage('BoardShop')"
      >
        <svg 
          xmlns="http://www.w3.org/2000/svg" 
          viewBox="0 0 24 24" 
          fill="none"
        >
          <path d="M19.5 8.19094V2.25H15.75V4.98984L12 1.5L0 12.75H3V22.5H9.75V15H14.25V22.5H21V12.75H24L19.5 8.19094Z"/>
        </svg>
        <span>{{loc('farmHouse_0')}}</span>
      </div>
      <div 
        :class="[{active: currentPage === 'BoardProfile'}, 'board__nav-item']"
        @click="setCurrentPage('BoardProfile')"
      >
        <svg 
          xmlns="http://www.w3.org/2000/svg" 
          viewBox="0 0 24 24" 
          fill="none"
        >
          <path d="M12 12C13.0384 12 14.0534 11.6921 14.9167 11.1152C15.7801 10.5383 16.453 9.7184 16.8504 8.75909C17.2477 7.79978 17.3517 6.74418 17.1491 5.72578C16.9466 4.70738 16.4465 3.77192 15.7123 3.03769C14.9781 2.30347 14.0426 1.80345 13.0242 1.60088C12.0058 1.39831 10.9502 1.50227 9.99091 1.89963C9.0316 2.29699 8.21166 2.9699 7.63478 3.83326C7.05791 4.69662 6.75 5.71165 6.75 6.75C6.75 8.14239 7.30312 9.47775 8.28769 10.4623C9.27226 11.4469 10.6076 12 12 12ZM12 13.5C8.74594 13.5 2.25 15.51 2.25 19.5V22.5H21.75V19.5C21.75 15.51 15.2541 13.5 12 13.5Z" fill="white"/>
        </svg>
        <span>{{loc('farmHouse_1')}}</span>
      </div>
    </div>
    <component 
      :is="currentPage"
      class="board__body"
    />
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import BoardProfile from './BoardProfile'
import BoardShop from './BoardShop'
export default {
  name: 'Board',
  components: {
    BoardProfile,
    BoardShop
  },
  data: function() {
    return {
      currentPage: null
    }
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    setCurrentPage: function(page) {
      this.currentPage = page
    }
  },
  mounted() {
    this.currentPage = 'BoardProfile'
  }
}
</script>

<style lang="scss" scoped>
.board{
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  height: 37.8rem;
  width: 56.5rem;
  background-color: #111111;
  position: relative;
  color: #FFFFFF;
  &:before{
    position: absolute;
    left: 50%;
    top: 0;
    transform: translateX(-50%) translateY(-73%) rotate(-6deg);
    content: 'Farmhouse';
    font-weight: bold;
    text-transform: uppercase;
    font-size: 4rem;
    line-height: 4rem;
    letter-spacing: 0.1em;
    color: #FFFFFF;
    width: 23rem;
    height: 8rem;
    display: flex;
    align-items: center;
    justify-content: center;
    background-image: url('/img/farmHouse/plate.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  &__nav{
    display: flex;
    width: 100%;
    align-items: flex-end;
    font-style: normal;
    font-weight: 600;
    font-size: 1.4rem;
    line-height: 1.6rem;
    height: 6.85rem;
    min-height: 6.85rem;
    background-image: url('/img/farmHouse/board.png');
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    &-item{
      width: 100%;
      padding-bottom: .9rem;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      opacity: .5;
      transition: .3s;
      &:hover{
        opacity: 1;
      }
      svg{
        width: 1.2rem;
        height: 1.2rem;
        fill: #fff;
        margin-right: .8rem;
      }
      &.active{
        opacity: 1;
        &:before{
          content: '';
          width: 10rem;
          height: .2rem;
          position: absolute;
          bottom: 0;
          background: #B6D300;
          box-shadow: 0 .4rem 1rem rgba(182, 211, 0, 0.2);
        }
      }
    }
  }
  &__body{
    height: calc(100% - (6.8rem));
  }
}
</style>
