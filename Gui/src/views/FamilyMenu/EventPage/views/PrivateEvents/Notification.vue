<template>
  <div class="modal-wrap">
    <div class="modal">
      <div class="text">{{loc(result)}}</div>
      <div class="desc">{{loc(message)}}</div>
      <div 
        class="btn btn-ok"
        @click="setNotification"
      >{{loc('fam:menu:priv:close')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'Notification',

  props: {
    message: String,
    result: String,
    setNotification: Function
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  }
}
</script>

<style lang="scss" scoped>
.modal-wrap{
  .modal{
    display: flex;
    flex-flow: column;
    padding: 2.5rem;
    width: 19.2rem;
    background: #0A0C11;
    .text{
      font-weight: normal;
      font-size: 2rem;
      line-height: 2.4rem;
      text-align: center;
      text-transform: uppercase;
      color: #FFFFFF;
      margin-bottom: .5rem;
    }
    .desc{
      font-weight: 300;
      font-size: 1rem;
      line-height: 1.2rem;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: 1.2rem;
    }
    .btn-ok{
      width: 100%;
      border: 1px solid #DCFF00;
      height: 2.4rem;
      min-height: 2.4rem;
      font-size: .9rem;
      line-height: 1.1rem;
      text-transform: uppercase;
      color: #DCFF00;
      &:hover{
        text-shadow: 0 0 0.2rem #dcff00;
        filter: drop-shadow(0 0 0.2rem #DCFF00);
      }
    }
  }
}
</style>
