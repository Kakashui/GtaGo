<template>
	<div class="orders-section-item">
    <div class="orders-section-item__info">
      <div class="desc">{{loc('phone:taxi:order:1')}}</div>
      <div class="value">{{item.Address}}</div>
      <div class="info-add">
        <div>
          <div class="desc">{{loc('phone:taxi:order:2')}}</div>
          <div class="value">{{distance}} m</div>
        </div>
        <div class="btn-apply" @click="acceptOrder">{{loc('phone:taxi:order:3')}}</div>
      </div>
    </div>
	</div>
</template>

<script>

import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'OrdersSectionItem',

  props: {
    item: Object
  },

	computed: {
		...mapState('smartphone', ['currentTab', 'geolocation']),
    ...mapGetters('localization', ['loc']),

    distance: function() {
      const pos1 = this.geolocation.currentPosition;
      const pos2 = this.item.Destination;

      return Math.sqrt(
          Math.pow((pos2.X ?? pos2.x) - (pos1.X ?? pos1.x), 2) +
          Math.pow((pos2.Y ?? pos2.y) - (pos1.Y ?? pos1.y), 2)
        ).toFixed(2);
    }
	},

	methods: {
		...mapMutations('smartphone', ['setCurrentTab']),
		...mapMutations('smartphone/taxiPage', ['taxijob_setCurrentOrder']),
    

    acceptOrder: function() {
      window.mp.triggerServer('phone::taxijob::acceptOrder', this.item.ID);
    }
	}
}
</script>

<style lang="scss" scoped>
.orders-section-item{
  display: flex;
  background: #FFFFFF;
  box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.5);
  border-radius: .7rem;
  padding: 1rem;
  margin-bottom: .35rem;
  &__info{
    padding-left: .95rem;
    position: relative;
    &:before{
      content: '';
      width: .6rem;
      height: .6rem;
      background: #FFFFFF;
      border: .2rem solid #F4C10B;
      box-sizing: border-box;
      position: absolute;
      left: 0;
      top: 0;
      border-radius: 50%;
    }
    .desc{
      font-size: .65rem;
      line-height: .75rem;
      color: rgba(0, 0, 0, 0.4);
      margin-bottom: 5px;
    }
    .value{
      font-size: .65rem;
      line-height: .75rem;
      color: #282828;
    }
    .info-add{
      display: flex;
      justify-content: space-between;
      align-items: center;
      position: relative;
      margin-top: 1.4rem;
      &:before{
        content: '';
        height: 1px;
        width: 100%;
        position: absolute;
        background: #EBEBEB;
        top: 0;
        transform: translateY(-.7rem);
      }
      &>div:nth-child(1){
        display: flex;
        flex-flow: column;
        justify-content: center;
        align-items: flex-start;
        .desc{
          margin-bottom: .15rem;
        }
        .value{
          font-size: .8rem;
          line-height: .95rem;
        }
      }
      .btn-apply{
        border: 1px solid #EBEBEB;
        box-sizing: border-box;
        border-radius: .4rem;
        font-size: .7rem;
        line-height: .8rem;
        width: 6rem;
        height: 2rem;
        min-height: 2rem;
        color: #282828;
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1;
        &:hover{
          color: #fff;
          background: #F1C115;
          box-shadow: 0 .5rem 1.5rem rgba(244, 193, 11, 0.3);
          transition: .3s;
          border-color: transparent;
        }
      }
    }
  }
}
</style>