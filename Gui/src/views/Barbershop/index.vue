<template>
  <div class="barbershop">
    <div class="barbershop__actions">
      <type-slider :currentTypes="getConfig(currentCategory.type)" :currentTitle="loc(currentCategory.title)" :price="price" :value="value" @onChange="cahnge" />
      <type-range :currentColors="getConfig(currentCategory.type).colors" :value="value"  @onChange="cahnge" />
      <div class="actions__buttons">
        <buy-button  @onClick="buy"/>
        <exit-button />
      </div>
      <div class="actions__price">
        <span class="price__title">{{loc('barber_4')}}:</span>
        <span class="price__value">{{ (getConfig(currentCategory.type).price / 100 * price).toFixed() }}$</span>
      </div>
    </div>
    <div class="barbershop__categories">
      <categories-item
        v-for="(category, index) in categories"
        :key="index"
        :category="category"
        @setCurrentCategory="setCurrentCategory"
        :currentType="currentCategory.type"
      />
    </div>
  </div>
</template>

<script>
import CategoriesItem from './components/CategoriesItem'
import TypeSlider from './components/CurrentTypeSlider'
import TypeRange from './components/CurrentTypeRange'
import BuyButton from './components/BuyButton'
import ExitButton from './components/ExitButton'
import {mapState, mapGetters} from 'vuex'

export default {
  name: 'Barbershop',

  components: {
    CategoriesItem,
    TypeSlider,
    TypeRange,
    BuyButton,
    ExitButton
  },

  data: function() {
    return {
      currentCategory: {type: 'hairstyle', title: 'barber_5'},
      categories: [
        {type: 'hairstyle', title: 'barber_5'},
        {type: 'eyebrows', title: 'barber_6'},
        {type: 'beard', title: 'barber_13'},
        {type: 'torso', title: 'barber_7'},
        {type: 'lenses', title: 'barber_8'},
        {type: 'pomade', title: 'barber_9'},
        {type: 'blush', title: 'barber_10'},
        {type: 'shadows', title: 'barber_11'}
      ],
      value:{
        style: 0,
        color: 0
      }
    }
  },

  computed: {
    ...mapState('barbershop', ['gender', 'price', 'config']),
    ...mapGetters('localization', ['loc']),
    currentTypes: function() {
      return this.types.filter(item => item.parent === this.currentCategory.type)
    },

    currentColors: function() {
      return this.colors.filter(item => item.parent === this.currentCategory.type)
    },

    hairs(){
      return this.gender ? this.config.hairM : this.config.hairF
    },
    hairColors(){
      return this.config.hairColor
    }
  },

  methods: {
    getConfig(key){
      return this.$store.getters['barbershop/config'](key);
    },
    setCurrentCategory: function(item) {
      this.currentCategory = item
    },
    cahnge(){
      window.mp.trigger("changeBarber", this.currentCategory.type, this.value.style, this.value.color)
    },
    buy(){
       window.mp.trigger("buyBarber", this.currentCategory.type, this.value.style, this.value.color)
    }
  },
  watch:{
    
  }
}
</script>

<style lang="scss" scoped>
.barbershop {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 4.5rem;
  &__actions {
    .actions__buttons {
      margin: 3rem 0 0 0;
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
    .actions__price {
      background: linear-gradient(90deg, #1E1E1E 0%, rgba(26, 26, 26, 0.43) 49.48%, #1E1E1E  100%);
      margin: 3.5rem 0 0 0;
      display: flex;
      justify-content: space-between;
      padding: 1rem;
      color: #FFFFFF;
      border-radius: .6rem;
    }
  }
}
</style>
