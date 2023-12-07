<template>
  <div class="price-menu-menu" >
    <button class="price-menu-menu__close" @click="close">&times;</button>
    <span class="price-menu-menu__title">
      Name
      <input
        type="text"
        class="price-menu-menu__input"
        v-model="costumeName"
      />
    </span>
      <div class="price-menu-menu__menu">
        <armor-item
          :item="armorItem"
        />
        <category-item
          v-for="item in clothesList"
          :key="item.name"
          :item="item"
        />
      </div>
      <button class="price-menu-menu__button" @click="save">Save</button>
  </div>
</template>

<script>
import CategoryItem from './components/CategoryItem'
import ArmorItem from './components/ArmorItem'

import { mapState, mapGetters } from 'vuex'

export default {
  name: 'CostumeMenu',
  components: { CategoryItem, ArmorItem },
  data: function () {
    return {
      costumeName: '',
      adding: false,
      newName: '',
      newPrice: '',
    }
  },

  computed: {
    ...mapState('costumeMenu', ['clothesList', 'armorItem']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {

    save () {
      if (this.costumeName)
        window.mp.trigger('costumeMenu:save', this.costumeName, JSON.stringify(this.clothesList), JSON.stringify(this.armorItem))
    },

    close () {
      window.mp.trigger('costumeMenu:close')
    }
  }
}
</script>

<style lang="scss" scoped>
.price-menu-menu {
  background-color: rgba(82, 82, 82, 0.9);
  padding: 1rem 2rem;
  position: absolute;
  height: 70%;
  width: 25rem;
  left: 5rem;
  top: 15%;
  &__title {
    color: #fff;
    font-size: 1.5rem;
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
    height: 85%;
    overflow-y: auto;
    padding: 0 0.5rem 0 0;
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
  &__button {
      padding: .35rem .8rem;
      outline: none;
      border:solid 1px rgb(0, 0, 0);
      font-size: 1rem;
      background-color: #4e9525;
      color: #fff;
      letter-spacing: .4px;
      margin: 0.5rem;
  }
}
</style>