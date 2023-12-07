<template>
  <div class="current-meeting">
    <div 
      v-if="currentMeeting.isCompleted === 1"
      class="current-meeting__prompt positive"
    >{{loc('parliamentPortal_10')}}</div>
    <div 
      v-else-if="currentMeeting.isCompleted === 2"
      class="current-meeting__prompt negative"
    >{{loc('parliamentPortal_11')}}</div>
    <div 
      v-else
      class="current-meeting__prompt"
    >{{loc('parliamentPortal_12')}}</div>
    <div class="current-meeting__title">{{currentMeeting.topic}}</div>
    <div class="current-meeting__desc">{{currentMeeting.desc}}</div>

    <div 
      class="current-meeting__vote-block"
      v-if="!currentMeeting.isCompleted"
    >
      <div class="title">{{loc('parliamentPortal_13')}}</div>
      <div class="vote-block__values">
        <vote-item 
          v-for="vote in voteList"
          :key="vote.value"
          :item="vote"
          @setVote="setVote"
        />
      </div>
      <div 
        class="vote-block__status"
        v-if="currentVoteData.currentVote !== null && currentVoteData.currentVote >= 0"
      >{{loc('parliamentPortal_14')}}</div>
      <div 
        class="vote-block__btn btn"
        @click="selectVote"
        v-else
      >{{loc('parliamentPortal_15')}}</div>
    </div>

    <template v-if="VotesAre">
      <div class="current-meeting__results-block">
        <div class="results-block__pie">
          <svg 
            id="root"
            xmlns="http://www.w3.org/2000/svg" 
            viewBox="0 0 100 100"
          >
            <path 
              v-for="(d, index) in pieChart" 
              :key="index"
              :d="d"
              :class="'path-' + index"
            ></path>
          </svg>
        </div>
        <div class="results-block__info">
          <div class="title">{{loc('parliamentPortal_16')}}</div>
          <div class="voted-wrap">
            <div class="voted-wrap__label voted-for"></div>
            <div class="voted-wrap__desc">{{loc('parliamentPortal_7')}}</div>
            <div class="voted-wrap__value">{{calcVotes(0)}}%</div>
          </div>
          <div class="voted-wrap">
            <div class="voted-wrap__label"></div>
            <div class="voted-wrap__desc">{{loc('parliamentPortal_8')}}</div>
            <div class="voted-wrap__value">{{calcVotes(1)}}%</div>
          </div>
          <div class="voted-wrap">
            <div class="voted-wrap__label voted-against"></div>
            <div class="voted-wrap__desc">{{loc('parliamentPortal_9')}}</div>
            <div class="voted-wrap__value">{{calcVotes(2)}}%</div>
          </div>
        </div>
      </div>

      <div class="current-meeting__more">
        <div 
          :class="[{collapsed: dropdownFlag}, 'drop-header']"
          @click="dropdownFlag = !dropdownFlag"
        >
          <div class="drop-header__value">{{loc('parliamentPortal_17')}}</div>
          <div class="drop-header__icon"></div>
        </div>
        <transition name="dropdown">
          <div 
            class="drop-body"
            v-if="dropdownFlag"
          >
            <div class="drop-body__head">
              <div class="block-value">
                <div class="block-value__value">{{currentVotes[0]}}</div>
                <div class="block-value__title">"{{loc('parliamentPortal_7')}}"</div>
              </div>
              <div class="block-value">
                <div class="block-value__value">{{currentVotes[2]}}</div>
                <div class="block-value__title">"{{loc('parliamentPortal_9')}}"</div>
              </div>
              <div class="block-value">
                <div class="block-value__value">{{currentVotes[1]}}</div>
                <div class="block-value__title">"{{loc('parliamentPortal_8')}}"</div>
              </div>
            </div>
            <div 
              class="drop-body__parties"
              v-for="parti in currentVoteData.partiesList"
              :key="parti.id"
            >
              <div class="parti-name">{{parti.name}}</div>
              <div class="parti-list">
                <div 
                  class="parti-list__item"
                  v-for="member in parti.members"
                  :key="member.id"
                >
                  <div class="nickname">{{member.nickname}}</div>
                  <div class="vote">{{voteToString(member.vote)}}</div>
                </div>
              </div>
            </div>
          </div>
        </transition>
      </div>
    </template>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import VoteItem from './VoteItem'
