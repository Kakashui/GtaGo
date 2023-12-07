<template>
	<div class="secondary-section">
		<div class="btn-back" @click="setCurrentTab({ key: currentTab.key, section: 'GeneralSection'})"></div>
		<div class="block-options">
			<div class="animate-car">
				<div class="car"></div>
			</div>
			<div class="header">
				<div class="title" v-if="taxiData.specifyLocation.type === 'known'">{{loc('phone:taxi:order:5')}}</div>
				<div class="title" v-if="taxiData.specifyLocation.type === 'unknown'">{{loc('phone:taxi:order:6')}}</div>
				<div class="price">{{ price }}</div>
			</div>
			<div class="block-options__params">
				<div class="block-options__location">
					<div class="icon"></div>
					<div class="location-point">
						<div class="descr">{{loc('phone:taxi:order:7')}}</div>
						<div class="value">{{ geolocation.zoneName }} - {{ geolocation.streetName }}</div>
					</div>
					<div class="location-point">
						<div class="descr">{{loc('phone:taxi:order:8')}}</div>
						<div class="value" v-if="taxiData.specifyLocation.type === 'known'">{{ taxiData.specifyLocation.name }}</div>
						<div class="value btn" v-if="taxiData.specifyLocation.type === 'unknown'" @click="sendToMap">{{loc('phone:taxi:order:9')}}</div>
					</div>
				</div>
				<div 
				:class="[{ active: this.paymentByCard !== true }, 'block-options__payment']"
				@click="setPayment"
				>
					<div class="payment-option">{{loc('phone:taxi:order:10')}}</div>
					<div class="payment-option">{{loc('phone:taxi:order:11')}}</div>
				</div>
				<div class="btn-apply" @click="toOrder">{{loc('phone:taxi:order:12')}}</div>
			</div>
		</div>
	</div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
	name: 'SecondarySection',

	data: function() {
		return{ 
			paymentByCard: true
		}
	},

	computed: {
		...mapState('smartphone', ['currentTab', 'geolocation']),
		...mapState('smartphone/taxiPage', ['taxiData']),
		...mapGetters('localization', ['loc']),

		price: function() {
			if (this.taxiData.specifyLocation.type === 'unknown')
				return this.taxiData.pricePerKm;

			const pos1 = this.geolocation.currentPosition;
			const pos2 = this.taxiData.specifyLocation.position;

			return Math.round(Math.sqrt(
				Math.pow((pos2.X ?? pos2.x) - (pos1.X ?? pos1.x), 2) +
				Math.pow((pos2.Y ?? pos2.y) - (pos1.Y ?? pos1.y), 2)
			) / 1000 * this.taxiData.pricePerKm);
		}
	},

	methods: {
		...mapMutations('smartphone', ['setCurrentTab']),
		toOrder: function() {
			if (this.taxiData.specifyLocation.type == 'unknown')
				return;
			
			window.mp.trigger('phone::taxi::requestTaxi', this.paymentByCard);
			// window.setData('smartphone/setCurrentTab', { key: this.currentTab.key, section: 'SearchSection' })
		},
		setPayment: function() {
			this.paymentByCard = !this.paymentByCard
		},
		sendToMap: () => {
			window.mp.trigger('phone::taxi::sendToMap');
		}
	}
}
</script>

