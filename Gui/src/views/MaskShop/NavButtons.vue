<template>
    <div class="maskshop-nav">
        <div class="maskshop-nav_item"><div>esc</div><span>Для выхода</span></div>
        <div class="maskshop-nav_item"><img src="/img/maskShop/lmouse.svg" alt="lmouse"><span>Вращение персонажа</span></div>
        <div class="maskshop-nav_item"><img src="/img/maskShop/cmouse.svg" alt="lmouse"><span>Приближение персонажа</span></div>
    </div>
</template>


<script>
export default {
    
}
</script>

<style lang="scss" scoped>
    .maskshop-nav{
        position: absolute;
        right: 0;
        top: 0;
        width: 30rem;
        height: 30rem;
        background: linear-gradient(222.6deg, #000000 0.16%, rgba(0, 0, 0, 0) 51.04%);
        padding: 2rem 0 0 10rem;
       
        span{
            color: rgba(#fff, .5);
            letter-spacing: .08rem;
            margin-left: 1.5rem;
        }
        &_item{
            display: flex;
            align-items: center;
            margin: .7rem;
        }
    }
</style>