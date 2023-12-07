<template>
  <div class="modal-wrap">
    <div class="modal-edit-rank">
      <div 
        class="btn btn-close"
        @click="setModalEditRank"
      ></div>
      <div class="title">{{loc('familyMenu_71')}}</div>
      <div class="ranks-list">
        <div 
          :class="[{active: item.rankId === currentRank}, 'rank']" 
          v-for="item in ranksList" 
          :key="item.key"
          @click="editCurrentRank(item.rankId)"
        >
          <div class="rank__name">{{item.rankName}}</div>
          <div 
            v-if="item.rankId === currentRank"
            class="btn btn-static"
          ></div>
          <div
            v-else
            class="btn btn-select"
            @click="editCurrentRank(item.id)"
          >{{loc('familyMenu_72')}}</div>
        </div>
      </div>
      <div 
        v-if="currentRank !== currentMember.rank"
        class="btns-wrap"
      >
        <div 
          class="btn"
          @click="setCurrentRank"
        >{{loc('familyMenu_35')}}</div>
        <div 
          class="btn btn-cancel"
          @click="setModalEditRank"
        >{{loc('familyMenu_36')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: "ModalEditRank",

  props: {
    currentMember: Object,
    ranksList: Array
  },

  data: function() {
    return {
      currentRank: null
    }
  },

  methods: {
    setModalEditRank: function() {
      this.$emit('setModalEditRank')
    },
    editCurrentRank: function(value) {
      this.currentRank = value
    },
    setCurrentRank: function() {
      window.mp.trigger('familyMenu:setCurrentRank', this.currentMember.id, this.currentRank)
      this.setModalEditRank();
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  mounted(){
    this.currentRank = this.currentMember.rank
  }
}
</script>

<style lang="scss" scoped>
.modal-edit-rank{
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  background-image: url('/img/familyMenu/modal-leave.png');
  border-radius: .5rem;
  display: flex;
  flex-flow: column;
  align-items: center;
  text-align: center;
  padding: 2.5rem;
  position: relative;
  .title{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    margin-bottom: 1.2rem;
    max-width: 12.35rem;
  }
  .ranks-list{
    display: flex;
    flex-flow: column;
    max-height: 19rem;
    overflow-y: auto;
    width: calc(100% + (2.5rem));
    margin-right: -2.5rem;
    padding-right: 2.5rem;
    &::-webkit-scrollbar{
      width: .3rem;
      background-color: transparent;
      &-thumb{
        background: rgba(255, 255, 255, 0.4);
        border-radius: .2rem;
      }
    }
    .rank{
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
      min-width: 25rem;
      margin-bottom: .5rem;
      height: 3.4rem;
      min-height: 3.4rem;
      padding: 0 1.6rem;
      background: linear-gradient(90deg, #191C1A 0%, #2D3236 100%);
      border: 1px solid #555555;
      transition: .2s;
      &:last-child{
        margin-bottom: 0;
      }
      &:hover, &.active{
        border-color: #DCFF00;
        transition: .2s;
      }
      &__name, .btn{
        font-size: 1.2rem;
      }
      .btn{
        color: #DCFF00;
      }
      .btn-static{
        background-image: url('/img/familyMenu/icon-done.svg');
        width: 3.5rem;
        height: 1.2rem;
      }
    }
  }
  .btns-wrap{
    display: flex;
    align-items: center;
    margin-top: 1.5rem;
    .btn{
      padding: 1rem 1.6rem;
      font-weight: bold;
      align-items: flex-end;
      font-size: 1.2rem;
      line-height: 1.2rem;
      border-radius: .1rem;
      background: #C1E704;
      margin-right: 1.2rem;
      &:hover{
        box-shadow: 0 0 1rem #C1E704;
      }
      &:last-child{
        margin-right: 0;
      }
      &:before{
        content: '';
        width: 1.5rem;
        height: 1.5rem;
        margin-right: .5rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        background-image: url('/img/familyMenu/btn.svg');
      }
      &.btn-cancel{
        background: rgba(255, 255, 255, 0.2);
        &:before{
          background-image: url('/img/familyMenu/btn-cancel.svg');
        }
        &:hover{
          box-shadow: 0 0 1rem rgba(255, 255, 255, 0.4);
        }
      }
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
    border-radius: none;
    margin: 0;
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
