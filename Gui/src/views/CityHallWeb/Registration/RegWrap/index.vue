<template>
  <div class="reg-bg">
    <div class="reg-wrap">
      <div class="greeting">
        <div class="greeting__decorate">
          <div 
            class="star"
            v-for="item in 3"
            :key="item.key"
          ></div>
        </div>
        <div class="greeting__text">{{loc('cityHallWeb_83')}}</div>
        <div class="greeting__prompt">{{loc('cityHallWeb_84')}}</div>
      </div>
      <components
        :is="currentPage"
        :setCurrentPage="setCurrentPage"
        @goToCard="setCurrentWrap('CardWrap')"
      />
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import FirstPage from './FirstPage'
import SecondPage from './SecondPage'
export default {
  name: 'RegWrap',

  props: {
    setCurrentWrap: Function
  },

  components: {
    FirstPage,
    SecondPage
  },

  data: function() {
    return{
      currentPage: '',
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentPage: function(value) {
      this.currentPage = value
    }
  },

  mounted(){
    this.setCurrentPage('FirstPage')
  }
}
</script>

<style lang="scss" scoped>
.reg-bg{
  background-size: cover;
  background-image: url('/img/cityHallWeb/registration-bg.png');
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;
}
.reg-wrap{
  display: flex;
  flex-flow: column;
  justify-content: space-between;
  background: #FFFFFF;
  border-radius: .5rem;
  width: 25.3rem;
  min-height: 37.4rem;
  &>div{
    width: 100%;
  }
  .greeting{
    display: flex;
    flex-flow: column;
    align-items: center;
    padding: 2.5rem 2.5rem 1.5rem 2.5rem;
    border-bottom: 1px solid rgba(70, 85, 95, 0.1);
    text-align: center;
    font-family: Merriweather;
    &__decorate{
      display: flex;
      align-items: center;
      margin-bottom: .5rem;
      .star{
        width: .9rem;
        height: .9rem;
        margin-right: .95rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/cityHallWeb/star-decore.svg');
      }
    }
    &__text{
      font-weight: bold;
      font-size: 2rem;
      line-height: 2.5rem;
      text-align: center;
      color: #293340;
      margin-bottom: .5rem;
    }
    &__prompt{
      font-size: .8rem;
      line-height: 1rem;
      text-align: center;
      color: rgba(70, 85, 95, 0.9);
      max-width: 12.9rem;
    }
  }
}
</style>
