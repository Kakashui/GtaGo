<template>
  <div class="business-actions">
    <input
      type="number"
      class="business-actions__input"
      v-model.number="inputModel"
      v-if="editing"
      v-focus
    >
    <button
      class="business-actions__button"
      @click="editOpen"
      v-else
    >{{loc('biz:menu:act:btn')}}</button>
    <button v-show="editing" class="business-actions__agree" @click="propFuncHandler">Ok</button>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'BusinessItemActions',

  props: ['input', 'propFunc'],

  data: function () {
    return {
      editing: false
    }
  },

  computed: {
    inputModel: {
      get () {
        return this.input
      },
      set (value) {
        this.$emit('typing', value)
      }
    },
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    editOpen () {
      this.editing = true
    },

    editClose () {
      this.editing = false
      this.clearInput()
    },

    clearInput () {
      this.$emit('typing', '')
    },

    propFuncHandler () {
      if (this.input.length === 0) {
        this.editClose()
        return
      }
      this.propFunc()
      this.editClose()
      this.clearInput()
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
  width: 14rem;
  margin: 0 0 0 1rem;
  &__input {
    width: 70%;
    border: 1px solid #4e9525;
    font-size: 1.2rem;
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
    padding: .35rem 0;
    width: 70%;
    outline: none;
    border: none;
    font-size: 1.2rem;
    background-color: #4e9525;
    color: #fff;
    letter-spacing: .4px;
  }
  &__agree {
    border: none;
    outline: none;
    background-color: #4e9525;
    color: #fff;
    font-size: 1.2rem;
    text-transform: uppercase;
    padding: 0 1rem;
    letter-spacing: .4px;
  }
}
</style>
