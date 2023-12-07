<template>
    <div class="confirm">
        <div class="confirm-dialog">
            <div class="confirm-tittle">
                {{loc("inv_confirm_tittle")}}
            </div>
            <div class="confirm-body">
                {{loc(msg)}}
            </div>
            <div class="confirm-buttons">
                    <div class="confirm-button" @click="confirm">{{loc("inv_split_confirm")}}</div>
                    <div class="confirm-button" @click="cancel">{{loc("inv_split_cancel")}}</div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
    props:['msg'],
    computed: {
        ...mapGetters('localization', ['loc'])
    },
    methods: {
        confirm(){
            this.$emit("confirmAction", true)
        },
        cancel(){
            this.$emit("confirmAction", false)
        }
    },
}
</script>

<style lang="scss" scoped>
    .confirm{
        position: fixed;
        left: 0;
        top: 0;
        width: 100vw;
        height: 100vh;
        z-index: 1100;
        background-color: rgba(0,0,0,.5);
        display: flex;
        justify-content: center;
        align-items: center;
        &-dialog{
            width: 19rem;
            border: 2px solid rgba(#fff, .2);
            box-shadow: 0px 1rem 3rem rgba(0, 13, 18, 0.5);
        }
        &-tittle{
            height: 4rem; 
            background: url(/img/inventory/common/otherHead.png) no-repeat center;
            background-size: cover;          
            font-size: 1.3rem;
            line-height: 1.3rem;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        &-body{
            color: #fff;
            padding: 2rem 1.2rem;
            font-size: .9rem;
            line-height: 1.2rem;
            letter-spacing: .05rem;
            background-color: #142126;
            text-align: center;
        }
        &-buttons{
            display: flex;
            align-items: center;
            justify-content: space-around;
        }
        &-button{
            height: 3.5rem;
            width: 49%;
            color: rgba(#fff, .5);
            background-color: #000D12;
            display: flex;
            justify-content: center;
            align-items: center;
            &:hover{
                background-color: #8E52AA;
                color: #fff;
            }
        }
    }
</style>