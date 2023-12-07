<template>
  <div class="prisoner-item">
    <div class="row">
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
      <div class="licenses">{{loc(item.licenses)}}</div>
    </div>
    <div class="arrest-info">
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:1')}} </div>
        <div class="value">{{item.arrestDate}}</div>
      </div>
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:2')}} </div>
        <div class="value">{{item.detained}}</div>
      </div>
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:3')}} </div>
        <div class="value">{{item.acticle}}</div>
      </div>
    </div>

    <div 
      class="arrest-info options"
      v-if="item.isEdit && !item.releaseDate"
    >
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:4')}} </div>
        <div 
          :class="[{fbi: type === 1}, 'btn btn-ban']"
          @click="overrideBail">{{loc('pda:prison:5')}}</div>
        <div class="input-wrap">
          <input 
            type="number"
            :placeholder="loc('pda:prison:plh')"
            v-model="currentBail"
          >
        </div>
        <div 
          :class="[{fbi: type === 1}, 'btn btn-release']"
          v-if="currentBail >= minBail && currentBail <= maxBail"
          @click="releaseOnBail"
        >{{loc('pda:prison:6')}}</div>
        <div 
          class="btn btn-release off"
          v-else
        >{{loc('pda:prison:6')}}</div>
      </div>
    </div>

    <div 
      class="arrest-info"
      v-if="item.strippedOfLicenses"
    >
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:7')}} </div>
        <div class="value">{{loc(item.strippedOfLicenses)}}</div>
      </div>
    </div>

    <div 
      class="arrest-info release"
      v-if="item.bail && item.bailOfficer"
    >
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:8')}} </div>
        <div class="value">{{item.bail}}</div>
      </div>
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:9')}} </div>
        <div class="value">{{item.bailOfficer}}</div>
      </div>
    </div>

    <div 
      class="arrest-info release"
      v-if="item.releaseDate"
    >
      <div class="arrest-info__block">
        <div class="desc">{{loc('pda:prison:110')}} </div>
        <div class="value">{{item.releaseDate}}</div>
      </div>
    </div>

  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'PrisonerItem',

  props: {
    item: Object
  },

  data: function() {
    return{
      currentBail: null
    }
  },

  methods: {
    releaseOnBail: function() {
      window.mp.trigger('pda:releaseOnBail', this.item.id, this.currentBail)
    },
    overrideBail: function() {
      window.mp.trigger('pda:overrideBail', this.item.id)
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('personalDigitalAssistant/dataBase', ['minBail', 'maxBail']),
    ...mapState('personalDigitalAssistant', ['type']),
  }
}
</script>

<style lang="scss" scoped>
.prisoner-item{
  width: 100%;
  padding: 0 1.5rem;
  background: rgba(255, 255, 255, 0.2);
  border-radius: .4rem;
  font-size: 1rem;
  margin-bottom: 1rem;
  .row{
    display: flex;
    align-items: center;
    justify-content: flex-start;
    position: relative;
    margin-bottom: 1.1rem;
    height: 4.5rem;
    min-height: 4.5rem;
    &:before{
      content: '';
      width: calc(100% + (1rem));
      height: 1px;
      position: absolute;
      left: -.5rem;
      bottom: 0;
      background: rgba(255, 255, 255, 0.2);
      
    }
    &>div{
      display: flex;
      &:nth-child(1){
        width: 15rem;
        min-width: 15rem;
      }
      &:nth-child(2){
        width: 7rem;
        min-width: 7rem;
      }
      &:nth-child(3){
        width: 2rem;
        min-width: 2rem;
        justify-content: center;
        margin-right: 1.9rem;
      }
      &:nth-child(4){
        width: 8.65rem;
        min-width: 8.65rem;
      }
      &:nth-child(5){
        width: 10.5rem;
        min-width: 10.5rem;
      }
      &:nth-child(6){
        width: 100%;
      }
    }
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
    }
  }
  .arrest-info{
    display: flex;
    flex-wrap: wrap;
    &.release{
      .arrest-info__block{
        .desc{
          color: rgba(252, 125, 125, 1);
        }
      }
    }
    &.options{
      .btn, .input-wrap{
        min-height: 2.5rem;
        height: 2.5rem;
        border-radius: .4rem;
      }
      .btn{
        padding: 0 1.2rem;
        border-radius: .4rem;
        font-weight: bold;
        font-size: 1rem;
        color: #000;
        background: #FFD130;
        margin-right: .85rem;
        &.off{
          background: rgba(255, 255, 255, 0.2);
          color: rgba(255, 255, 255, 0.3);
        }
        &.fbi{
          color: #FFFFFF;
          background: #FB7712;
        }
      }
      .input-wrap{
          background: rgba(255, 255, 255, 0.2);
         
          padding-left: 2.2rem;
          position: relative;
          display: flex;
          align-items: center;
          margin-right: .5rem;
          width: 10.9rem;
          margin-left: 1rem;
          input{
            outline: none;
            border: none;
            border-left: 1px solid rgba(255, 255, 255, 0.2);
            height: 1.8rem;
            width: 100%;
            padding-left: .8rem;
            background: transparent; 
            color: #fff;
            &::placeholder{
              color: rgba(255, 255, 255, 0.3);
            }
            &::-webkit-outer-spin-button,
            &::-webkit-inner-spin-button {
              -webkit-appearance: none;
              margin: 0; 
            }
          }
          &:before{
            content: '$';
            position: absolute;
            left: .8rem;
          }
          &:after{
            content: '';
            width: 1px;
            height: 2.8rem;
            position: absolute;
            left: -1rem;
            background: rgba(255, 255, 255, 0.2);
          }
      }
    }
    &__block{
      display: flex;
      align-items: center;
      font-size: 1rem;
      margin-right: 2rem;
      margin-bottom: 1.4rem;
      .desc{
        font-weight: bold;
        text-transform: uppercase;
        color: #FFFFFF;
        margin-right: .5rem;
      }
      .value{
        font-weight: normal;
        color: rgba(255, 255, 255, 0.6);
      }
    }
  }
}
</style>
