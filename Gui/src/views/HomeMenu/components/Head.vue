<template>
  <div class="head">{{titleText}}</div>
</template>

<script>
export default {
  name: 'Head',

  props: {
    titleText: String
  }
}
</script>

<style lang="scss" scoped>
.head{
  font-size: 1.2rem;
  line-height: 1.45rem;
  letter-spacing: 0.65em;
  padding-bottom: .8rem;
  position: relative;
  &:before{
    content: '';
    width: 3.25rem;
    height: .2rem;
    background: #C1E704;
    box-shadow: 0 .2rem 1.1rem rgba(193, 231, 4, 0.65);
    position: absolute;
    bottom: 0;
    left: 0;
  }
}
</style>
