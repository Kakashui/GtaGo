<template>
    <div class="frac-money">
        <img src="/img/optionsMenu/organizationTab/moneybag.png" alt="moneybag" class="frac-money-bag">
        <div class="frac-money-bank">
            <div class="frac-money-bank-sub">{{loc('mmain:frac:info:money:sub')}}</div>
            <div class="frac-money-bank-tit">${{fraction.bank.toLocaleString()}}</div>
            <div class="frac-money-bank-btns">
                <div class="frac-money-bank-btn" @click="withdrawMoney">{{loc('mmain:frac:info:money:b1')}}</div>
                <div class="frac-money-bank-btn" @click="depositMoney">{{loc('mmain:frac:info:money:b2') }}</div>
            </div>
        </div>
        <div class="frac-money-common">
            <img src="/img/optionsMenu/organizationTab/moneybag2.svg" alt="moneybag2" class="frac-money-common-bag">
            <div class="frac-money-common-info">
                <div class="frac-money-common-sub">{{loc('mmain:frac:commom:sub1')}}</div>
                <div class="frac-money-common-tit"><span class="frac-money-common-plus">+</span>${{fraction.lastHour.toLocaleString()}}</div>
                <div class="frac-money-common-sub">{{loc('mmain:frac:commom:sub2')}}</div>
                <div class="frac-money-common-tit"><span class="frac-money-common-plus">+</span>${{fraction.lastDay.toLocaleString()}}</div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState, mapMutations } from 'vuex'

export default {
    computed: {
        ...mapGetters('localization', ['loc']),
        ...mapState('optionsMenu', ['fraction'])
    },
    methods: {
        ...mapMutations('optionsMenu', ['setDialog']),
        withdrawMoney(){
            if(this.lastCheck > Date.now()) return;
            this.lastCheck = Date.now() + 1000;
            if(!this.fraction.canWhithdraw){
                window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:nopermission", time: 3000});
                return;
            }
            this.setDialog({
               input: 'number',
               callback: (val)=>{
                   val = parseInt(val);
                    if(isNaN(val) || val < 1) return;
                    if(this.fraction.bank < val){
                        window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:dialog:wmoney:err", time: 3000});
                        return;
                    }
                   window.mp.triggerServer('fmenu:money:withdraw', +val);
               },
               value: '',
               placeholder: 'mmain:frac:dialog:wmoney:pl',
               tittle: `mmain:frac:dialog:wmoney:tit`,
               subtittle: 'mmain:frac:dialog:wmoney:sub',
               bg: 'invite'
           });
        },
        depositMoney(){
            this.setDialog({
               input: 'number',
               callback: (val)=>{
                   val = parseInt(val);
                    if(isNaN(val) || val < 1) return;
                   window.mp.triggerServer('fmenu:money:deposit', +val);
               },
               value: '',
               placeholder: 'mmain:frac:dialog:wmoney:pl',
               tittle: `mmain:frac:dialog:dmoney:tit`,
               subtittle: 'mmain:frac:dialog:wmoney:sub',
               bg: 'invite'
           });
        }
    }
}
</script>

<style lang="scss" scoped>
    .frac-money{
    &-bag{
        position: absolute;
        bottom: 0;
        right: -5rem;
    }
    margin-top: 2rem;
    color: #fff;
    font-weight: 300;
    width: 100%;
    letter-spacing: .04rem;
    display: flex;
    justify-content: space-between;
    
    &-bank{
        width: 45%;
        &-sub{
            font-size: .9rem;
            color: rgba(#fff, .5);
        }
        &-tit{
            font-size: 3.2rem;
            font-weight: 300;
        }
        &-btns{
            display: flex;
            width: 100%;
            align-items: center;
            justify-content: space-between;
            margin-top: 1rem;
        }
        &-btn{
            width: 46%;
            height: 2.8rem;
            border: 2px solid #B6D300;
            display: flex;
            align-items: center;
            justify-content: center;
            &:hover{
                background-color: #B6D300;
            }
        }
    }
    &-common{
        display: flex;
        align-items: center;
        width: 45%;
        &-bag{
            height: 100%;
            margin-right: 1rem;
        }
        &-sub{
            color: rgba(#fff, .5);
        }
        &-tit{
            font-size: 2.5rem;
        }
        &-plus{
            color: #B6D300;
        }
    }
}
</style>