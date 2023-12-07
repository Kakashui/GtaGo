<template>
  <div class="current-contract">
    <div class="current-contract__title">{{contract.Name}}</div>
    <div class="current-contract__desc">{{contract.Desc}}</div>
    <div class="current-contract__reward">
      <div class="current-contract__reward-prompt">
        <div class="current-contract__title-small">{{loc('options_program_9')}}</div>
        <div class="current-contract__reward-desc">{{loc('options_program_10')}}</div>
      </div>
      <div class="current-contract__reward-list">
        <div 
          class="current-contract__reward-list__item"
          v-for="(item, index) in contract.Rewards"
          :key="index"
        >
          <div class="current-contract__reward-list__item__decorate">
            <div class="current-contract__reward-list__item__decorate-img"
              :style="{backgroundImage: `url(/img/optionsMenu/programTab/${item.Image}.png)`}"
            ></div>
          </div>
          <div class="current-contract__reward-list__item-desc" >{{loc(item.Desc)}}</div>
          <div v-if="item.Value" class="current-contract__reward-list__item-value">{{item.Value}}</div>
        </div>
      </div>
    </div>
    <template v-if="contractState && contractState.InProgress">
      <div 
        class="current-contract__find"
        v-if="contract.Coords"
      >
        <div class="current-contract__title-small">{{loc('options_program_11')}}</div>
        <div 
          class="current-contract__find-list"
        >
          <div 
            class="current-contract__find-list__item"
            v-for="(coord, index) in contract.Coords"
            :key="index"
            @click="selectCoordinats(coord)"
          >{{coord.Name}}</div>
        </div>
      </div>
      <div class="current-contract__progress">
        <div class="current-contract__progress__title">
          <div class="current-contract__date">{{loc('options_program_12')}} <span> {{contractState.ExpirationDate.toLocaleString("ru", options)}}</span></div>
          <div class="current-contract__progress-value">{{currentProgress}}%</div>
        </div>
        <div class="current-contract__progress-bar">
          <div 
            :class="[{active: item <= ((currentProgress / 100) * 36)}, 'current-contract__progress-bar__item']"
            v-for="(item, index) in 36"
            :key="index"
          ></div>
        </div>      
      </div>
      <div class="current-contract__data-btn" v-if="contractState.ExpirationDate < new Date(Date.now())" @click="acceptContract">
        <div class="current-contract__data-btn-icon">
          <svg xmlns="http://www.w3.org/2000/svg"  viewBox="0 0 20 20" fill="none"> <g> <path d="M19.4142 6.14034L8.86336 16.6904C8.08205 17.4719 6.81466 17.4719 6.03261 16.6904L0.586199 11.2436C-0.1954 10.4622 -0.1954 9.19463 0.586199 8.41303C1.36795 7.63129 2.63524 7.63129 3.41664 8.41274L7.4484 12.4445L16.5833 3.3096C17.3651 2.52785 18.6324 2.52844 19.4139 3.3096C20.1953 4.0912 20.1953 5.3583 19.4142 6.14034Z" fill="black"/> </g> </svg>
        </div>
        <div class="current-contract__data-btn-text">{{loc('options_program_32')}}</div>
      </div>
    </template>
    <div class="current-contract__data" v-else>
      <div class="current-contract__title-small">{{loc('options_program_13')}}</div>
      <div class="current-contract__data-row">
        <div class="current-contract__data-row-desc">{{loc('options_program_23')}} </div>
        <div class="current-contract__data-row-value">{{contract.PriceContract}}</div>
      </div>
      <div class="current-contract__data-row" v-if="this.contract.ContractType === 'Family'">
        <div class="current-contract__data-row-desc">{{loc('options_program_14')}} </div>
        <div class="current-contract__data-row-value">{{contract.MinReputation}}</div>
      </div>
      <div class="current-contract__data-row" v-if="this.contract.ContractType === 'Family'">
        <div class="current-contract__data-row-desc">{{loc('options_program_15')}} </div>
        <div class="current-contract__data-row-value">{{contract.MinMembers}}</div>
      </div>
      <div class="current-contract__data-row">
        <div class="current-contract__data-row-desc">{{loc('options_program_12')}} </div>
        <div class="current-contract__data-row-value">{{contract.MinutesToComplete}} {{loc('options_program_22')}}</div>
      </div>
      <div class="current-contract__data-btn" @click="acceptContract">
        <div class="current-contract__data-btn-icon">
          <svg xmlns="http://www.w3.org/2000/svg"  viewBox="0 0 20 20" fill="none"> <g> <path d="M19.4142 6.14034L8.86336 16.6904C8.08205 17.4719 6.81466 17.4719 6.03261 16.6904L0.586199 11.2436C-0.1954 10.4622 -0.1954 9.19463 0.586199 8.41303C1.36795 7.63129 2.63524 7.63129 3.41664 8.41274L7.4484 12.4445L16.5833 3.3096C17.3651 2.52785 18.6324 2.52844 19.4139 3.3096C20.1953 4.0912 20.1953 5.3583 19.4142 6.14034Z" fill="black"/> </g> </svg>
        </div>
        <div class="current-contract__data-btn-text">{{loc('options_program_16')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'CurrentContract',
  props: {
    contract: Object
  },
  data: function () {
    return {
      options: {
        month: 'long',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
      },
    }
  },
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('optionsMenu', ['myContracts', 'familyContracts']),
    contractState() {
      if (this.contract.ContractType === "Family")
        return this.familyContracts.find(c => c.ContractName == this.contract.ContractName);
      if (this.contract.ContractType === "Single")
        return this.myContracts.find(c => c.ContractName == this.contract.ContractName);
      return null;
    },
    currentProgress() {
      if (!this.contractState) return 0;
      return Math.round(this.contractState.CurrentLevel * 100 / this.contract.MaxLevel);
    }
  },
  methods: {
    selectCoordinats: function (coord) {
      if (!this.contractState) return;
      if (coord.Positions.length == 1)
        window.mp.trigger('personalEvents:selectCoordinats', JSON.stringify(coord.Positions[0]));
      else
      {
        console.log(JSON.stringify(this.contractState.PointsVisited))
        console.log(JSON.stringify(coord.Positions))
        for(let i=0; i<coord.Positions.length; i++)
          if (this.contractState.PointsVisited.findIndex(pt => pt.x == coord.Positions[i].x && pt.y == coord.Positions[i].y ) < 0)
          {
            window.mp.trigger('personalEvents:selectCoordinats', JSON.stringify(coord.Positions[i]));
            return;
          }
      }
      
    },
    acceptContract: function() {
      window.mp.triggerServer('personalEvents:acceptContract', this.contract.ContractName)
    }
  }
}
</script>

