<template>
    <div class="customiztion-nav">       
        <div class="customiztion-nav_btn" 
            v-for="(category, index) in categories" 
            :key="index" 
            :class="{'customiztion-nav_selected': current === index}"
             @click="selectCategory(index)"
        >
            <img :src="`/img/customization/categories/${category.img}`" alt="category">
        </div>
        <div class="customiztion-nav_random">
            <p> {{loc("customiz:nav:random")}}<br>{{loc("customiz:nav:select")}}</p>
            <div class="customiztion-nav_btn" @click="random">
                <img src="/img/customization/common/random.svg" alt="random">
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    props:{
        current: Number,
        categories: Array
    },
    computed:{
        ...mapGetters("localization", ["loc"])
    },
    methods: {
        random(){
            this.$emit("onSelectRandom");
        },
        selectCategory(index){     
            this.$emit("onSelectCategory", index)
        }
    }
}
</script>

<style lang="scss">
.customiztion-nav{
    height: 44rem;
    width: 5.5rem;
    border: solid;
    border-image-slice: 1;
    border-width: 1px;
    border-image-source: linear-gradient(to top, rgba(#fff, 0), rgba(#fff, .5) 5%, rgba(#fff, .5) 95%, rgba(#fff, 0) 100%);
    position: relative;   
    &_btn{
        width: 4rem;
        height: 4rem;
        margin: .1rem auto;
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(255, 255, 255, 0.1);
        &:hover{
            background-color: rgba(255, 255, 255, 0.2);
        }
    }
    &_selected{
        background-color: #B6D300 !important;
    }
    &_random{
        margin-top: auto;  
        width: 4rem;
        position: absolute;
        bottom: 0;
        width: 100%;
        p{
            position: absolute;
            transform: rotate(-90deg);
            bottom: 6rem;
            letter-spacing: .06rem;
        }
    }
}
</style>