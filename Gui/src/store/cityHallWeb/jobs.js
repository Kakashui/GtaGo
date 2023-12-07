export default {
  namespaced: true,
  state: {
    jobs: [
      {
        key: 'porter',
        img: 'porter',
        text: 'Грузчик',
        desc: 'У тебя уже есть первый уровень и водительские права класса B? Вперёд! Развози заказы по разным точкам и получай за это деньги. Только не забудь подкачаться. Нужны сила и выносливость, чтобы таскать тяжёлые грузы.',
        requiredLevel: 1,
      },
      {
        key: 'porter1',
        img: 'porter',
        text: 'Грузчик1',
        desc: 'У тебя уже есть первый уровень и водительские права кам и получай за это деньги. Только не забудь подкачаться. Нужны сила и выносливость, чтобы таскать тяжёлые грузы.',
        requiredLevel: 1,
      },
      {
        key: 'porter2',
        img: 'porter',
        text: 'Грузчик2',
        desc: 'Забудь подкачаться. Нужны сила и выносливость, чтобы таскать тяжёлые грузы.',
        requiredLevel: 1,
      },
      {
        key: 'porter3',
        img: 'porter',
        text: 'Грузчик3',
        desc: 'У тебя уже есть первый уровень. Развози заказы по разным точкам и получай за это деньги.',
        requiredLevel: 1,
      },      
      {
        requiredLevel: 1,
        //busdriver
        img: 'busdriver',
        text: 'mm_info_w_t_1',
        desc: 'mm_info_w_d_1',
        point: 1,
        key: false
      },
      {
        requiredLevel: 1,
        //electric
        img: 'electric',
        text: 'mm_info_w_t_2',
        desc: 'mm_info_w_d_2',
        point: 2,
        key: false
      },
      {
        requiredLevel: 1,
        //loader
        img: 'loader',
        text: 'mm_info_w_t_3',
        desc: 'mm_info_w_d_3',
        point: 3,
        key: false
      },
      {
        requiredLevel: 1,
        //farmer
        img: 'farmer',
        text: 'mm_info_w_t_4',
        desc: 'mm_info_w_d_4',
        point: 4,
        key: false
      },
      {
        requiredLevel: 1,
        // carThief
        img: 'cartheif',
        text: 'mm_info_w_t_5',
        desc: 'mm_info_w_d_5',
        point: 5,
        key: false
      },
      {
        requiredLevel: 1,
        //hunter
        img: 'hunter',
        text: 'mm_info_w_t_6',
        desc: 'mm_info_w_d_6',
        point: 6,
        key: false
      },
      {
        requiredLevel: 25,
        //pilot
        img: 'pilot',
        text: 'mm_info_w_t_7',
        desc: 'mm_info_w_d_7',
        point: 7,
        key: false
      },
      {
        requiredLevel: 1,
        //trucker
        img: 'truckdriver',
        text: 'mm_info_w_t_8',
        desc: 'mm_info_w_d_8',
        point: 8,
        key: false
      },
      {
        requiredLevel: 1,
        //taxiDriver
        img: 'taxi',
        text: 'mm_info_w_t_9',
        desc: 'mm_info_w_d_9',
        point: 9,
        key: true
      },
      {
        requiredLevel: 1,
        //loader
        img: 'loader',
        text: 'mm_info_w_t_10',
        desc: 'mm_info_w_d_10',
        point: 10,
        key: true
      },
      {
        requiredLevel: 1,
        //sawmill
        img: 'loader',
        text: 'mm_info_w_t_11',
        desc: 'mm_info_w_d_11',
        point: 11,
        key: true
      },
      {
        requiredLevel: 1,
        //mainer
        img: 'loader',
        text: 'mm_info_w_t_12',
        desc: 'mm_info_w_d_12',
        point: 12,
        key: true
      }
    ]
  },
  mutations:{
  }
}
