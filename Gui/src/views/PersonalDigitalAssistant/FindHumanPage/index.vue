<template>
  <div class="find-human-page">
    <div class="title">
      <div class="text">{{loc('Pda_2')}}</div>
      <div 
        :class="[{active: item.key === currentSearch}, {army: type === 2}, {fbi: type === 1}, 'btn']"
        v-for="item in searchFilters"
        :key="item.id"
        @click="setCurrentSearch(item.key)"
      >{{loc(item.text)}}</div>
    </div>
    <div class="search-wrap">
      <input 
        type="text"
        v-model="currentSearchText"
        :placeholder="loc('Pda_32')"
        v-if="currentSearch === 'passport'"
      >
      <input 
        type="text"
        v-model="currentSearchText"
        :placeholder="loc('Pda_33')"
        v-else-if="currentSearch === 'nickname'"
      >
      <input 
        type="text"
        v-model="currentSearchText"
        :placeholder="loc('Pda_34')"
        v-else
      >
      <div 
        class="btn btn-discard"
        @click="discardCurrentSearchText"
        v-if="currentSearchText"
      ></div>
      <div 
        class="btn btn-find"
        @click="setSearchHuman"
      >{{loc('Pda_6')}}</div>
    </div>
    <template v-if="searchHuman.name">
      <div class="desc">{{loc('Pda_7')}}:</div>
      <div class="search-result">
        <div
          class="search-result__head"
        >
          <div class="text"
            v-for="item in headNav"
            :key="item.id"
          >{{loc(item)}}</div>
        </div>
        <div class="search-result__body">
          <div>{{searchHuman.name}}</div>
          <div>{{searchHuman.passport}}</div>
          <div>{{loc(searchHuman.sex ? 'Pda_28' : 'Pda_29')}}</div>
          <div>{{searchHuman.number}}</div>
          <div class="wanted">
            <div 
              :class="[{active: (index + 1) <= searchHuman.wantedLevel }, 'star']"
              v-for="(item, index) in 5"
              :key="item.key"
            ></div>
          </div>
          <div class="licenses">{{searchHuman.licenses}}</div>
          <div>
            <div 
              :class="[{army: type === 2}, {fbi: type === 1}, 'btn', 'btn-remove-wanted']"
              @click="editSearchHumanWantedLevel"
              v-if="searchHuman.wantedLevel > 0"
            >{{loc('Pda_9')}}</div>
            <div 
              :class="[{army: type === 2}, {fbi: type === 1}, 'btn', 'btn-wanted']"
              @click="editSearchHumanWantedLevel"
              v-else
            >{{loc('Pda_10')}}</div>
          </div>
        </div>
      </div>
    </template>
    <modal-wanted
      v-if="modalShow"
      :typeModal="'player'"
      :id="this.searchHuman.passport"
      :wantedLevel="this.searchHuman.wantedLevel"
      @closeModal="closeModal"
    />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import ModalWanted from '../components/ModalWanted'

