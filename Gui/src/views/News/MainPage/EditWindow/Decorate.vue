<template>
  <div class="decorate">
    <div class="decorate-img"></div>
    <div class="decorate-title">{{loc('news:decor:tit')}}</div>
    <div class="decorate-desc">{{loc('news:decor:desc')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  computed: {
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
.decorate{
  width: 100%;
  padding: 1.25rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  font-size: 1.1rem;
  line-height: 1.25rem;
  text-align: center;
  &-img{
    width: 7rem;
    height: 7rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/news/edit-decorate.png');
    margin-bottom: .6rem;
  }
  &-title{
    font-weight: bold;
    color: #D92C2C;
    margin-bottom: .3rem;
  }
  &-desc{
    color: #AFB9C5;
  }
}
</style>
