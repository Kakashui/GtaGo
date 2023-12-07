<template>
  <div class="color-type">
    <span class="color-type__name">
      {{ name }}
    </span>
    <input
      v-model="modelColor"
      class="color-type__value"
    >
  </div>
</template>

<script>
export default {
  props: {
    name: {
      type: String,
      default: ''
    },
    color: {
      type: String,
      default: ''
    }
  },

  computed: {
    modelColor: {
      get: function() {
        return this.color
      },
      set: function(val) {
        this.$emit('inputColor', val)
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.color-type {
  display: flex;
  height: 1.65rem;
  width: 10rem;
  font-size: 0.9rem;
  &__name {
    width: 24%;
    color: #fff;
    background: #5C5554;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
  }
  &__value {
    width: 76%;
    text-align: center;
    border: 0;
    color: rgba(255, 255, 255, 0.65);
    background: rgba(32, 32, 32, 0.95);
  }
}
</style>
