<template>
	<div class="group-list">
		<div class="header">
			<div class="nickname">{{loc('captmenu_7')}}</div>
			<div class="rang">{{loc('captmenu_8')}}</div>
		</div>
		<div class="group-list_list">
			<GroupListItem 
			v-for="(item, index) in gangList"
			:key="item.id"
			:item="item"
			:index="index"
			/>
		</div>
	</div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

import GroupListItem from './GroupListItem'

export default {
  name: 'GroupList',

  components: {
		GroupListItem
  },

  computed: {
    ...mapState('captures', ['capturing', 'gangList']),
    ...mapGetters("localization", ["loc"])
	},

  methods: {
    
  }

}
</script>

<style lang="scss" scoped>
	.group-list{
		height: 83%;
		position: relative;
		.header{
			display: flex;
			align-items: flex-start;
			justify-content: flex-start;
			width: 100%;
			color: rgba(255, 255, 255, 0.4);
			font-size: .8rem;
			padding: 0 .9rem 1.25rem 2rem;
			.nickname{
				padding-left: .8rem;
				width: 44%;
			}
			.rang{
				width: 37%;
			}
		}
		&_list{
			padding-right: .75rem;
			overflow-y: scroll;
			height: 90%;
			&::-webkit-scrollbar-track {
				border-radius: .25rem;
			}
			&::-webkit-scrollbar {
				width: .2rem;
			}
			&::-webkit-scrollbar-thumb {
				border-radius: .25rem;
				background: rgba(255, 255, 255, 0.15);
			}
		}
	}
</style>