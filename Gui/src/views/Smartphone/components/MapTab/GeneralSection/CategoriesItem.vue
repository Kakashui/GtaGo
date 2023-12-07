<template>
  <div class="categories-item" @click="gps_openCategory(category.key)">
    <div class="categories-item__icon" :style="{ '-webkit-mask': 'url(' + `img/hud/smartphone/mapTab/categories/${category.icon}.svg` + ') no-repeat center / contain' }"></div>
    <span class="categories-item__title">{{ loc(category.title) }}</span>
  </div>
</template>

<script>
import { mapState, mapActions, mapGetters } from 'vuex'

export default {
  name: 'CategoriesItem',

  props: {
    category: Object
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapActions('smartphone', ['gps_openCategory'])
  }
}
</script>

<style lang="scss" scoped>
.categories-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 4.4rem;
  height: 4.4rem;
  background: #fff;
  border-radius: 0.5rem;
  &:hover {
    background: linear-gradient(268.27deg, #1BBD1B 4.17%, #2ADF5D 97.17%);
    box-shadow: 0px 0.5rem 1rem rgba(40, 218, 82, 0.3);
    .categories-item__icon {
      background: #fff;
    }
    .categories-item__title {
      color: #fff;
    }
  }
  &__icon {
    width: 1rem;
    height: 1rem;
    background: #1BBD1B;
  }
  &__title {
    font-size: 0.65rem;
    margin: 0.5rem 0 0 0;
    text-align: center;
    padding: 0 0.25rem;
  }
}
</style>
