<template>
    <div class="weedfarm">
        <component :is="currentPage" />
    </div>
</template>

<script>
import { mapState } from 'vuex'
import Greeting from './Greeting'
import Components from './Components'
import ControlBoard from './ControlBoard'

export default {
    computed:{
        ...mapState('weedFarm', ['currentPage']),
    },
    components:{
        Greeting,
        Components,
        ControlBoard
    }
}
</script>

<style lang="scss">
.weedfarm {
    width: 70rem;
    height: 40.5rem;
    position: relative;
    box-shadow: 0 0 1rem 0 #000;
    color: #fff;
}
</style>