<template>
    <div class="menu">
        <div class="menu-item" v-for="(menu, index) in menus" :key="index" @click="open(menu.page)">
            {{ loc(menu.name) }}
        </div>
    </div>
</template>
<script>
import {mapGetters} from 'vuex'

export default {
    data() {
        return {
            menus:[
                {name:'cef_228',page: 'Person'},
                {name:'cef_229',page: 'Car'},
                {name:'cef_230',page: 'List'},
                {name:'cef_231',page: 'Close'}
            ]
        }
    },
    methods: {
        open(page){
            this.$emit('onChangePage', page);
        }
    },
    computed:{        
        ...mapGetters('localization',['loc']),
    }
}
</script>

<style lang="scss" scoped>
    .menu{
        justify-content: center;
        align-items: center;
        margin: auto;
        width: 30vh;
        border: 1px solid #000;
        &-item{
            color: #fff;
            text-align: center;
            padding: .5vh 2vh;
            background: linear-gradient(#0c1a33,#152c53,#152c53,#0c1a33);
            margin: .4vh 0;
            &:hover{
                background: linear-gradient(#20437e,#244d92,#244d92,#20437e);
                transform: scale(1.02);
            }
        }
    }
</style>