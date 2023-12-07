<template>
    <div class="chat" v-if="isShow" :class="{'chat-hidden': opacity}">
        <div class="chat-body">
            <div class="chat-messages" ref="list">
                <div 
                    class="chat-message" 
                    v-for="(message, index) in messages" 
                    :key="index"
                    v-html="message"
                ></div>
            </div>
            <div class="chat-input">
                <input type="text" v-model="input"  @change="onChnge" :disabled="!enabled" ref="text" placeholder="Enter text..." :style="{'opacity': enabled ? 1 : 0}">
            </div>
            <div class="chat-nav" :style="{'opacity': enabled ? 1 : 0}">
                <div class="chat-nav__item" 
                    v-for="(btn, index) in buttons" :key="index"
                    :class="{'active': current == index}"
                    @click="selectChatType(index)"
                >
                    {{loc(btn.name)}}
                </div>
                <div class="chat-nav__item chat-nav__clear" @click="clear">
                    Clear chat
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapState} from 'vuex'

export default {
    data() {
        return {
            messages:[],
            history:[],
            buttons:[
                {name: "IC", preffix: 'chat '},                
                {name: "OOC", preffix: 'b '},                
                {name: "chat_2", preffix: 'f '},                
                {name: "chat_3", preffix: 'fam '}
            ],
            input: "",
            current: 0,
            historyLength: 150,
            enabled: false,
            active: true,
            isShow: true,
            time: 20000,
            lastMsg: 0,
            opacity: false,
            interval: null,
            index: -1
        }
    },
    computed:{
        ...mapGetters('localization', ['loc']),
        ...mapState('familyMenu/controlPage', ['chatOptions']),
        ...mapState(['currentPage'])
    },
    methods: {
        selectChatType(index){
            this.current = index;
            this.focus()
        },
        nextChatType(){
            this.current++;
            if(this.current >= this.buttons.length)this.current = 0;
            this.focus()
        },
        onChnge(){
            //this.resetOpacity();
            this.index = -1;
            this.input = this.input.replace(/</g, '');
            this.input = this.input.replace(/\]/g, '');
            this.input = this.input.replace(/\\/g, '');
            this.input = this.input.replace(/&/g, 'and');
            this.input = this.input.replace(/#/g, '');
            if(this.input.length > 0){
                this.history.push(this.input);
                if(this.input[0] === '/') {
                    const value = this.input.substr(1);
                    if(value.length > 0)
                    window.mp.invoke("command", value);
                }else{
                    window.mp.invoke("command", `${this.buttons[this.current].preffix} ${this.input}`);
                }
            }
            this.enableChatInput(false);
        },
        tick(){
            if(!this.opacity && !this.enabled && this.lastMsg < Date.now()) this.opacity = true;
        },
        getColor(key){
            switch (key) {
                case "white": return '#FFFFFF'
                case "turquoise": return '#40FFD1'
                case "red": return '#FE5353'
                case "blue": return '#40A3FF'
                case "green": return '#3BEA62'
                case "orange": return '#FF891D'
                case "darkred": return '#EA3B3B'
				case "yellow": return '#EABE3B'
                case "citric": return '#FFDB1E'
                case "me": return "#F060A5"
                case "do": return "#EC328B"
                case "true": return "#74F060"
                case "false": return "#FFFFFF"
                case "warning": return "#EA3B3B"
                case "grey": return "#888888"
                case "greengo": return "#B6D300"
                default: return '#FFFFFF'
            }
        },
        getFamilyColor(){
            return this.chatOptions.currentColor;
        },
        getFamilyIcon(){
            return this.chatOptions.currentIcon;
        },
        push(type, msg, id, from, toId, to, friend){
            let tag;
            if(msg == undefined) {
                if(type === "RAGE MP: Connection lost. Reconnecting..."){
                    window.mp.trigger("onConnectionLost");
                }
                this.messages.push(`
                    <span style="color:${this.getColor("white")}">${this.loc(type)}</span>
                `)
            } else {
                switch (type) {
                    case 0: //обычный чат +
                        if(!friend) from = this.loc("chat_7");
                        this.messages.push(`
                            <span style="color:${this.getColor("white")};">${from} [${id}]: ${this.loc(msg)}</span>
                        `)
                        break;
					case 1: //крик +
                        if(!friend) from = this.loc("chat_7");
                        this.messages.push(`
                            <span style="color:${this.getColor("white")};">${from} [${id}] ${this.loc("chat_4")}: ${this.loc(msg)}</span>
                        `)
                        break;
					case 2: //оос чат  +
                        if(!friend) from = this.loc("chat_7");
                        this.messages.push(`
                            <span style="color:${this.getColor("white")};">${from} [${id}]: (( ${this.loc(msg)} ))</span>
                        `)
                        break;
                    case 3: //сообщение админа +
                         this.messages.push(`
                            <span style="color:${this.getColor("white")};"><span style="color:${this.getColor("turquoise")};">[A] ${from} [${id}]:</span> ${this.loc(msg)}</span>
                        `)
                        break;
                    case 4: //ответ от администратора+
                        this.messages.push(`
                            <span style="color:${this.getColor("white")};"><span style="color:${this.getColor("red")};">> ${this.loc("chat_5")} ${from}:</span> ${this.loc(msg)}</span>
                        `)
                        break;
                    case 5: //чат фракции+
                        if (toId) {
                            this.messages.push(`
                                <span style="color:${this.getColor("white")};"><span class="global-chat__icon global-chat__icon-circle global-chat__icon-circle-fraction"></span><span style="color:${this.getColor("blue")};">[F]:</span> ${this.loc(msg)}</span>
                            `)
                        } else {
                            this.messages.push(`
                                <span style="color:${this.getColor("white")};"><span class="global-chat__icon global-chat__icon-circle global-chat__icon-circle-fraction"></span><span style="color:${this.getColor("blue")};">[F] ${from} [${id}]:</span> ${this.loc(msg)}</span>
                            `)
                        }
                        
                        break;
                    case 6: //чат семьи+
                          this.messages.push(`
                            <span style="color:${this.getColor("white")};">
                                <span class="global-chat__icon global-chat__icon-circle global-chat__icon-circle-family" style="background-image: url(/img/familyMenu/chatOptions/icon-${this.getFamilyIcon()}.svg); background-color:${this.getFamilyColor()};"></span>
                                <span style="color:${this.getFamilyColor()};">[F] ${from} [${id}]:</span> 
                                ${this.loc(msg)}
                            </span>
                        `)
                        break;
                    case 7://чат гос структуры+
                      switch(toId){
                        case 7:
                          tag = "LSPD";
                          break;
                        case 8:
                          tag = "EMS";
                          break;
                        case 9:
                          tag = "FIB";
                          break;
                        case 14:
                          tag = "ARMY";
                          break;
                        case 15:
                          tag = "NEWS";
                          break;
                        default:
                          tag = "GOV";
                          break;
                      }
                        this.messages.push(`
                            <span style="color:${this.getColor("white")};">
                                <span class="global-chat__icon global-chat__icon-gos"></span>
                                <span style="color:${this.getColor("orange")};">
                                    [Dep](${tag}) ${from} [${id}]:
                                </span>
                                ${this.loc(msg).replace('_', ' ')}
                            </span>
                        `)
                        break;
                    case 8://бан/кик/мут от админа  +
                        this.messages.push(`
                            <span style="color:${this.getColor("darkred")};">${this.loc(msg)}</span>
                        `)
                        break;
                    case 9://ответ администратора игроку (для админа)
                         this.messages.push(`
                            <span style="color:${this.getColor("white")};">
                                <span style="color:${this.getColor("yellow")};">
                                    [ANSWER] ${from} [${id}] > ${to} [${toId}]:
                                </span> 
                                ${this.loc(msg)}
                            </span>
                        `)
						break;
					case 10://me+
                        if(!friend) from = this.loc("chat_7");
                        this.messages.push(`
                            <span style="color:${this.getColor("me")};">${from} [${id}] ${this.loc(msg)}</span> 
                        `)
                        this.addActionTo(id, msg);
						break;
					case 11://do+
                        if(!friend) from = this.loc("chat_7") + `[${id}]`;
                        this.messages.push(`
                            <span style="color:${this.getColor("do")};">${this.loc(msg)} ${from} [${id}]</span> 
                        `)
                        this.addActionTo(id, msg);
						break;
                    case 12://try
                        msg = msg.indexOf("False") === -1 ? msg.replace("True", `: <span style="color:${this.getColor("true")};">${this.loc('Core_298')}</span>`) : msg.replace("False", `: <span style="color:${this.getColor("false")};">${this.loc('Core_299')}</span>`);
                        if(!friend) from = this.loc("chat_7") + `[${id}]`;
                        this.messages.push(`
                            <span style="color:${this.getColor("do")};">${from} [${id}] ${this.loc(msg)}</span> 
                        `)
						break;
                    case 13://сообщение в гов или в орало
                    
                        switch(toId){                           
                            case 7: 
                                tag = "LSPD";
                                break;
                            case 8: 
                                tag = "EMS";
                                break;
                            case 9: 
                                tag = "FIB";
                                break;
                            case 14: 
                                tag = "ARMY";
                                break;
                            case 15: 
                                tag = "NEWS";
                                break;                          
                            default:
                                tag = "GOV";
                                break;        
                        }
						this.messages.push(`
                            <span style="color:${this.getColor("yellow")};">[${tag}] ${from}: ${this.loc(msg)}</span>
                        `)
						break;
					case 14://новости от админа
						this.messages.push(`
							<span style="color:${this.getColor("citric")};">${this.loc("chat_5")} ${from}: ${this.loc(msg)}</span>	
						`)
						break;
					case 15://мегафон для гос сотрудников
                        this.messages.push(`
                            <span style="color:${this.getColor("white")};">
                                <span class="global-chat__icon global-chat__icon-megaphone"></span>
                                <span style="color:${this.getColor("yellow")};">
                                    [Megaphone] ${from} [${id}]:
                                </span> 
                                ${this.loc(msg)}
                            </span>
                        `)
						break;
					case 16:
                        this.messages.push(`
                            <span style="color:${this.getColor("greengo")};">
                                ${from}: ${id}
                            </span>
                            <div class="sender-wrap">
                                <div class="global-chat__icon global-chat__icon-circle global-chat__icon-circle-call"></div>
                                <div class="global-chat__icon global-chat__icon-circle global-chat__icon-circle-message"></div> 
                                <div class="sender__nickname-tittle">${this.loc('chat_6')}: </div>
                                <div class="sender__nickname">${msg}</div>
                                <div class="sender__nickname-phone">${toId == -1 ? '' : `tel: ${toId}`}</div>
                            </div>
                        `)
						break;
					case 17:
                        this.messages.push(`
                            <span style="color:${this.getColor("warning")};">${this.loc(msg)}</span>
                        `)
						break;
					case 18:
                        this.messages.push(`
                            <span style="color:${this.getColor("white")};">${this.loc(msg)}</span>
                        `)
						break;
                }              
            }
            if(this.historyLength < this.messages.length) this.messages.shift();           
            //this.resetOpacity(); 
            this.scrollBottom();
            return id;
        },
        scrollBottom(){
            setTimeout(()=>{
                const list = this.$refs.list;
                if(list) list.scrollTop = list.scrollHeight;
            }, 0)
        },
        clear(){
            this.messages = [];
            if(this.enabled){
                window.setTimeout(()=>{
                    this.$refs.text.focus()
                },0);
            }
        },
        activate(toggle){
            this.active = toggle;
            if(!toggle && this.enabled) this.enableChatInput(false);
        },
        focus(){
            window.setTimeout(()=>{
                if(this.$refs.text) this.$refs.text.focus()
            },0);
        },
        show(toggle){
            this.isShow = toggle
            this.enabled = false;
            if(toggle) this.scrollBottom();
        },
        enableChatInput(enable){
            if(this.active){            
                window.mp.invoke("focus", enable);
                window.mp.invoke("setTypingInChatState", enable);
                this.enabled = enable;
                if (enable)
                    this.focus();
                else                    
                    window.mp.trigger("cahat:api:disable");

            }else {
                window.mp.trigger("cahat:api:disable");  
                this.enabled = false;
            }
            this.input = "";           
        },
        resetOpacity(){
            this.lastMsg = Date.now() + this.time;
            this.opacity = false;
        },
        addActionTo(id, msg){
            window.mp.trigger("tag:add:action", id, this.loc(msg))
        },
        keyUp(e){
            e.preventDefault();
            switch (e.keyCode) {
                // case 84: //t
                //     if(this.currentPage === undefined || this.currentPage === ""){
                //         if(!this.enabled && this.active)
                //             this.enableChatInput(true);
                //     }
                   
                //     break;

                case 9: //tab
                    if(this.enabled)
                        this.nextChatType();
                    this.scrollBottom();
                    break;
                

                case 13: //enter
                    if(this.enabled)
                        this.onChnge();
                    break;

                case 38://up
                    if(!this.enabled) return;
                    if(this.history.length === 0) return;
                    this.index -= 1;
                    if(this.index < 0) this.index = this.history.length - 1;
                    this.input = this.history[this.index];
                    break;
                
                case 40://down
                    if(!this.enabled) return;
                    if(this.history.length === 0) return;
                    this.index += 1;
                    if(this.index >= this.history.length) this.index = 0;
                    this.input = this.history[this.index];
                    break;
                default:
                    break;
            }
        }
    },
    mounted() {
        window.chatAPI = {
            push: this.push,
            clear: this.clear,
            activate: this.activate,
            show: this.show,
            enable: this.enableChatInput
        }
        document.addEventListener("keyup", this.keyUp);
    },
    beforeDestroy(){
        document.removeEventListener("keyup", this.keyUp);
    }
}
</script>

