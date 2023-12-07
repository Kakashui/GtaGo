<template>
  <div class="tattoo-shop">
    <div class="substrate substrate-top"></div>
    <div class="substrate substrate-bottom"></div>
    <div class="tattoo-shop__header">
      <div class="desc">{{loc('tattoo_shop_1')}}</div>
      <div class="title">{{ loc(currentCategory.title || defaultTittle) }}</div>
    </div>
    <div class="tattoo-shop__main">
      <transition name="tattoo-tab">
        <CategoriesTab @onSelectCtegory="selectCategory" v-if="tattooList.length == 0"/>
        <TattoosTab @onBack="back"  :items="tattooList" :category="currentCategory.key" v-else/>
      </transition>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import CategoriesTab from './components/CategoriesTab'
import TattoosTab from './components/TattoosTab'

export default {
  name: 'TattooShop',

  components: { CategoriesTab, TattoosTab },

  data: function() {
    return {
      tattooList: [],
      currentCategory: {},
      defaultTittle: "tattoo_shop_16"
    }
  },

  methods: {
    selectCategory(category, tattoos){
      //window.console.log(category, tattoos)
      this.currentCategory = category;
      this.tattooList = tattoos;
    },
    back(){
      this.tattooList = [];
      this.currentCategory = {};
    }
  },

  computed: {
    ...mapState('tattooShop', ['tattoos', 'gender']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@300;500&display=swap');

@keyframes fade-in {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.tattoo-shop{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  width: 100%;
  height: 100%;
  padding: 3.4rem 0 3.4rem 3.2rem;
  background: radial-gradient(55.56% 52.59% at 50% 47.41%, rgba(0, 0, 0, 0) 0%, rgba(0, 0, 0, 0.7) 100%);
  font-family: 'Roboto', sans-serif;
  :lang(ge){        
      font-family: 'BPG Arial Caps', sans-serif !important;
  }
  .substrate{
    content: '';
    position: absolute;
    width: 58rem;
    height: 58rem;
    background: rgba(193, 231, 4, 0.5);
    opacity: 0.4;
    filter: blur(4.5rem);
    z-index: 0;
    border-radius: 50%;
    &-top{
      left: -32.5rem;
      top: -40rem;
    }
    &-bottom{
      left: -30rem;
      bottom: -44.5rem;
    }
  }
  &__header{
    display: flex;
    flex-flow: column;
    position: relative;
    z-index: 2;
    &:after{
      content: '';
      position: absolute;
      height: 1px;
      width: 15rem;
      bottom: 0;
      left: 0;
      background: linear-gradient(90.01deg, #C1E704 0%, rgba(255, 255, 255, 0) 85.03%);
      border-radius: .25rem;
    }
    .desc, .title{
      text-transform: uppercase;
      color: #FFFFFF;
    }
    .desc{
      font-weight: normal;
      font-size: 1.5rem;
      line-height: 1.5rem;
    }
    .title{
      font-weight: bold;
      font-size: 4rem;
      line-height: 4rem;
      display: inline-block;
    }
  }
  &__main {
    z-index: 2;
    position: relative;
  }
}
.tattoo-tab-enter-active, .tattoo-tab-leave-active {
  transition: all .3s;
}
.tattoo-tab-enter, .tattoo-tab-leave-to {
  opacity: 0;
  position: absolute;
}
.tattoo-tab-enter {
  transform: translate(-100%, 0);
  z-index: 2;
}
</style>
