export default {
  namespaced: true,
  state: {
    minBail: 10000,
    maxBail: 1000000,
    prisoners: [  
      // {
      //   id: 0,
      //   name: 'Alexey Vsevolodov',
      //   passport: '333 333 33',
      //   sex: 1,
      //   number: '333 333 3333',
      //   wantedLevel: 1,
      //   licenses: 'A B C D E F', 
      //   arrestDate: '02.02 14:00',
      //   detained: 'SANDREXA_DOMINA12344 ID: 123456',
      //   acticle: 'Очень длинное название статьи с полноценной информацией',
      //   strippedOfLicenses: '',
      //   isEdit: true, // состояние определения освобождения под залог (запретить выпускать под залог/выпустить под залог)
      //   bail: null, // сумма залога
      //   bailOfficer: null, // кто выпустил, + время
      //   releaseDate: null // дата выхода, если запрет освобождения под залог
      // },
      // // Вид объекта, если запретили выпускать под залог
      // {
      //   id: 1,
      //   name: 'Alexey Vsevolodov',
      //   passport: '333 333 33',
      //   sex: 0,
      //   number: '333 333 3333',
      //   wantedLevel: 1,
      //   licenses: 'A B C D E F', 
      //   arrestDate: '02.02 14:00',
      //   detained: 'SANDREXA_DOMINA12344 ID: 123456',
      //   acticle: 'Очень длинное название статьи с полноценной информацией',
      //   strippedOfLicenses: 'A B C D E F',
      //   isEdit: false,
      //   bail: null,
      //   bailOfficer: null,
      //   releaseDate: '20.12.21 12:20'
      // },
      // // Вид объекта, если выпустили под залог
      // {
      //   id: 2,
      //   name: 'Alexey Vsevolodov',
      //   passport: '333 333 33',
      //   sex: 0,
      //   number: '333 333 3333',
      //   wantedLevel: 1,
      //   licenses: 'A B C D E F', 
      //   arrestDate: '02.02 14:00',
      //   detained: 'SANDREXA_DOMINA12344 ID: 123456',
      //   acticle: 'Очень длинное название статьи с полноценной информацией',
      //   strippedOfLicenses: 'A B C D E F',
      //   isEdit: false,
      //   bail: '$2 200 000',
      //   bailOfficer: 'SANDREXA_DOMINA12344 22.02 14:00',
      //   releaseDate: null
      // },
    ]
  },

  mutations: {
    setPrisoners: function (state, data) {
      state.prisoners = data;
      state.prisoners.sort((a, b) => { return b.id-a.id; });
    },
    updatePrisoners: function (state, value) {
      const index = state.prisoners.findIndex(item => item.id == value.id);
      if (index > -1)
      {
        state.prisoners[index] = value;
      }
      else
        state.prisoners.push(value);
      state.prisoners.sort((a, b) => { return b.id-a.id; });
    }
   
  }
}
