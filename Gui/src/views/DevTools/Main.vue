<template>
    <div class="tools-main" >
        <div class="tools-main-item" v-for="(menu, index) in menus" :key="index" @click="selectMenu(index)">
            {{menu}}
        </div>
    </div>
</template>

<script>
import {mapState, mapMutations} from 'vuex'

export default {
  computed: {
    ...mapState('devTools', ['menus'])
  },
  methods: {
    ...mapMutations('devTools', ['selectMenu'])
  }
}
</script>

<style lang="scss" scoped>
.tools-main{
  &-item{
    text-align: center;
    color: #fff;
    padding: .3rem;
    margin: .6rem auto;
    background-color: rgba(#000, .5);
    border: 1px solid rgba(#000, 0);
    width: 70%;
    &:hover{
      border: 1px solid #ccc;
      cursor: pointer;
    }
  }
}
</style>