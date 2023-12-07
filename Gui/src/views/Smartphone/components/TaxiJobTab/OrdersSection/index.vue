<template>
	<div class="orders-section">
    <OrdersSectionList/>
	</div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

import OrdersSectionList from './OrdersSectionList'

export default {
  name: 'OrdersSection',
  
  components: { 
    OrdersSectionList
  },

	computed: {
		...mapState('smartphone', ['currentTab'])
	},

	methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme']),
    ...mapMutations('smartphone/taxiPage', ['taxijob_setStateWork']),

    setProfile: function() {
      this.setCurrentTab({ key: this.currentTab.key, section: 'ProfileSection', data: null })
      this.taxijob_setStateWork({value: 'working'})
    }
  },
  
  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
.orders-section{
  display: flex;
  flex-flow: column;
  padding: 0 0 .5rem 0;
  position: relative;
  height: 100%;
  &:after{
    content: '';
    width: 100%;
    height: 4rem;
    background: linear-gradient(180deg, rgba(243, 242, 247, 0) 0%, #F3F2F7 100%);
    border-radius: 0 0 1.6rem 1.6rem;
    position: absolute;
    bottom: 0;
    left: 0;
  }
}
</style>