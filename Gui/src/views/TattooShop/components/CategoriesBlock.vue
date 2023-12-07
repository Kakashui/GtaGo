<template>
  <div class="category">
    <span class="category__header">{{ loc(category.title) }}</span>
    <div class="category__list">
      <categories-item
        v-for="(item, index) in category.items"
        :key="index"
        :item="item"
        @onItemClick="setCurrentCategory"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import CategoriesItem from './CategoriesItem'

export default {
  name: 'CategoriesBlock',

  components: { CategoriesItem },
  methods: {
    setCurrentCategory(item){
      const config = this.tattoos[this.category.key];
      if(config == undefined) return;
      const tattoos = [];
      config.forEach( c => {
        let check = false;
        item.ids.forEach(id => {
          if(this.gender ? c.Male !== '' : c.Female !== ''){
            if(c.Slots.indexOf(id) !== -1){
              check = true;
              return;
            }
          }
        });

        if(check) 
          tattoos.push(c);
      });
      window.console.log(item.ids[0])
      this.$emit("onSelectCtegory", this.category, tattoos, this.cameraPosition(item.ids[0]))
    }
  },
  props: {
    category: Object
  },

  computed: {
    ...mapState('tattooShop', ['tattoos', 'gender']),
    ...mapGetters('localization', ['loc']), 
    ...mapGetters('tattooShop', ['cameraPosition'])
  },

}
</script>

<style lang="scss" scoped>
.category {
  margin: 1.6rem 0 0 0;
  &:first-child {
    margin: 0;
  }
  &__header {
    text-transform: uppercase;
    font-size: 1.5rem;
    color: #fff;
  }
  &__list {
    display: flex;
    flex-direction: column;
  }
}
</style>
