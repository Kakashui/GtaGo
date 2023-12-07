<template>
  <div class="price-menu-item">
    <div class="title">
      <div class="title-text">
          {{ item }}, Price:{{ value }}
      </div>
    <input
      type="number"
      class="business-actions__input"
      v-model.number="input"
      v-if="editing"
      v-focus
    >
    <button
      class="business-actions__button"
      @click="editOpen"
      v-else-if="!deleting"
    >{{loc('biz:menu:act:btn')}}</button>
    <button v-show="editing" class="business-actions__button" @click="propFuncHandler">Ok</button>
    <button v-show="editing" class="business-actions__button" @click="editClose">Cancel</button>
    <button v-show="!editing && !deleting" @click="deleteOpen" class="title-button">X</button>
    <button v-show="deleting" class="business-actions__button" @click="deleteCompleate">Delete</button>
    <button v-show="deleting" class="business-actions__button" @click="deleteClose">Cancel</button>
    </div>
    <div class="price">
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
// import PriceMenuItemActions from './PriceMenuItemActions'

export default {
  name: 'PriceMenuItem',

  // components: { PriceMenuItemActions },

  props: ['item', 'value'],
  data: function () {
    return {
      editing: false,
      deleting: false,
      input: 0,
    }
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    changePrice (price) {
      window.mp.triggerServer('priceMenu:changePrice', this.item, price)
    },
    deleteItem () {
      window.mp.triggerServer('priceMenu:deleteItem', this.item)
    },
    editOpen () {
      this.editing = true
      this.input = this.value
    },
    deleteOpen () {
      this.deleting = true
    },
    deleteClose () {
      this.deleting = false
    },

    deleteCompleate () {
      this.deleteItem();
      this.deleteClose ();
    },

    editClose () {
      this.editing = false
    },

    propFuncHandler () {
      if (this.input.length === 0) {
        this.editClose()
        return
      }
      this.changePrice(this.input)
      this.editClose()
    }
  },
  directives: {
    focus: {
      inserted: function (el) {
        el.focus()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.business-actions {
  display: flex;
  justify-content: space-between;
  margin: 0 0 0 1rem;
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
  &__button {
    border:solid 1px rgb(0, 0, 0);
    padding: .35rem .35rem;
    width: 7rem;
    font-size: 1rem;
    background-color: #4e9525;
    color: #fff;
    letter-spacing: .4px;
    margin: 0 1rem 0 1rem;
  }
}
.price-menu-item {
  display: block;
  background-color: rgba(33, 33, 37, .9);
  margin: 0 0 1rem 0;
  padding: 1rem;
  border: .1rem solid rgba(78, 149, 37, .3);
  border-radius: .2rem;
  .title {
    display: flex;
    align-items: center;
    &-text {
      font-size: 1.5rem;
      color: #fff;
      width: 15rem;
    }
    &-button {
      padding: .35rem .8rem;
      outline: none;
      border:solid 1px rgb(0, 0, 0);
      font-size: 1rem;
      background-color: #4e9525;
      color: #fff;
      letter-spacing: .4px;
    }
  }
}
</style>