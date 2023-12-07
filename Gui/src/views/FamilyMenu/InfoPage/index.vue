<template>
  <div class="info-page">
    <div class="head">
      <div class="title">
        <div class="title__desc">{{loc('familyMenu_15')}}:</div>
        <div class="title__name">{{organizationName.value}}</div>
      </div>
      <div class="btns-wrap">
        <div 
          class="btn btn-rating"
          @click="showRating"
        >
          <div class="icon"></div>
          <div class="text">{{loc('familyMenu_16')}}</div>
        </div>
        <!-- <div class="btn btn-quests">
          <div class="icon"></div>
          <div class="text">Builds</div>
        </div> -->
        <div 
          class="btn btn-charter"
          @click="setModalRules"
        >
          <div class="icon"></div>
          <div class="text">{{loc('familyMenu_17')}}</div>
        </div>
      </div>
    </div>
    <div class="body">
      <div class="body__item" id="leadersPage">
        <div 
          v-if="leaders.length > 1"
          class="btn btn-watch"
          @click="setModalLeaders"
        ></div>
        <div class="icon"></div>
        <div class="info">
          <div class="title">{{loc('familyMenu_18')}}: {{leaders.length}}</div>
          <div class="desc">{{leaders[0].nickname}}</div>
        </div>
      </div>
      <div class="body__item" id="nacionPage">
        <div 
          v-if="isLeader"
          class="btn btn-options"
          @click="setModalNation"
        ></div>
        <div class="icon"></div>
        <div class="info">
          <div class="title">{{loc('familyMenu_19')}}</div>
          <div class="desc">{{infoPage.nation}}</div>
        </div>
      </div>
      <div class="body__item" id="paymentPage">
        <div class="icon"></div>
        <div class="info">
          <div class="title">{{loc('familyMenu_20')}}:</div>
          <div class="desc">$ {{getIncome}}</div>
        </div>
      </div>
      <div class="body__item" id="membersPage">
        <div class="icon"></div>
        <div class="info">
          <div class="title">{{loc('familyMenu_21')}}</div>
          <div class="desc">{{getMembersCount}} {{loc('familyMenu_22')}}</div>
        </div>
      </div>
      <div class="body__item" id="bioPage">
        <div 
          v-if="isLeader"
          class="btn btn-options"
          @click="setModalBio"
        ></div>
        <div class="img" style="background-image: url(/img/familyMenu/home-img.png)">
          <div 
            class="btn btn-to-house"
            @click="editHouse"
          >{{loc('familyMenu_23')}}</div>
          <div class="desc">{{infoPage.houseType}}</div>
          <div class="adress">{{infoPage.houseAdress}}</div>
        </div>
        <div class="info">
          <div class="desc">{{loc('familyMenu_24')}}</div>
          <div class="bio-text">{{infoPage.biography}}</div>
        </div>
      </div>
      <div class="body__item" id="activityPage">
        <div class="desc">{{loc('familyMenu_25')}}</div>
        <div class="circle-wrap">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 175 175" fill="none">
            <circle 
              cx="87.5" 
              cy="87.5" 
              r="82.5" 
              stroke="white"
              stroke-opacity="0.1" 
              stroke-width="10"/>
            <circle 
              stroke-linecap="round" 
              cx="87.5"
              cy="87.5"
              r="82.5"
              stroke="#D5FF03"
              stroke-width="10"
              :stroke-dasharray="calcCircumference" 
              :stroke-dashoffset="circleValue" 
            />
          </svg>
          <div class="value">{{activeOnline}}%</div>
        </div>
        <div class="online">{{getOnlineMembersCount}} {{loc('familyMenu_26')}}</div>
      </div>
      <div class="body__item" id="ratingPage">
        <div class="value">{{getFamilyRank}}</div>
        <div class="info">
          <div class="title">{{loc('familyMenu_27')}}:</div>
          <div class="desc">{{loc('familyMenu_28')}}</div>
        </div>
      </div>
      <div class="body__item" id="buissPage">
        <div class="value">{{getBusinessCount}}</div>
        <div class="title">{{loc('familyMenu_29')}}</div>
      </div>
      <div class="body__item" id="bankPage">
        <div class="icon"></div>
        <div class="info">
          <div class="title">{{loc('familyMenu_30')}}</div>
          <div class="desc">$ {{infoPage.bankBalance}}</div>
        </div>
        <div 
          class="btn btn-put"
          @click="setModalMoney(1)"
        >
          <div class="icon"></div>
          <div class="text">{{loc('familyMenu_31')}}</div>
        </div>
        <div 
          class="btn btn-take"
          @click="setModalMoney(0)"
        >
          <div class="icon"></div>
          <div class="text">{{loc('familyMenu_32')}}</div>
        </div>
      </div>
    </div>
    <modal-leaders 
      v-if="modalLeaders"
      :leaders="leaders"
      @closeModalLeaders="setModalLeaders"
    />
    <modal-nation 
      v-if="modalNation"
      @closeModalNation="setModalNation"
    />
    <modal-money 
      v-if="modalMoney.show"
      :type="modalMoney.type"
      @closeModalMoney="setModalMoney"
    />
    <modal-rules
      v-if="modalRules"
      @closeRulesModal="setModalRules"
    />
    <modal-bio
      v-if="modalBio"
      @closeBioModal="setModalBio"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'

