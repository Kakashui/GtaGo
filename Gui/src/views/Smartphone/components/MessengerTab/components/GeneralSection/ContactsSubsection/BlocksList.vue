<template>
  <div :style="styles" class="blocks-list" >
    <list-block
      v-for="(block, index) in currentItems"
      :key="index"
      :block="block"
    />
  </div>
</template>

<script>
import ListBlock from './ListBlock'

export default {
  name: 'BlocksList',

  props: { items: Array, styles: Object },

  components: { ListBlock },

  computed: {
    currentItems: function() {
      return this.formingItems(this.items);
    }
  },

  methods: {  
    formingItems: function(items) {
      const letters = items.map(item => item.Name.charAt(0).toUpperCase())
      const filterLetters = letters.filter((value, index) => letters.indexOf(value) === index)
      const sortFilterLetters = filterLetters.sort((a, b) => a.localeCompare(b))
      const result = sortFilterLetters.map(letter => {
        const contacts = this.items.filter(object => object.Name.charAt(0).toUpperCase() === letter)
        return {
          category: letter,
          contacts
        }
      })

      return result
    }
  }
}
</script>
