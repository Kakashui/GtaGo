<template>
   <div class="plist">
        <div class="head"><img src="/img/playerList/logo.png"><h1>ELITE RP</h1><span>ONLINE: {{ items.length }}</span></div>
        <div class="list">
            <table>
                <tr>
                    <th v-on:click="sortPlayers(0)" v-bind:class="{selectedSort: playerssort == 0, reversedSort: sortrevert}">ID</th>
                    <th v-on:click="sortPlayers(1)" v-bind:class="{selectedSort: playerssort == 1, reversedSort: sortrevert}">{{ loc('cef_282') }}</th>
                    <th v-on:click="sortPlayers(2)" v-bind:class="{selectedSort: playerssort == 2, reversedSort: sortrevert}">{{ loc('cef_283') }}</th>
                    <th v-on:click="sortPlayers(3)" v-bind:class="{selectedSort: playerssort == 3, reversedSort: sortrevert}">{{ loc('cef_284') }}</th>
                </tr>
                <tr v-for="(item, index) in items" :key="index">
                    <td>{{item[0]}}</td>
                    <td>{{item[1]}}</td>
                    <td>{{item[2]}}</td>
                    <td>{{item[3]}}</td>
                </tr>
            </table>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

export default {
    data() {
        return{
            active: false,
            playerssort: 0,
            sortrevert: 0            
        }
    },
    methods: {
        reset: function () {
            this.items = [];
            this.online = 0;
        },
        hide: function () {
            this.reset();
            this.active = false;
        },
        sortPlayers: function(sorttype) {

            if(this.playerssort != sorttype) {
                this.playerssort = sorttype;
                this.sortrevert = 0;
            }
            else this.sortrevert = !this.sortrevert;

            var sortarray = [];
            sortarray = this.items;

            sortarray.sort(function(a, b) {
                if (a[sorttype] > b[sorttype]) return 1;
                if (a[sorttype] < b[sorttype]) return -1;
                return 0;
            });
        
            if(this.sortrevert) sortarray.reverse();
            this.members = sortarray;
        }
    },
    computed:{
        ...mapGetters('localization',['loc']),
        ...mapState('playerList',['items'])
    }
}
</script>

<style lang="scss" scoped>
.plist {
    background-color: rgba(0, 0, 0, 0.9);
    height: 80%;
    margin-left: -300px;
    border-radius: 5px;
    position: absolute;
    width: 700px;
    left: 50%;
    top: 10%;
    overflow: hidden;
    font-family: "BPG Arial Caps", sans-serif;
}

.plist .head {
    margin: 10px;
    font-family: "BPG Arial Caps","sans-serif";
}

.plist img {
    float: left;
    width: 70px;
    margin: 20px;
    font-family: "BPG Arial Caps", sans-serif;
}

.plist h1 {
    float: left;
    margin: 35px;
    color: white;
    font-size: 18pt;
    font-family: "BPG Arial Caps", sans-serif;
}

.plist span {
    float: right;
    color: #14ba01;
    font-size: 18pt;
    margin: 40px 12px 0px 0px;
    font-family: "BPG Arial Caps", sans-serif;
}

.plist .list {
    color: white;
    width: 100%;
    margin: 40px 0;
    overflow: hidden;
    overflow-y: scroll;
    height: 87%;
    font-family: "BPG Arial Caps", sans-serif;
}

.plist table {
    width: 100%;
    text-align: center;
    font-family: "BPG Arial Caps", sans-serif;
}

.plist .selectedSort::after {
    content: url("/img/playerList/arrow.svg");
    padding: 0px 7px 0px 7px;
    width: 10px;
    position: absolute;
    display:inline-block;
    font-family: "BPG Arial Caps", sans-serif;
}

.plist .reversedSort::after {
    transform: rotate(-180deg);
    padding-bottom: 2px;
    font-family: "BPG Arial Caps", sans-serif;
}
</style>