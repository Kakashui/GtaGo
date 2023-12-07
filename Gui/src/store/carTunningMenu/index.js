export default {
  namespaced: true,

  state: {
    balance : 2000000,
    currentTab: 'FirstTab',
    specificationMaxValues: {
      speed: 400,
      braking: 300,
      acceleration: 400,
      traction: 400
    },
    specifications: {
      title: 'Mercedes AMG',
      speed: 100,
      braking: 200,
      acceleration: 110,
      traction: 200
    },
    specificationsPossible: {
      speed: 300,
      braking: 220,
      acceleration: 200,
      traction: 80
    },
    isBuyEnable: true,
    fistTabItems: [
      { title: 'car_tunning_7', key: 'body' },
      { title: 'car_tunning_8', key: 'power' },
      { title: 'car_tunning_9', key: 'painting' }
    ],
    secondTabData: {
      parent: 'power',
      title: 'Покраска',
      items: [
        // BODY
        // { title: 'Спойлер', key: 'spoiler' },
        // { title: 'Бампер', key: 'bumper'},
        // { title: 'Капот', key: 'hood'},
        // { title: 'Крыша', key: 'roof'},
        // { title: 'Диски', key: 'disks'},
        // { title: 'Двигатель', key: 'engine'},
        // { title: 'Тонировка', key: 'toning'},
        // { title: 'Номера', key: 'numbers'},
        // { title: 'Решетка радиатора', key: 'radiator_grille'},
        // { title: 'Подвеска', key: 'suspension'},
        // { title: 'Коробка', key: 'box'},
        // { title: 'Выхлопная система', key: 'exhaust_system'},
        // { title: 'Зеркала заднего вида', key: 'rear_view_mirrors'},
        // { title: 'Пороги', key: 'rapids'},
        // { title: 'Выхлоп', key: 'exhaust'},
        // { title: 'Каркас безопасности', key: 'safety_cage'},
        // { title: 'Крылья', key: 'wings'},
        // { title: 'Крышка багажника', key: 'cap_trunk'},
        // { title: 'Броня', key: 'armor'},
        // { title: 'Колеса', key: 'wheels'},
        // { title: 'Колесо мото', key: 'wheel_moto'},
        // { title: 'Лобовая надпись', key: 'frontal_inscription'}
        // POWER
        // { title: 'Турбина', key: 'turbine' },
        // { title: 'Тормоза', key: 'brakes' },
        // { title: 'Двигатель', key: 'engine' },
        // { title: 'Трансмиссия', key: 'transmission' },
        // PAINTING
        { title: 'Фары', key: 'headlights' },
        { title: 'Подсветка Подсветка', key: 'backlight' },
        { title: 'Сигнал', key: 'signal' },
        { title: 'Неон', key: 'neon' },
        { title: 'Винилы', key: 'vinyls' },
        { title: 'Кузов', key: 'body' },
        { title: 'Диски', key: 'disks' },
      ]
    },
    currentItemPrice: 5000,
    
    currentSelectItem: '',
    thirdTabData: {
      title: 'Цвет',
      parent: 'Color',
      sliderColors: [
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
        {"Number":141,"Red":10,"Green":12,"Blue":23},
        {"Number":142,"Red":12,"Green":13,"Blue":24},
        {"Number":143,"Red":14,"Green":13,"Blue":20},
        {"Number":0,"Red":13,"Green":17,"Blue":22},
        {"Number":132,"Red":255,"Green":255,"Blue":251},
      ],
      currentMainColorType: { id: 0, title: 'car_tunning_7' },
      currentAdditionalColorType: { id: 0, title: 'car_tunning_7' },
      colorPickerMainColor: '(23, 112, 14)',
      colorPickerAdditionalColor: '(23, 112, 144)',
      colorPickerNeonColor: '(23, 112, 144)',
      colorPickerNeon2Color: '(23, 112, 144)',
      colorPickerTyreSmokeColor: '(23, 112, 144)',      
      colorTypeItems: [
        { id: 0, title: 'car_tunning_7' },
        { id: 1, title: 'car_tunning_8' },
        { id: 3, title: 'car_tunning_9' },
        { id: 4, title: 'car_tunning_10' },
        { id: 5, title: 'car_tunning_11' }
      ],
      items: [
        // disks
        { title: 'FrontWheels', key: 'Color', rank: 'IV' },
        { title: 'Off-road', key: 'off_road', rank: 'I' },
        { title: 'Classic', key: 'classic', rank: undefined },
        { title: 'SUV', key: 'suv', rank: 'I' }
        // headlights
        // { title: 'Синие', key: 'blue'},
        // { title: 'Желтые', key: 'yellow'},
        // { title: 'Красные', key: 'red'},
      ]
    },
    fourthTabData: {
      title: 'Возможный заголовок',
      parent: 'sport',
      items: [
        { title: 'First', key: 'first', rank: 'IV' },
        { title: 'Second', key: 'second', rank: 'IV' },
        { title: 'Third', key: 'third', rank: undefined }
      ]
    }
  },

  mutations: {
    setCurrentTab: function(state, value) {
      state.currentTab = value
    },

    setSpecificationMaxValues: function(state, object) {
      state.specificationMaxValues = object
    },

    setSpecificationsValues: function(state, object) {
      state.specifications = object
    },

    setSpecificationsPossibleValues: function(state, object) {
      state.specificationsPossible = object
    },

    setFistTabItems: function(state, payload) {
      state.fistTabItems = payload
    },

    setSecondTabData: function(state, payload) {
      state.secondTabData = payload
    },

    setThirdTabData: function(state, payload) {
      state.thirdTabData.parent = payload.parent
      state.thirdTabData.items = payload.items
    },

    setFourthTabData: function(state, payload) {
      state.fourthTabData.parent = payload.parent
      state.fourthTabData.items = payload.items
    },

    setThirdCurrentMainColorType: function(state, item) {
      state.thirdTabData.currentMainColorType = item
    },
    
    setThirdCurrentAdditionalColorType: function(state, item) {
      state.thirdTabData.currentAdditionalColorType = item
    },

    setColorPickerMainColor: function(state, color) {
      state.thirdTabData.colorPickerMainColor = color
    },

    setColorPickerAdditionalColor: function(state, color) {
      state.thirdTabData.colorPickerAdditionalColor = color
    },

    setColorPickerNeonColor: function(state, color) {
      state.thirdTabData.colorPickerNeonColor = color
    },

    setColorPickerNeon2Color: function(state, color) {
      state.thirdTabData.colorPickerNeon2Color = color
    },
    setColorPickerTyreSmokeColor: function(state, color) {
      state.thirdTabData.colorPickerTyreSmokeColor = color
    },
    
    setCurrentItemPrice: function(state, value) {
      state.currentItemPrice = value
    },

    setIsBuyEnable: function(state, value) {
      state.isBuyEnable = value
    },

    setSliderColors: function(state, value) {
      state.thirdTabData.sliderColors = value
    },

    setCurrentSelectItem: function(state, value) {
      state.currentSelectItem = value
    },

    setColorTypeItems: function(state, value) {
      state.thirdTabData.colorTypeItems = value
    },

    setThirdTabDataTitle: function(state, value) {
      state.thirdTabData.title = value
    },

    setFourthTabDataTitle: function(state, value) {
      state.fourthTabData.title = value
    }

    
  }
}
