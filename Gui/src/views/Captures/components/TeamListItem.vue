<template>
	<div class="item">
		<div class="nickname online">{{item.nickname}}</div>
		<div class="rang">{{item.rang}}</div>
		<div class="btn" @click="kickMember(item.nickname)"></div>
	</div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  name: 'TeamListItem',

  computed: {
    ...mapState('captures', ['capturing', 'gangList', 'teamList'])
  },

  props: {
		item: Object, index: Number
  },

  methods: {
    kickMember: function(value) {
      window.mp.trigger('capt:kickMember', value);
    }
  }

}
</script>

<style lang="scss" scoped>
	.item{
		display: flex;
		align-items: center;
		justify-content: flex-start;
		height: 1.75rem;
		min-height: 1.75rem;
		margin-bottom: 1rem;
		padding: 0;
		color: #fff;
		font-weight: 500;
		font-size: .7rem;
		width: 100%;
		&:last-child{
			margin-bottom: 0;
		}
		.nickname{
			padding-left: .8rem;
			position: relative;
			display: flex;
			align-items: center;
			width: 44%;
			&:before{
				content: '';
				width: .3rem;
				height: .3rem;
				border-radius: 50%;
				background: #99A99D;
				position: absolute;
				left: 0;
			}
			&.online{
				&:before{
					background: #10CD45;
				}
			}
		}
		.rang{
			display: flex;
			align-items: center;
			justify-content: flex-start;
			width: 50%;
		}
		.btn{
			width: 1.5rem;
			min-width: 1.5rem;
			height: 1.5rem;
			display: flex;
			align-items: center;
			justify-content: center;
			background-image: url('/img/captures/btn-dell.svg');
			background-size: contain;
			background-position: center;
			background-repeat: no-repeat;
			&:hover{
				transform: scale(1.1);
				transition: .2s;
			}
		}
	}
</style>