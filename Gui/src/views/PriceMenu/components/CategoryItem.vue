<template>
  <div class="price-menu-item">
    <input
      type="checkbox"
      v-model="inputModel"
    />
    {{item.cat}}
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'CategoryMenuItem',

  data: function () {
    return {
      check: true
    }
  },

  props: {
    item: Object
  },
  computed:{
    ...mapGetters('localization', ['loc']),
    inputModel: {
      get () {
        return this.item.show
      },
      set (value) {
        this.setCategoryShow({cat:this.item.cat, show:value})
      }
    },
  },
  methods: {
    ...mapMutations('priceMenu', ['setCategoryShow'])
  }
}
</script>

<style lang="scss" scoped>
.price-menu-item {
  display: block;
  background-color: rgba(33, 33, 37, .9);
  margin: 0 0 1rem 0;
  padding: 1rem;
  border: .1rem solid rgba(78, 149, 37, .3);
  border-radius: .2rem;
  font-size: 1.3rem;
  color: #fff;
  .title {
    display: flex;
    align-items: center;
  }
}
</style>
