<template>
  <div class="wanted-list-page">
    <div class="wanted-list-page__nav">
      <div
        :class="[{active: item.key === currentSearch}, {army: type === 2}, {fbi: type === 1}, 'btn']"
        v-for="item in navList"
        :key="item.id"
        @click="setCurrentSearch(item.key)"
      >{{loc(item.text)}}</div>
    </div>
    <template v-if="currentSearch === 'humans'">
      <div class="wanted-list-page__head nav-humans">
        <div
          class="head-item"
          v-for="item in humansNav"
          :key="item.id"
        >{{loc(item)}}</div>
      </div>
      <div class="wanted-list-page__body body-humans scroll">
        <div 
          class="body-item"
          v-for="item in humanListWanted"
          :key="item.key"
        >
          <div>{{item.name}}</div>
          <div>{{item.passport}}</div>
          <div>{{loc(item.sex ? 'Pda_28' : 'Pda_29')}}</div>
          <div>{{item.number}}</div>
          <div class="wanted">
            <div 
              :class="[{active: (index + 1) <= item.wantedLevel }, 'star']"
              v-for="(star, index) in 5"
              :key="star.key"
            ></div>
          </div>
          <div class="licenses">{{item.licenses}}</div>
          <div>
            <div 
              :class="[{army: type === 2}, {fbi: type === 1}, 'btn', 'btn-remove-wanted']"
              @click="editSearchWantedLevel('player', item.passport, item.wantedLevel, item.reason)"
            >{{loc('Pda_4')}}
              <!-- <svg 
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24" 
                fill="none"
                class="icon"
              >
                <path d="M3 17.2501V21.0001H6.75L17.81 9.94006L14.06 6.19006L3 17.2501ZM20.71 7.04006C21.1 6.65006 21.1 6.02006 20.71 5.63006L18.37 3.29006C17.98 2.90006 17.35 2.90006 16.96 3.29006L15.13 5.12006L18.88 8.87006L20.71 7.04006Z"/>
              </svg> -->
            </div>
            <!-- <div 
              :class="[{army: type === 2}, {fbi: type === 1}, 'btn', 'btn-remove-wanted']"
              @click="pushToBase(item.id)"
            >move to Data base</div> -->
          </div> 
        </div>
      </div>
    </template>
    <template v-else>
      <div class="wanted-list-page__head nav-cars">
        <div
          class="head-item"
          v-for="item in carsNav"
          :key="item.id"
        >{{loc(item)}}</div>
      </div>
      <div class="wanted-list-page__body body-cars scroll">
        <div 
          class="body-item"
          v-for="item in carListWanted"
          :key="item.id"
        >
          <div>{{item.name}}</div>
          <div>{{item.number}}</div>
          <div>{{item.carModel}}</div>
          <div class="wanted">
            <div 
              :class="[{active: (index + 1) <= item.wantedLevel }, 'star']"
              v-for="(star, index) in 5"
              :key="star.key"
            ></div>
          </div>
          <div>
            <div 
              :class="[{army: type === 2}, {fbi: type === 1}, 'btn', 'btn-remove-wanted']"
              @click="editSearchWantedLevel('car', item.key, item.wantedLevel, item.reason)"
            >{{loc('Pda_4')}}</div>
          </div>
        </div>
      </div>
    </template>
    <modal-wanted
      v-if="modalShow"
      :typeModal="currentType"
      :id="currentId"
      :wantedLevel="currentWantedLevel"
      :reason="currentReason"
      @closeModal="closeModal"
    />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ModalWanted from '../components/ModalWanted'

export default {
  name: 'WantedListPage',

  components: {
    ModalWanted
  },

  data: function() {
    return {
      currentSearch: null,
      modalShow: false,
      currentType: null,
      currentId: null,
      currentWantedLevel: null,
      currentReason: null,
      navList: [
        {
          key: 'humans',
          text: 'Pda_12'
        },
        {
          key: 'cars',
          text: 'Pda_13'
        }
      ],
      carsNav: [
        'Pda_14',
        'Pda_15',
        'Pda_16',
        'Pda_17'
      ],
      humansNav: [
        'Pda_14',
        'Pda_18',
        'Pda_19',
        'Pda_20',
        'Pda_17',
        'Pda_21'
      ],
    }
  },

  computed: {
    ...mapState('personalDigitalAssistant', ['humanListWanted', 'carListWanted', 'type']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    closeModal: function() {
      this.modalShow = false
    },
    setCurrentSearch: function(key) {
      this.currentSearch = key
    },
    editSearchWantedLevel: function(currentType, currentId, currentWantedLevel, currentReason) {
      this.modalShow = true
      this.currentType = currentType
      this.currentId = currentId
      this.currentWantedLevel = currentWantedLevel
      this.currentReason = currentReason
    },
    pushToBase: function(id) {
      window.mp.trigger('pda:pushToBase', id)
    }
  },

  created(){
    this.setCurrentSearch('humans')
  }
}
</script>

<style lang="scss" scoped>
.wanted-list-page{
  display: flex;
  flex-flow: column;
  width: 100%;
  height: 100%;
  &__nav{
    display: flex;
    align-items: center;
    margin-bottom: 1rem;
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
  &__head{
    display: flex;
    align-items: center;
    width: 100%;
    padding: 0 1rem;
    margin-bottom: .5rem;
    color: rgba(255, 255, 255, 0.7);
  }
  &__head.nav-humans>div, &__body.body-humans .body-item>div{
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
   &__head.nav-cars>div, &__body.body-cars .body-item>div{
    &:nth-child(1){
      width: 9.45rem;
      min-width: 9.45rem;
    }
    &:nth-child(2){
      width: 9.15rem;
      min-width: 9.15rem;
    }
    &:nth-child(3){
      width: 10rem;
      min-width: 10rem;
    }
    &:nth-child(4){
      width: 9rem;
      min-width: 9rem;
    }
  }
  &__body{
    display: flex;
    flex-flow: column;
    width: 100%;
    padding-right: .6rem;
    overflow-y: auto;
    height: 100%;
    max-height: 100%;
    &.body-humans{
      .body-item{
        .btn-remove-wanted{
          min-height: 2.9rem;
          margin-right: .6rem;
          &:last-child{
            margin-right: 0;
          }
          .icon{
            width: 1.2rem;
            height: 1.2rem;
          }
          &.fbi{
            .icon{
              fill: #FB7712;
            }
          }
          &.army{
            .icon{
              fill: #8DA997;
            }
          }
        }
      }
    }
    .body-item{
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
      .btn-remove-wanted{
        border-radius: .4rem;
        padding: .6rem;
        font-size: .8rem;
        max-width: 5.7rem;
        background: #333333;
        color: #FFD130;
        width: fit-content;
        text-align: center;
        &:hover{
          transform: scale(1.1);
        }
        &:active{
          transform: scale(1.05);
        }
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
