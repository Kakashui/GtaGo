<template>
  <div class="body-tab community-tab">
    <div class="community-tab__title">{{loc('cityHallWeb_0')}}</div>
    <div class="page">
      <div class="page__founders">
        <div class="desc">{{loc('cityHallWeb_1')}}:</div>
        <div class="owner">{{name}}</div>
        <div class="prompt">{{loc('cityHallWeb_2')}}</div>
        <div class="title-wrap">
          <div  
            class="title-wrap__value"
            v-if="foundersList.length > 0"
          >{{loc('cityHallWeb_3')}}:</div>
          <div 
            class="title-wrap__btn btn"
            @click="addFounder"
          >
            <div 
              class="text"
              v-if='foundersList.length === 0'
            >{{loc('cityHallWeb_4')}}</div>
          </div>
        </div>
        <div class="founders-list scroll">
          <div 
            class="founders-list__item"
            v-for="item in foundersList"
            :key="item.id"
          >
            <input 
              type="text" 
              v-model="item.value"
              :placeholder="loc('chall:commun:6')"
            >
          </div>
        </div>
      </div>
      <div class="prompt-wrap">
        <div class="prompt-wrap__desc">{{loc('cityHallWeb_5')}}</div>
        <div class="prompt-wrap__prompt">{{loc('cityHallWeb_6')}}</div>
      </div>
      <div class="page__info">
        <div class="info-wrap scroll">
          <div class="page__desc">{{loc('cityHallWeb_7')}}</div>
          <menu-drop
            :dropList="list"
            :currentItem="currentCommunity"
            :setCurrentItem="setCurrentItem"
          />
          <div class="page__desc">{{loc('cityHallWeb_8')}}</div>
          <input
            :placeholder="loc('chall:commun:7')"
            v-model="currentName"
          >
          <div class="page__desc">{{loc('cityHallWeb_9')}}</div>
          <input
            :placeholder="loc('chall:commun:7')"
            v-model="currentNation"
          >
          <div class="page__desc">{{loc('cityHallWeb_10')}}</div>
          <textarea
            :placeholder="loc('chall:commun:7')"
            v-model="currentReason"
          ></textarea>
          <div class="page__desc">{{loc('cityHallWeb_11')}}</div>
          <Payment
            :currentPayment="currentPayment"
            :setCurrentPayment="setCurrentPayment"
          />
        </div>
        <div 
          v-if="currentCommunity && currentName && currentNation && currentReason"
          class="page__btn btn"
          @click="sendForm"
        >
          <span>{{loc('cityHallWeb_12')}}</span>
          <span>${{currentTax}}</span>
        </div>
        <div 
          v-else
          class="page__btn btn"
        >
          <span>{{loc('cityHallWeb_12')}}</span>
          <span>${{currentTax}}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import MenuDrop from '../components/MenuDrop'
