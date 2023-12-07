<template>
  <div class="result-block">
    <div class="title">{{loc('fam:btl:res:cond')}}</div>
    <div class="block-target">
      <div class="desc">{{loc('fam:btl:res:targ')}} <span v-if="!currentBuiss">{{loc('fam:btl:res:nosel')}}</span></div>
      <div 
        class="value-buiss"
        v-if="currentBuiss"
      >
        <div class="current-name">{{businessList.find(element => element.id === currentBuiss).name}}</div>
        <div class="current-org">
          <div class="desc">{{loc('fam:btl:res:own')}}</div>
          <div class="value">{{businessList.find(element => element.id === currentBuiss).famOwner}}</div>
        </div>
        <div class="current-income">
          <div class="desc">{{loc('fam:btl:res:inhour')}}</div>
          <div class="value">${{businessList.find(element => element.id === currentBuiss).income}}</div>
        </div>
      </div>
    </div>
    <div class="other-blocks">
      <div class="block-place">
        <div class="desc">{{loc('fam:btl:res:place')}} </div>
        <div 
          class="value current-place"
          v-if="currentPlace"
        >
          {{loc(placesList.find(element => element.id === currentPlace).name)}}
        </div>
        <span
          v-else
        >{{loc('fam:btl:res:nosel')}}</span>
      </div>
      <div class="block-weapon">
        <div class="desc">{{loc('fam:btl:res:weap')}}</div>
        <div 
          class="value current-weapon"
          v-if="currentWeapon"
        >
          {{loc(weaponsList.find(element => element.id === currentWeapon).name)}}
        </div>
        <span
          v-else
        >{{loc('fam:btl:res:nosel')}}</span>
      </div>
    </div>
    <template v-if="currentPlace && currentBuiss && currentWeapon">
      <div class="date-blocks">
        <div class="date-blocks__item">
          <div class="desc">{{loc('fam:btl:res:date')}}</div>
          <input 
            type="date"
            class="date"
            v-model="date"
          >
        </div>
        <div class="date-blocks__item">
          <div class="desc">{{loc('fam:btl:res:time')}}</div>
            <div>
            <input 
              type="number"
              class="time"
              v-model="timeHour"
              @input="onChangeHours"
            >
            :
            <input 
              type="number"
              class="time"
              v-model="timeMinute"
              @input="onChangeMinutes"
            >    
            </div>       
        </div>
      </div>
      <div class="title-text">{{loc('fam:btl:res:comment')}}</div>
      <textarea v-model="text"></textarea>
      <div 
        class="btn btn-push"
        @click="pushRegBattle"
      >{{loc('fam:btl:res:send')}}</div>
    </template>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'ResultBlock',

  props: {
    currentPlace: Number,
    currentBuiss: Number,
    currentWeapon: Number,
    setNotification: Function
  },

  data: function() {
    return{
      date: null,
      time: "13:00",
      timeHour: '00',
      timeMinute: '00',
      text: null
    }
  },

  methods: {
    pushRegBattle: function() {
      if (this.date && this.timeHour != '' && this.timeMinute != '') {
        //this.setNotification()
        window.mp.trigger('familyMenu:pushRegBattle', this.currentPlace, this.currentBuiss, this.currentWeapon, this.date, `${this.timeHour}:${this.timeMinute}`, this.text)
        this.date = null
        this.timeHour = 0;
        this.timeMinute = 0;
        this.text = null
        this.$emit('pushRegBattle')
      }
    },
    onChangeHours: function() {
      if (this.timeHour > 23)
        this.timeHour = 23
      else if (this.timeHour < 0)
        this.timeHour = 0;
      if (this.timeHour.toString().length > 2)
        this.timeHour = this.timeHour * 1;
      if (this.timeHour.toString().length == 1)
        this.timeHour = `0${this.timeHour}`;
    },
    onChangeMinutes: function() {
      if (this.timeMinute > 59)
        this.timeMinute = 59
      else if (this.timeMinute < 0)
        this.timeMinute = 0;
      if (this.timeMinute.toString().length > 2)
        this.timeMinute = this.timeMinute * 1;
      if (this.timeMinute.toString().length == 1)
        this.timeMinute = `0${this.timeMinute}`;
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('familyMenu/eventsPage', ['placesList', 'weaponsList', 'businessList'])
  }
}
</script>

