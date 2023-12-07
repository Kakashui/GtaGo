<template>
    <div class="robshop-progress">
        <div class="robshop-progress_stady" :style="{'width': `${100 * stady}%`}">
            <div class="robshop-progress_thumb" :style="{'left': `-${100 - stady1value}%`, 'background-color': '#B6D300'}"></div>
        </div>
        <div class="robshop-progress_stady" :style="{'width': `${100 - 100 * stady}%`}">
            <div class="robshop-progress_thumb" :style="{'left': `-${100 - stady2value}%`, 'background-color': '#D39800'}"></div>
        </div>        
    </div>
</template>

<script>
export default {
    props: ["progress"],
    computed:{
        stady1(){
            return 100 * this.stady
        },
        stady1value(){
            return this.progress >= this.stady1 ? 100 : Math.random(this.progress / this.stady1 * 100);
        },
        stady2(){
            return 100 - this.stady1;
        },
        stady2value(){
            return this.progress <= this.stady1 ? 0 : Math.floor((this.progress - this.stady1) / this.stady2 * 100);
        }
    },
    data() {
        return {
            stady: .3
        }
    },
    mounted() {
        console.log(this.stady1, this.stady2, this.stady1value, this.stady2value)
    },
}
</script>

<style lang="scss">
    .robshop-progress{
        display: flex;
        align-items: center;
        position: relative;
        width: 98%;
        margin-top: .5rem;
        &_stady{
            overflow: hidden;
            height: 1.3rem;     
            //border: 1px solid #000;      
            position: relative; 
            background-color: rgba(#fff, .2);
            margin-right: 1%;
        }
        &_thumb{
            position: absolute;
            top: 0;
            height: 100%;
            width: 100%;
            //border: 1px solid #fff;
            transition: all 1s;
        }
    }
</style>