import Payment from '../components/Payment'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'CommunityTab',

  components: {
    MenuDrop,
    Payment
  },

  data: function() {
    return {
      foundersList: [],
      currentCommunity: null,
      currentName: null,
      currentNation: null,
      currentReason: null,
      currentTax: null,
      currentPayment: 0, //  0 - money, 1 - card
    }
  },
  

  methods: {
    addFounder: function() {
      this.foundersList.push({value: ''})
    },

    setCurrentItem: function(obj) {
      this.currentCommunity = obj
    },

    setCurrentTax: function(value) {
      this.currentTax = value
    },

    setCurrentPayment: function(value) {
      this.currentPayment = value
    },

    sendForm: function() {
      window.mp.trigger(
        'cityHallWeb:sendFormTaxPage', 
        JSON.stringify(this.foundersList.map(item => item.value)),
        this.currentCommunity,
        this.currentName,
        this.currentNation,
        this.currentReason,
        this.currentPayment
      )
      this.foundersList = []
      this.currentCommunity = null
      this.currentName = null
      this.currentNation = null
      this.currentReason = null
    },
  },

  computed: {
    ...mapState('cityHallWeb', ['name', 'cardId']),
    ...mapState('cityHallWeb/community', ['list', 'tax']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentCommunity = this.list[0]
    this.setCurrentTax(this.tax)
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    padding: 0;
    background-color: transparent;
    font-family: Merriweather;
    font-style: normal;
    font-weight: bold;
    font-size: 2.4rem;
    line-height: 3rem;
  }
  .community-tab{
    display: flex;
    flex-flow: column;
    input, textarea{
      width: 100%;  
    }
    .scroll{
      &::-webkit-scrollbar{
        width: .3rem;
        background-color: transparent;
        &-thumb{
          background: rgba(70, 85, 95, 0.1);
          border-radius: .1rem;
        }
      }
    }
    &__title{
      color: #000000;
      text-transform: initial;
      margin-bottom: 1.5rem;
    }
    .page{
      width: 100%;
      height: calc(100% - (4.5rem));
      background: #FFFFFF;
      border-radius: 1rem;
      padding: 1rem 4.25rem 1rem 1rem;
      flex-flow: initial;
      &__founders{
        display: flex;
        flex-flow: column;
        width: 25rem;
        min-width: 25rem;
        margin-right: 2.5rem;
        height: 100%;
        background: #F3F5F7;
        border-radius: .9rem;
        padding: 2.5rem;
        color: #46555F;
        font-family: Roboto;
        font-style: normal;
        .desc{
          font-weight: 500;
          font-size: .9rem;
          line-height: 1.05rem;
          text-transform: uppercase;
          margin-bottom: .6rem;
        }
        .owner{
          font-weight: bold;
          font-size: 2rem;
          line-height: 2.35rem;
          text-transform: uppercase;
          margin-bottom: .25rem;
        }
        .prompt{
          font-weight: normal;
          font-size: .8rem;
          line-height: .95rem;
          padding-right: 4.75rem;
          padding-bottom: 1rem;
          border-bottom: 1px solid rgba(70, 85, 95, 0.2);
          margin-bottom: 1rem;
        }
        .title-wrap{
          display: flex;
          align-items: center;
          justify-content: space-between;
          margin-bottom: 1.2rem;
          &__value{
            font-weight: bold;
            font-size: .9rem;
            line-height: 1.05rem;
            text-transform: uppercase;
          }
          &__btn{
            justify-content: flex-start;
            .text{
              font-weight: 500;
              font-size: .9rem;
              letter-spacing: 0.07em;
            }
            &:hover{
              &:before{
                transform: scale(1.1);
                transition: .2s;
              }
            }
            &:before{
              content: '+';
              text-transform: uppercase;
              font-style: normal;
              font-weight: bold;
              font-size: 1.2rem;
              letter-spacing: 0.03em;
              color: #46555F;
              display: flex;
              align-items: center;
              justify-content: center;
              width: 2rem;
              height: 2rem;
              border: .1rem solid #E00B29;
              border-radius: 50%;
              margin-right: .5rem;
              transition: .2s;
            }
          }
        }
        .founders-list{
          display: flex;
          flex-flow: column;
          width: calc(100% + .5rem);
          margin-right: -.5rem;
          padding-right: .5rem;
          overflow-y: auto;
          &__item{
            width: calc(100% - .5rem);
            height: 3.4rem;
            min-height: 3.4rem;
            margin-bottom: 1.2rem;
            input{
              width: 100%;
            }
          }
        }
      }
      .prompt-wrap{
        &__desc{
          font-family: Merriweather;
          font-style: normal;
          font-weight: bold;
          font-size: 2rem;
          line-height: 2.5rem;
          color: #46555F;
          margin-bottom: .5rem;
        }
        &__prompt{
          font-family: Roboto;
          font-style: normal;
          font-weight: normal;
          font-size: .8rem;
          line-height: .95rem;
          color: #46555F;
        }
        margin-right: 3rem;
      }
      &__info{
        width: 20rem;
        min-width: 20rem;
        .info-wrap{
          width: calc(100% + .5rem);
          margin-right: -.5rem;
          padding-right: .5rem;
          overflow-y: auto;
          height: calc(100% - (4.7rem));
          margin-bottom: 1.2rem;
          .payment-methods{
            display: flex;
            &__item{
              display: flex;
              align-items: center;
              padding: .3rem .2rem;
              border-bottom: .1rem solid transparent;
              margin-right: 3.2rem;
              &.active{
                border-color: #E00B29;
                svg{
                  fill: #E00B29;
                }
                .text{
                  color: #E00B29;
                }
              }
              &:last-child{
                margin-right: 0;
              }
              svg{
                fill: rgba(70, 85, 95, 0.6);
                width: 1.2rem;
                height: 1.2rem;
                margin-right: .6rem;
              }
              .text{
                color: rgba(70, 85, 95, 0.6);
                font-family: Roboto;
                font-style: normal;
                font-weight: bold;
                font-size: .8rem;
                text-transform: uppercase;
              }
            }
          }
        }
        .page__btn{
          span{
            &:nth-child(1){
              margin-right: .6rem;
              padding-right: .65rem;
              position: relative;
              display: flex;
              align-items: center;
              &:after{
                content: '';
                width: 1px;
                height: 1.5rem;
                background-color:  rgba(255, 255, 255, 0.3);
                position: absolute;
                right: 0;
              }
            }
          }
        }
      }
    }
  }
}
</style>
