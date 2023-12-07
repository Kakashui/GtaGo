<template>
    <div class="customiztion-nav-title">
        <div class="customiztion-nav-title_decor"></div>
        <div class="customiztion-nav-title_text">{{loc(text)}}</div> 
    </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
    props:{
        text: String
    },
    computed:{
        ...mapGetters("localization", ["loc"])
    }
}
</script>

<style lang="scss">
.customiztion-nav-title{
    display: flex;
    align-items: center;
    margin-bottom: .3rem;
    &_decor{
        width: .2rem;
        height: 1.8rem;
        background-color: #fff;
        box-shadow: 0 0 .5rem 0 #fff;
    }
    &_text{
        font-size: 1.2rem;
        letter-spacing: .09rem;
        margin-left: .5rem;
    }
}
</style>