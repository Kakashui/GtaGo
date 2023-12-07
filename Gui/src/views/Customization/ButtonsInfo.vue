<template>
    <div class="customiztion-btninfo">
        <div class="customiztion-btninfo_btn">
            <img src="/img/customization/common/mouse_left.svg" alt="mouse_left">
            {{loc('customiz:mouse:1')}}
        </div>
        <div class="customiztion-btninfo_btn">
            <img src="/img/customization/common/mouse_wheel.svg" alt="moause_wheel">
            {{loc('customiz:mouse:2')}}
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    computed:{
        ...mapGetters("localization", ["loc"])
    }
}
</script>

<style lang="scss">
.customiztion-btninfo{
    position: absolute;
    right: 0;
    top: 0;
    height: 30rem;
    width: 30rem;
    background: linear-gradient(222.6deg, #000000 0.16%, rgba(0, 0, 0, 0) 51.04%);
    &_btn{
        height: 1.5rem;
        width: 17rem;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        color: rgba(#fff, .5);
        margin: .7rem 0 0 auto;
        letter-spacing: .07rem;
        img{
            margin: 0 1rem;
        }
    }
}
</style>