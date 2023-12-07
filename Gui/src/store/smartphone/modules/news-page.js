export default {
    namespaced: true,
    state: {
        priceSymbol: 3,
        priceImage: 3000,

        newsData: [
          {
            Id: 0,
            Title: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis p.',
            Picture: 'https://3dnews.ru/assets/external/illustrations/2019/10/14/995569/sm.screen17_1_.0.750.jpg',
            Sender: 'QURITOFLDMVJFK_TIROPCSHFL',
            Editor: 'TIROPCSHFL_QURITOFLDMVJFK',
            Simnum: 7874123
          },
          {
            Id: 1,
            Title: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis p.',
            Picture: '',
            Sender: 'QURITOFLDMVJFK_TIROPCSHFL',
            Editor: 'TIROPCSHFL_QURITOFLDMVJFK',
            Simnum: 7874123
          },
          {
            Id: 2,
            Title: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis p.',
            Picture: 'https://3dnews.ru/assets/external/illustrations/2019/10/14/995569/sm.screen17_1_.0.750.jpg',
            Sender: 'QURITOFLDMVJFK_TIROPCSHFL',
            Editor: 'TIROPCSHFL_QURITOFLDMVJFK',
            Simnum: 7874123
          },
          {
            Id: 3,
            Title: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis p.',
            Picture: 'https://3dnews.ru/assets/external/illustrations/2019/10/14/995569/sm.screen17_1_.0.750.jpg',
            Sender: 'QURITOFLDMVJFK_TIROPCSHFL',
            Editor: 'TIROPCSHFL_QURITOFLDMVJFK',
            Simnum: 7874123
          },
          {
            Id: 4,
            Title: 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis p.',
            Picture: 'https://3dnews.ru/assets/external/illustrations/2019/10/14/995569/sm.screen17_1_.0.750.jpg',
            Sender: 'QURITOFLDMVJFK_TIROPCSHFL',
            Editor: 'TIROPCSHFL_QURITOFLDMVJFK',
            Simnum: 7874123
          },
        ]
    },
    mutations: {
        setNewsData: function(state, data) {
            state.newsData = data
        },
        addNews: function(state, value) {
            state.newsData.unshift(value)
        }
    },

    actions: {
    }
}