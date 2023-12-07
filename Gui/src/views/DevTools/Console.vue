<template>
    <div class="devconsole">
        <div class="devconsole-info" ref="list">
            <div class="devconsole-info_msg" v-for="(message, index) in messages" :key="index" :class="{'devconsole-info_out': message.output}">
                {{message.msg}}
            </div>
        </div>
        <textarea class="devconsole_ta" name="code" id="console" cols="30" rows="10" v-model="console.execstring"></textarea>
        <button @click="exec">Exec</button>
        <button @click="clear">Clear</button>
    </div>
</template>

<script>
import { mapMutations, mapState } from 'vuex';


export default {
    data() {
        return {
            alt: false,
            index: 0
        }
    },
    computed:{
        ...mapState('devTools', ['console']),
        messages(){
            return this.console.messages  
        }
    },
    methods: {
        ...mapMutations('devTools',['addConsoleMessage']),
        exec(){
            if(this.console.execstring.length > 5){
                window.mp.trigger("mal:script:exec", this.console.execstring);
                this.console.history.unshift(this.console.execstring);
            }
            this.addConsoleMessage({msg:this.console.execstring, output: true})
            this.clear();            
        },
        clear(){
            this.index = 0;
            this.console.execstring = '';
        },
        scrollBottom(){
            setTimeout(()=>{
                const list = this.$refs.list;
                if(list) list.scrollTop = list.scrollHeight;
            }, 0)
        },
        keyDown(e){
            if(e.keyCode === 17)
                this.alt = true;
            else if(e.keyCode === 13 && this.alt)
                this.exec();
            else if(e.keyCode === 8 && this.alt)
                this.clear();
            else if(e.keyCode === 38 && this.alt){
                if(this.console.history[this.index + 1]){
                    this.index++;
                    this.console.execstring = this.console.history[this.index]
                }else this.console.execstring = "";
            }                
            else if(e.keyCode === 40 && this.alt){
                if(this.console.history[this.index - 1]){
                    this.index--;
                    this.console.execstring = this.console.history[this.index]
                }else this.console.execstring = "";
            }
        },
        keyUp(e){ 
            if(e.keyCode === 17)
                this.alt = false;
        }
    },
    mounted(){
        document.addEventListener("keydown", this.keyDown);
        document.addEventListener("keyup", this.keyUp);
        document.getElementById("console").focus();
        
    },
    watch:{
        messages(){
            this.scrollBottom();
        }
    },
    created(){
        this.scrollBottom();
    },
    beforeDestroy(){
        document.removeEventListener("keydown", this.keyDown)
        document.removeEventListener("keyup", this.keyUp)
    }
}
</script>

<style lang="scss" scoped>
    .devconsole{
        z-index: 1000;
        position: fixed;
        right: 0;
        bottom: 2rem;
        width: 40vw;
        height: 25vh;
        &_ta{
            width: 100%;
            height: 100%;
            color: #fff;
            background-color: rgba(#000, .8);
        }
        &-info{
            position: fixed;
            right: 0;
            top: 0;
            height: 70vh;
            width: 40vw;
            padding: 2rem;
            background-color: rgba(0,0,0, .3);
            color: #fff;
            overflow-y: scroll;
            &_msg{
                margin-top: 1rem;
                width: 100%;
                text-align: right;
            }
            &_out{
                text-align: left;
                color: rgb(18, 206, 27);
            }
        }
    }
</style>