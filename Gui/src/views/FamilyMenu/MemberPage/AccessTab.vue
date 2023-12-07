<template>
  <div class="access-tab">
    <div class="members-list">
      <member-item
        v-for="item in members"
        :key="item.key"
        :item="item"
        :currentMember="currentMember"
        @setCurrentMember="setCurrentMember"
      />
    </div>
    <div class="main-info">
      <div class="main-info__title">
        <div class="nickname">{{currentMember.nickname}}</div>
        <div 
          class="online"
          v-if="currentMember.online"
        >{{loc('familyMenu_26')}}</div>
      </div>
      <div class="main-info__wrap">
        <div class="rank-info">
          <div class="current-rank">
            <div class="current-rank__icon"></div>
            <div class="current-rank__info">
              <div class="desc">{{loc('familyMenu_108')}}:</div>
              <div 
                class="value"
                v-if="currentRank"
              >{{currentRank.rankName}}</div>
            </div>
          </div>
          <template v-if="ranksList.find(element => element.rankId === currentRankId).accessMembers">
            <div 
              class="btn btn-edit"
              @click="setModalEditRank"
            >{{loc('familyMenu_49')}}</div>
            <div 
              class="btn btn-kick"
              @click="setModalLeave"
            >{{loc('familyMenu_50')}}</div>
          </template>
        </div>
        <div 
          v-if="currentRank"
          class="access-info"
        >
          <div class="desc">{{loc('familyMenu_51')}}</div>
          <div class="prompt">{{loc('familyMenu_52')}}</div>
          <div class="access-info__list">
            <div class="item">{{loc('familyMenu_53')}}: <span class="on">{{loc(accessHouseList.list[currentRank.accessHouse])}}</span></div>
            <div class="item">{{loc('familyMenu_54')}}: <span :class="{on: currentRank.accessClothes}">{{switchOn(currentRank.accessClothes)}}</span></div>
            <div class="item">{{loc('familyMenu_55')}}: <span :class="{on: currentRank.accessWar}">{{switchOn(currentRank.accessWar)}}</span></div>
            <div class="item">{{loc('familyMenu_110')}}: <span :class="{on: currentRank.accessMembers}">{{switchOn(currentRank.accessMembers)}}</span></div>
            <div class="item">{{loc('familyMenu_56')}}: <span :class="{on: currentRank.accessFurniture !== 0}">{{loc(accessFurnitureList.list[currentRank.accessFurniture])}}</span></div>
          </div>
          <div class="access-info__cars">
            <div class="access-info__cars-title">
              <div class="desc">{{loc('familyMenu_57')}}</div>
              <div class="desc">{{loc('familyMenu_58')}}</div>
            </div>
            <div class="access-info__cars-list">
              <div 
                class="item"
                v-for="item in currentRank.accessCars"
                :key="item.key"
              >
                <div :class="[{on: item.access !== 0}, 'item__type']">{{loc(accessCarsList.list[item.access])}}</div>
                <div class="item__model">{{item.carName}}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <modal-leave
      v-if="modalLeave"
      @setModalLeave="setModalLeave"
      :currentMember="currentMember"
    />
    <modal-edit-rank
      v-if="modalEditRank"
      @setModalEditRank="setModalEditRank"
      :currentMember="currentMember"
      :ranksList="ranksList"
    />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import MemberItem from './components/MemberItem'
import ModalLeave from './components/ModalLeave'
import ModalEditRank from './components/ModalEditRank'
export default {
  name: 'AccessTab',

  components: {
    MemberItem,
    ModalLeave,
    ModalEditRank
  },

  data: function() {
    return {
      currentMember: {},
      modalLeave: false,
      modalEditRank: false,
    }
  },

  methods: {
    setCurrentMember: function(obj) {
      this.currentMember = obj
    },
    switchOn: function(boolean) {
      if (boolean) {
        return 'on'
      } else {
        return 'off'
      }
    },
    setModalLeave: function() {
      this.modalLeave = !this.modalLeave
    },
    setModalEditRank: function() {
      this.modalEditRank = !this.modalEditRank
    },
  },

  computed: {
    ...mapState('familyMenu', ['isLeader', 'currentRankId']),
    ...mapState('familyMenu/membersPage', ['members']),
    ...mapState('familyMenu/controlPage', [
      'ranksList', 
      'accessHouseList', 
      'accessFurnitureList', 
      'accessCarsList'
    ]),
    ...mapGetters('localization', ['loc']),
    currentRank: function() {
      return this.ranksList.find(element => element.rankId === this.currentMember.rank)
    },
  },

  mounted() {
    this.currentMember = {...this.members[0]}
  }
}
</script>

