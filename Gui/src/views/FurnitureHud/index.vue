<template>
    <div class="body">
        <div class="furniture">
            <div class="line">
              <div class="desc">{{loc('furn_1')}}</div><br>
              <img src="/img/furniture/left.png" alt="left"> 
                <img src="/img/furniture/right.png" alt="right"> 
            </div>
            <div class="line">
              <div class="desc">{{loc('furn_2')}}</div>
              <img src="/img/furniture/z.png" alt="up">
              <img src="/img/furniture/x.png" alt="down"> 
            </div>
            <div class="line">
              <div class="desc">{{loc('furn_3')}}</div>
              <img src="/img/furniture/mouse.png" alt="left"> 
            </div>
            <div class="line">
              <div class="desc">{{loc('furn_5')}}</div>
              <img src="/img/furniture/Y.png" alt="Y"> 
            </div>
        </div>            
    </div>
</template>

<script>
import {mapGetters} from 'vuex'

export default {
    data() {
        return {
            
        }
    },
    computed:{
        ...mapGetters('localization', ['loc'])
    }
}
</script>
<style lang="scss" scoped>
    .body{
        display: flex;
        position: absolute;
        left: 0;
        top: 0;
        justify-content: left;
        align-items: flex-end;
        width: 100vw;
        height: 100vh;
    }
    .furniture{
        width: 15%;
        justify-content: center;
        //background-color: rgba(#fff, .2);    
        flex-wrap: wrap;
        padding: 20px;
    }
    .line{
        display: flex;
        justify-content: flex-start;
        align-items: center;
        margin: 5px 15px;
        width: 30%;
        padding: 5px;
        img{
            width: 30px;
            margin: 2px;
        }
        .desc{
            font-size: 1.5rem;
            color: #fff;
           text-transform: uppercase;
            padding-right: 10px;
        }
    }
</style>