<template>
  <div class="choice target-choice">
    <div class="target">{{loc('fam:btl:choice:targ')}}</div>
    <div class="desc">{{loc('fam:btl:choice:biz')}}</div>
    <div class="target-list">
      <div 
        :class="[{active: item.id === currentBuiss}, 'item']"
        @click="setCurrentBuiss(item.id)"
        v-for="item in businessList"
        :key="item.key"
      >
        <div class="item__main">
          <div class="desc">id: {{item.id}}</div>
          <div class="value">{{loc(item.name)}}</div>
        </div>
        <div class="item__owner">
          <div class="desc">{{loc('fam:btl:choice:owner')}}:</div>
          <div class="value">{{item.famOwner}}</div>
        </div>
        <div class="item__income">
          <div class="desc">{{loc('fam:btl:choice:inhour')}}</div>
          <div class="value">${{item.income}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'TargetChoice',
  props: {
    setCurrentBuiss: Function,
    currentBuiss: Number
  },

  computed: {
    ...mapState('familyMenu/eventsPage', ['businessList']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.target-choice{
  display: flex;
  flex-flow: column;
  .target{
    font-size: 2rem;
    line-height: 2.4rem;
    letter-spacing: 0.03em;
    margin-bottom: .1rem;
  }
  .desc{
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: 1.5rem;
  }
  .target-list{
    display: flex;
    flex-flow: column;
    width: calc(100% + (.8rem) + (.4rem));
    margin-left: -.4rem;
    max-height: calc(100vh - (15.7rem));
    overflow-y: auto;
    padding: 0 .8rem .4rem .4rem;
    &::-webkit-scrollbar{
      background-color: transparent;
      width: .2rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.1);
        border-radius: .1rem;
      }
    }
    .item{
      width: 100%;
      margin-bottom: .5rem;
      height: 5.4rem;
      min-height: 5.4rem;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0 2rem;
      background: rgba(15, 15, 16, 0.7);
      &>div{
        z-index: 2;
      }
      &:hover{
        &:after{
          opacity: 1;
          transition: .2s;
        }
      }
      &:before, &:after{
        content: '';
        position: absolute;
        height: 100%;
        opacity: 0;
      }
      &:before{
        width: .2rem;
        left: -.2rem;
        transform: translateX(-100%);
        background: #DCFF01;
      }
      &:after{
        width: 100%;
        top: 0;
        left: 0;
        z-index: 1;
        background-image: url('/img/familyMenu/eventsPage/target-bg.svg');
        background-size: contain;
        background-position: center left;
        background-repeat: no-repeat;
      }
      &.active{
        &:before, &:after{
          opacity: 1;
        }
      }
      .value{
        font-size: .9rem;
        line-height: .9rem;
        font-weight: bold;
      }
      .desc{
        font-size: .9rem;
        line-height: 1.1rem;
        text-transform: uppercase;
        margin-bottom: .35rem;
      }
      &__main, &__owner, &__income{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        justify-content: center;
        position: relative;
      }
      &__main{
        width: 9.7rem;
        min-width: 9.7rem;
        .value{
          font-size: 1.2rem;
          line-height: 1.2rem;
        }
      }
      &__owner, &__income{
        &:before{
          content: '';
          position: absolute;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          width: 1.3rem;
          height: 1.3rem;
          left: -.5rem;
          transform: translateX(-100%);
        }
      }
      &__owner{
        &:before{
          background-image: url('/img/familyMenu/eventsPage/sword.svg');
        }
      }
      &__income{
        &:before{
          background-image: url('/img/familyMenu/eventsPage/money.svg');
        }
      }
    }
  }
}
</style>
