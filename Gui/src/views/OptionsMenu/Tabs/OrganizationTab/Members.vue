<template>
    <div class="frac-memb" v-if="fraction.id > 0">
        <div class="frac-memb-tit" >
            <img src="/img/optionsMenu/organizationTab/hand.svg" alt="moneybag2" class="frac-memb-hand">
            <div class="frac-memb-tit-count">{{fracMembers.length}}</div>
            <div class="frac-memb-tit-text">{{loc('mmain:frac:memb:tit1')}}</div>
            <div class="frac-memb-tit-invite" @click="invitePlayer">
                <div class="frac-memb-tit-invite-btn">+</div>
                <div class="frac-memb-tit-invite-txt">{{loc('mmain:frac:memb:invite')}}</div>
            </div>
        </div>
        <div class="frac-memb-list-head">
            <div class="frac-memb-list-id" @click="selectSort('id')" :class="{'frac-sort-selected': sortBy == 'id'}">Id</div>
            <div class="frac-memb-list-status" @click="selectSort('status')" :class="{'frac-sort-selected': sortBy == 'status'}">{{loc('mmain:frac:memb:status')}}</div>
            <div class="frac-memb-list-name" @click="selectSort('name')" :class="{'frac-sort-selected': sortBy == 'name'}">{{loc('mmain:frac:memb:name')}}</div>
            <div class="frac-memb-list-rank" @click="selectSort('rank')" :class="{'frac-sort-selected': sortBy == 'rank'}">{{loc('mmain:frac:memb:rank')}}</div>
            <input class="frac-memb-list-search" :placeholder="loc('mmain:frac:memb:search')" v-model="filter"/>
        </div>
        
        <div class="frac-memb-list">
            <div class="frac-memb-list-item" v-for="(member, index) in sortedMembers" :key="index">
                <div class="frac-memb-list-id" > 
                    <span v-if="member.id > -1">
                        {{ member.id}}
                    </span> 
                    <span v-else>
                        -
                    </span>
                </div>
                <div class="frac-memb-list-status">
                    <div class="frac-memb-list-status-off" v-if="member.id < 0"><span></span>Ofline</div>
                    <div class="frac-memb-list-status-on" v-else><span></span>Online</div>
                </div>
                <div class="frac-memb-list-name">{{member.username.replace('_', ' ')}}</div>
                <div class="frac-memb-list-rank">{{member.rank}}</div>
                <div class="frac-memb-list-up" @click="setRank(member.username, member.rank)">{{loc('mmain:frac:memb:setrank')}}</div>
                <div class="frac-memb-list-kick" @click="kick(member.username)">x</div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'

export default {
    computed:{
        ...mapGetters('localization', ['loc']),
        ...mapGetters('optionsMenu', ['fracMembers']),
        ...mapState('optionsMenu', [ 'fraction']),
        sortedMembers(){
            let sorted = [...this.fracMembers];
            switch (this.sortBy) {
                case "id":
                    sorted.sort((a, b)=>{
                        if(a.id < 0) return 1;
                        if(b.id < 0) return -1;
                        if(a.id > b.id) return 1;
                        if(a.id < b.id) return -1;
                        if(a.id == b.id) return 0;
                    });
                    break;
                case "status":
                    sorted.sort(function(a, b){
                        if(a.id < 0) return 1;
                        if(a.id == b.id) return 0;
                        return -1;
                    });
                    break;
                case "rank":
                    sorted.sort(function(a, b){                        
                        return a.rank - b.rank;
                    });
                    break;
                case "name":
                    sorted.sort(function(a, b){                        
                        if(a.username < b.username) return -1;                      
                        if(a.username > b.username) return 1;
                        return 0;
                    });
                    break;
                default:
                    break;
            }
            if(this.reverse) sorted.reverse()
            return this.filter.length > 2 ? sorted.filter(u=>u.username.toLowerCase().includes(this.filter.toLowerCase())) : sorted;
        }
    },
    data() {
        return {
            lastCheck: 0,
            sortBy: "name",
            filter: "",
            reverse: false
        }
    },
    methods: {
        selectSort(sort){
            if(this.sortBy == sort)
                this.reverse = !this.reverse;
            else{
                this.reverse=false
                this.sortBy = sort;
            }
        },
        kick(name){
            if(this.lastCheck > Date.now()) return;
            this.lastCheck = Date.now() + 1000;
            if(!this.fraction.canKick) {
                window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:nopermission", time: 3000});
                return;
            }
            this.setDialog({
                input: undefined,
                callback: (val)=>{
                   window.mp.triggerServer('fmenu:kick', name);
                    if(val) return;
                },
                value: '',
                placeholder: '',
                tittle: `mmain:frac:dialog:kick:tit@${name}`,
                subtittle: 'mmain:frac:dialog:kick:sub',
                bg: 'invite'
            });
        },
         setRank(name, rank){
            if(this.lastCheck > Date.now()) return;
            this.lastCheck = (Date.now() + 1000);
            if(!this.fraction.canRank) {
                window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:nopermission", time: 3000});
                return;
            }
            this.setDialog({
                input: 'number',
                callback: (val)=>{
                    if(+val < 0){
                        window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:dialog:data:wrong", time: 3000});
                    }else{
                       window.mp.triggerServer('fmenu:rank', name, +val);
                    }
                },
                value: rank,
                placeholder: '',
                tittle: `mmain:frac:dialog:rank:tit@${name}`,
                subtittle: 'mmain:frac:dialog:rank:sub',
                bg: 'invite'
            });
        },
        invitePlayer(){
            if(this.lastCheck > Date.now()) return;
            this.lastCheck = Date.now() + 1000;
            if(!this.fraction.canInvite){
                window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:nopermission", time: 3000});
                return;
            } 
            this.setDialog({
               input: 'number',
               callback: (val)=>{
                   window.mp.triggerServer('fmenu:invite', +val);
               },
               value: '',
               placeholder: 'mmain:frac:dialog:invite:pl',
               tittle: `mmain:frac:dialog:invite:tit`,
               subtittle: 'mmain:frac:dialog:rank:sub',
               bg: 'invite'
           });
        },
        ...mapMutations('optionsMenu', ['setDialog'])
    },
}
</script>

