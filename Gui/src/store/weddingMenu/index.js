export default {
    namespaced: true,

    state: {
        isWeddingComplete: false,
        congratulationsName: '',
        disableExit: true,
        weddingName: ''
    },

    mutations: {
        setIsWeddingComplete: function(state, value) {
            const audio = document.getElementById('completeWeddingSound')
            state.isWeddingComplete = value

            if (value) {
                audio.volume = 0.6
                audio.play()
            }
            state.disableExit = true

            setTimeout(() => {
                state.disableExit = false
            }, 10000)
        },

        setCongratulationsName: function(state, name) {
            state.congratulationsName = name
        },

        setWeddingName: function(state, name) {
            state.weddingName = name
        }
    }
}