<style lang="scss" scoped>
.current-contract{
  padding: 2.75rem 5rem 0 3rem;
  min-width: 30rem;
  background: linear-gradient(0deg, rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7));
  height: 100%;
  z-index: 1;
  overflow-y: auto;
  &::-webkit-scrollbar{
    width: .2rem;
    background-color: transparent;
    &-thumb{
      background: #29FFF2;
      border-radius: .5rem;
    }
  }
  &__title-small{
    margin-bottom: .25rem;
    font-size: 2rem;
    line-height: 2.4rem;
    letter-spacing: 0.03em;
  }
  &__title{
    font-size: 4rem;
    line-height: 4rem;
    color: #FFFFFF;
    margin-bottom: .5rem;
  }
  &__desc{
    width: 100%;
    font-size: .9rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    padding-bottom: 1.5rem;
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    margin-bottom: 1.5rem;
  }
  &__reward{
    width: 100%;
    display: flex;
    justify-content: space-between;
    padding-bottom: 1.7rem;
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    margin-bottom: 2.5rem;
    &-prompt{
      display: flex;
      flex-direction: column;
      margin-right: 1rem;
    }
    &-desc{
      font-size: .9rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.5);
      width: 8.5rem;
    }
    &-list{
      display: flex;
      align-items: flex-start;
      &__item{
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 4.5rem;
        margin-right: .75rem;
        text-align: center;
        &:last-child{
          margin-right: 0;
        }
        &>div{
          z-index: 2;
        }
        &__decorate{
          height: 4.5rem;
          width: 4.5rem;
          display: flex;
          align-items: center;
          justify-content: center;
          border: .1rem solid #FFFFFF;
          border-radius: 50%;
          position: relative;
          margin-bottom: .35rem;
          z-index: 1;
          &>div{
            z-index: 2;
          }
          &:before, &:after{
            content: '';
            border-radius: 50%;
            position: absolute;
            box-sizing: border-box;
          }
          &:before{
            width: 4.5rem;
            height: 4.5rem;
            z-index: 0;
            border: .3rem solid rgba(255, 255, 255, 0.2);
            border-radius: 50%;
          }
          &:after{
            width: 3.5rem;
            height: 3.5rem;
            background: #29FFF2;
            filter: blur(2rem);
            z-index: 1;
          }
          &-img{
            width: 3.5rem;
            height: 3.5rem;
            background-color: rgba(255, 255, 255, 0.2);
            border-radius: 50%;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
          }
        }
        &-desc{
          font-size: .9rem;
          line-height: 1.1rem;
          letter-spacing: 0.03em;
          color: rgba(255, 255, 255, 0.5);
        }
        &-value{
          font-weight: bold;
          font-size: .9rem;
          line-height: 1.05rem;
          text-transform: uppercase;
          text-shadow: 0 .1rem .35rem rgba(0, 0, 0, 0.2);
        }
      }
    }
  }
  &__find{
    display: flex;
    flex-direction: column;
    margin-bottom: 3rem;
    .current-contract__title-small{
      margin-bottom: .5rem;
    }
    &-list{
      display: flex;
      width: 100%;
      &__item{
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(255, 255, 255, 0.2);
        border-radius: 5rem;
        width: 100%;
        height: 3.2rem;
        margin-right: .5rem;
        font-weight: bold;
        font-size: 1rem;
        transition: .3s;
        &:last-child{
          margin-right: 0;
        }
        &:hover{
          background: #B6D300;
          box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
        }
      }
    }
  }
  &__date{
    span{
      color: rgba(182, 211, 0, 1);
    }
  }
  &__progress{
    display: flex;
    flex-direction: column;
    width: 100%;
    &__title{
      width: 100%;
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      margin-bottom: .3rem;
    }
    &-bar{
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: 2rem;
      &__item{
        height: 100%;
        width: .2rem;
        background: rgba(255, 255, 255, 0.5);
        &.active{
          background: rgba(255, 255, 255, 1);
        }
      }
    }
  }
  &__data{
    display: flex;
    flex-direction: column;
    &-row{
      display: flex;
      align-items: center;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      margin-bottom: .6rem;
      &-desc{
        margin-right: .5rem;
      }
      &-value{
        line-height: 1rem;
        color: #B6D300;
      }
    }
    &-btn{
      display: flex;
      padding: .2rem 3.5rem .2rem .2rem;
      align-items: center;
      background: #B6D300;
      width: fit-content;
      margin-top: 2rem;
      border-radius: 5rem;
      transition: .3s;
      &:hover{
        box-shadow: 0px 10px 80px rgba(168, 195, 2, 0.6);
      }
      &-icon{
        width: 2.8rem;
        height: 2.8rem;
        background-color: #fff;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-right: 1.65rem;
        svg{
          width: 1rem;
          height: 1rem;
        }
      }
      &-text{
        font-weight: bold;
        font-size: 1rem;
      }
    }
  }
}
</style>
