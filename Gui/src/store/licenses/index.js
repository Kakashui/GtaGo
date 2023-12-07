export default {
  namespaced: true,

  state: {
    nickname: 'Sandrexa_domina',
    gender: 0, // 0 - ж, 1 - м
    bithday: '20.12.05',
    licenses: [
      // {
      //   type:'vehicle',
      //   name:'A',
      //   img: 'bike',
      //   date:'2020.03.20',
      // },
      // {
      //   type:'vehicle',
      //   name:'B',
      //   img: 'car',
      //   date:'2020.04.20',
      // },
      // {
      //   type:'vehicle',
      //   name:'C',
      //   img: 'bus',
      //   date:'2020.05.22',
      // },
      // {
      //   type:'vehicle',
      //   name:'D',
      //   img: 'truck',
      //   date:'2020.06.20',
      // },
      // {
      //   type:'vehicle',
      //   name:'E',
      //   img: 'helicopter',
      //   date:'2020.07.21',
      // },
      // {
      //   type:'vehicle',
      //   name:'F',
      //   img: 'flight',
      //   date:'2020.07.21',
      // },
      // {
      //   type:'vehicle',
      //   name:'G',
      //   img: 'ship',
      //   date:'2020.07.21',
      // },
      // {
      //   type:'medical',
      //   name:'med',
      //   dateEnd:'2020.07.21',
      // },
      // {
      //   type:'weapon',
      //   name:'gun',
      //   date:'2020.07.21',
      // },
      // {
      //   type:'job',
      //   name:'taxi',
      //   date:'2020.07.21',
      //   img: 'taxi'
      // },
      // {
      //   type:'job',
      //   name:'Сборка оружия',
      //   date:'2020.07.21',
      //   img: 'weapon'
      // },
      // {
      //   type:'job',
      //   name:'Добыча и переплавка руды',
      //   date:'2020.07.21',
      //   img: 'iron'
      // },
      // {
      //   type:'job',
      //   name:'Охота',
      //   date:'2020.07.21',
      //   img: 'hunting'
      // },
      // {
      //   type:'job',
      //   name:'Дальнобой',
      //   date:'2020.07.21',
      //   img: 'truck'
      // },
      // {
      //   type:'job',
      //   name:'Рыбалка',
      //   date:'2020.07.21',
      //   img: 'fishing'
      // },
      // {
      //   type:'military',
      //   name:'Военный билет',
      //   date:'2020.07.21',
      // },
    ]
  },

  mutations: {
    setData:function(state, data) {
      state.nickname = data.nickname;
      state.gender = data.gender;
      state.bithday = data.bithday
      state.licenses = data.lic      
    }
  }
}
