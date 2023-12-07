<template>
  <div class="frac">
    <Info />
    <Members />
  </div>
</template>

<script>
import { mapMutations } from 'vuex'
import Info from './Information'
import Members from './Members'

export default {
  name: 'OrganizationTab',
    mounted() {
      this.resetFraction();
      if(process.env.NODE_ENV == 'development'){
        this.$store.commit('optionsMenu/setFractionMembers', [
          {id:10,username:"Avraam_Russo",rank: 2},
          {id:-1,username:"Ferrarii_Gotu",rank: 2},
          {id:11,username:"Name_Third",rank: 8},
          {id:21,username:"Yandex_Dick",rank: 4},
          {id:4,username:"Name_Fourth",rank: 4},
          {id:8,username:"Name_Fourth",rank: 4},
          {id:-1,username:"Bengamin_Franklin",rank: 4},
          {id:6,username:"Name_Fourth",rank: 4},
          {id:2,username:"Aremenian_Men",rank: 4}
        ]);      
      }
      
      window.mp.triggerServer("openfmenu");
    },
    methods:{
      ...mapMutations('optionsMenu',['resetFraction'])
    },
    components:{
      Info,
      Members
    }
}
</script>

<style lang="scss">
.frac {
  height: 95%;
  width: 100%;
  position: relative;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;  
}
</style>
