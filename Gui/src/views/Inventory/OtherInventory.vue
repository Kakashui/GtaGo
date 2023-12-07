<template>
    <div class="other" :style="{'top': `${y}px`, 'left': `${x}px`}">
        <div class="other-tittle" @mousedown="onMoveStart">
            <img src="/img/inventory/common/backpack.png" alt="backpack" class="other-tittle-img">
            <div class="other-tittle-weight">
                <div class="other-tittle-weight-current">{{(currentWeight/1000).toFixed()}}</div>
                <div class="other-tittle-weight-max">/{{(maxWeight/1000).toFixed()}} kg</div>
            </div>
            <div class="other-tittle-name">{{ loc(name) }}</div>
        </div>
        <ListItems
            :id="id"
            :drag="drag"
            :sortByIndex ="true"
            :canDrop="true"
            @onMouseDown="onMouseDown" 
            @onDoubleClick="onDoubleClick"
            class="other-inventory"
        />
        <div class="other-footer" @click="close">
            {{loc('inv_close_other')}}
        </div>
    </div>
</template>

<script>
import ListItems from './ListItems'
import {mapGetters} from 'vuex'

export default {
    props:['id', 'name', 'drag'],
    data() {
        return {
            x: 150,
            y: 300,
            offsetX: 0,
            offsetY: 0,
            move: false,
            maxWeight: 0
        }
    },
    computed: {
        ...mapGetters('inventory', ['getById']),
        ...mapGetters('localization', ['loc']),
        currentWeight(){
            const inventory = this.getById(this.id);
            let total = 0;
            inventory.items.forEach(item => {
                total += item.getWeight();
            });
            return total;
        }
    },
    methods: {
        onMouseDown(dragObject){
            this.$emit("onMouseDown", dragObject)
        },
        onDoubleClick(adress, item){
            this.$emit("onDoubleClick", adress, item)
        },
        onMouseRelease(e){
            if(e.button == 0) this.move = false;
        },
        onMousemove(e){
            if(this.move){
                this.x = e.clientX + this.offsetX;
                this.y = e.clientY + this.offsetY;
            }
        },
        onMoveStart(e){
            if(e.button == 0) {
                const pos = e.target.getBoundingClientRect(); 
                this.offsetX = pos.x - e.clientX; 
                this.offsetY = pos.y - e.clientY; 
                this.move = true;
            }
        },
        close(){
            this.$store.commit('inventory/closeOther', this.id)
        }
    },
    components:{
        ListItems
    },
    mounted(){
        this.maxWeight = this.getById(this.id).maxWeight;
        document.addEventListener("mouseup", this.onMouseRelease)
        document.addEventListener("mousemove", this.onMousemove)
    },
    beforeDestroy(){
        document.removeEventListener("mouseup", this.onMouseRelease)
        document.removeEventListener("mousemove", this.onMousemove)
    }
}
</script>

<style lang="scss" scoped>
    .other{
        position: fixed;
        border: 2px solid rgba(255, 255, 255, 0.15);
        background-color: #000;
        box-shadow: 0 1.5rem 3rem rgba(0, 13, 18, 0.7);
        width: 23.4rem;
        //overflow: hidden;
        &-inventory{
            margin-left: 1rem;
            margin-top: 1rem;
            height: 21.5rem;
        }
        &-tittle{
            height: 4.5rem;
            background: url(/img/inventory/common/otherHead.png) no-repeat center;
            background-size: cover;
            padding-left: 1.15rem;
            padding-top: .7rem;        
            color: #fff;
            position: relative;
            &-img{
                pointer-events: none;
                position: absolute;
                bottom: 0;
                right: 5rem;
            }
            &-weight{ 
                pointer-events: none;
                display: flex;
                align-items: center;              
                &-max{
                pointer-events: none;
                    color: rgba(#fff, .5);
                }
            }
            &-name{
                pointer-events: none;
                font-size: 1.8rem;
                line-height: 1.8rem;
                font-weight: normal;
            }
        }
        &-close{
            margin-left: auto;
            border: 1px solid #fff;
            width: 1.5rem;
            height: 1.5rem;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        &-footer{
            height: 3.75rem;
            display: flex;
            align-items: center;
            justify-content: center;
            color: rgba(#fff, .5);
            font-size: .9rem;
            line-height: 1.1rem;
            &:hover{                
                color: rgba(#fff, .8);
            }
        }
    }
</style>