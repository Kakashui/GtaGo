import levels from '../../configs/farm/levels'

export default {
  namespaced: true,
  state: {
    levels,
    currentExp: 15000,
    nickname: 'Sandexa_Domina',
    seedsList: [
      {
        id: 'cabbage',
        minTime: 2,
        title: 'Семена капусты',
        cost: 20,
        img: 'cabbage'
      },
      {
        id: 'pumpkin',
        minTime: 2,
        title: 'Семена тыквы',
        cost: 20,
        img: 'pumpkin'
      },
      {
        id: 'zucchini',
        minTime: 2,
        title: 'Семена кабачка',
        cost: 20,
        img: 'zucchini'
      },
      {
        id: 'watermelon',
        minTime: 2,
        title: 'Семена арбуза',
        cost: 20,
        img: 'watermelon'
      },
      {
        id: 'tomato',
        minTime: 2,
        title: 'Семена помидора',
        cost: 20,
        img: 'tomato'
      },
      {
        id: 'strawberry',
        minTime: 2,
        title: 'Семена клубники',
        cost: 20,
        img: 'strawberry'
      },
      {
        id: 'raspberry',
        minTime: 2,
        title: 'Семена малины',
        cost: 20,
        img: 'raspberry'
      },
      {
        id: 'radish',
        minTime: 2,
        title: 'Семена редиса',
        cost: 20,
        img: 'radish'
      },
      {
        id: 'potato',
        minTime: 2,
        title: 'Семена картофеля',
        cost: 20,
        img: 'potato'
      },
      {
        id: 'orange',
        minTime: 2,
        title: 'Семена апельсина',
        cost: 20,
        img: 'orange'
      },
      {
        id: 'cucumber',
        minTime: 2,
        title: 'Семена огурца',
        cost: 20,
        img: 'cucumber'
      },
      {
        id: 'carrot',
        minTime: 2,
        title: 'Семена моркови',
        cost: 20,
        img: 'carrot'
      },
      {
        id: 'banana',
        minTime: 2,
        title: 'Семена банана',
        cost: 20,
        img: 'banana'
      },
      {
        id: 'apple',
        minTime: 2,
        title: 'Семена яблока',
        cost: 20,
        img: 'apple'
      },

    ],
    needfulList: [
      {
        id: 'watering-can',
        param: '1 литр',
        paramType: 'water.png', 
        title: 'Лейка',
        cost: 200,
        img: 'watering-can'
      },
      {
        id: 'box',
        param: '20кг',
        paramType: null, 
        title: 'Ящик',
        cost: 200,
        img: 'box'
      },
      {
        id: 'WateringBig',
        param: '10',
        paramType: null, 
        title: 'item_wateringbig',
        cost: 12000,
        img: 'wateringbig'
      }
    ],
    fertilizersList: [
      {
        id: 'strawberry-acc',
        minTime: 2,
        exp: 2,
        title: 'Удобрение для клубники',
        cost: 20,
        img: 'strawberry'
      },
      {
        id: 'cucumber-acc',
        minTime: 2,
        exp: 2,
        title: 'Удобрение для огурцов',
        cost: 20,
        img: 'cucumber'
      },
      {
        id: 'potato-acc',
        minTime: 2,
        exp: 2,
        title: 'Удобрение для картофеля',
        cost: 20,
        img: 'potato'
      },
      {
        id: 'strawberry-acc1',
        minTime: 2,
        exp: 2,
        title: 'Удобрение для клубники',
        cost: 20,
        img: 'strawberry'
      },
      {
        id: 'cucumber-acc1',
        minTime: 2,
        exp: 2,
        title: 'Удобрение для огурцов',
        cost: 20,
        img: 'cucumber'
      },
      {
        id: 'potato-acc1',
        minTime: 2,
        exp: 2,
        title: 'Удобрение для картофеля',
        cost: 20,
        img: 'potato'
      }
    ],
    levelsList: [
      {
        requiredLevel: 1,
        plantHoles: [
          {
            title: 'farmHouse_29',
            time: 'standart',
            exp: 1,
            fetus: 0,
            img: 'small-plant'
          },
        ]
      },
      {
        requiredLevel: 2,
        plantHoles: [
          {
            title: 'farmHouse_30',
            time: '- 5%',
            exp: 2,
            fetus: 1,
            img: 'small-plant'
          },
        ]
      },
      {
        requiredLevel: 3,
        plantHoles: [
          {
            title: 'farmHouse_31',
            time: '- 30%',
            exp: 2,
            fetus: 1,
            img: 'medium-plant'
          },
        ]
      },
      {
        requiredLevel: 4,
        plantHoles: [
          {
            title: 'farmHouse_32',
            time: '- 20%',
            exp: 3,
            fetus: 2,
            img: 'big-plant'
          },
        ]
      },
      {
        requiredLevel: 5,
        plantHoles: [
          {
            title: 'farmHouse_33',
            time: '- 15%',
            exp: 4,
            fetus: 2,
            img: 'medium-plant'
          },
        ]
      },
      {
        requiredLevel: 6,
        plantHoles: [
          {
            title: 'farmHouse_34',
            time: '- 25%',
            exp: 5,
            fetus: 3,
            img: 'big-plant'
          },
        ]
      },
      {
        requiredLevel: 7,
        plantHoles: [
          {
            title: 'farmHouse_35',
            time: '- 30%',
            exp: 4,
            fetus: 2,
            img: 'big-plant'
          },
        ]
      },
    ]
  },
  getters: {
    userLevel: (state) => {
      let currentLvl = Math.max.apply(null, state.levels.filter(item => item.exp <= state.currentExp).map(item => item.level))      
      let nextExperience = Math.min.apply(null, state.levels.filter(item => item.exp > state.currentExp).map(item => item.exp))
      if (nextExperience === Infinity)
        nextExperience = state.currentExp
      return { level: currentLvl, exp: nextExperience };
    }
  },
  mutations: {
    setSeedsList: function(state, data) {
      state.seedsList = data;
    },
    setNeedfulList: function(state, data) {
      state.needfulList = data;
    },
    setFertilizersList: function(state, data) {
      state.fertilizersList = data;
    },
    setExp: function(state, value) {
      state.currentExp = value;
    },
    setNickname: function(state, value) {
      state.nickname = value
    }

  },
  actions: {
  },
  modules: {
  }
}
