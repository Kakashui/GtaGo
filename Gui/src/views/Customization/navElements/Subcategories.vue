<template>
    <div class="customiztion-nav-subcat">
        <div class="customiztion-nav-subcat_item" 
            v-for="(item, index) in items" 
            :key="index"
            :class="{'customiztion-nav-subcat_selected': index == currentItem}"
            @click="selectSubcatregory(index)"
        >
            <img :src="`/img/customization/subcategories/${item.img}`" alt="category">
        </div>
    </div>
</template>

<script>
export default {
    props:{
        items: Array,
        currentItem: Number
    },
    methods: {
        selectSubcatregory(index){
            this.$emit("onSelectSubcategory", index)
        }
    },
}
</script>

<style lang="scss">
.customiztion-nav-subcat{
    &_item{
        width: 3.5rem;
        height: 3.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    &_selected{
        background: linear-gradient(90deg, rgba(182, 211, 0, 0.5) 0%, rgba(182, 211, 0, 0) 100%);
    }
}
</style>