export default {
  namespaced: true,
  state: {
    // вкл/выкл звук
    isVolume: true,
    // вкл/выкл демонстрации
    isStream: false,
    // Текущий работник
    workerId: 12345,
    // Цена за 1 символ объявления
    pricePerSymbol: 3,
    // Доп. цена за премиум объявление (с картинкой)
    pricePrePremium: 3000,
    name: 'Ivan_Ivan',
    // Объект для конкретного объявления, не трогать
    currentAd: {
    },
    currentPage: 'MainPage',
    // Объект для управления модальным окном
    modal: {
      show: false,
      currentModal: null,
      data: null,
    },
    adsList: [
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 124453,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'ТТаким образом укрепление и развитие структуры позволяет оценить значение новых предложений. Значимость этих проблем настолько очевидна, что дальнейшее развитие различных форм деятельности требуют от нас Таким образом укрепление и развитие структуры позволяет оценить значение новых предложений. Значимость этих проблем настолько очевидна, что дальнейшее развитие различных форм деятельности требуют от нас Таким образом ',
        /**
         * @param Status
         * @values 
         * 'Created' - объявление на проверке
         * 'Compleate' - объявление утверждено
         * 'Canceled' - объявление отклонено
         */
        Status: 'Created',
        Editor: '',
        PrimeAdvert: false,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 124453341,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Таким образом укрепление и развитие структуры позволяет оценить. Значимость этих проблем действительно важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Compleate',
        Editor: 'Editor Name',
        PrimeAdvert: false,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 1244,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых предложений. Значимость этих  важно, поскольку текст слишком действительно важно, поскольку текст слишком длинный и нужно написать что-то  длинный и нужно написать что-то еще.',
        Status: 'Compleate',
        Editor: 'Editor Name',
        Picture: 'https://img1.goodfon.ru/wallpaper/nbig/1/d7/bmw-m5-f10-black-1922.jpg',
        PrimeAdvert: true,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 2974301943,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых предложений. Значимость  действительно важно, поскольку текст слишком длинный и нужно написать что-то  этих  важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Created',
        Editor: 'Editor Name',
        PrimeAdvert: true,
      },
      {
        Sender: 'Test Testovich',
        SenderUUID: 408244932,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых действительно важно, поскольку текст слишком длинный и нужно написать что-то  этих  важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Created',
        Editor: '',
        Picture: 'https://img1.goodfon.ru/wallpaper/nbig/1/d7/bmw-m5-f10-black-1922.jpg',
        PrimeAdvert: true,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 1531,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых действительно важно, поскольку текст слишком длинный и нужно написать что-то  предложений. Значимость этих  важно, поскольку текст действительно важно, поскольку текст слишком длинный и нужно написать что-то  и нужно написать что-то еще.',
        Status: 'Canceled',
        Editor: 'Editor Name',
        PrimeAdvert: false,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 23,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых действительно важно, поскольку текст слишком длинный и  важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Compleate',
        Editor: 'Editor Name',
        Picture: 'https://img2.goodfon.ru/wallpaper/nbig/c/37/bmw-m5-e39-blue.jpg',
        PrimeAdvert: true,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 1,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых предложений. Значимость этих  важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Created',
        Editor: '',
        Picture: 'https://img2.goodfon.ru/wallpaper/nbig/c/37/bmw-m5-e39-blue.jpg',
        PrimeAdvert: true,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 98765,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых предложений. Значимость этих  важно, действительно важно, поскольку текст слишком длинный и нужно написать что-то   текст слишком длинный и нужно написать что-то еще.',
        Status: 'Created',
        Editor: '',
        Picture: 'https://img2.goodfon.ru/wallpaper/nbig/c/37/bmw-m5-e39-blue.jpg',
        PrimeAdvert: true,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 456789,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить действительно важно, поскольку текст слишком длинный и нужно написать что-то  новых предложений. Значимость этих  важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Canceled',
        Editor: 'Editor Name',
        PrimeAdvert: false,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 987654321,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых предложений. Значимость этих  важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Created',
        Editor: '',
        PrimeAdvert: false,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 496987,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить значение новых предложений. Значимость этих  действительно важно, поскольку текст слишком длинный и нужно написать что-то , поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Created',
        Editor: '',
        Picture: 'https://img2.goodfon.ru/wallpaper/nbig/c/37/bmw-m5-e39-blue.jpg',
        PrimeAdvert: true,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 29847923,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет действительно важно, поскольку текст слишком длинный и нужно написать что-то  значение новых предложений. Значимость этих  важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Compleate',
        Editor: 'Editor Name',
        Picture: 'https://img2.goodfon.ru/wallpaper/nbig/c/37/bmw-m5-e39-blue.jpg',
        PrimeAdvert: true,
      },
      {
        Sender: 'Имя Фамилия',
        SenderUUID: 84529663,
        DateCreate: 1699999999,
        Simnum: 1818186654,
        Title: 'Позволяет оценить действительно важно, поскольку текст слишком длинный и нужно написать что-то важно, поскольку текст слишком длинный и нужно написать что-то еще.',
        Status: 'Canceled',
        Editor: 'Editor Name',
        Picture: 'https://img2.goodfon.ru/wallpaper/nbig/c/37/bmw-m5-e39-blue.jpg',
        PrimeAdvert: true,
      },
    ],

    workersList: [
      {
        workerId: 1,
        isOnline: true,
        workerName: 'Sandera_Pomina',
        workerRank: 1,
        editedPerDay: 100,
        editedPerMonth: 600,
        rating: 100
      },
      {
        workerId: 12345,
        isOnline: false,
        workerName: 'Sandrexa_Domina',
        workerRank: 2,
        editedPerDay: 5,
        editedPerMonth: 234,
        rating: 6400
      },
      {
        workerId: 3,
        isOnline: true,
        workerName: 'Sandera_Lomina',
        workerRank: 3,
        editedPerDay: 1,
        editedPerMonth: 645,
        rating: 9000
      },
      {
        workerId: 4,
        isOnline: true,
        workerName: 'Sandera_Fomina',
        workerRank: 4,
        editedPerDay: 93,
        editedPerMonth: 43524,
        rating: 99
      },
      {
        workerId: 5,
        isOnline: true,
        workerName: 'Sandera_Zomina',
        workerRank: 5,
        editedPerDay: 67,
        editedPerMonth: 4321,
        rating: 74327
      },
      {
        workerId: 6,
        isOnline: true,
        workerName: 'Sandera_Womina',
        workerRank: 6,
        editedPerDay: 45,
        editedPerMonth: 323,
        rating: 423
      },
      {
        workerId: 7,
        isOnline: false,
        workerName: 'Sandera_Momina',
        workerRank: 7,
        editedPerDay: 75857,
        editedPerMonth: 2732348,
        rating: 2943892734
      },
      {
        workerId: 8,
        isOnline: true,
        workerName: 'Sandera_Xomina',
        workerRank: 8,
        editedPerDay: 1,
        editedPerMonth: 423,
        rating: 42658762
      },
      {
        workerId: 9,
        isOnline: true,
        workerName: 'Sandera_Somina',
        workerRank: 9,
        editedPerDay: 100,
        editedPerMonth: 54222,
        rating: 53
      },
    ],

    ranksList: [
      {
        rankId: 1,
        rankName: 'Стажер',
        income: 980,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: false,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: false,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: false,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 2,
        rankName: 'Корректор',
        income: 1500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: false,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: false,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 3,
        rankName: 'Радиоведущий',
        income: 1500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: false,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 4,
        rankName: 'Сотрудник медиа отдела',
        income: 1500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: false,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 5,
        rankName: 'Корреспондент',
        income: 2500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: false,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 6,
        rankName: 'Репортер',
        income: 2500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: false,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 7,
        rankName: 'Редактор',
        income: 2500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 8,
        rankName: 'Журналист',
        income: 4500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 9,
        rankName: 'Оператор',
        income: 4500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 10,
        rankName: 'Старший редактор',
        income: 4500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: false,
          },
        ]
      },
      {
        rankId: 11,
        rankName: 'Сотрудник отдела кадров',
        income: 4500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 12,
        rankName: 'Заместитель главы эфирного отдела',
        income: 6500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 13,
        rankName: 'Исполнительный продюсер',
        income: 6500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 14,
        rankName: 'Заместитель главы отдела редакции',
        income: 6500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 15,
        rankName: 'Заместитель главы отдела кадров',
        income: 6500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 16,
        rankName: 'Глава эфирного отдела',
        income: 8500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 17,
        rankName: 'Генеральный продюсер',
        income: 8500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 18,
        rankName: 'Глава отдела редакции',
        income: 8500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 19,
        rankName: 'Глава отдела кадров',
        income: 8500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 20,
        rankName: 'Заместитель генерального директора',
        income: 9500,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
      {
        rankId: 21,
        rankName: 'Генеральный директор',
        income: 15000,
        accessList: [
          {
            id: 0,
            desc: 'Редакция обычных объявлений',
            value: true,
          },
          {
            id: 1,
            desc: 'Редакция премиум объявлений',
            value: true,
          },
          {
            id: 2,
            desc: 'Выход в прямой эфир',
            value: true,
          },
          {
            id: 3,
            desc: 'Скрывать редакцию',
            value: true,
          },
        ]
      },
    ]
  },

  mutations: {
    setCurrentPage: function(state, value) {
      state.currentPage = value
    },
    setCurrentAd: function(state, obj) {
      state.currentAd = obj
    },
    setModal: function(state, {show, currentModal, data}) {
      state.modal.show = show
      state.modal.currentModal = currentModal
      state.modal.data = data
    },
    setAdsList: function(state, data) {
      state.adsList = data
    },
    updateAdvert: function(state, value) {
      const index = state.adsList.findIndex(item => item.Id == value.Id);
      if (index >= 0)
        state.adsList.splice(index, 1, value)
      else
        state.adsList.push(value);
    },
    setName: function(state, value) {
      state.name = value
    }
  }
}
