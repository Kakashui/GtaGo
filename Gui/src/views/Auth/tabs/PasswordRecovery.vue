<template>
  <div class="container">
    <div class="about">
      <div class="about__title">{{ loc('auth:rec:forget')}}</div>
      <div class="about__text">{{ loc('auth:rec:desc')}}</div>
    </div>
    <div class="action">
      <input-label :item="input" v-model="input.model" />
      <action-button
        :text="loc('auth:recovery:action')"
        @event="send"
        :style="{ width: '25rem', margin: '0 0 1.25rem 0' }"
      />
      <empty-button
        @event="back"
        :text="loc('auth:rec:back')"
        :styles="{ margin: '0 auto' }"
      />
    </div>
  </div>
</template>

<script>
import ActionButton from '../common/ActionButton.vue'
import InputLabel from '../common/InputLabel.vue'
import EmptyButton from '../common/EmptyButton'
import { mapMutations, mapGetters } from 'vuex'

export default {
  name: 'PasswordRecovery',

  components: {
    InputLabel,
    ActionButton,
    EmptyButton
  },

  data: function() {
    return {
      flood: 0,
      clickInterval: 5000,
      input: { name: 'auth:recovery:email', model: '', key: 'login', icon: 'mail', type: 'email' },
    }
  },
  computed: {
    ...mapGetters('localization', ['loc']),
  },
  
  methods: {
    ...mapMutations('auth', ['setCurrentTab']),
    
    send: function() {
      if(this.flood > Date.now())return;
      this.flood = Date.now() + this.clickInterval;
      window.mp.triggerServer("auth:passRecovered", this.input.model)
    },

    back: function() {
      this.setCurrentTab({ page: 'LoginTab' })
    }
  }
}
</script>

<style lang="scss" scoped>
.container {
  display: flex;
  width: fit-content;
}
.about {
  color: #fff;
  width: 25rem;
  &__title {
    font-size: 6rem;
    line-height: 5rem;
    font-weight: bold;
  }
  &__text {
    font-size: 1.2rem;
    line-height: 1.5rem;
  }
}
</style>
