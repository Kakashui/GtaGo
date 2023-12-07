<template>
    <div class="maskshop-item" @click="select">
        <div class="maskshop-item_img" :class="{'maskshop-item_selected': selected}">
            <img :src="`/img/maskShop/${item.Variation}.png`" alt="bg">
        </div>
        <div class="maskshop-item_info">
            <div class="maskshop-item_title">{{`Mask ${item.Variation}`}}</div>
            <div class="maskshop-item_price">$ {{price}}</div>
            
        </div>
    </div>
</template>

<script>
export default {
    props:["item", "price", "selected"],
    methods: {
        select(){
            this.$emit("onselect")
        }
    },
}
</script>

<style lang="scss">
    .maskshop-item{
        direction: ltr;
        height: 4.5rem;
        width: 15rem;
        display: flex;
        align-items: center;
        margin: 1rem;
        margin-right: auto;
        &_img{
            border: .1rem solid rgba(#fff, .3);
            height: 4.5rem;
            width: 4.5rem;
            position: relative;
            img{
                width: 100%;
                height: 100%;
            }
        }
        &_info{
            margin-left: 1rem;
            letter-spacing: .05rem;
        }
        &_title{
            color: #B6D300;
        }
        &_price{
            font-size: 1.4rem;
        }
        &_selected{
            border: .1rem solid #B6D300;
            box-shadow: 0 0 .7rem 0 #B6D300;
        }
    }
</style>