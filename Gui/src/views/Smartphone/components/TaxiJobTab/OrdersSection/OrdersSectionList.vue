<template>
	<div class="orders-section-list">
    <div class="orders-info">
      <div class="title">{{loc('phone:taxi:order:4')}}</div>
      <div class="value">{{taxiJobData.orders.length}}</div>
    </div>
    <OrdersSectionItem
      v-for="item in taxiJobData.orders"
      :key="item.id"
      :item="item"
    />
	</div>
</template>

<script>

import { mapState, mapMutations, mapGetters } from 'vuex'

import OrdersSectionItem from './OrdersSectionItem'

export default {
  name: 'OrdersSectionList',
  
  components: {
    OrdersSectionItem
  },

	computed: {
		...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiJobData']),
    ...mapGetters('localization', ['loc'])
	},

	methods: {
		...mapMutations('smartphone', ['setCurrentTab']),
	}
}
</script>

<style lang="scss" scoped>
@keyframes page-transition {
  from {
    opacity: 0;
    transform: translateY(100%)
  }
  to {
    opacity: 1;
    transform: translateY(0)
  }
}
.orders-section-list{
  display: flex;
  flex-flow: column;
  padding: 0 .5rem .5rem .5rem;
  overflow-y: auto;
  &>div{
    animation: page-transition 0.4s;
  }
  &::-webkit-scrollbar {
    width: 0;
  }
  &::-webkit-scrollbar-track, &::-webkit-scrollbar-thumb, &::-webkit-scrollbar-thumb:hover{
    background: transparent;
  }
  .orders-info{
    display: flex;
    flex-flow: column;
    align-items: center;
    margin-bottom: .9rem; 
    padding-top: 2rem;
    .title{
      font-size: .7rem;
      line-height: .8rem;
      margin-top: .15rem;
      color: rgba(0, 0, 0, 0.4);
    }
    .value{
      font-weight: 300;
      font-size: 1.1rem;
      line-height: 1.3rem;
      color: #000000;
    }
  }
  .btn-close{  
    background: #FFFFFF;
    width: 2.5rem;
    height: 2.5rem;
    border-radius: .5rem;
    position: absolute;
    left: 1rem;
    top: 2rem;
    &:hover{
      box-shadow: 0 .5rem 1.5rem rgba(111, 90, 16, 0.3);
      transition: .3s;
    }
  }
}
</style>