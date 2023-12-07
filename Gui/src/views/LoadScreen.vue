<template>
    <div class="loadScreen">
        <img src="/img/common/loading.gif" alt="load">
        <div class="loadScreen-tittle">{{tittle}}</div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            text: "Loading",
            points: 0,
            pointsMax: 5,
            tittle: "Loading",
            interval: -1
        }
    },   
    mounted() {
        this.interval = setInterval(()=>{
            this.tittle = this.text;
            for (let index = 0; index < this.points; index++)
                this.tittle += ".";
            this.points += 1;
            if(this.points >= this.pointsMax) this.points = 0;
        }, 300)
    },
    beforeDestroy(){
        clearInterval(this.interval);
    }
}
</script>

<style lang="scss" scoped>
    .loadScreen{
        position: fixed;
        left: 0;
        top: 0;
        width: 100vw;
        height: 100vh;
        background-color: #000;

        // background: url(/img/common/banner.jpg) no-repeat center;
        // background-size: cover;
        
        display: flex;
        align-items: center;
        justify-content: center;
        img{
            height: 14rem;
        }
        &-tittle{
            text-transform: uppercase;
            font-size: 1.2rem;
            letter-spacing: .06rem;
            position: absolute;
            margin-top: 15rem;
            left: 48%;
            color: #fff;
        }
    }
</style>