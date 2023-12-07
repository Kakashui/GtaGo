<template>
  <div class="vote-item">
    <input 
      type="radio" 
      name="vote" 
      :id="'vote-' + item.value"
      @input="setVote"
      :checked="currentVoteData.currentVote === item.value"
      :disabled="currentVoteData.currentVote !== null && currentVoteData.currentVote >= 0"
    >
    <label :for="'vote-' + item.value">{{loc(item.title)}}</label>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'VoteItem',

  props: {
    item: Object
  },

  methods: {
    setVote: function() {
      this.$emit('setVote', this.item.value)
    }
  },

  computed: {
    ...mapState('parliamentPortal', ['currentVoteData']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.vote-item{
  width: 9rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: relative;
  input{
    opacity: 0;
    position: absolute;
    &+label{
      display: flex;
      align-items: center;
      font-style: normal;
      font-weight: 500;
      font-size: .8rem;
      line-height: .95rem;
      text-transform: uppercase;
      color: #46555F;
      padding-left: 2.2rem;
      &:before, &:after{
        content: '';
        width: 1.5rem;
        height: 1.5rem;
        border-radius: 50%;
        position: absolute;
        left: 0;
        box-sizing: border-box;
      }
      &:before{
        border: 1px solid rgba(123, 135, 150, 0.3);
        z-index: 1;
      }
      &:after{
        background: #3FA8E3;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        background-image: url('/img/parliamentPortal/icon-done.svg');
        opacity: 0;
        z-index: 2;
      }
    }
    &:checked{
      &+label{
        &:after{
          opacity: 1;
        }
      }
    }
  }    
}
</style>
