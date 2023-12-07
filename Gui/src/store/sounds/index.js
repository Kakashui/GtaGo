import sounds from "./sounds";
export default {
    namespaced: true,
    state: {
        sounds,
        loaded:{},
        current: "",
        host: "https://language-api-zszc.onrender.com/sounds"
    },
    mutations: {
        play(state, {name, volume = 1, loop = false}){
            if(loop && state.loaded[state.current]){                    
                state.loaded[state.current].pause();
                state.loaded[state.current].currentTime = 0;
            }
            if(state.loaded[name]){
                state.loaded[name].loop = loop;                
                state.loaded[name].volume = volume;
                state.loaded[name].play()
            }else{
                if(state.sounds[name]){
                    state.loaded[name] = new Audio();
                    state.loaded[name].src = state.host + state.sounds[name];
                    state.loaded[name].loop = loop;                
                    state.loaded[name].volume = volume;
                    state.loaded[name].play();
                }
            }
            if(loop)state.current=name;
        },
        playLang(state, {name, lang, volume = 1, loop = false}){
            if(loop && state.loaded[state.current]){                    
                state.loaded[state.current].pause();
                state.loaded[state.current].currentTime = 0;
            }
            if(state.loaded[name]){
                state.loaded[name].loop = loop;                
                state.loaded[name].volume = volume;
                state.loaded[name].play()
            }else{
                if(state.sounds[name]){
                    state.loaded[name] = new Audio();
                    state.loaded[name].src = state.host + `/${lang}` + state.sounds[name];
                    state.loaded[name].loop = loop;                
                    state.loaded[name].volume = volume;
                    state.loaded[name].play();
                }
            }
            if(loop)state.current=name;
        },
        stop(state){
            if(state.loaded[state.current]){
                state.loaded[state.current].pause();                
                state.loaded[state.current].currentTime = 0;
                state.current = "";
            }
        }
    },
    actions: {
    },
    modules: {
    }
}
  