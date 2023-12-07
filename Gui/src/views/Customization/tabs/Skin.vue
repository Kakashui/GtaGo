<template>
    <div class="customiztion-tab-skin">
        <div class="customiztion-tab-skin_item" 
            v-for="(item, index) in itemData" :key="index"
        >
            <Title :text="item.title"/>
            <Range :itemData="item"/>
            <div class="customiztion-tab_delemiter"></div>
        </div>
    </div>
</template>

<script>
import Title from '../navElements/Title'
import Range from '../navElements/Range'


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