export default {
  name: 'FindHumanPage',

  components: {
    ModalWanted
  },

  data: function() {
    return {
      currentSearch: 'number',
      currentSearchText: null,
      searchFilters: [
        {
          text: 'Pda_24',
          key: 'passport'
        },
        {
          text: 'Pda_25',
          key: 'nickname'
        },
        {
          text: 'Pda_26',
          key: 'number'
        }
      ],
      headNav: [
        'Pda_14',
        'Pda_18',
        'Pda_19',
        'Pda_20',
        'Pda_17',
        'Pda_21'
      ],
      modalShow: false
    }
  },

  computed: {
    ...mapState('personalDigitalAssistant', ['searchHuman', 'type']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    closeModal: function() {
      this.modalShow = false
    },
    setCurrentSearch: function(key) {
      this.currentSearch = key
      this.currentSearchText = null
    },
    discardCurrentSearchText: function() {
      this.currentSearchText = null
    },
    setSearchHuman: function() {
      window.mp.trigger('pda:setSearchHuman', this.currentSearchText, this.currentSearch)
    },
    editSearchHumanWantedLevel: function() {
      this.modalShow = true
    }
  }
}
</script>

<style lang="scss" scoped>
.find-human-page{
  height: 100%;
  width: 100%;
  display: flex;
  flex-flow: column;
  justify-content: flex-start;
  .title{
    display: flex;
    align-items: center;
    justify-content: flex-start;
    margin-bottom: 1.5rem;
    min-height: 2rem;
    .text{
      font-weight: bold;
      font-size: 1.4rem;
      line-height: 1.6rem;
      text-transform: uppercase;
      margin-right: 1.5rem;
    }
    .btn{
      margin-right: 1rem;
      padding: .5rem .8rem;
      font-size: .8rem;
      font-weight: bold;
      color: #616162;
      background: #F2F3F5;
      border-radius: 1rem;
      width: fit-content;
      &.active{
        background: #FFD130;
        color: #000000;
      }
      &.fbi{
        &.active{
          background: #FB7712;
          color: #fff;
        }
      }
      &.army{
        &.active{
          color: #fff;
          background: #6C8073;
        }
      }
    }
  }
  .search-wrap{
    width: 100%;
    height: 3rem;
    min-height: 3rem;
    margin-bottom: 1.5rem;
    display: flex;
    align-items: center;
    position: relative;
    input{
      height: 100%;
      width: 100%;
      text-align: left;
      padding: 0 3rem 0 1rem;
      border-left: .1rem solid #F2F3F5;
      border-top: .1rem solid #F2F3F5;
      border-bottom: .1rem solid #F2F3F5;
      box-sizing: border-box;
      border-radius: .1rem 0 0 .1rem;
      outline: none;
      background: transparent;
      color: rgba(255, 255, 255, 1);
      &::placeholder{
        color: rgba(255, 255, 255, 0.6);
      }
    }
    .btn-find{
      font-weight: bold;
      font-size: 1rem;
      line-height: 1.15rem;
      padding: .9rem;
      height: 100%;
      white-space: pre;
      background-size: cover;
      background-position: center right;
      background-repeat: no-repeat;
      background-image: url('/img/personalDigitalAssistant/btn-find.svg');
      color: #000000;
      transform: translateX(-4%);
      &:hover{
        transform: translateX(-1%);
      }
    }
    .btn-discard{
      width: 1.7rem;
      height: 1.7rem;
      position: absolute;
      right: 10.7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/personalDigitalAssistant/btn-discard.svg');
      &:hover{
        transform: scale(1.1);
      }
    }
  }
  .desc{
    font-weight: bold;
    font-size: 1.2rem;
    line-height: 1.4rem;
    margin-bottom: 1.5rem;
    color: rgba(255, 255, 255, 0.6);
  }
  .search-result{
    width: 100%;
    display: flex;
    flex-flow: column;
    font-size: 1rem;
    line-height: 1.15rem;
    color: rgba(255, 255, 255, 0.7);
    &__head{
      display: flex;
      align-items: center;
      width: 100%;
      padding: 0 1rem;
      margin-bottom: .5rem;
    }
    &__head>div, &__body>div{
      &:nth-child(1){
        width: 8.85rem;
        min-width: 8.85rem;
      }
      &:nth-child(2){
        width: 6.7rem;
        min-width: 6.7rem;
      }
      &:nth-child(3){
        width: 3.45rem;
        min-width: 3.45rem;
      }
      &:nth-child(4){
        width: 8.35rem;
        min-width: 8.35rem;
      }
      &:nth-child(5){
        width: 10.2rem;
        min-width: 10.2rem;
      }
      &:nth-child(6){
        width: 6rem;
        min-width: 6rem;
      }
    }
    &__body{
      width: 100%;
      padding: 0 1rem;
      display: flex;
      align-items: center;
      justify-content: flex-start;
      background: rgba(255, 255, 255, 0.2);
      border-radius: .4rem;
      font-size: 1rem;
      height: 4.5rem;
      min-height: 4.5rem;
      margin-bottom: 1rem;
      .wanted, .licenses{
        display: flex;
        align-items: center;
      }
      .wanted{
        .star{
          margin-right: .5rem;
          width: 1.2rem;
          height: 1.2rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/personalDigitalAssistant/wanted-star.svg');
          &.active{
            background-image: url('/img/personalDigitalAssistant/wanted-star-active.svg');
          }
        }
      }
      &>div{
        display: flex;
        align-items: center;
        &:last-child{
          width: 100%;
          justify-content: flex-end;
        }
      }
      .btn-wanted, .btn-remove-wanted{
        border-radius: .4rem;
        padding: .6rem;
        font-size: 1rem;
        width: fit-content;
        white-space: pre;
        &:hover{
          transform: scale(1.1);
        }
        &:active{
          transform: scale(1.05);
        }
      }
      .btn-wanted{
        background: #FFD130;
        color: #000;
        font-weight: bold;
        &.fbi{
          background: #FB7712;
          color: #fff;
        }
        &.army{
          background: #6C8073;
          color: #fff;
        }
      }
      .btn-remove-wanted{
        background: #333333;
        color: #FFD130;
        &.fbi{
          color: #FB7712;
        }
        &.army{
          color: #8DA997;
        }
      }
    }
  }
}
</style>
