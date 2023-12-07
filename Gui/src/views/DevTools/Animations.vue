<template>
    <div class="tools-anim">
        <div class="tools-anim-lists">
            <div class="tools-anim-list">
                <div class="tools-anim-list-item" 
                   v-for="(dict, index) in animations" 
                   :key="`dict_${index}`" 
                   @click="selectDict(index)"
                   :class="{'tools-anim-selected': anim.currentDict == index}"
                >
                    {{dict.dictName}}
                </div>
            </div>
            <div class="tools-anim-list">
                <div class="tools-anim-list-item" 
                    v-for="(a, index) in animList" 
                    :key="`anim_${index}`"
                    @click="selectAnim(index);playAnim();"
                    :class="{'tools-anim-selected': anim.currentAnim == index}"
                >
                    {{a}}
                </div>
            </div>
        </div>
        <div class="tools-anim-settings">
            <p>Anim flags:</p>
            <input type="checkbox"  v-for="(flag, index) in anim.flags" :key="index"   v-model="anim.flags[index]" @change="changeFlag">
        </div>
        <div class="tools-anim-settings">
            <p>Options:</p>
            <label for="onServer">Play on server</label> 
            <input type="checkbox" id="onServer" v-model="anim.onServer" @change="changeFlag">       
            <label for="disCA">Dis CA</label> 
            <input type="checkbox" id="disCA" v-model="anim.disCA" @change="changeFlag">           
            <label for="onServer">plyer id:</label> 
            <input type="number" v-model="anim.playerId">
            <label for="onServer">start time:</label> 
            <input type="number" v-model="anim.start">
        </div>  
      
        <div class="tools-anim-buttons">
            <button @click="playAnim">Play anim</button>
            <button @click="stopAnim">Stop anim</button>
            <button @click="back">Back</button>
        </div>
    </div>
</template>

<script>
import {mapState, mapMutations} from 'vuex'

export default {
    data() {
        return {
            check: true
        }
    },
  computed: {
    ...mapState('devTools', ['animations','anim']),
    animList(){
        return this.animations[this.anim.currentDict]["anims"]
    },
    currentDict(){
        return this.animations[this.anim.currentDict].dict;
    },
    currentAnim(){
        return this.animList[this.anim.currentAnim];
    }
  },
 
    methods: {
        ...mapMutations('devTools', ['selectDict', 'selectAnim', 'changeAnimFlag', 'selectMenu']),
        playAnim(){
            window.mp.trigger("devtool:anim:play", this.currentDict, this.currentAnim, JSON.stringify(this.anim.flags), this.anim.onServer, this.anim.playerId, this.anim.disCA, this.anim.start);
        },
        changeFlag(){
            this.playAnim()
        },
        stopAnim(){
            window.mp.trigger("devtool:anim:stop")
            //this.close();
        },
        close(){
            this.$store.commit('setPage', undefined)
        },
        back(){
            this.selectMenu(-1);
        }
    },
}
</script>

<style lang="scss" scoped>
.tools-anim{
    height: 90%;
    &-lists{        
        display: flex;
        justify-content: space-around;
        margin-top: .4rem;
        height: 50%;
    }
    &-list{
        width: 45%;
        background-color: rgba(#000, .5);
        margin: .2vh;
        text-align: center;
        &-item{
            color: #fff;
            padding: .4rem;
            &:hover{
                background-color: rgba(#fff, .2);
            }
        }
    }
    &-selected{
        background-color: rgba(#fff, .5);
    }
    &-settings{
        margin-top: .7rem;
        p{
            color: #fff;
            margin: 0 .7rem;
        }
        input{
            background-color: rgba(#000, .2);
            color: #fff;
            padding: .1rem;
            width: 3rem;
            border: 1px solid #fff;
            margin: 1rem 0;
        }
        label{
            color: #fff;
        }
    }
    &-buttons{
        margin-top: 2rem;
        button{
            margin: 0 .7rem;
            padding: .4rem .8rem;
            background-color: rgba(#000, .5);
            border: 1px solid rgba(#000, 0);
            color: #fff;
            &:hover{
                cursor: pointer;
                border: 1px solid rgba(#fff, 1);
            }
        }
    }
}
</style>