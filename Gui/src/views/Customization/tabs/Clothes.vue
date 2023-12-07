<template>
    <div class="customiztion-tab-clothes">
        <Title :text="itemData.title"/>
        <ImageList :itemData="itemData" @onSelect="selectClothes"/>
    </div>
</template>

<script>
import Title from '../navElements/Title'
import ImageList from '../navElements/ImageList'

export default {
    props:{
        itemData: Object
    },
    methods:{
        selectClothes(tag, value){
            window.mp.trigger("customization:update", tag, value);
        },
    },
    components:{
        Title,
        ImageList
    }
}
</script>
