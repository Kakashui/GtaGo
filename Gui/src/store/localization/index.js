export default {
    namespaced: true,
    state: {
        local: null,
        apiURL: "https://language-api-zszc.onrender.com/api/localize/", //https://translate.mikzzz.ru/api/localize/
        listUrl: "https://language-api-zszc.onrender.com/api/localize/list", 
        langs:{
          list:null,
          cur: 'no'
        }
    },
    actions: {
       translateAction({getters}, {id, msg}){           
            window.mp.trigger("tag:add:action", id, getters["loc"](msg))
       },
       setLang({state, dispatch}, lang){
            window.mp.trigger("language:save", lang);
            if(lang === state.langs.cur) return;
            state.langs.cur = lang            
            //document.getElementsByTagName("html")[0].lang = state.langs.cur;
            dispatch("loadLanguage");
        },
        async loadLanguage({state, dispatch}){
            let response = await window.fetch(state.apiURL + state.langs.cur);
            if(response.ok){
                state.local = await response.json();
            }else{
                setTimeout(()=>{
                    dispatch("loadLanguage");
                }, 1000)
            }
        },
        async loadLangs({state}){
            let response = await window.fetch(state.listUrl);
            if(response.ok){
                state.langs.list = await response.json();
            }else{
                state.langs.list = {"ru":"Russian"};
            }
        }
    },
    modules: {},
    getters:{
        loc: state => msg =>{
            if(state.local === null || typeof msg !== "string") return msg;           
            const array = msg.split('@')
            let key = array[0];
            if(state.local[key] === undefined){
                if(process.env.NODE_ENV == 'development') window.console.log(`Key ${key} don't have value`);
                return key;
            }else{
                let message = state.local[key];
                const params = array.slice(1);
                if(params.length > 0){
                    for (let index = 0; index < params.length; index++) {
                        const param = params[index];
                        if(message.indexOf(`{${index}}`) === -1)
                            message += ` ${state.local[param] ? state.local[param] : param}`
                        else
                            message = message.replace(`{${index}}`, state.local[param] ? state.local[param] : param);
                    }
                }
                return message;
            }
            
        },
        font(state){
            switch (state.langs.cur) {
                case 'ge': return 'Georgian';
                default: return 'Russian'
            } 
        }
    }
}