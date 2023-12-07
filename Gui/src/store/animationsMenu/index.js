import categories from './categoriesData'
import animations from './animationsData'
import quickAccess from './quickAccessData'

export default {
  namespaced: true,

  state: {
    categories,
    animations,
    currentTab: {
      key: 'categoriesList',
    },
    show: true,
    quickAccess,
    currentAnimations: {
      key: 'favorites',
      array: animations.filter(animation => animation.isFavorite)
    },
    currentKey: null,
  },

  mutations: {
    setCurrentAnimationsList: function(state, key) {
      state.currentAnimations.key = key
      if (key !== 'favorites') {
        state.currentAnimations.array = state.animations.filter(animation => animation.category === key).sort((a, b) => b.isFavorite - a.isFavorite);
      }
      else {
        state.currentAnimations.array = state.animations.filter(animation => animation.isFavorite)
      }
    },

    setFavorite: function(state, obj) {
      const element = state.animations.find(anim => anim.key === obj.key);
      element.isFavorite = !element.isFavorite;

      window.mp.trigger('animations::setFavourite', element.key, element.isFavorite);
    },

    saveAnim: function(state, animationObj) {
      if (state.currentKey !== null) {
        const quickAccessElement = state.quickAccess.find(q => q.key === state.currentKey);
        
        quickAccessElement.animation = animationObj.key;
        quickAccessElement.animationName = animationObj.name;

        state.currentKey = null

        window.mp.trigger('animations::setQuickAccess', quickAccessElement.key, animationObj.key);
      }
    },

    setCurrentKey: function(state, key) {
      state.currentKey = key
    },

    init: function(state, dto) {
      // init set of quick access
      for (let key in dto.quickAccess) {
        const animation = state.animations.find(anim => anim.key === dto.quickAccess[key]);
        if (animation) {
          const quickAccessElement = state.quickAccess.find(q => q.key === key);

          quickAccessElement.animation = animation.key;
          quickAccessElement.animationName = animation.name;
        }
      }

      // init set of favorites animations
      for (let key in dto.favorites) {
        const animation = state.animations.find(anim => anim.key === key);
        if (animation) {
          animation.isFavorite = true;
        }
      }


      // update array of animations
      state.currentAnimations.key = 'favorites';
      state.currentAnimations.array = state.animations.filter(animation => animation.isFavorite);
    }
  }
}