<style lang="scss" scoped>

.chat{
	//perspective: 25vh;
	position: fixed;
	left: 1.5rem;
	top: 1.25rem;
	transition: all 1s;
    
	&-body{
		width: 28rem;
		height: 40vh;
		position: relative;
		display: flex;
		flex-direction: column;
		justify-content: flex-end;
		align-items: center;
	}
	&-messages{
		width: 100%;
		overflow: hidden auto;
		&::-webkit-scrollbar {
			width: .3rem;
		}
		&::-webkit-scrollbar-track {
			border-radius: 5px;
			background-color: rgba(#444, 0);
			opacity: 0;
		}
		&::-webkit-scrollbar-thumb {
			border-radius: 5px;
			background-color: rgba(#fff, 0);
		}
	}
	&-message{
		width: 100%;
		color: #fff;
		margin-top: .3rem;
		font-size: .9rem;
		line-height: 1.1rem;
		letter-spacing: 0.02em;
		text-shadow: 0 0 .25rem rgba(0, 0, 0, 1);
        margin-left: .1rem;
	}
	&-input{
		height: 2.5rem;
		bottom: 0;
		width: 100%;
		margin: 1.5rem 0 .25rem 0;
		border: 0;
        input{
            width: 100%;
            height: 100%;
            color: #FFF;
            background: rgba(0, 0, 0, 0.5);
            padding: 0 .5rem;
            font-size: 1rem;
            letter-spacing: 0.02em;
            display: flex;
            align-items: center;
            border-radius: .2rem;
            justify-content: flex-start;
            &::placeholder{
                color: #FFF;
                font-size: .9rem;
            }          
        }
	}
	&-nav{
		display: flex;
		align-items: center;
		justify-content: flex-start;
		margin-top: .25rem;
		align-self: flex-start;
        width: 100%;    
		&__item{
			display: flex;
			align-items: center;
			justify-content: center;
			height: 1.4rem;
			min-height: 1.4rem;
			padding: 0 .4rem;
			min-width: 2rem;
			margin-right: .25rem;
			background: rgba(0, 0, 0, 0.5);
			border-radius: .2rem;
			font-size: .75rem;
			letter-spacing: 0.02em;
			color: rgba(255, 255, 255, 0.5);
			&.active, &:hover{
				background: #B6D300;
				color: rgba(255, 255, 255, 1);
				box-shadow: 0 0 1rem 0 rgba(168, 195, 2, 0.8);
				transition: .3s;
			}
		}
        &__clear{
            margin-left: auto;
        }
	}
	&-hidden{
		opacity: 0.1;
	}
}
</style>

<style lang="scss">
    .global-chat__icon{
        width: 1.3rem;
        min-width: 1.3rem;
        height: 1.3rem;
        margin-right: .3rem;
        margin-bottom: -.3rem;
        display: inline-block;
        position: relative;
        &:before{
            content: '';
            width: 1.2rem;
            height: 1.2rem;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translateX(-50%) translateY(-50%);
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
        }
        &-circle{
            border-radius: 50%;
            background-color: rgba(255, 255, 255, 0.2);
            &:before{
                width: .7rem;
                height: .7rem;
            }
            &-fraction{
                &:before{
                    background-image: url('/img/chat/icon-fraction.svg');
                }
            }
            &-family{
                background-repeat: no-repeat;
                background-position: center;
                background-size: 60%; 
            }
            &-call{
                &:before{
                    background-image: url('/img/chat/icon-call.svg');
                }
            }
            &-message{
                &:before{
                    background-image: url('/img/chat/icon-message.svg');
                }
            }
        }
        &-gos{
            &:before{
                background-image: url('/img/chat/icon-gos.svg');
            }
        }
        &-megaphone{
            &:before{
                background-image: url('/img/chat/icon-megaphone.svg');
            }
        }
    }
    .sender-wrap{
        display: flex;
        align-items: center;
        justify-content: flex-start;
        font-size: .75rem;
        font-weight: 300;
        padding-left: .3rem;
        margin-top: .2rem;
        margin-bottom: .5rem;
        .global-chat__icon-circle-call{
            background: #B6D300;
            box-shadow: 0 0 .6rem 0 #A8C302;
        }
        .global-chat__icon-circle-message{
            margin-right: .5rem;
        }
        .sender__nickname{
            font-weight: 400;
            &-tittle{
                margin-right: .2rem;
            }
            &-phone{
                margin-left: .2rem;
            }
        }
    }
</style>
