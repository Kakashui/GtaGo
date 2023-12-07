<template>
    <div class="customiztion-tab-parents">
        <Title :text="itemData.title"/>
        <ImageList :itemData="itemData" @onSelect="selectParent"/>
        <div class="customiztion-tab_delemiter"></div>        
        <Title text="customiz:parent:form"/>
        <Range :itemData="similar"/>
        <div class="customiztion-tab_delemiter"></div>        
        <Title text="customiz:parent:skin"/>
        <Range :itemData="skin"/>
    </div>
</template>

<script>
import ImageList from '../navElements/ImageList'
import Title from '../navElements/Title'
import Range from '../navElements/Range'
import { mapMutations, mapState } from 'vuex'

export default {
    props:{
        itemData: Object,
    },
    data() {
        return {
            currentParent: 0,
            
        }
    },
    computed:{
        ...mapState("customization", ["similar", "skin"]),
       
    },
    methods: {
        ...mapMutations("customization", ["updateSimilar", "updateSkin"]),
        selectParent(tag, value){
            window.mp.trigger("customization:update", tag, value)
        }
    },
    components:{
        ImageList,
        Title,
        Range
    }
}
</script>

<style lang="scss">
.customiztion-tab-parents{
    
}
</style>