<style lang="scss" scoped>
.access-tab{
  display: flex;
  align-items: flex-start;
  .members-list{  
    display: flex;
    flex-flow: column;
    width: 16.7rem;
    min-width: 16.7rem;
    margin-right: 4.65rem;
    padding-right: 1rem;
    height: calc(100vh - (10.4rem));
    overflow-y: auto;
    transform: rotateY(180deg);
    &::-webkit-scrollbar{
      background: rgba(255, 255, 255, 0.1);
      border-radius: .1rem;
      width: .1rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.6);
        border-radius: .1rem;
      }
    }
  }
  .main-info{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    &__title{
      position: relative;
      display: flex;
      align-items: flex-end;
      padding-bottom: .1rem;
      margin-bottom: 2.5rem;
      .nickname{
        font-size: 3rem;
        line-height: 3.6rem;  
        margin-right: .55rem;
        font-weight: 500;
      }
      .online{
        font-size: 1.3rem;
        line-height: 90%;
        color: #DCFF00;
        margin-bottom: .65rem;
      }
      &:before{
        content: '';
        width: 5.65rem;
        height: .1rem;
        background: linear-gradient(90deg, #DCFF00 0%, rgba(0, 0, 0, 0) 100%);
        position: absolute;
        bottom: 0;
        left: 0;
      }
    }
    &__wrap{
      display: flex;
      align-items: flex-start;
      .rank-info, .access-info{
        display: flex;
        flex-flow: column;
      }
      .rank-info{
        width: 10.95rem;
        min-width: 10.95rem;
        margin-right: 6.5rem;
        display: flex;
        flex-flow: column;
        .current-rank{
          display: flex;
          align-items: center;
          margin-bottom: 1rem;
          &__icon{
            width: 2.2rem;
            height: 2.2rem;
            margin-right: .65rem;
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
            background-image: url('/img/familyMenu/icon-rank.svg');
          }
          &__info{
            display: flex;
            flex-flow: column;
            .desc{
              font-size: 1rem;
              line-height: 1rem;
              color: rgba(255, 255, 255, 0.6);
              margin-bottom: .2rem;
            }
            .value{
              font-size: 1.5rem;
              line-height: 1.5rem;
            }
          }
        }
        .btn{
          width: 100%;
          height: 2.5rem;
          min-height: 2.5rem;
          border-radius: .1rem;
          font-size: 1rem;
          margin-bottom: .6rem;
        }
        .btn-edit{
          border: 1px solid #DCFF00;
          &:hover{
            color: #DCFF00;
            text-shadow: 0 0 .2rem #DCFF00;
            filter: drop-shadow(0 0 .2rem #DCFF00);
          }
        }
        .btn-kick{
          border: 1px solid rgba(255, 255, 255, 0.2);
          &:hover{
            text-shadow: 0 0 .2rem #FFFFFF;
            filter: drop-shadow(0 0 .2rem #FFFFFF);
          }
        }
      }
      .access-info{
        width: 100%;
        .desc{
          font-size: 2.5rem;
          line-height: 3rem;
          margin-bottom: .4rem;
        }
        .prompt{
          font-size: 1rem;
          line-height: 1.2rem;
          color: rgba(255, 255, 255, 0.4);
          margin-bottom: .9rem;
        }
        &__list{
          display: flex;
          flex-wrap: wrap;
          justify-content: flex-start;
          border-top: 1px dashed rgba(255, 255, 255, 0.2);
          border-bottom: 1px dashed rgba(255, 255, 255, 0.2);
          padding: 1.7rem 0 .2rem 0;
          margin-bottom: 1.75rem;
          .item{
            margin-bottom: 1.5rem;
            margin-right: 1.2rem;
            min-width: 46%;
            span{
              margin-left: .4rem;
              color: rgba(255, 255, 255, 0.6);
              &.on{
                color: #DCFF00;
              }
            }
          }
        }
        &__cars{
          display: flex;
          flex-flow: column;
          &-title{
            display: flex;
            align-items: center;
            margin-bottom: 1rem;
            .desc{
              font-weight: bold;
              font-size: 1rem;
              line-height: 1rem;
              &:first-child{
                min-width: 8.5rem;
              }
            }
          }
          &-list{
            display: flex;
            flex-flow: column;
            overflow-y: auto;
            height: 16.25rem;
            width: 100%;
            padding-right: .7rem;
            margin-left: -.7rem;
            transform: rotateY(180deg);
            &::-webkit-scrollbar{
              background-color: transparent;
              width: .1rem;
              &-thumb{
                background: rgba(255, 255, 255, 0.1);
                border-radius: .1rem;
              }
            }
            .item{
              transform: rotateY(-180deg);
              width: 100%;
              display: flex;
              align-items: center;
              height: 1.15rem;
              min-height: 1.15rem;
              margin-bottom: 1.2rem;
              &__type, &__model{
                color: rgba(255, 255, 255, 0.6);
                font-weight: bold;
                font-size: 1rem;
                line-height: 1rem;
              }
              &__type{
                min-width: 8.5rem;
                &.on{
                  color: #DCFF00;
                }
              }
              &__model{
                font-weight: 600;
              }
            }
          }
        }
      }
    }
  }
}
</style>
