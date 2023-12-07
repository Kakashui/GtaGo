<template>
  <div class="job-tab body-tab">
    <div class="body-tab__title">
      <div class="desc">{{loc('Licenses_4')}}</div>
      <div class="value">{{loc('Licenses_5')}}</div>
    </div>
    <div class="job-tab__licenses">
      <template v-if="currentCategories.length > 0">
        <div 
          class="item" 
          v-for="item in currentCategories"
          :key="item.name"
        >
          <div class="item__info">
            <div class="icon-wrap">
              <div 
                class="icon"
                :style="{backgroundImage: 'url(/img/licenses/'+ item.img +'.svg)'}"
              ></div>
            </div>
            <div class="desc">{{loc('Licenses_10')}}:</div>
            <div class="date">{{item.dateEnd}}</div>
          </div>
          <div 
            class="item__img"
            :style="{backgroundImage: 'url(/img/licenses/'+ item.img +'.png)'}"
          >
            <div :class="[{'small-text': item.name.length > 16}, 'name']">{{loc(item.name)}}</div>
          </div>
        </div>
      </template>
      <div 
        v-else
        class="no-categories"
      >{{loc('Licenses_7')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'Job',

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('licenses', ['licenses']),

    currentCategories: function() {
      let array = this.licenses
      return array.filter(element => element.type === 'job')
    }
  }
}
</script>

<style lang="scss" scoped>
.job-tab{
  &__licenses{
    width: 100%;
    height: 11.65rem;
    overflow-y: auto;
    position: relative;
    &::-webkit-scrollbar{
      width: 0;
      background-color: transparent;
    }
    .item{
      display: flex;
      align-items: center;
      justify-content: space-between;
      background: #FFFFFF;
      border: 1px solid rgba(71, 80, 89, 0.2);
      margin-bottom: .5rem;
      height: 5.7rem;
      &__info{
        height: 100%;
        width: 5.3rem;
        min-width: 5.3rem;
        padding: .7rem .8rem .7rem 1rem;
        display: flex;
        flex-flow: column;
        .icon-wrap{
          width: 100%;
          height: 1.7rem;
          margin-bottom: .6rem;
          background-color: rgba(51, 61, 71, 0.1);
          display: flex;
          align-items: center;
          justify-content: center;
          .icon{
            width: 1.2rem;
            height: 1.2rem;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
          }
        }
        .desc{
          font-weight: 600;
          font-size: .7rem;
          line-height: .8rem;
          color: rgba(51, 61, 71, 0.4);
          margin-bottom: .3rem;
        }
        .date{
          font-weight: 600;
          font-size: .8rem;
          line-height: .9rem;
          color: #333D47;
        }
      }
      &__img{
        width: 100%;
        height: 100%;
        background-size: cover;
        background-repeat: no-repeat;
        background-position: top;
        display: flex;
        align-items: center;
        justify-content: center;
        .name{
          font-weight: 800;
          font-size: 2rem;
          line-height: 2.3rem;
          letter-spacing: 0.1em;
          text-transform: uppercase;
          color: #FFFFFF;
          &.small-text{
            font-size: 1.2rem;
            line-height: 1.35rem;
          }
        }
      }
    }
  }
  .no-categories{
    position: absolute;
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column;
    align-items: center;
    justify-content: center;
    font-weight: 600;
    font-size: .7rem;
    line-height: .8rem;
    text-align: center;
    letter-spacing: 0.04em;
    color: #475059;
    &:before{
      content: '';
      width: 5rem;
      height: 5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      margin-bottom: .5rem;
      background-image: url('/img/licenses/unemployment.svg');
    }
  }
}
</style>