export default {
  name: 'CurrentMeeting',

  props: {
    item: Object
  },

  components: {
    VoteItem
  },

  data: function() {
    return {
      votesAngles: [],
      // pathParams: [],
      voteList: [
        {
          title: 'parliamentPortal_7',
          value: 2
        },
        {
          title: 'parliamentPortal_9',
          value: 0
        },
        {
          title: 'parliamentPortal_8',
          value: 1
        }
      ],
      currentVote: null,
      dropdownFlag: false
    }
  },

  computed: {
    ...mapState('parliamentPortal', ['currentMeeting', 'currentVoteData']),
    ...mapGetters('localization', ['loc']),
    currentVotes: function() {
      let arr = [];
      let newArr = [];
      let voteFor = 0;
      let voteNetural = 0;
      let voteAgainst = 0;
      if (this.VotesAre) {
        this.currentVoteData.partiesList.forEach(element => {
          element.members.forEach(element => 
            arr.push(element)
          )
        })
        arr.forEach(element => {
          if (element.vote === 1) {
            voteNetural += 1
            
          } else if (element.vote === 2) {
            voteFor += 1
          } else {
            voteAgainst += 1
          }
        })
        newArr = [voteFor, voteNetural, voteAgainst]
        return newArr
      } else {
        return [0, 0, 0]
      }
    },
    VotesAre: function() {
      let boolean = false
      if (this.currentVoteData.partiesList.length > 0) {
        for (let index = 0; index < this.currentVoteData.partiesList.length; index++) {
          const element = this.currentVoteData.partiesList[index]
          if (element.members.length > 0) {
            boolean = true
          } else {
            boolean = false
          }
        }
      } else { 
        boolean = false
      }
      return boolean
    },

    pieChart: function() {
      this.nullVoteAngles()
      let all = 0;
      let arrAngles = this.votesAngles
      for (const value of this.currentVotes) {
        let n = value
        all = all + n
      }
      for (const value of this.currentVotes) {
        let n = value
        let angle = n/all*360
        arrAngles.push(angle)
      }
      return this.piePathParams(arrAngles)
    },
  },

  methods: {
    polar2Cartesian: function (centerX, centerY, radius, angleInDegrees) {
      const angleInRadians = (angleInDegrees - 90) * Math.PI / 180.0;
      return {
        x: centerX + (radius * Math.cos(angleInRadians)),
        y: centerY + (radius * Math.sin(angleInRadians))
      };
    },

    nullVoteAngles: function() {
      this.votesAngles = []
    },

    arc: function (x, y, radius, startAngle, endAngle) {
      const start = this.polar2Cartesian(x, y, radius, endAngle);
      const end = this.polar2Cartesian(x, y, radius, startAngle);
      const largeArcFlag = endAngle - startAngle <= 180 ? "0" : "1";
      if (startAngle === 0 && endAngle === 360) {
        return [
        'M', (start.x - radius), '50',
        'a 25,25 0 1,1', 2*radius,',0',
        'a 25,25 0 1,1 ', -2*radius,',0'
      ].join(" ")
      } else {
      return [
        "M", start.x, start.y,
        "A", radius, radius, 0, largeArcFlag, 0, end.x, end.y
      ].join(" ");
      }
    },

    piePathParams: function (angles) {
      const centerX = 50, centerY = 50;
      const radius = 40;
      let runningAngle = 0;
      const params = [];
      for (let index = 0; index < angles.length; index++) {
        const a = angles[index];
        const startAngle = runningAngle;
        const endAngle = runningAngle + a;
        const param = this.arc(centerX, centerY, radius, startAngle, endAngle);
        // console.log(`Start: ${startAngle} , end: ${endAngle}`);
        runningAngle = endAngle;
        params.push(param)
      }
      return params;
    },
    
    calcVotes: function(index) {
      let capacityVotes = this.currentVotes[1] + this.currentVotes[0] + this.currentVotes[2]
      return Math.round((this.currentVotes[index]/capacityVotes) * 100)
    },
    
    setVote: function(value) {
      this.currentVote = value
    },

    selectVote: function() {
      if (this.currentVote !== null) {
        window.mp.trigger('parliamentPortal:selectVote', this.currentMeeting.id, this.currentVote)
      }
    },

    voteToString: function(number) {
      if (number === 2) {
        return this.loc('parliamentPortal_7')
      } else if (number === 1) {
        return this.loc('parliamentPortal_8')
      } else {
        return this.loc('parliamentPortal_9')
      }
    }
  },
}
</script>

