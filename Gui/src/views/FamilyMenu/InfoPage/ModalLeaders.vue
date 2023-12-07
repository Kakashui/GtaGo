<template>
  <div class="modal-wrap">
    <div class="modal">
      <div 
        class="btn btn-close"
        @click="closeModal"
      ></div>
        <div class="title">{{loc('familyMenu_38')}}</div>
        <div 
          class="leader-row"
          v-for="item in leaders"
          :key="item.key"
        >
          <div class="leader-row__id">ID: {{item.id}}</div>
          <div class="leader-row__nickname">{{item.nickname}}</div>
        </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ModalLeaders',

  props: {
    leaders: Array
  },

  methods: {
    closeModal: function() {
      this.$emit('closeModalLeaders')
    },
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.modal{
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  background-image: url('/img/familyMenu/modal-leave.png');
  border-radius: .5rem;
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  text-align: center;
  padding: 2rem;
  position: relative;
  width: 17.2rem;
  .title{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    margin-bottom: .5rem;
    margin-bottom: 1rem;
  }
  .leader-row{
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    font-size: 1rem;
    line-height: 1.2rem;
    margin-bottom: .8rem;
    &:last-child{
      margin-bottom: 0;
    }
    &__id{
      color: rgba(255, 255, 255, 0.6);
    }
    &__nickname{
      text-transform: uppercase;
      color: #FFFFFF;
    }
  }
  .btn-close{
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-color: transparent;
    width: 2rem;
    height: 2rem;
    background-image: url('/img/familyMenu/btn-close.svg');
    position: absolute;
    right: 0;
    top: 0;
    transform: translateX(100%);
    &:hover{
      box-shadow: none;
      transform: translateX(100%) scale(1.1);
    }
  }
}
</style>