import ModalLeaders from './ModalLeaders'
import ModalNation from './ModalNation'
import ModalMoney from './ModalMoney'
import ModalRules from './ModalRules'
import ModalBio from './ModalBio'

export default {
  name: 'InfoPage',

  components: {
    ModalLeaders,
    ModalNation,
    ModalMoney,
    ModalRules,
    ModalBio
  },

  data: function() {
    return {
      modalLeaders: false,
      modalNation: false,
      modalMoney: {
        show: false,
        type: null,
      },
      modalRules: false,
      modalBio: false
    }
  },

  computed: {
    ...mapState('familyMenu', ['isLeader', 'infoPage']),
    ...mapState('familyMenu/controlPage', ['organizationName']),
    ...mapState('familyMenu/ratingPage', ['show']),
    ...mapState('familyMenu/membersPage', ['members']),
    ...mapState('familyMenu/propertyPage', ['propertyList']),
    ...mapGetters('localization', ['loc']),
    leaders: function() {
      return this.members.filter(element => element.rank === 0)
    },
    activeOnline: function() {
      let number = (this.getOnlineMembersCount/this.getMembersCount * 100)
      return number.toFixed()
    },
    calcCircumference: function () {
      let number = 2 * Math.PI * 82.5
      return  number
    },
    circleValue: function () {
      const maxCircleValue = this.calcCircumference
      const maxCircleValuePer = ((maxCircleValue /100) / this.getMembersCount * 100) * this.getOnlineMembersCount
      const curCircleVal = maxCircleValue - maxCircleValuePer
      
      return curCircleVal
    },
    getBusinessCount: function () {      
      return this.propertyList.length
    },
    getMembersCount: function () {      
      return this.members.length
    },
    getOnlineMembersCount: function () {      
      return this.members.filter(element => element.online).length
    },
    getIncome: function () {         
      return this.propertyList.reduce((sum, item) => sum + item.income, 0);
    },
    getFamilyRank: function () {         
      return this.$store.getters['familyMenu/ratingPage/getFamilyRank'](this.infoPage.familyId);
    }
  },

  methods: {
    ...mapMutations('familyMenu', ['toggleNav', 'setCurrentPage']),
    setModalLeaders: function() {
      this.modalLeaders = !this.modalLeaders
    },
    setModalNation: function() {
      this.modalNation = !this.modalNation
    },
    setModalMoney: function(type) {
      this.modalMoney.show = !this.modalMoney.show
      this.modalMoney.type = type
    },
    setModalRules: function() {
      this.modalRules = !this.modalRules
    },
    setModalBio: function() {
      this.modalBio = !this.modalBio
    },
    editHouse: function() {
      window.mp.trigger('familyMenu:editHouse')
    },
    showRating: function() {
      this.toggleNav(false);
      this.setCurrentPage('RatingPage')
    }
  }
}
</script>

