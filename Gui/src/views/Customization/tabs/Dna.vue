<template>
    <div class="customiztion-tab-dna">
        <div class="customiztion-tab-dna_item" 
            v-for="(item, index) in itemData" 
            :key="index"
        >
            <Title :text="item.title" />
            <Range :itemData="item"/>
            <div class="customiztion-tab_delemiter"></div>  
        </div>
    </div>
</template>

<script>
import Range from '../navElements/Range'
import Title from '../navElements/Title'

export default {
    props:{
        itemData: Array
    },
    components:{
        Range,
        Title
    }    
}
</script>

<style lang="scss">
.customiztion-tab-dna{
    
}
</style>