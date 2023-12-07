<template>
  <div class="body-tab vote-menu">
    <div class="vote-menu__title">
      <div class="desc">{{loc('voteMenu_0')}}</div>
      <div class="value">{{loc('voteMenu_1')}}</div>
      <div class="date">
          <div 
            class="date__desc" 
            v-if="!voteDone"
          >{{loc('voteMenu_2')}}:</div>
          <div 
            class="date__desc" 
            v-else
          >{{loc('voteMenu_3')}}:</div>
          <div class="date__value">{{finishDate}}</div>
      </div>
    </div>
    <div class="vote-menu__body">
      <div class="vote-list scroll">
        <div 
          class="vote-list__item"
          v-for="item in partiesList"
          :key="item.key"
        >
          <div class="title">{{loc(item.name)}}</div>
          <div class="desc">{{loc(item.desc)}}</div>
        </div>
      </div>
      <div class="vote-results">
        <template v-if="!voteDone">
          <template v-if="!currentVote">
            <div class="vote-results__title">{{loc('voteMenu_4')}}</div>
            <div class="vote-results__desc">{{loc('voteMenu_5')}}</div> 
          </template>
          <div 
            class="vote-results__title"
            v-else
          >{{loc('voteMenu_6')}}</div>
          <div class="results-list scroll">
            <div 
              class="results-list__item"
              v-for="item in partiesList"
              :key="item.key"
            >
              <div class="radio-wrap">
                <input 
                  type="radio" 
                  :id="item.id"
                  name="vote"
                  :checked="item.id === currentRadio"
                  :disabled="currentVote"
                  @click="selectVote(item.id)"
                >
                <label :for="item.id">{{item.name}}</label>
              </div>
            </div>
          </div>
          <div 
            v-if="!currentVote"
            class="btn"
            @click="sendVote"
          >{{loc('voteMenu_7')}}</div>
          <div 
            v-else
            class="vote-done"
          >{{loc('voteMenu_8')}}</div>

        </template>
        <template v-else>
          <div 
            class="vote-results__title"
          >{{loc('voteMenu_6')}}</div>
          <div class="results-list scroll">
            <div 
              class="results-list__item"
              v-for="item in partiesList"
              :key="item.key"
            >
              <div class="radio-wrap">
                <input 
                  type="radio" 
                  :id="item.id"
                  name="vote"
                  :checked="item.id === currentVote"
                  disabled
                >
                <label :for="item.id">{{item.name}}</label>
              </div>
              <div class="info-wrap">
                <div class="info-wrap__value">{{calcPercent(item.countVote)}}%</div>
                <div class="info-wrap__progress">
                  <div 
                    class="line" 
                    :style="{width: calcPercent(item.countVote) + '%'}"
                  ></div>
                </div>
              </div>
            </div>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'VoteMenu',

  data: function() {
    return{
      currentRadio: null
    }
  },

  methods: {
    selectVote: function(value) {
      this.currentRadio = value
    },
    sendVote: function() {
      window.mp.trigger('voteMenu:sendVote', this.currentRadio)
    },

    calcPercent: function(number) {
      let summ = 100*((number)/(this.summVotes))
      if (number !== 0) {
        return Math.round(summ)
      } else {
        return 0
      }
    }
  },

  computed: {
    ...mapState('cityHallWeb/vote', [
      'partiesList',
      'voteDone',
      'finishDate',
      'currentVote'
    ]),
    ...mapGetters('localization', ['loc']),

    summVotes: function() {
      return this.partiesList.reduce((sum, party) => sum + party.countVote, 0)
    },
  },

  mounted(){
    this.currentRadio = this.currentVote
  }
}
</script>

