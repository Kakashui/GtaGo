<template>
  <div class="price-menu-menu" >
    <span class="price-menu-menu__title">{{ loc("") }}</span>
    <button class="price-menu-menu__close" @click="close">&times;</button>
    <span class="price-menu-menu__title">
      search
      <input
        type="text"
        class="price-menu-menu__input"
        v-model="search"
      />
    </span>
    <div class="price-menu-menu__data">
      <div class="price-menu-menu__menu">
        <category-item
          v-for="item in priceCategories"
          :key="item.cat"
          :item="item"
        />
      </div>
      <div class="price-menu-menu__items">
        <price-menu-item
          v-for="(item, index) in currKeys"
          :key="index"
          :item="item"
          :value="priceList[item]"
        />
      </div>
    </div>
    <button v-if="!adding" class="price-menu-menu__button" @click="openAdd">{{loc('biz:menu:btn:add')}}</button>
    <div v-if="adding" class="price-menu-menu__block">
      Name: <input
        type="text"
        class="price-menu-menu__input"
        v-model="newName"
      />
      Price: <input
        type="number"
        class="price-menu-menu__input"
        v-model="newPrice"
      />
    </div>
    <div v-if="adding" class="price-menu-menu__block">
      <button class="price-menu-menu__button" @click="addItem">Ok</button>
      <button class="price-menu-menu__button" @click="closeAdd">Cancel</button>
    </div>
  </div>
</template>

<script>
import PriceMenuItem from './components/PriceMenuItem'
import CategoryItem from './components/CategoryItem'
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'PriceMenu',
  components: { PriceMenuItem, CategoryItem },
  data: function () {
    return {
      search: '',
      adding: false,
      newName: '',
      newPrice: '',
    }
  },

  computed: {
    ...mapState('priceMenu', ['priceList', 'priceCategories']),
    ...mapGetters('localization', ['loc']),
    currKeys() {
      return Object.keys(this.priceList).filter(priceCat => 
        (!this.search || priceCat.includes(this.search)) && 
        this.priceCategories.findIndex(item => item.show && item.cat == priceCat.slice(0, priceCat.indexOf('::'))) > -1 &&
        this.priceList[priceCat] >= 0
      );
    }
  },

  methods: {

    openAdd () {
      this.adding = true;
      this.newName = '';
      this.newPrice = '';
    },
    closeAdd () {
      this.adding = false;
    },

    addItem () {
      if (this.newName && this.newPrice > 0) {
        window.mp.triggerServer('priceMenu:addItem', this.newName, this.newPrice)
        this.closeAdd();
      }
    },

    close () {
      window.mp.trigger('priceMenu:close')
    }
  }
}
</script>

<style lang="scss" scoped>
.price-menu-menu {
  display: flex;
  flex-direction: column;
  align-items: center;
  background-color: rgba(0, 0, 0, .9);
  padding: 1rem 2rem;
  position: relative;
  height: 80%;
  &__input {
    width: 10rem;
    border:solid 1px rgb(255, 255, 255);
    font-size: 1rem;
    background-color: transparent;
    padding: .3rem;
    outline: none;
    color: #fff;
    &::-webkit-outer-spin-button,
    &::-webkit-inner-spin-button {
      -webkit-appearance: none;
      margin: 0;
    }
  }
  &__title {
    color: #fff;
    font-size: 2.6rem;
  }
  &__data {
    display: flex;
    flex-direction: row;
    height: 80%;
  }
  &__close {
    position: absolute;
    background-color: transparent;
    border: none;
    color: #fff;
    font-size: 2.5rem;
    height: 3rem;
    width: 3rem;
    padding: 0;
    right: 0;
    top: 0;
    outline: none;
  }
  &__menu {
    flex-direction: row;
    width: 20rem;
    height: 80%;
    overflow-y: auto;
    padding: 0 1rem 0 0;
    margin: 1rem 0 0 0;
    color: #fff;
    &::-webkit-scrollbar-track {
      background-color: #959595;
    }
    &::-webkit-scrollbar {
      width: .35rem;
      background-color: transparent;
    }
    &::-webkit-scrollbar-thumb {
      background-color: #ffffff;
      border: .1rem solid #ffffff;
      height: 33.3%;
    }
  }
  &__items {
    width: 50rem;
    height: 80%;
    overflow-y: auto;
    padding: 0 1rem 0 0;
    margin: 1rem 0 0 0;
    &::-webkit-scrollbar-track {
      background-color: #959595;
    }
    &::-webkit-scrollbar {
      width: .35rem;
      background-color: transparent;
    }
    &::-webkit-scrollbar-thumb {
      background-color: #ffffff;
      border: .1rem solid #ffffff;
      height: 33.3%;
    }
  }
  &__button {
      padding: .35rem .8rem;
      outline: none;
      border:solid 1px rgb(0, 0, 0);
      font-size: 1rem;
      background-color: #4e9525;
      color: #fff;
      letter-spacing: .4px;
  }
  &__block {
    border: none;
    outline: none;
    font-size: 1.6rem;
    color: #fff;
    border-radius: .1rem;
    margin: 1rem 0 0 0;
  }
}
</style>