<style lang="scss" scoped>
.info-page{
  display: flex;
  flex-flow: column;
  width: 100%;
  margin-top: 3rem;
  .head{
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 1rem;
    .title{
      display: flex;
      flex-flow: column;
      &__desc{
        font-family: "Roboto";
        font-style: normal;
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.15rem;
        text-transform: uppercase;
        color: rgba(255, 255, 255, 0.6);
        margin-bottom: .5rem
      }
      &__name{
        font-weight: bold;
        font-size: 2.5rem;
        line-height: 2.5rem;
      }
    }
    .btns-wrap{
      display: flex;
      align-items: center;
      justify-content: flex-end;
      .btn{
        background: radial-gradient(82.95% 1381.5% at 3.97% 16.22%, #333333 0%, #0F0F13 100%);
        border: 1px solid #97908B;
        padding: .8rem 1rem;
        margin-left: 1rem;
        font-family: "Roboto";
        font-style: normal;
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.15rem;
        text-transform: uppercase;
        &:hover{
          box-shadow: 0 0 1rem rgba(255, 255, 255, 0.2);
        }
        &:first-child{
          margin-left: 0;
        }
        .icon{
          width: 1.2rem;
          height: 1.2rem;
          margin-right: .5rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
        &.btn-rating{
          .icon{
            background-image: url('/img/familyMenu/btn-rating.svg');
          }
        }
        &.btn-quests{
          .icon{
            background-image: url('/img/familyMenu/btn-quests.svg');
          }
        }
        &.btn-charter{
          .icon{
            background-image: url('/img/familyMenu/btn-charter.svg');
          }
        }
      }
    }
  }
  .body{
    display: grid;
    height: 34.6rem;
    grid-gap: 1rem;
    grid-template-rows: 6.15rem 1fr 8.3rem;
    grid-template-columns: 20.95rem 16.5rem 13.55rem 1fr;
    &__item{
      background: rgba(25, 28, 30, 0.8);
      padding: 1.5rem;
      display: flex;
      align-items: flex-start;
      text-transform: uppercase;
      position: relative;
      .btn-watch, .btn-options{
        width: 1.2rem;
        height: 1.2rem;
        position: absolute;
        right: .5rem;
        top: .5rem;
        &:hover{
          filter: drop-shadow(0 0 .9rem rgba(255, 255, 255, 0.8));
        }
      }
      .btn-watch{
        background-image: url('/img/familyMenu/btn-watch.svg');
      }
      .btn-options{
        background-image: url('/img/familyMenu/btn-options.svg');
      }
      .icon{
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        width: 2.2rem;
        height: 2.2rem;
        margin-right: 1rem;
      }
      .info{
        display: flex;
        flex-flow: column;
      }
      .title{
        font-family: "Roboto";
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: .95rem;
        color: rgba(255, 255, 255, 0.6);
        margin-bottom: .5rem;
      }
      .desc{
       text-transform: uppercase;
        font-style: normal;
        font-weight: 300;
        font-size: 1.7rem;
        line-height: 2.05rem;
      }
      .value{
        font-size: 3.5rem;
        line-height: 3.5rem;
        text-transform: uppercase;
        color: #D4FF00;
        margin-right: 1rem;
      }
    }
    #leadersPage{
      .icon{
        background-image: url('/img/familyMenu/leadersPage.svg');
      }
    }
    #nacionPage{
      .icon{
        background-image: url('/img/familyMenu/nacionPage.svg');
      }
    }
    #paymentPage{
      .icon{
        background-image: url('/img/familyMenu/paymentPage.svg');
      }
    }
    #membersPage{
      .icon{
        background-image: url('/img/familyMenu/membersPage.svg');
      }
    }
    #bioPage{
      grid-column: 1/4;
      padding: 1.5rem 0 1.5rem 1.5rem;
      .btn-to-house{
        position: absolute;
        left: 1rem;
        top: 1rem;
        padding: 1rem 1.2rem;
        background: #C1E704;
        border-radius: .1rem;
        font-weight: bold;
        font-size: 1.2rem;
        &:hover{
          transform: scale(1.05);
        }
      }
      .img{
        height: 100%;
        width: 24.45rem;
        min-width: 24.45rem;
        padding: 1.5rem 1rem;
        margin-right: 1.5rem;
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        justify-content: flex-end;
        position: relative;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        z-index: 1;
        &>div{
          z-index: 3;
        }
        &:after{ 
          content: '';
          width: 100%;
          height: 100%;
          position: absolute;
          left: 0;
          bottom: 0;
          background: linear-gradient(179.84deg, #171A1D 0.14%, rgba(23, 26, 29, 0) 0.15%, #171A1D 94.1%);
          z-index: 2;
        }
        .desc{
          font-family: "Roboto";
          font-style: normal;
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.15rem;
          text-transform: uppercase;
          color: rgba(255, 255, 255, 0.4);
          margin-bottom: .5rem;
        }
        .adress{
          font-family: "Roboto";
          font-style: normal;
          font-weight: normal;
          font-size: 1.2rem;
          line-height: 1.4rem;
        }
      }
      .desc{
        margin-bottom: 1rem;
      }
      .bio-text{
        font-family: "Roboto";
        font-style: normal;
        font-weight: normal;
        text-transform: initial;
        font-size: .8rem;
        line-height: 1.1rem;
        height: 100%;
        max-height: 12.65rem;
        overflow-y: auto;
        padding-right: 1.5rem;
        &::-webkit-scrollbar{
          width: .2rem;
          background: transparent;
          &-thumb{
            background: rgba(255, 255, 255, 0.1);
            border-radius: .1rem;
          }
        }
      }
    }
    #activityPage{
      flex-flow: column;
      align-items: center;
      justify-content: center;
      .desc{
        margin-bottom: 1rem;
      }
      .circle-wrap{
        width: 8.75rem;
        height: 8.75rem;
        margin-bottom: 1.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
        svg{
          width: 100%;
          height: 100%;
          transform-origin: 50% 50%;
          transform: rotate(-90deg);
        }
        .value{
          position: absolute;
          margin: 0;
        }
      }
      .online{
        font-family: "Roboto";
        font-style: normal;
        font-weight: 500;
        font-size: .8rem;
        line-height: .95rem;
        text-transform: uppercase;
        color: #D4FF00;
      }
    }
    #ratingPage{
      align-items: center;
      justify-content: center;
      .title{
        margin-bottom: .2rem;
      }
    }
    #buissPage{
      align-items: center;
      justify-content: center;
      .title{
        max-width: 5rem;
      }
    }
    #bankPage{
      align-items: center;
      grid-column-start: 3;
      grid-column-end: 5;
      .soon{
        font-size: 3rem;
        line-height: 3rem;
        color: rgba(255, 255, 255, 0.3);
        text-align: center;
        font-weight: 300;
        width: 100%;
      }
      .icon{
        background-image: url('/img/familyMenu/bankPage.svg');
      }
      .info{
        margin-right: 2.75rem;
      }
      .btn{
        display: flex;
        align-items: center;
        margin-right: 1rem;
        &:last-child{
          margin-right: 0;
        }
        &:hover{
          .icon{
            transform: scale(1.05);
            transition: .3s;
          }
        }
        .icon{
          margin-right: .5rem;
          background-color: rgba(255, 255, 255, 0.1);
          border-radius: .1rem;
          transition: .3s;
        }
        .text{
          font-family: "Roboto";
          font-style: normal;
          font-weight: normal;
          font-size: .8rem;
          line-height: .95rem;
          text-transform: uppercase;
          color: #D4FF00;
        }
        &.btn-put{
          .icon{
            background-image: url('/img/familyMenu/btn-put.svg');
          }
          .text{
            max-width: 5rem;
          }
        }
        &.btn-take{
          .icon{
            background-image: url('/img/familyMenu/btn-take.svg');
          }
          .text{
            max-width: 3.8rem;
          }
        }
      }
    }
  }
}
</style>
