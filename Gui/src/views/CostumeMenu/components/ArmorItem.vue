<template>
  <div class="costume-menu-item">
    Armor
    <input class="costume-menu-item__radio"
      type="radio"
      v-model="typeModel"
      value="0"
    />Only type
    <input class="costume-menu-item__radio"
      type="radio"
      v-model="typeModel"
      value="1"
    />This model
    <div class="costume-menu-item__block">
      <div v-if="item.type == 0">
        <input class="costume-menu-item__input"
          type="number"
          v-model="armorType"
        />
      </div>
      <div v-else>
        <input class="costume-menu-item__input"
          type="number"
          v-model="drawableModel"
        />
        <input class="costume-menu-item__input"
          type="number"
          v-model="textureModel"
        />
      </div>
      <input class="costume-menu-item__checkbox"
        type="checkbox"
        v-model="viewArmor"
        value="true"
      />View armor
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'ArmorItem',

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
    viewArmor: {
      get () {
        return this.item.viewArmor
      },
      set (value) {
        this.setViewArmor(value)
      }
    },
    typeModel: {
      get () {
        return this.item.type
      },
      set (value) {
        this.setTypeArmor(value)
      }
    },
    armorType: {
      get () {
        return this.item.armType
      },
      set (value) {
        this.setArmType(value)
      }
    },
    drawableModel: {
      get () {
        return this.item.drawable
      },
      set (value) {
        this.setArmor({drawable: value, texture: this.item.texture})
      }
    },
    textureModel: {
      get () {
        return this.item.texture
      },
      set (value) {
        this.setArmor({drawable: this.item.drawable, texture: value})
      }
    },
  },
  methods: {
    ...mapMutations('costumeMenu', ['setArmType', 'setArmor', 'setTypeArmor', 'setViewArmor'])
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
  &__block {
    padding: .35rem .8rem;
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
  &__radio {
      padding: .35rem .8rem;
      outline: none;
      border:solid 1px rgb(0, 0, 0);
      font-size: 0.8rem;
      background-color: #5050507a;
      color: #fff;
      letter-spacing: .4px;
      width: 2rem;
  }
  &__checkbox {
      padding: .35rem .8rem;
      outline: none;
      border:solid 1px rgb(0, 0, 0);
      font-size: 0.8rem;
      background-color: #5050507a;
      color: #fff;
      letter-spacing: .4px;
      width: 2rem;
  }
}
</style>
