<template>
    <div 
        class="context-menu" 
        v-show="context.visible" 
        :style="{'top': context.y + 'px', 'left': context.x + 'px'}"
    >
        <div class="button" v-for="(item, index) in buttons" :key="index" @click="select(item.action)">
            {{loc(item.name)}}
        </div>
    </div>
</template>
<script>
import { mapGetters } from 'vuex'

export default {
    data() {
        return {      
            buttonsInventory:[
                {name: "gui_751", action: 1},                
                {name: "gui_752", action: 2},
                {name: "gui_753", action: 4},
            ],           
            buttonsOutside:[
                {name: "gui_754", action: 3},
            ]
        }
    },
    computed: {
        context:{
            set(val){
                this.$store.commit("inventory/setContextData", {visible: val});
            },
            get(){
                return this.$store.state.inventory.context;
            }
        },
        buttons(){
            return this.context.outside ? this.buttonsOutside : this.buttonsInventory;
        },
        outsideType(){
            return this.$store.state.inventory.outsideType;
        },
        ...mapGetters('localization',['loc'])
    },
    methods:{
        select(action){
            window.mp.trigger("boardCB", action, this.context.outside ? this.outsideType : 0, this.context.index);
            this.context = false;
        }
    }
}
</script>

<style lang="scss" scoped>
.context-menu{
    position: absolute;
    background-color: #fff;
    border-radius: .5vh;
    border: 2px solid #000;
    width: 15vh;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    padding: .1vh;
    .button{
        width: 100%;
        margin: .1vh;
        font-size: 1.8vh;
    }
}
</style>