<style lang="scss" scoped>
.result-block{
  display: flex;
  flex-flow: column;
  padding: .5rem 0 0 1.5rem;
  border-left: 1px solid rgba(255, 255, 255, 0.1);
  height: 100%;
  width: 17.85rem;
  min-width: 17.85rem;
  margin-left: 2.35rem;
  .title{
    font-size: 2rem;
    line-height: 2.4rem;
    letter-spacing: 0.03em;
    margin-bottom: 1rem;
  }
  .block-target, .other-blocks .block-place, .other-blocks .block-weapon{
    .desc, span{
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.03em;
    }
    .desc{
      color: rgba(255, 255, 255, 0.5);
    }
  }
  .block-target{
    display: flex;
    flex-flow: column;
    padding-bottom: 1.7rem;
    width: 100%;
    .desc, span{
      span{
        color: #fff;
        margin-left: 1.1rem;
      }
    }
    .value-buiss{
      display: flex;
      justify-content: space-between;
      margin-top: 1rem; 
      position: relative;
      padding-top: 1.7rem;
      .current-name{
        position: absolute;
        white-space: pre;
        left: 0;
        top: 0;
      }
      .current-org, .current-income{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        position: relative;
        padding-left: 1.9rem;
        &:before{
          content: '';
          width: 1.3rem;
          min-width: 1.3rem;
          height: 1.3rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          position: absolute;
          left: 0;
          top: 0;
        }
        .desc{
          font-size: 1rem;
          line-height: 1.2rem;
          text-transform: uppercase;
          color: rgba(255, 255, 255, 0.5);
          margin-bottom: .2rem;
        }
        .value{
          font-weight: bold;
          font-size: 1rem;
          line-height: 1rem;
        }
      }
      .current-org{
        &:before{
          background-image: url('/img/familyMenu/eventsPage/sword.svg');
        }
      }
      .current-income{
        &:before{
          background-image: url('/img/familyMenu/eventsPage/money.svg');
        }
      }
    }
  }
  .other-blocks{
    display: flex;
    width: 100%;
    align-items: flex-start;
    justify-content: space-between;
    border-top: 1px solid rgba(255, 255, 255, 0.2);
    padding: 1.7rem 0;
    .block-place, .block-weapon{
      width: 47%;
      display: flex;
      flex-flow: column;
      .desc{
        margin-bottom: 1.2rem;
      }
      .value{
        display: flex;
        align-items: flex-start;
        font-size: 1.1rem;
        line-height: 1.3rem;
        &:before{
          content: '';
          width: 1.3rem;
          min-width: 1.3rem;
          height: 1.3rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          margin-right: .6rem;
        }
        &.current-place{
          &:before{
            background-image: url('/img/familyMenu/eventsPage/map.svg');
          }
        }
        &.current-weapon{
          &:before{
            background-image: url('/img/familyMenu/eventsPage/gun.svg');
          }
        }
      }
    }
  }
  .date-blocks{
    display: flex;
    align-items: flex-start;
    margin-bottom: 1.5rem;
    &__item{
      display: flex;
      flex-flow: column;
      &:first-child{
        margin-right: 2rem;
      }
      .desc{
        font-size: 1.2rem;
        line-height: 1.45rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.5);
        margin-bottom: .65rem;
      }
      .time{
        width: 1.5rem;
      }
      input{
        background-color: transparent;
        border: none;
        border-bottom: 1px solid #DCFF00;
        padding: 0 .2rem .15rem .2rem;
        font-size: 1.2rem;
        line-height: 1.45rem;
        letter-spacing: 0.03em;
       text-transform: uppercase;
        color: #fff;
        font-weight: 300;
        flex-flow: row;
        &::-webkit-inner-spin-button, &::-webkit-calendar-picker-indicator, &::-webkit-clear-button, &::-webkit-datetime-edit-ampm-field{
          display: none;
          -webkit-appearance: none;
          -moz-appearance: none;
          -o-appearance: none;
          -ms-appearance:none;
          appearance: none;
        }
      }
    }
  }
  .title-text{
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.03em;
    margin-bottom: .9rem;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: #fff;
    &::placeholder{
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.5);
    }
  }
  textarea{
    margin-bottom: 1.4rem;
    width: 100%;
    background-color: transparent;
    border: 1px solid rgba(255, 255, 255, 0.2);
    padding: .6rem .8rem;
    resize: none;
    outline: none;
   text-transform: uppercase;
    max-width: 100%;
    color: #fff;
    &::-webkit-scrollbar{
      width: 0;
      background-color: transparent;
    }
  }
  .btn-push{
    width: 100%;
    height: 2.8rem;
    min-height: 2.8rem;
    background-color: #C1E704;
    font-weight: bold;
    font-size: 1rem;
    letter-spacing: 0.03em;
  }
}
</style>
