<template>    
    <div class="fishing-action" ><img src="/img/common/elements/hook.png" alt="hook"></div>
</template>

<style lang="scss" scoped>
.fishing-action{
    position: absolute;
    right: 1rem;
    top: 20rem;
    width: 4rem;
    height: 4rem;
    background: url(/img/common/textures/texture_wite_1024.jpg) no-repeat center;
    background-size: contain;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    box-shadow: .1rem .1rem .3rem 0 #000;
    img{
        width: 70%;
    }
}
</style>