<template>
    <div class="stock">
        <div class="stock-tittle">
            <div class="stock-tittle-text">{{loc(tittles[stock.type])}}</div>
            <div class="stock-tittle-weight">
                <div class="stock-tittle-weight-text">
                    {{loc('inv_weight')}}:
                </div>
                <div class="stock-tittle-weight-cur">
                    
                    <div class="stock-tittle-weight-amount">                        
                        {{(currentWeight / 1000).toFixed()}}
                    </div>
                </div>                
                <div class="stock-tittle-weight-max">/{{(inventory.maxWeight/1000).toFixed()}} kg</div><div class="stock-tittle-weight-hr"></div>
            </div>
            <img :src="`/img/inventory/stock/${stock.type}.png`" alt="stock" class="stock-tittle-img">
        </div>
        <ListItems 
            :id ="stock.id"
            :drag="drag"
            :width="5"
            :sortByIndex ="true"
            :canDrop="true"
            @onMouseDown="onMouseDown"
            @onDoubleClick="onDoubleClick"
        />

        <div class="stock-footer" @click="close">{{loc('inv_stock_close')}}</div>
    </div>
</template>

<script>
import {mapState, mapGetters} from 'vuex'
import ListItems from './ListItems'

export default {
    props: ['drag'],
    data() {
        return {
            tittles:{
                "0": "stock:tit:1",
                "1": "stock:tit:2",
                "2": "stock:tit:1"
            }
        }
    },
    methods: {
        onMouseDown(dragObject){
            this.$emit("onMouseDown", dragObject);
        },
        onDoubleClick(adress, item){
            this.$emit("onDoubleClick", adress, item);
        },
        close(){
            window.mp.trigger("cef:inv:close");
        }
    },
    computed:{
        ...mapState('inventory', ['stock']),
        ...mapGetters('inventory', ['getById']),
        ...mapGetters('localization', ['loc']),
        currentWeight(){
            let amount = 0;
            this.inventory.items.forEach(item => {
                amount += item.getWeight();
            });
            return amount;
        },
        inventory(){
            return this.getById(this.stock.id);
        },
    },
    components:{
        ListItems
    }
}
</script>

<style lang="scss" scoped>
    .stock{
        &-tittle{
            &-text{
                text-shadow: -10px -10px 40px rgba(255, 255, 255, 0.5);
                color: #fff;
                font-size: 4rem;
                line-height: 4rem;
                font-weight: 250;
            }       
            &-img{
                position: absolute;
                top: 0;
                right: 0;
            }     
            &-weight{
                display: flex;
                align-items: center;
                margin-bottom: 1rem;    
                &-text{
                    color: rgba(#fff, .5);
                    font-size: 1.5rem;
                    line-height: 1.5rem;
                    letter-spacing: .03rem;
                    margin-right: .5rem;
                }
                &-amount{
                    color: #fff;
                    font-size: 1.5rem;
                    line-height: 1.5rem;
                }
                &-max{
                    color: rgba(#fff, .5);
                    font-size: 1rem;
                    line-height: .9rem;
                    letter-spacing: .03rem;
                    align-self: flex-end;
                    margin-bottom: .2rem;
                    padding-left: .15rem;
                    text-align: left;
                }
                &-hr{                    
                    border-bottom: 2px solid #FF3380;
                    //border: .01rem solid rgba(#fff, .5);
                    width: 10rem;
                    margin-left: 1rem;
                };
            }
        }
        &-footer{
            height: 3.5rem;
            display: flex;
            align-items: center;
            justify-content: center;
            color: rgba(#fff, .5);
            width: 26.5rem;
            background-color:rgba(0, 13, 18, 0.5);
            margin-top: 1rem;
            &:hover{                
                background-color:rgba(50, 63, 68, 0.5);
            }
        }
    }
</style>