<template>
    <div class="mplant-element" :style="{'width': `${2.5 * element.scale}rem`, 'height': `${2.5 * element.scale}rem`}">
        <img :src="element.bg" alt="bg" class="mplant-element_bg">
        <img :src="element.img" alt="img" class="mplant-element_img">
    </div>
</template>

<script>
export default {
    props:['element']
}
</script>

<style lang="scss">
    .mplant-element{
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
        img{
            position: absolute;
            pointer-events: none;
        }
        &_bg{
            width: 100%;
            height: 100%;
        }
        &_img{
            width: 50%;
            height: 50%;
        }

    }
</style>