<style lang="scss" scoped>
.frac-memb{
    width: 50%;
    margin-top: 3.5rem;
    font-weight: 300;
    color: #fff;
    &-tit{
        display: flex;
        align-items: center;
        &-count{
            margin: 0 1rem;
            font-size: 6.5rem;
            background: linear-gradient(180deg, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
            background-clip: text;
            -webkit-text-fill-color: transparent;
        }
        &-text{
            font-size: 2.5rem;
            width: 13rem;
            letter-spacing: .05rem;
            line-height: 2.5rem;
        }
        &-invite{
            display: flex;
            align-items: center;
            margin-top: 2.5rem;
            &-txt{
                color: #B6D300;
                width: 5rem;
            }
            &-btn{
                margin: 0 .5rem;
                width: 2rem;
                height: 2rem;
                border-radius: 50%;
                border: 2px solid #B6D300;
                font-size: 1.3rem;
                display: flex;
                align-items: center;
                justify-content: center;
                &:hover{
                    background-color: #B6D300;
                }
            }
        }
    }
    &-list{
        margin-top: 1rem;
        width: 99%;
        height: 33rem;
        overflow: hidden scroll;        
        &::-webkit-scrollbar {
            width: .1rem;
        }
        &::-webkit-scrollbar-track {
            border: solid 0 transparent;
            margin: .1rem;
        }
        &::-webkit-scrollbar-thumb {
            border: solid 0 transparent;      
            background-color: rgba(#fff,.1);
        }
        &-head{
            width: 100%;
            display: flex;
            align-items: center;
            justify-content: space-between;
            height: 2.5rem;
            color: rgba(#fff, .5);
        }
        &-item{
            background-color: rgba(#fff, .05);
            height: 4rem;
            width: 100%;
            display: flex;
            align-items: center;
            justify-content: space-between;
            color: #fff;
            margin-top: .5rem;
            border: 1px solid rgba(#fff, .01);
            position: relative;
        }
        &-id{
            width: 4rem;
            text-align: center;
        }
        &-status{
            width: 7rem;
            &-on{
                display: flex;
                align-items: center;
                span{
                    margin-right: .3rem;
                    width: 0.5rem;
                    height: 0.5rem;
                    background: #7FB000;
                    box-shadow: 0px 0px .4rem #7FB000;
                    border-radius: 50%;
                }
            }
            &-off{
                display: flex;
                align-items: center;
                color: rgba(#fff, .5);  
                span{
                    margin-right: .3rem;
                    width: 0.5rem;
                    height: 0.5rem;
                    background: rgba(255, 255, 255, 0.3);
                    border-radius: 50%;  
                }
            }
        }
        &-name{
            width: 12rem;
        }
        &-rank{
            color: inherit;
            width: 5rem;
        }
        &-search{
            margin-left: auto;
            background-color: rgba(#fff, .05);
            height: 2rem;
            border: 1px solid rgba(#fff, .1);
            padding-left: 1rem;
            width: 12rem;
            color: #fff;
            &::placeholder{
                text-transform: uppercase;
                color: rgba(#fff, .5);;
            }
        }
        &-up{
            margin-left: auto;
            height: 2rem;
            width: 8rem;
            border: 2px solid #B6D300;
            display: flex;
            align-items: center;
            justify-content: center;
            letter-spacing: .04rem;
            &:hover{
                background-color:#B6D300;
            }
        }
        &-kick{
            width: 2rem;
            height: 2rem;
            font-size: 1.1rem;
            color: #fff;
            border: 1px solid rgba(#fff, .1);
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 .5rem;
        }
    }
}
.frac-sort-selected{
    color: #fff;
}
</style>