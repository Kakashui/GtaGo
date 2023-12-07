<template>
    <div class="pc">
        <img src="/img/policeCarMenu/pcui.png" alt="ui" class="pc-ui">
        <div class="pc-screen">
            <component :is="currentPage" @onChangePage="setPage"/>
        </div>
    </div>  
</template>

<script>
import {mapState, mapGetters} from 'vuex'
import Main from './MainMenu'
import Person from './PersonMenu'
import Car from './CarMenu'
import List from './ListMenu'
export default {
    data() {
        return {
            currentPage: "Main"
        }
    },
    methods: {
        setPage(page){
            if(page == 'Close') window.mp.trigger('pccb', -1)
            else this.currentPage = page;
        }
    },
    computed: {
        ...mapGetters('localization', ['loc']),
        ...mapState('policeCarMenu', [])
    },
    components:{
        Main,
        Person,
        Car,
        List
    }
}
</script>

<style lang="scss" scoped>
    .pc{
        width: 100vh;
        height: 85vh;
        background: url(/img/policeCarMenu/policepc.png) center no-repeat;
        background-size: contain;
        align-self: flex-end;
        position: relative; 
        .pc-ui{
            position: absolute;
            width: 80vh;
            height: 53vh;
            top: 8vh;
            left: 10vh;
        }
        &-screen{
            position: absolute;
            width: 73vh;
            height: 39vh;
            top:17vh;
            left: 13vh;
        }
    }
</style>