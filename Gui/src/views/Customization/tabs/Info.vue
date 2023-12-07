<template>
    <div class="customiztion-tab-info">        
        <Title text="customiz:info:1"/>
        <div class="customiztion-tab-info_btns">
            <div class="customiztion-tab-info_btn" :class="{'customiztion-tab-info_selected': gender}" @click="changeGender(true)">
                <img src="/img/customization/common/male.svg" alt="male">
                {{loc("customiz:info:2")}}
            </div>
            <div class="customiztion-tab-info_btn" :class="{'customiztion-tab-info_selected': !gender}" @click="changeGender(false)">
                <img src="/img/customization/common/female.svg" alt="female">
                {{loc("customiz:info:3")}}
            </div>
        </div>
        <template v-if="firstCreate">
            <div class="customiztion-tab_delemiter"></div>
            <Title text="customiz:info:4"/>
            <input type="text" class="customiztion-tab-info_name" :placeholder="loc('customiz:info:4')" v-model="fitrstNameModel">
            <input type="text" class="customiztion-tab-info_name" :placeholder="loc('customiz:info:5')" v-model="lastNameModel">
        </template>
        
        
        <!-- <div class="customiztion-tab_delemiter"></div>

        <Title text="customiz:info:6"/>
        <Range :itemData="age"/> 
        -->

    </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import Title from '../navElements/Title'
//import Range from '../navElements/Range'

export default {
    props:{
        items: Array
    },
    computed:{
        ...mapGetters("localization", ["loc"]),
        ...mapState("customization", ["gender", "firstName", "lastName", "age", "firstCreate"]),
        fitrstNameModel:{
            get(){
                return this.firstName;
            },
            set(value){
                this.updateFirstName(value)
            }
        },
        lastNameModel:{
            get(){
                return this.lastName;
            },
            set(value){
                this.updateLastName(value)
            }
        }
    },
    methods: {
        ...mapMutations("customization", ["updateFirstName", "updateLastName", "updateAge", "changeGender"])
    },
    components:{
        Title,
        //Range
    }
}
</script>

<style lang="scss">
.customiztion-tab-info{
    &_btns{
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin: .5rem 0;
    }

    &_btn{
        width: 7rem;
        height: 3rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: .3rem;
        background-color: rgba(#fff, .1);
        letter-spacing: .06rem;
        img{
            margin-right: .5rem;
        }
    }

    &_selected{        
        background-color: #B6D300;
    }
    &_name{
        height: 3.5rem;
        width: 15rem;
        margin: .25rem 0;
        border-radius: .2rem;
        background-color: rgba(#fff, .1);
        padding: .0 1rem;
        color: #fff;
        letter-spacing: .07rem;
        font-size: 1rem;
        &::placeholder{
            color: rgba(#fff, .4);
            font-size: .9rem;
            text-transform: uppercase;
        }
    }
}
</style>