<style lang="scss" scoped>
.current-meeting{
  padding: 2.4rem 1rem 2.4rem 5.5rem;
  margin-right: 7rem;
  overflow-y: auto;
  &>div{
    min-width: 30rem;
  }
  &::-webkit-scrollbar{
    background: transparent;
    width: .2rem;
    &-thumb{
      background: rgba(70, 85, 95, 0.2);
      border-radius: .1rem;
    }
  }
  &__prompt, &__title{
    font-family: Merriweather;
    font-weight: bold;
  }
  &__prompt{
    font-size: .8rem;
    line-height: 1rem;
    color: rgba(70, 85, 95, 0.3);
    margin-bottom: .6rem;
    display: flex;
    align-items: center;
    &.negative, &.positive{
      &:after{
        content: '';
        width: 1.2rem;
        height: 1.2rem;
        margin-left: .3rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
      }
    }
    &.negative{
      color: #F14B30;
      &:after{
        background-image: url('/img/parliamentPortal/negative.svg');
      }
    }
    &.positive{
      color: #6BC64B;
      &:after{
        background-image: url('/img/parliamentPortal/positive.svg');
      }
    }
  }
  &__title{ 
    font-size: 1.7rem;
    line-height: 2.15rem;
    color: #46555F;
    margin-bottom: .6rem;
  }
  &__desc{
    margin-bottom: 1.5rem;
    max-height: 9rem;
    overflow-y: auto;
    font-weight: normal;
    font-size: .8rem;
    line-height: 130%;
    color: #46555F;
  }
  &__vote-block, &__results-block{
    border: 1px solid rgba(70, 85, 95, 0.2);
    border-radius: .3rem;
    width: 100%;
    display: flex;
    margin-bottom: 1.5rem;
    .title{
      font-family: Merriweather;
      font-style: normal;
      font-weight: bold;
      font-size: 1.1rem;
      line-height: 1.4rem;
      color: #000000;
      margin-bottom: 1rem;
    }
  }
  &__vote-block{
    padding: 1.2rem 1.5rem;
    flex-flow: column;
    align-items: flex-start;
    min-height: 10rem;
    .vote-block{
      &__values{
        display: flex;
        align-items: center;
        margin-bottom: 1rem;
        justify-content: flex-start;
        .vote-item{
          width: fit-content;
          margin-right: 1.5rem;
          &:last-child{
            margin-right: 0;
          }
        }
      }
      &__status{
        display: flex;
        align-items: center;
        font-weight: 500;
        font-size: .8rem;
        line-height: .95rem;
        letter-spacing: 0.04em;
        margin-top: .5rem;;
        color: #3FA8E3;
        &:before{
          content: '';
          width: 1.3rem;
          height: 1.3rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          background-image: url('/img/parliamentPortal/icon-status.svg');
          margin-right: .5rem;
        }
      }
      &__btn{
        padding: .9rem 1.3rem;
        background: linear-gradient(0deg, #3FA8E3, #3FA8E3), #E00B29;
        border-radius: .3rem;
        font-weight: 500;
        font-size: .8rem;
        color: #FFFFFF;
      }
    }
  }
  &__results-block{
    align-items: center;
    padding: 1rem 1.5rem;
    .results-block{
      &__pie{
        width: 7.5rem;
        height: 7.5rem;
        margin-right: 2rem;
        border-radius: 50%;
        svg{
          width: 7.5rem;
          height: 7.5rem;
          path {
            stroke-width: 15;
            fill: none;
            &.path-0 {
              stroke: #3FA8E3;
            }
            &.path-1 {
              stroke: #E2E5E8;
            }
            &.path-2 {
              stroke: #BAE0F5;
            }
          }
        }
      }
      &__info{
        display: flex;
        flex-flow: column;
        width: 9rem;
        .voted-wrap{
          width: 100%;
          display: flex;
          align-items: center;
          justify-content: space-between;
          position: relative;
          text-align: left;
          padding-left: 1.6rem;
          margin-bottom: .7rem;
          &:last-child{
            margin-bottom: 0;
          }
          &__label{
            width: 1rem;
            height: 1rem;
            position: absolute;
            left: 0;
            background-color: #E2E5E8;
            &.voted-for{
              background-color: #3FA8E3;
            }
            &.voted-against{
              background-color: #BAE0F5;
            }
          }
          &__desc, &__value{
            font-size: .7rem;
            line-height: .8rem;
            text-align: center;
            color: #46555F;
          }
          &__desc{
            font-weight: normal;
          }
          &__value{
            font-weight: 500;
          }
        }
      }
    }
  }
  &__more{
    width: 100%;
    overflow: hidden;
    background: #FFFFFF;
    border-radius: .3rem;
    &>div{
      width: 100%;
    }
    .drop-header{
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 1rem 1.1rem;
      z-index: 2;
      background-color: #fff;
      &.collapsed{
        .drop-header__icon{
          transform: rotate(-180deg);
          transition: .3s;
        }
      }
      &__value{
        font-family: Merriweather;
        font-style: normal;
        font-weight: bold;
        font-size: .8rem;
        line-height: 1rem;
        color: #000000;
      }
      &__icon{
        width: 1.2rem;
        height: 1.2rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/parliamentPortal/arr.svg');
        transition: .3s;
        transform-origin: 50% 50%;
      }
    }
    .drop-body{
      z-index: 1;
      &__head{
        background: rgba(70, 85, 95, 0.03);
        border-top: 1px solid rgba(70, 85, 95, 0.1);
        border-bottom: 1px solid rgba(70, 85, 95, 0.1);
        padding: .75rem 5rem;
        display: flex;
        align-items: center;
        justify-content: space-between;
        .block-value{
          display: flex;
          flex-flow: column;
          align-items: center;
          text-align: center;
          color: #3FA8E3;
          font-weight: 500;
          &__value{
            font-size: 1.5rem;
            line-height: 1.75rem;
          }
          &__title{
            font-size: .8rem;
            line-height: .95rem;
            display: flex;
            align-items: center;
            justify-content: center;
            width: 1px;
          }
        }
      }
      &__parties{
        &:last-child{
          .parti-list__item{
            &:last-child{
              border-bottom: 0;
            }
          }
        }
        .parti-name, .parti-list .parti-list__item{
          padding-left: 1.15rem;
          padding-right: 1.15rem;
        }
        .parti-name{
          padding-top: 1rem;
          padding-bottom: .75rem;
        }
        .parti-list{
          &__item{
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding-top: .6rem;
            padding-bottom: .6rem;
            border-bottom: 1px solid rgba(70, 85, 95, 0.08);
            &:first-child{
              border-top: 1px solid rgba(70, 85, 95, 0.08);
            }
            &:nth-child(2n){
              background-color: #fff;
            }
            &:nth-child(2n+1){
              background-color: rgba(123, 135, 150, 0.05);
            }
            .nickname{
              font-weight: normal;
              font-size: .8rem;
              line-height: .95rem;
              color: #46555F;
            }
            .vote{
              font-weight: 500;
              font-size: .8rem;
              line-height: .95rem;
              text-align: right;
              color: #46555F;
            }
          }
        }
      }
    }
  }
  .dropdown-enter-active, .dropdown-leave-active {
    transition: opacity .2s;
  }
  .dropdown-enter, .dropdown-leave-to /* .dropdown-leave-active below version 2.1.8 */ {
    opacity: 0;
  }
}
</style>
