<template>
    <div class="customiztion-nav-colors">
        <div class="customiztion-nav-colors_item" 
            v-for="(item, index) in colors" 
            :key="index"
            :class="{'customiztion-nav-colors_selected': index === current}"
            :style="{'background-color': item.color}"
            @click="selectColor(index)"
        >
        </div>
    </div>
</template>

<script>

export default {   
    props:{
        current: Number,
        colors: Array
    },
    methods: {
        selectColor(index){
            this.$emit("onSelect", index, this.colors[index].value)
        }
    }
}
</script>

<style lang="scss">
.customiztion-nav-colors{
    //background-color: rgba(0,0,0,.9);
    display: flex;
    width: 14rem;
    height: 3.5rem;
    flex-wrap: wrap;
    align-items: center;
    justify-content: center;
    &_item{
        width: 1.5rem;
        height: 1.5rem;
        border: 1px solid #000;
        margin: .1rem;
    }
    &_selected{        
        border: 1px solid #fff;
    }
}
</style>