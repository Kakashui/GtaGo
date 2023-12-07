<template>
	<div class="item">
		<div class="nickname">{{item.nickname}}</div>
		<div class="rang">{{item.rang}}</div>
		<div 
		class="btn"
		v-if="item.status === false" 
		@click="inviteMember(item.nickname)"
		>Invite</div>
		<div 
		class="preloader"
		v-if="item.status === true" 
		>
			<div class="icon"></div>
		</div>
	</div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  name: 'GroupListItem',

  computed: {
    ...mapState('captures', ['capturing', 'gangList', 'teamList'])
	},
	
	props: {
		item: Object,
		index: Number
  },

  methods: {
    inviteMember: function(value) {
      window.mp.trigger('capt:inviteMember', value);
    }
  }

}
</script>

<style lang="scss" scoped>
	.item{
		display: flex;
		align-items: center;
		justify-content: space-between;
		height: 3.25rem;
		min-height: 3.25rem;
		padding: 0 .9rem 0 2rem;
		color: #fff;
		font-weight: 500;
		font-size: .7rem;
		&:nth-child(2n+1){
			background: rgba(255, 255, 255, 0.1);
			border-radius: 0 .25rem .25rem 0;
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
				background: #10CD45;
				position: absolute;
				left: 0;
			}
		}
		.rang{
			display: flex;
			align-items: center;
			justify-content: flex-start;
			width: 37%;
		}
		.btn, .preloader{
			width: 4.8rem;
			min-width: 4.8rem;
			height: 1.8rem;
			display: flex;
			align-items: center;
			justify-content: center;
		}
		.btn{
			background: #10CD45;
			border-radius: .1rem;
			&:hover{
				filter: drop-shadow(0 .2rem .2rem rgba(0, 0, 0, 0.25));
				transition: .3s;
			}
		}
		.icon{
			width: 1.3rem;
			height: 1.3rem;
			transform-origin: 50% 50%;
			background-image: url('/img/captures/codicon_loading.svg');
			background-size: contain;
			background-repeat: no-repeat;
			background-position: center;
			animation: rotating 1s linear infinite;
		}
	}
	.protect{
		.item{
			&:nth-child(2n+1){
				background: linear-gradient(90deg, rgba(255, 255, 255, 0.1) 0%, rgba(255, 255, 255, 0) 100%);
				border-radius: 0;
			}
			.btn{
				background: rgba(255, 255, 255, 0.2);
			}
		}
	}
	@keyframes rotating{
		from{
			transform: rotate(0deg);
		}
		to{
			transform: rotate(360deg);
		}
	}
</style>