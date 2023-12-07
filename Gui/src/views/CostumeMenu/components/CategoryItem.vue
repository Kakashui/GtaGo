<template>
  <div class="costume-menu-item">
    {{item.name}}
    <input class="costume-menu-item__input"
      type="number"
      v-model="drawableModel"
    />
    <input class="costume-menu-item__input"
      type="number"
      v-model="textureModel"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'CategoryItem',

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
    drawableModel: {
      get () {
        return this.item.drawable
      },
      set (value) {
        this.setDrawable({drawable: value, key: this.item.key, type: this.item.type})
      }
    },
    textureModel: {
      get () {
        return this.item.texture
      },
      set (value) {
        this.setTexture({texture: value, key: this.item.key, type: this.item.type})
      }
    },
  },
  methods: {
    ...mapMutations('costumeMenu', ['setDrawable', 'setTexture'])
  }
}
</script>

<style lang="scss" scoped>
.costume-menu-item {
  display: block;
  background-color: rgba(33, 33, 37, .9);
  margin: 0 0 0.5rem 0;
  padding: 0.5rem;
  border: .1rem solid rgba(78, 149, 37, .3);
  border-radius: .2rem;
  font-size: 1rem;
  color: #fff;
  .title {
    display: flex;
    align-items: center;
  }
  &__input {
      padding: .35rem .8rem;
      outline: none;
      border:solid 1px rgb(0, 0, 0);
      font-size: 0.8rem;
      background-color: #5050507a;
      color: #fff;
      letter-spacing: .4px;
      width: 5rem;
  }
}
</style>
