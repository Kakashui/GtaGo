export default {
    namespaced: true,
    state: {
        weatherData: {
            Now: {
                Weather: 'RAIN',
                Degrees: 0
            },
            Future: [
                {
                    Date: "2021-04-20T12:00:00",
                    Time: '12:00',
                    Weather: 'RAIN',
                    Degrees: 0
                },
                {
                    Date: "2029-04-20T13:00:00",
                    Time: '13:00',
                    Weather: 'RAIN',
                    Degrees: 0
                },
                {
                    Date: "2029-04-20T14:00:00",
                    Time: '14:00',
                    Weather: 'RAIN',
                    Degrees: 0
                },
                {
                    Date: "2029-04-20T15:00:00",
                    Time: '15:00',
                    Weather: 'RAIN',
                    Degrees: 0
                },
                {
                    Date: "2029-04-20T16:00:00",
                    Time: '16:00',
                    Weather: 'RAIN',
                    Degrees: 0
                },
                {
                    Date: "2029-04-20T17:00:00",
                    Time: '17:00',
                    Weather: 'RAIN',
                    Degrees: 0
                }
            ]
        },
    },
    mutations: {
        setFuture: function (state, data) {
            state.weatherData.Future = data
        },
        setNowWeather: function (state, value) {
            state.weatherData.Now.Weather = value
        },
        setNowDegrees: function (state, value) {
            state.weatherData.Now.Degrees = value
        }
    },

    actions: {
    }
}
