<template>
  <div class="business-menu" v-if="show">
    <span class="business-menu__title">{{ loc(businessData.TypeName) }}</span>
    <button class="business-menu__close" @click="close">&times;</button>
    <div class="business-menu__items">
      <business-item
        v-for="(item, index) in businessData.Items"
        :key="index"
        :item="item"
      />
    </div>
    <button class="business-menu__button" @click="addItem">{{loc('biz:menu:btn:add')}}</button>
  </div>
</template>

<script>
import BusinessItem from './components/BusinessItem'
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'Business',

  components: { BusinessItem },

  computed: {
    ...mapState('businessMenu', ['businessData', 'show']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('businessMenu', ['setShow']),

    addItem () {
      window.mp.trigger('bizsetts:addnew')
    },

    close () {
      window.mp.trigger('bizsetts:close')
    }
  }
}
</script>

<style lang="scss" scoped>
.business-menu {
  display: flex;
  flex-direction: column;
  align-items: center;
  background-color: rgba(0, 0, 0, .9);
  padding: 1rem 3rem;
  border-radius: .4rem;
  position: relative;
  &__title {
    color: #fff;
    font-size: 2.6rem;
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
  &__items {
    height: 30rem;
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
    background-color: #4e9525;
    border: none;
    outline: none;
    padding: 1rem 5rem;
    font-size: 1.6rem;
    color: #fff;
    border-radius: .1rem;
    margin: 1rem 0 0 0;
  }
}
</style>
