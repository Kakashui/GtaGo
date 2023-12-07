<template>
    <div>
        <div class="robshop-info">
            <div class="robshop-info-block robshop-info_row">
                <div class="robshop-info_f">F</div>
                <div class="robshop-info_title">{{loc("robshop:info:title")}}</div>
            </div>
            <div class="robshop-info-block">
                <div class="robshop-info-block-money">
                    <div class="robshop-info-block-money_title">{{loc("robshop:info:money")}}</div>
                    <div class="robshop-info-block-money_value">{{money.toLocaleString()}}$</div>
                </div>
            </div>
        </div>
        <div class="robshop-info_decor"></div>
    </div>
    
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    props: ["money"],
    computed:{
        ...mapGetters("localization", ["loc"])
    }
}
</script>

<style lang="scss">
.robshop-info{    
    width: 98%;
    //border: 1px solid #000;
    display: flex;
    align-items: center;
    justify-content: space-between;
    position: relative;
    margin-top: 1rem;
    &-block{
        min-width: 50%;
        width: 50%;
        //border: 1px solid #000;
        position: relative;
        &-money{
            &_title{
                text-align: right;
                width: 100%;
                //border: 1px solid #000;
            }
            &_value{
                text-align: right;
                width: 100%;

            }
        }
    }
    &_f{
        width: 2rem;
        min-width: 2rem;
        height: 2rem;;
        min-height: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        background-color: rgba(#fff, .2);
        border-radius: .2rem;
        color: rgba(#fff, .3);
        font-size: 1.5rem;
    }
    &_title{
        color: rgba(#fff, .3);
        margin-left: .5rem;
    }
    &_row{
        display: flex;
    }
    &_decor{
        position: absolute;
        height: 72%;
        width: .05rem;
        background-color: rgba(#fff, .2);
        bottom: 0;
        right: -.1rem;
    }
}
</style>