<style lang="scss" scoped>
.vote-menu{
  display: flex;
  width: 100%;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 1;
  background: #F3F5F7;
  padding: 2rem!important;
  .scroll{
    overflow-y: auto;
    transform: rotateY(180deg);
    padding-right: 1rem;
    margin-left: -1rem;
    &::-webkit-scrollbar{
      width: .2rem;
      &-thumb{
        background: #EDEEEF;
        border-radius: .1rem;
      }
    }
    &>div{
      transform: rotateY(180deg);
    }
  }
  &__title{
    display: flex;
    flex-flow: column;
    font-family: Merriweather;
    font-weight: bold;
    max-width: 20rem;
    margin-right: 2rem;
    .desc{
      font-size: 1rem;
      line-height: 1.25rem;
      color: rgba(70, 85, 95, 0.6);
      margin-bottom: .5rem;
    }
    .value{
      font-size: 2.5rem;
      line-height: 3.15rem;
      text-transform: uppercase;
      color: #46555F;
      margin-bottom: 1rem;
    }
    .date{
      display: flex;
      flex-flow: column;
      position: relative;
      padding-top: 1rem;
      &:before{
        content: '';
        width: 5.4rem;
        height: .1rem;
        position: absolute;
        left: 0;
        top: 0;
        background: rgba(70, 85, 95, 0.2);
        border-radius: .1rem;
      }
      &__desc{
        font-size: 1rem;
        line-height: 1.25rem;
        color: #46555F;
        margin-bottom: .35rem;
        font-family: Merriweather;
        font-weight: normal;
      }
      &__value{
        font-size: 1.2rem;
        line-height: 1.4rem;
        text-transform: uppercase;
        color: #E00B29;
        font-family: Roboto;
        text-transform: initial;
      }
    }
  }
  &__body{
    display: flex;
    flex-flow: initial!important;
    justify-content: space-between;
    width: 100%;
    height: 100%;
    background: #FFFFFF;
    border-radius: 1rem;
    .vote-list{
      width: 27rem;
      min-width: 27rem;
      &__item{
        display: flex;
        flex-flow: column;
        color: #46555F;
        font-family: Roboto;
        padding: 1.5rem 0 1.5rem 1.5rem;
        background: #F3F5F7;
        border-radius: .3rem;
        margin-bottom: .6rem;
        // min-height: 11.35rem;
        .title{
          font-size: .9rem;
          line-height: 1.05rem;
          letter-spacing: 0.04em;
          text-transform: uppercase;
          padding: 0 1.5rem 1.5rem 0;
          margin-bottom: 1rem;
          width: 100%;
          border-bottom: .1rem solid rgba(70, 85, 95, 0.1);
        }
        .desc{
          font-weight: normal;
          font-size: .8rem;
          line-height: 1.2rem;
          // overflow-y: auto;
          padding-right: 1.5rem;
          // &::-webkit-scrollbar{
          //   width: .2rem;
          //   &--thumb{
          //     background-color: #E2E5E8;
          //     border-radius: .2rem;
          //   }
          // }
        }
      }
    }
    .vote-results{
      display: flex;
      flex-flow: column;
      width: 19.85rem;
      min-width: 19.85rem;
      font-family: Roboto;
      &__title{
        font-weight: bold;
        font-size: 1.7rem;
        line-height: 2.15rem;
        text-transform: uppercase;
        color: #46555F;
        margin-bottom: .45rem;
        font-family: Merriweather;
      }
      &__desc{
        font-weight: normal;
        font-size: .8rem;
        line-height: .95rem;
        color: rgba(70, 85, 95, 0.6);
        margin-bottom: 1.2rem;
      }
      .results-list{
        &__item{
          display: flex;
          flex-flow: column;
          width: 100%;
          margin-bottom: 1.5rem;
          .radio-wrap{
            position: relative;
            margin-bottom: .25rem;
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            input{
              position: absolute;
              opacity: 0;
            }
            label{
              width: 100%;
              font-size: .8rem;
              line-height: .95rem;
              text-transform: uppercase;
              color: #46555F;
              min-height: 1.5rem;
              display: flex;
              justify-content: space-between;
              align-items: flex-end;
            }
            input + label{
              &:before, &:after{
                content: '';
                width: 1.5rem;
                height: 1.5rem;
                position: absolute;
                right: 0;
                bottom: 0;
                box-sizing: border-box;
                border-radius: 50%;
              }
              &:before{
                border: .1rem solid rgba(123, 135, 150, 0.3);
                border-radius: 1rem;
                z-index: 1;
              }
              &:after{
                background-size: contain;
                background-repeat: no-repeat;
                background-position: center;
                background-image: url('/img/voteMenu/radio.svg');
                background-color: #E00B29;
                opacity: 0;
                z-index: 2;
              }
            }
            input:checked + label{
              &:after{
                opacity: 1;
              }
            }
          }
          .info-wrap{
            display: flex;
            align-items: center;
            justify-content: space-between;
            &__value{
              font-size: .8rem;
              line-height: .95rem;
              color: #E00B29;
            }
            &__progress{
              width: 89%;
              height: .3rem;
              background: rgba(70, 85, 95, 0.1);
              border-radius: 1rem;
              position: relative;
              .line{
                height: .3rem;
                position: absolute;
                top: 0;
                left: 0;
                background: #E00B29;
                border-radius: 1rem;
              }
            }
          }
        }
      }
      .btn{
        background: #E00B29;
        border-radius: .3rem;
        display: flex;
        align-items: center;
        justify-content: center;
        width: fit-content;
        height: 3.5rem;
        padding: 0 2rem;
        font-weight: bold;
        font-size: 1.1rem;
        line-height: 1.25rem;
        text-transform: uppercase;
        text-align: center;
        text-transform: uppercase;
        color: #FFFFFF;
        margin-top: 1rem;
        transition: .2s;
        &:hover{
          transform: scale(1.1);
          transition: .2s;
        }
      }
      .vote-done{
        font-weight: 500;
        font-size: 1.1rem;
        line-height: 1.3rem;
        color: rgba(70, 85, 95, 0.5);
        margin-top: 1rem;
        position: relative;
        display: flex;
        align-items: center;
        &:before{
          content: '';
          width: 1.3rem;
          height: 1.3rem;
          margin-right: .6rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/voteMenu/icon-done.svg');
        }
      }
    }
  }
}
</style>
