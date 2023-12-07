<template>
    <div class="personal">
        
        <div class="personal-tittle">
            <div class="personal-tittle-text">{{loc('inv_personal_text')}}</div>
            <div class="personal-tittle-weight">
                <img src="/img/inventory/common/pesa_1.svg" alt="pesa">
                <div class="personal-tittle-weight-cur">
                    <div class="personal-tittle-weight-text">
                        {{loc('inv_weight')}}
                    </div>
                    <div class="personal-tittle-weight-amount">                        
                        {{(currentWeight / 1000).toFixed()}}
                    </div>
                </div>
                <div class="personal-tittle-weight-max">/{{(inventory.maxWeight/1000).toFixed()}} kg</div>
            </div>
            <img src="/img/inventory/common/rect.png" alt="rect" class="personal-tittle-rect">
        </div>
        <div class="personal-subtittle">
            <div class="personal-subtittle-text">
                {{loc('inv_personal_items')}}               
            </div>
            <div class="personal-subtittle-progress">
                <div class="personal-subtittle-progress-thumb" :style="{'width': `${progress}%`}">

                </div>
            </div>
        </div>
        <ListItems
            :id ="personalId"
            :drag="drag"
            :width="6"
            :sortByIndex ="true"
            :canDrop="true"
            @onMouseDown="onMouseDown"  
            @onDoubleClick="onDoubleClick" 
        />
    </div>
</template>

<script>
import ListItems from './ListItems'
import {mapState, mapGetters} from 'vuex'    

export default {
    props: ['drag'],
    methods: {
        onMouseDown(dragObject){
            this.$emit("onMouseDown", dragObject)
        },
        onDoubleClick(adress, itemId){
            this.$emit("onDoubleClick", adress, itemId)
        }
    },
    computed:{
        ...mapState('inventory', ['personalId']),
        ...mapGetters('inventory', ['getById']),
        ...mapGetters('localization', ['loc']),
        inventory(){
            return this.getById(this.personalId);
        },
        currentWeight(){
            let amount = 0;
            this.inventory.items.forEach(item => {
                amount += item.getWeight();
            });
            return amount;
        },
        progress(){
            return this.currentWeight / this.inventory.maxWeight * 100;
        }
    },
    components:{
        ListItems
    }
}
</script>

<style lang="scss" scoped>
.personal{
    width: 34rem;
    margin-right: 2rem;
    &-tittle{
        width: 100%;
        position: relative;
        display: flex;
        justify-content: space-between;
        padding-right: 2rem;
        margin-bottom: .4rem;
        &-text{
            font-size: 80px;
            line-height: 80px;
            font-weight: 300;
            color: #FFFFFF;
            text-shadow: -.5rem -.5rem 2rem rgba(255, 255, 255, .5);
        }
        &-rect{
            position: absolute;
            left: -11.5rem;
            top: -4.5rem;
            pointer-events: none;
            -webkit-user-drag: none;
            -o-user-drag: none;
        }
        &-weight{
            display: flex;
            align-items: center;         
            img{
                margin-right: .75rem;
            }
            &-text{
                color: rgba(#fff, .5);
                font-size: .9rem;
                line-height: 1.2rem;
                letter-spacing: .03rem;
            }
            &-amount{
                color: #fff;
                font-size: 2.5rem;
                line-height: 2.5rem;
                letter-spacing: .03rem;
            }
            &-max{
                color: rgba(#fff, .5);
                font-size: .9rem;
                line-height: 1.2rem;
                letter-spacing: .03rem;
                align-self: flex-end;
                margin-bottom: .2rem;
                padding-left: .15rem;
                text-align: left;
            }
        }
    }
    &-subtittle{
        width: 100%;
        margin-bottom: 1.2rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding-right: 2rem;
        &-text{
            color: #fff;
            font-size: .9rem;
            line-height: 1.2rem;
            letter-spacing: 0.04rem;
            color: rgba(255, 255, 255, 0.5);
        }
        &-progress{
            width: 25rem;
            height: 0.1rem;
            background-color: rgba(#fff, .1);
            position: relative;
            overflow: hidden;
            border-radius: .08rem;
            &-thumb{
                background-color: #FF3380;
                height: 100%;
            }
        }
    }
}
</style>