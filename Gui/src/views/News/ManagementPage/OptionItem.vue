<template>
  <div 
    :class="[{active: option.value}, 'option-item']"
    @click="setRankOption"
  >{{option.desc}}</div>
</template>

<script>
export default {
  name: 'OptionItem',

  props: {
    option: Object
  },

  methods: {
    setRankOption: function() {
      this.$emit('setRankOption', this.option)
    }
  }
}
</script>

<style lang="scss" scoped>
.option-item{
  font-weight: normal;
  font-size: 1rem;
  line-height: 1.15rem;
  padding-left: 1.5rem;
  color: #000000;
  position: relative;
  display: flex;
  align-items: center;
  &:before, &:after{
    content: '';
    width: 1rem;
    height: 1rem;
    position: absolute;
    left: 0;
    box-sizing: border-box;
    border-radius: .1rem;
  }
  &:before{
    border: .1rem solid #EFA7A7;
  }
  &:after{
    background-color: #D92C2C;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/news/icon-done.svg');
    opacity: 0;
  }
  &.active{ 
    &:after{
      opacity: 1;
    }
  }
}
</style>