<style lang="scss" scoped>
@keyframes slide-to-left {
  from {
		transform: translateX(50%);
    opacity: 0;
  }
  to {
		transform: translateX(0);
    opacity: 1;
  }
}
@keyframes slide-in {
  from {
    transform: translateY(50%);
		opacity: 0;
  }
  to {
    transform: translateY(0);
		opacity: 1;
  }
}
.secondary-section{
		height: 100%;
		background-image: url('/img/hud/smartphone/taxiTab/general-back.png');
		background-size: contain;
		padding: 0 .5rem 1.5rem .5rem;
		display: flex;
		flex-flow: column;
		justify-content: flex-end;
		.btn-back{
			width: 2.5rem;
			height: 2.5rem;
			position: absolute;
			top: 2.5rem;
			left: .75rem;
			background:  url('/img/hud/smartphone/taxiTab/btn-back.svg'), linear-gradient(68.11deg, #0F0F15 0%, #4A4A55 100%);
			background-size: contain;
			background-repeat: no-repeat;
			background-position: center;
			border-radius: .5rem;
			animation: slide-to-left 0.4s;
		}
		.block-options{
				background: linear-gradient(68.11deg, #0F0F15 0%, #4A4A55 100%);
				border-radius: .75rem;
				width: 100%;
				position: relative;
				animation: slide-in 0.4s;
				.animate-car{
					position: absolute;
					top: 0;
					left: 0;
					width: 100%;
					height: 5.7rem;
					overflow: hidden;
					.car{
							width: 100%;
							height: 100%;
							background-image: url('/img/hud/smartphone/taxiTab/car-anim.png');
							background-size: contain;
							animation: 0.4s linear car;
						}
				}
				.header{
						padding: 1rem 0 0.75rem 0;
						margin: 0 0 0 1rem;
						position: relative;
						&:before{
							content: 'GO';
							position: absolute;
							font-weight: bold;
							font-size: 3rem;
							line-height: 3.5rem;
							top: -0.3rem;
							right: 2.65rem;
							color: rgba(255, 255, 255, 0.2);
						}
						.title{
								font-size: .65rem;
								line-height: .75rem;
								color: rgba(255, 255, 255, 0.5);
						}
						.price{
							font-size: 1.4rem;
							color: #fff;
							&:before{
									content: '$';
							}
						}
				}
				&__params{
						background: #FFFFFF;
						box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.5);
						border-radius: .75rem;
						padding: 1.7rem .75rem .75rem .75rem;
						box-shadow: 0rem .5rem 1.5rem rgba(177, 188, 203, 0.5);
				}
				&__location{
						padding: 0 0 .15rem .9rem;
						border-bottom: 1px solid #EBEBEB;
						position: relative;
						display: flex;
						flex-flow: column;
						.icon{
								display: flex;
								align-items: center;
								width: 2.05rem;
								height: 1px;
								background: #EBEBEB;
								position: absolute;
								top: 2.35rem;
								left: 0;
								transform-origin: 50% 50%;
								transform: rotate(90deg) translateY(.65rem);
								&:before, &:after{
										content: '';
										position: absolute;
										width: .65rem;
										height: .65rem;
										background: #FFFFFF;
										border: .2rem solid;
										border-radius: 50%;
										box-sizing: border-box;
								}
								&:before{
										transform: translateX(-100%);
										left: -.15rem;
										border-color: #000000
								}
								&:after{
										transform: translateX(100%);
										right: -.15rem;
										border-color: #F4C10B;
								}
						}
						.location-point{
								border-bottom: 1px solid #EBEBEB;
								padding-bottom: .6rem;
								margin-bottom: .6rem;
								&:last-child{
										border-bottom: none;
										margin-bottom: 0;
								}
								.descr{
										font-size: .65rem;
										line-height: .75rem;
										color: rgba(0, 0, 0, 0.4);
										margin-bottom: 5px;
								}
								.value{
										font-size: .65rem;
										line-height: .75rem;
										white-space: pre;
										color: #000000;
								}
						}
				}
				&__payment{
						background: #F9F9F9;
						border-radius: .5rem;
						height: 2.5rem;
						display: flex;
						align-items: center;
						margin: 1rem 0 .75rem 0;
						position: relative;
						&:before{
								content: '';
								position: absolute;
								background: #FFFFFF;
								box-shadow: 0px 10px 40px rgba(0, 0, 0, 0.15);
								border-radius: 10px;
								height: 100%;
								width: 50%;
								top: 0;
								left: 0%;
								z-index: 1;
								transition: .3s;
						}
						&.active{
								&:before{
										left: 50%;
								}
						}
						.payment-option{
								width: 50%;
								font-size: .65rem;
								line-height: .75rem;
								color: #000000;
								display: flex;
								align-items: center;
								justify-content: center;
								z-index: 2;
								&:before{
										content: '';
										width: 1rem;
										height: 1rem;
										background-image: url('/img/hud/smartphone/taxiTab/card-icon.svg');
										background-size: contain;
										margin-right: .5rem;
								}
						}
				}
				.btn-apply{
						width: 100%;
						display: flex;
						align-items: center;
						justify-content: center;
						height: 2.5rem;
						font-size: .7rem;
						color: #000000;
						background: #F4C10B;
						border-radius: .5rem;
				}
		}
}
@keyframes car {
    from {
      transform: translateX(100%);
    }

    to {
      transform: translateX(0%);
    }
  }
</style>