<template>
  <div class="menu-clock">
    <span class="date">{{ date }}</span>
    <span class="time">{{ time }}</span>
  </div>
</template>

<script>
export default {
  data: function () {
    return {
      time: '',
      date: '',
      week: ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT']
    }
  },

  methods: {
    zeroPadding(num, digit) {
      let zero = ''

      for(var i = 0; i < digit; i++) {
          zero += '0'
      }
      
      return (zero + num).slice(-digit)
    },

    updateTime () {
      let cd = new Date()

      this.time = this.zeroPadding(cd.getHours(), 2) + ':' + this.zeroPadding(cd.getMinutes(), 2) + ':' + this.zeroPadding(cd.getSeconds(), 2)
      this.date = this.zeroPadding(cd.getFullYear(), 4) + '-' + this.zeroPadding(cd.getMonth() + 1, 2) + '-' + this.zeroPadding(cd.getDate(), 2) + ' ' + this.week[cd.getDay()]
    }
  },

  mounted: function () {
    setInterval(this.updateTime, 1000)
  }
}
</script>

<style lang="scss" scoped>
.menu-clock {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  font-weight: bold;
  font-size: 1rem;
  line-height: 1.15rem;
  letter-spacing: 0.04em;
  color: #FFFFFF;
  .date{
    margin-bottom: .1rem;
  }
}
</style>
