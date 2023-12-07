<template>
    <div 
        class="avatar" 
        v-if="drag !== null && drag.dragStart" 
        :style="{'left': `${drag.clientX}px`, 'top': `${drag.clientY}px`}"
    >
        <img :src="drag.item.getImage()" alt="avatar">
        <div class="avatar-split" v-if="drag.alt">
            Split
        </div>
         <div class="avatar-drop" v-if="!drag.alt && drag.overAdress == 'out' ">
            <div>Drop</div>
            <img src="/img/inventory/common/drop_2.svg" alt="drop">
        </div>
    </div>
</template>

<script>
export default {
    props:['drag']
}
</script>

<style lang="scss" scoped>
    .avatar{
        position: fixed;
        width: 4rem;
        height: 4rem;
        pointer-events: none;
        &-split{
            color: #fff;
            position: absolute;
            top: -.7rem;
            left: 0;
            font-size: .7rem;
            line-height: .85rem;
            letter-spacing: .05rem;
        }
        &-drop{
            display: flex;
            align-items: center;
            justify-content: space-between;
            color: rgba(#fff, .5);
            position: absolute;
            top: -.7rem;
            font-size: .7rem;
            line-height: .85rem;
            letter-spacing: .05rem;
            img{
                width: .7rem;
                height: .7rem;
                margin-left: .3rem;
            }
        }
    }
</style>