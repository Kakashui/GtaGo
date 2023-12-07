<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_121')}}</div>
    <div class="current-id">ID: {{cardId}}</div>
    <div class="current-name">{{name}}</div>
    <div class="status-list">
      <div 
        class="status-list__block"
        v-for="block in socialStatus"
        :key="block.key"
      >
        <div class="page__desc">{{ loc(block.desc) }}</div>
        <div class="text">
          <div 
            class="text__value"
            v-for="value in block.values"
            :key="value.key"
          >{{ loc(value) }}</div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'StatusPage',

  computed: {
    ...mapGetters('localization',['loc']),
    ...mapState('cityHallWeb', ['cardId', 'name', 'socialStatus'])
  },
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    padding: 3rem 12rem 3rem 3rem;
    .page{
      width: 23.8rem;
      height: 100%;
    }
    .current-id{
      font-family: Roboto;
      font-style: normal;
      font-weight: 500;
      font-size: .8rem;
      line-height: .95rem;
      text-align: left;
      color: rgba(0, 0, 0, 0.4);
    }
    .current-name{
      font-family: Roboto;
      font-style: normal;
      font-weight: bold;
      font-size: 1.7rem;
      line-height: 2rem;
      text-transform: uppercase;
      color: rgba(0, 0, 0, 0.7);
      margin-bottom: 1rem;
      text-align: left;
    }
    .status-list{
      display: flex;
      flex-flow: column;
      padding-right: .7rem;
      margin-right: -.7rem;
      overflow-y: auto;
      &::-webkit-scrollbar{
        width: .2rem;
        background-color: transparent;
        &-thumb{
          background: rgba(70, 85, 95, 0.2);
          border-radius: .1rem;
        }
      }
      &__block{
        display: flex;
        align-items: flex-start;
        justify-content: space-between;
        border-bottom: 1px solid rgba(123, 135, 150, 0.2);
        padding-bottom: 1.5rem;
        margin-bottom: 1.2rem;
        &:last-child{
          margin-bottom: 0;
          padding-bottom: 0;
          border-bottom: 0;
        }
        .page__desc{
          margin-right: 2rem;
          margin-bottom: 0;
          &::before{
            top: .3rem;
          }
        }
        .text{
          display: flex;
          flex-flow: column;
          min-width: 14.3rem;
          width: 14.3rem;
          &__value{
            font-family: Roboto;
            font-style: normal;
            font-weight: normal;
            font-size: .8rem;
            line-height: .95rem;
            color: rgba(0, 0, 0, 0.4);
            margin-bottom: .5rem;
            &:last-child{
              margin-bottom: 0;
            }
          }
        }
      }
    }
  }
}
</style>
