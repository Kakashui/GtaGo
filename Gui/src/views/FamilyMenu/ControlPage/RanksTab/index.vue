<template>
  <div class="ranks-tab">
    <div class="title">{{loc('familyMenu_76')}}</div>
    <div 
      v-if="isLeader"
      class="btn btn-add"
      @click="addNewRank"
    >{{loc('familyMenu_95')}}</div>
    <div class="line"></div>
    <div :class="[{modify: isLeader}, 'ranks-list']">
      <div 
        v-for="item in ranksList"
        :key="item.key"
        class="ranks-list__item"
      >
        <div class="ranks-list__item-name">
          <div class="text">{{item.rankName}}</div>
          <div
            v-if="isLeader"
            class="btn btn-edit"
            @click="goToRank(item)"
          ></div>
        </div>
        <div
          v-if="isLeader && item.rankId !== 0 && item.rankId !== 1"
          class="btn btn-delete"
          @click="deleteRank(item.rankId)"
        >{{loc('familyMenu_74')}}</div>
        <div 
          v-else-if="item.rankId !== 0 && item.rankId !== 1"
          class="btn btn-watch"
          @click="goToRank(item)"
        >{{loc('familyMenu_96')}}</div>
      </div>
    </div>
    <modal-control
      v-if="showModal"
      @closeModal="closeModal"  
      :rank="currentRank"
    />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import ModalControl from './ModalControl'

export default {
  name: 'RanksTab',

  components: {
    ModalControl
  },

  data: function() {
    return {
      showModal: false,
      currentRank: null
    }
  },

  computed: {
    ...mapState('familyMenu/controlPage', ['ranksList']),
    ...mapState('familyMenu', ['isLeader']),
    ...mapGetters('localization', ['loc'])
  },

  methods:{
    setModal: function() {
      this.showModal = !this.showModal
    },
    closeModal: function() {
      this.setModal()
      this.currentRank = null
    },
    goToRank: function(obj) {
      this.currentRank = obj
      this.setModal()
    },
    addNewRank: function() {
      window.mp.trigger('familyMenu:addNewRank')
    },
    deleteRank: function(rank) {
      window.mp.trigger('familyMenu:deleteRank', rank)
    }
  }
}
</script>

<style lang="scss" scoped>
.ranks-tab{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  .title{
    font-weight: bold;
    font-size: 2rem;
  }
  .btn-add{
    background: linear-gradient(90deg, #191C1A 0%, #2D3236 100%);
    border: 1px solid rgba(220, 255, 0, 0.3);
    height: 3.4rem;
    min-height: 3.4rem;
    width: 32.6rem;
    padding: 0 1.6rem;
    justify-content: flex-start;
    color: #DCFF00;
    font-weight: bold;
    font-size: 1.2rem;
    margin-top: 1.6rem;
    &:before{
      content: '';
      width: 1.5rem;
      height: 1.5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/familyMenu/icon-add.svg');
      margin-right: .5rem;
    }
  }
  .line{
    width: 32.6rem;
    height: 1px;
    background: rgba(255, 255, 255, 0.2);
    margin: 1.5rem 0;
  }
  .ranks-list{
    display: flex;
    flex-flow: column;
    padding-right: 1.75rem;
    max-height: calc(100vh - (13.55rem));
    overflow-y: auto;
    overflow-x: hidden;
    padding-bottom: .5rem;
    &.modify{
      max-height: calc(100vh - (18.55rem));
    }
    &::-webkit-scrollbar{
      background: rgba(255, 255, 255, 0.04);
      width: .1rem;
      border-radius: .1rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.4);
        border-radius: .1rem;
      }
    }
    &__item{
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 1rem;
      height: 1rem;
      height: 3.4rem;
      min-height: 3.4rem;
      width: 32.6rem;
      padding: 0 0 0 1.6rem;
      position: relative;
      margin-bottom: .5rem;
      &:last-child{
        margin-bottom: 1rem;
      }
      &>div{
        z-index: 2;
      }
      &:before, &:after{
        content: '';
        width: 100%;
        height: 100%;
        position: absolute;
        left: 0;
        top: 0;
        z-index: 1;
        border: 1px solid #303539;
        transition: .2s;
      }
      &:before{
        background: linear-gradient(90deg, #191C1A 0%, #2D3236 100%);
        opacity: 1;
      }
      &:after{
        background: linear-gradient(90deg, #2D3236 0%, #191C1A 100%);
        opacity: 0;
      }
      &:hover{
        &:before{ 
          opacity: 0;
          transition: .2s;
        }
        &:after{
          opacity: 1;
          transition: .2s;
        }
        .btn-delete, .btn-watch{
          opacity: 1;
          transition: .2s;
        }
      }
      &-name{
        font-weight: bold;
        font-size: 1.2rem;
        display: flex;
        align-items: center;
        .text{
          margin-right: .5rem;
        }
        .btn-edit{
          background-image: url('/img/familyMenu/icon-edit.svg');
          width: 1rem;
          height: 1rem;
        }
      }
      .btn-delete, .btn-watch{
        opacity: 0;
        height: 100%;
        padding: 0 1.6rem;
        transition: .2s;
      }
      .btn-delete{
        color: #FF3838;
      }
      .btn-watch{
        color: #DCFF00;
      }
    }
  }
}
</style>
