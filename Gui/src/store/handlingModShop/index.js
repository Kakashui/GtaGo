export default {
  namespaced: true,
  state: {
    visible: true,
    items:[
      {
        key: 1, //Масса
        onlyAdmin: true,
        value:{
          min: 0,
          max: 20000,
          step: 10,
          current: 1500
        },
        defaultValue: 1500,
      },
      {
        key: 2,//Коэф. аэродинамического сопротивления
        onlyAdmin: true,
        value:{
          min: 0,
          max: 120,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 6,//Привод тс
        onlyAdmin: false,
        value:{
          min: 0,
          max: 1,
          step: 0.01,
          current: 0.5
        },
        defaultValue: 0.5,
      },
      {
        key: 7,//Количество передач
        onlyAdmin: true,
        value:{
          min: 1,
          max: 16,
          step: 1,
          current: 8
        },
        defaultValue: 8,
      },
      {
        key: 8,//множитель тягового усиления
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 2,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 9,//Множитель инерции
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 5,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 10,//множитель сцепления при смещении вверх;
        onlyAdmin: true,
        value:{
          min: 0.1,
          max: 10,
          step: 0.1,
          current: 5
        },
        defaultValue: 5,
      },
      {
        key: 11,//множитель сцепления при смещении вниз;
        onlyAdmin: true,
        value:{
          min: 0.1,
          max: 10,
          step: 0.1,
          current: 6
        },
        defaultValue: 6,
      },
      {
        key: 12,//теоретическая максимальная скорость
        onlyAdmin: true,
        value:{
          min: 0,
          max: 1000,
          step: 1,
          current: 200
        },
        defaultValue: 200,
      },
      {
        key: 13,//множитель силы торможения
        onlyAdmin: false,
        value:{
          min: 0.0,
          max: 10,
          step: 0.1,
          current: 2
        },
        defaultValue: 2,
      },
      {
        key: 14,//смещение тормозов
        onlyAdmin: false,
        value:{
          min: 0.01,
          max: 1,
          step: 0.01,
          current: 0.65
        },
        defaultValue: 0.65,
      },
      {
        key: 15,//мощность ручного тормоза
        onlyAdmin: false,
        value:{
          min: 0.1,
          max: 10,
          step: 0.1,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 16,//Угол поворота руля
        onlyAdmin: false,
        value:{
          min: 0.01,
          max: 2,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 17,//сцепление колес на повороте
        onlyAdmin: false,
        value:{
          min: 0.1,
          max: 10,
          step: 0.1,
          current: 4
        },
        defaultValue: 4,
      },
      {
        key: 18,//пробуксовка колес
        onlyAdmin: false,
        value:{
          min: 0.1,
          max: 10,
          step: 0.1,
          current: 4
        },
        defaultValue: 4,
      },
      {
        key: 19,//боковая сила сцепления
        onlyAdmin: false,
        value:{
          min: 0.01,
          max: 2,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 20,//расстояние от земли, при котором транспортное средство теряет сцепление
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 1,
          step: 0.01,
          current: 0.25
        },
        defaultValue: 0.25,
      },
      {
        key: 21,//параметр, отвечающий за силу сцепления транспортного средства при низкой скорости (значение 0.0, //исходная сила сцепления);
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 1,
          step: 0.01,
          current: 0.5
        },
        defaultValue: 0.5,
      },
      // {
      //   key: 22,//параметр, отвечающий за угол развала колес у мотоциклов;
      //   onlyAdmin: true,
      //   value:{
      //     min: 0.01,
      //     max: 10,
      //     step: 0.01,
      //     current: 1
      //   },
      //   defaultValue: 1,
      //   setPrice: 70000,
      //   delPrice: 70000
      // },
      {
        key: 23,//параметр, отвечающий за распределение тяги между передними и задними колесами (от 0.01 до 0.99);
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 1,
          step: 0.01,
          current: 0.5
        },
        defaultValue: 0.5,
      },
      {
        key: 24,//сцепления шины с дорогой;
        onlyAdmin: false,
        value:{
          min: 0.1,
          max: 2,
          step: 0.1,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 25,//сила подвески
        onlyAdmin: false,
        value:{
          min: 0.01,
          max: 5,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 26,//вибрация подвески;
        onlyAdmin: false,
        value:{
          min: 0.01,
          max: 1,
          step: 0.01,
          current: 0.1
        },
        defaultValue: 0.1,
      },
      {
        key: 27,//вибрация подвески на высоких скоростях
        onlyAdmin: false,
        value:{
          min: 0.01,
          max: 1,
          step: 0.01,
          current: 0.1
        },
        defaultValue: 0.1,
      },
      {
        key: 28,//максимальная высота кузова транспортного средства;
        onlyAdmin: true,
        value:{
          min: 0.001,
          max: 0.5,
          step: 0.001,
          current: 1
        },
        defaultValue: 0.01,
      },
      {
        key: 29,//минимальная высота кузова транспортного средства;
        onlyAdmin: true,
        value:{
          min: -0.5,
          max: 0.5,
          step: 0.01,
          current: -0.1
        },
        defaultValue: -0.1,
      },
      {
        key: 30,//высота подвески;
        onlyAdmin: false,
        value:{
          min: 0,
          max: 1,
          step: 0.01,
          current: 0.1
        },
        defaultValue: 0.1,
      },
      {
        key: 31,//смещение подвески вперед
        onlyAdmin: true,
        value:{
          min: 0.1,
          max: 2,
          step: 0.1,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 32,//сила стабилизатора подвески;
        onlyAdmin: false,
        value:{
          min: 0.01,
          max: 5,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 33,//смещение силы стабилизатора подвески вперед
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 5,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 34,//высота центра крена (перед)
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 0.5,
          step: 0.01,
          current: 1
        },
        defaultValue: 1,
      },
      {
        key: 35,//высота центра крена (зад)
        onlyAdmin: true,
        value:{
          min: 0.01,
          max: 0.5,
          step: 0.01,
          current: 0.2
        },
        defaultValue: 0.2,
      },
      {
        key: 999,
        onlyAdmin: false,
        value: {
          current: false,
        },
        defaultValue: false,       
      },
    ],
    isAdmin: false,
    pricePart: 100,
    priceCar: 5000000,
    percentCarPrice: 5000000,
  },
  mutations: {
    hideMenu(state){
      state.visible = false;
    },    
    showMenu(state){
      state.visible = true;
    },
    setData(state, data){
      state.priceCar = data.priceCar;
      state.pricePart = data.pricePart;
      state.isAdmin = data.isAdmin;
      state.items.forEach(item => {
        item.onlyAdmin = data.items[item.key].onlyAdmin;
        item.value = data.items[item.key].value;
        item.defaultValue = data.items[item.key].defaultValue;
      });
    },
    updateData(state, item){
      const index = state.items.findIndex(i=>i.key === item.key);
      if(index >= 0)
        state.items[index].value.current = item.current;
    }
  },
  actions: {
  },
  modules: {
  }
}
  