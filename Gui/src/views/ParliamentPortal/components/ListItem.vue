<template>
  <div class="list-item">
    <div class="list-item__info">
      <div class="row">
        <div class="info-block time">
          <div class="icon icon-date"></div>
          <div class="info-block__title">{{loc('parliamentPortal_3')}}</div>
          <div class="info-block__value">{{item.date}}</div>
        </div>
        <div class="info-block">
          <div class="icon icon-spicker"></div>
          <div class="info-block__title">{{loc('parliamentPortal_4')}}</div>
          <div class="info-block__value">{{item.spicker}}</div>
        </div>
      </div>
      <div class="info-block">
        <div class="icon icon-topic"></div>
        <div class="info-block__title">{{loc('parliamentPortal_5')}}</div>
        <div class="info-block__value">{{item.topic}}</div>
      </div>
      <div 
        v-if="currentMeeting && item.id === currentMeeting.id"
        class="active btn btn-select"
      >
        <div class="btn-select__text">{{loc('parliamentPortal_6')}}</div>
        <svg 
          xmlns="http://www.w3.org/2000/svg"  
          class="btn-select__icon" 
          viewBox="0 0 24 24" 
          fill="none"
        >
          <path 
            d="M8.625 18.75L15.375 12L8.625 5.25" 
            stroke-width="2" 
            stroke-linecap="round" 
            stroke-linejoin="round"
          />
        </svg>
      </div>
      <div 
        v-else
        class="btn btn-select"
        @click="selectMeeting(item)"
      >
        <div class="btn-select__text">{{loc('parliamentPortal_6')}}</div>
        <svg 
          xmlns="http://www.w3.org/2000/svg"  
          class="btn-select__icon" 
          viewBox="0 0 24 24" 
          fill="none"
        >
          <path 
            d="M8.625 18.75L15.375 12L8.625 5.25" 
            stroke-width="2" 
            stroke-linecap="round" 
            stroke-linejoin="round"
          />
        </svg>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'ListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('parliamentPortal', ['currentMeeting']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('parliamentPortal', ['setCurrentMeeting']),
    selectMeeting: function(obj) {
      window.mp.trigger('parliamentPortal:selectMeeting', obj.id)
      this.setCurrentMeeting(obj)
    }
  }
}
</script>

<style lang="scss" scoped>
.list-item{
  display: flex;
  flex-flow: column;
  background: #FFFFFF;
  border-radius: .3rem;
  padding: 1.2rem .2rem .6rem .6rem;
  margin-bottom: .5rem;
  &__info{
    padding: 0 .5rem;
    margin-bottom: 1.2rem;
    display: flex;
    flex-flow: column;
    .row{
      width: 100%;
      display: flex;
      justify-content: flex-start;
      margin-bottom: 1.2rem;
      .info-block{
        margin-bottom: 0;
      }
    }
    .info-block{
      display: flex;
      flex-flow: column;
      padding-left: 1.8rem;
      position: relative;
      margin-bottom: 1.2rem;
      &.time{
        width: 8.85rem;
        margin-right: 1.6rem;
      }
      .icon{
        width: 1.2rem;
        height: 1.2rem;
        position: absolute;
        left: 0;
        top: 0;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        &.icon-date{
          background-image: url('/img/parliamentPortal/icon-date.svg');
        }
        &.icon-spicker{
          background-image: url('/img/parliamentPortal/icon-spicker.svg');
        }
        &.icon-topic{
          background-image: url('/img/parliamentPortal/icon-topic.svg');
        }
      }
      &__title{
        font-style: normal;
        font-weight: bold;
        font-size: .8rem;
        line-height: 130%;
        color: #46555F;
        margin-bottom: .3rem;
      }
      &__value{
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: .95rem;
        color: rgba(70, 85, 95, 0.6);
      }
    }
  }
  .btn-select{
    width: 100%;
    height: 2.4rem;
    border: 1px solid #3FA8E3;
    border-radius: .3rem;
    padding: 0 1.1rem;
    background-color: transparent;
    justify-content: space-between;
    &__text{
      font-style: normal;
      font-weight: 500;
      font-size: .8rem;
      color: #3FA8E3;
    }
    &__icon{
      width: 1.2rem;
      height: 1.2rem;
      path{
        stroke: #3FA8E3;
      }
    }
    &:hover, &.active{
      background-color: #3FA8E3;
      .btn-select__text{
        color: #fff;
      }
      .btn-select__icon{
        path{
          stroke: #fff;
        }
      }
    }
  }
}
</style>
