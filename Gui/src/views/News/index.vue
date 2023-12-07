<template>
  <div class="news">
    <div class="news__tablet">
      <component 
        class="tablet-display" 
        :is="currentPage"
      />
      <Modal 
        v-if="modal.show"
        :currentModal="modal.currentModal"
      />
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import MainPage from './MainPage'
import ManagementPage from './ManagementPage'
import StatsPage from './StatsPage'
import Modal from './Modal'
export default {
  name: 'News',

  components: {
    MainPage,
    ManagementPage,
    StatsPage,
    Modal
  },
  
  computed: {
    ...mapState('news', ['currentPage', 'modal'])
  }
}
</script>

<style lang="scss" scoped>
.news{
  width: 100%;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  &__tablet{
    display: flex;
    padding: 3rem;
    height: 51.6rem;
    width: 74.4rem; 
    position: relative;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-image: url('/img/news/bg.png');
    .tablet-display{
      width: 100%;
      height: 100%;
      background: #F3F5F7;
      border-radius: 1rem;
      overflow: hidden;
      display: flex;
    }
  }
}
</style>

<style lang="scss">
.news{
  .img-contain-style{
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    transition: .2s;
  }
}
</style>

