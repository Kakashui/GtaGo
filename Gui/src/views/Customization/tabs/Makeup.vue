<template>
    <div class="customiztion-tab-makeup">
        <Title :text="itemData.title"/>
        <Range :itemData="itemData"/>
        <div class="customiztion-tab_delemiter"></div>
        <Title text="customiz:color:main"/>
        <Colors :current="itemData.color1" @onSelect="selectColor1" :colors="colors"/>
        <template v-if="itemData.secondColor">
            <div class="customiztion-tab_delemiter"></div>
            <Title text="customiz:color:second"/>
            <Colors :current="itemData.color2" @onSelect="selectColor2" :colors="colors"/>
        </template>
        <!-- <template v-if="itemData.opacity !== undefined">
            <div class="customiztion-tab_delemiter"></div>
            <Title text="customiz:opacity"/>
            <Range :itemData="itemData.opacity" />
        </template> -->
    </div>
</template>

<script>
import Title from '../navElements/Title'
import Range from '../navElements/Range'
import Colors from '../navElements/Colors'

export default {
    props:{
        itemData: Object
    },
    data() {
        return {
            colors:[
                {value: 0, color:"#992532"},
                {value: 1, color:"#C8395D"},
                {value: 3, color:"#B8637A"},
                {value: 5, color:"#B1434C"},
                {value: 6, color:"#7F3133"},
                {value: 8, color:"#C18779"},
                {value: 10, color:"#C6918F"},
                {value: 11, color:"#AB6F63"},
                {value: 61, color:"#826355"},
                {value: 15,  color:"#CA7F92"},
                {value: 18, color:"#DE3E81"},
                {value: 21, color:"#4F1F2A"},
                {value: 23, color:"#DE2034"},
                {value: 27, color:"#C227B2"},
                {value: 35, color:"#1E74BB"},
                {value: 39, color:"#27C07D"}
            ]
        }
    },
    methods:{
        selectColor1(index, color){
            this.itemData.color1 = index;
            window.mp.trigger("customization:update", `${this.itemData.tag}Color1`, color)
        },
        selectColor2(index, color){
            this.itemData.color2 = index;
            window.mp.trigger("customization:update", `${this.itemData.tag}Color2`, color)
        }
    },
    components:{
        Range,
        Title,
        Colors
    }
}
</script>