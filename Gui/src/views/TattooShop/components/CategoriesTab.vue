<template>
  <div class="categories-tab">
    <div class="categories-tab__list">
      <categories-block
        v-for="(category, index) in categories"
        :key="index"
        :category="category"
        @onSelectCtegory="categorySelected"
      />
    </div>
    <div class="categories-tab__actions">
      <main-button
        v-for="(button, index) in buttons"
        :key="index"
        :button="button"
      />
    </div>
  </div>
</template>

<script>
import CategoriesBlock from './CategoriesBlock'
import MainButton from './MainButton'

export default {
  name: 'CategoriesTab',

  components: { CategoriesBlock, MainButton },
  data: function() {
    return {
      buttons: [
        { text: 'tattoo_shop_18', class: 'close', event: this.toClose },
        { text: 'tattoo_shop_20', class: 'act', event: this.goFurther },
      ],
      categories: [
        {
          id: 0,
          title: 'tattoo_shop_4',
          key: 'torso',
          items: [
            { ids: [6,7,8,13,14], title: 'tattoo_shop_5', key: 'leftNipple', count: null },
            { ids: [9,10,11,12], title: 'tattoo_shop_6', key: 'leftLowerBack', count: null },
          ]
        },
        {
          id: 1,
          title: 'tattoo_shop_7',
          key: 'head',
          items: [
            { ids: [0,1,2,4,5], title: 'tattoo_shop_9', key: 'frontNeck', count: null},           
            { ids: [3], title: 'tattoo_shop_10', key: 'backNeck', count: null },
          ]
        },
        {
          id: 2,
          title: 'tattoo_shop_11',
          key: 'leftarm',
          items: [
            { ids: [15], title: 'tattoo_shop_13', key: 'wrist', count: null },
            { ids: [16], title: 'tattoo_shop_14', key: 'upElbow', count: null },
            { ids: [17], title: 'tattoo_shop_15', key: 'aboveElbow', count: null }
          ]
        },
        {
          id: 3,
          title: 'tattoo_shop_12',
          key: 'rightarm',
          items: [
            { ids: [18], title: 'tattoo_shop_13', key: 'wrist', count: null },
            { ids: [19], title: 'tattoo_shop_14', key: 'upElbow', count: null },
            { ids: [20], title: 'tattoo_shop_15', key: 'aboveElbow', count: null }
          ]
        },
        {
          id: 4,
          title: 'tattoo_shop_21',
          key: 'leftleg',
          items: [
            { ids: [21], title: 'tattoo_shop_23', key: 'upKnee', count: null },
            { ids: [22], title: 'tattoo_shop_24', key: 'aboveKnee', count: null},
          ]
        },
        {
          id: 5,
          title: 'tattoo_shop_22',
          key: 'rightleg',
          items: [
            { ids: [23], title: 'tattoo_shop_23', key: 'upKnee', count: null },
            { ids: [24], title: 'tattoo_shop_24', key: 'aboveKnee', count: null},
          ]
        }
      ]
    }
  },

  methods: {
    toClose: function() {
      window.mp.trigger('tattoo:close');
    },
    categorySelected(category, tattoos, camera){
      window.mp.trigger("camMoveCamZ", camera.cameraZ);
      window.mp.trigger("camMovePointZ", camera.pointZ);
      window.mp.trigger("camMoveAngleX", camera.angle);
      this.$emit("onSelectCtegory", category, tattoos);
    },
    goFurther: function() {
      this.setCurrentTab('TattoosTab')
    }
  }
}
</script>

<style lang="scss" scoped>
.categories-tab {
  padding: 0.4rem 0 0 0;
  &__list {
    display: flex;
    flex-direction: column;
    height: 36.55rem;
    padding: 0 0.55rem 0 0;
    overflow-y: auto;
    &::-webkit-scrollbar-track {
      background-color: transparent;
    }
    &::-webkit-scrollbar {
      width: 0.2rem;
      background-color: transparent;
    }
    &::-webkit-scrollbar-thumb {
      background: rgba(255, 255, 255, 0.15);
      border-radius: 0.2rem;
    }
  }
  &__actions {
    margin: 1.05rem 0 0 0;
    display: flex;
    width: 15.4rem;
    justify-content: space-between;
  }
}
</style>
