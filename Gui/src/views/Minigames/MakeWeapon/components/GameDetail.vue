<template>
  <div class="game-detail" @click.stop="checkAnswer($event)">
    <span class="game-detail__message">{{message}}</span>
    <img v-bind:src="imgPath" class="game-detail__image" />
  </div>
</template>
<script>
import { mapState, mapActions, mapGetters, mapMutations } from "vuex";
export default {
  name: "GameDetail",
  props: {
    gunNum: {
      type: Number,
      required: true,
    },
    currentDetail: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      message: ''
    }
  },
  computed: {
    ...mapState("gameMakeWeapon/gunsData", ["details"]),
    ...mapState("gameMakeWeapon/gameState", [
      "currentGun",
      "gameProgress",
      "gameProgressNum",
      "dateTime",
      "isGameBegine"
    ]),
    ...mapState("gameMakeWeapon/pageState", ["animationActive", 'helpVisibility']),
    ...mapGetters('gameMakeWeapon/gameState', ['checkResult']),
    imgPath() {
      let gunNum = "gun" + this.gunNum;
      let detailNum = "detail" + this.currentDetail;
      let detailPath = "/img/miniGames/makeWeapon/details/" + gunNum + "/" + detailNum + ".png";
      return detailPath;
    },
  },
  methods: {
    ...mapActions("gameMakeWeapon/gameState", [
      "updateGameProgress",
      "updateCurrentDetail",
      "updateGameBonus",
      "compleeteGame"
    ]),
    ...mapMutations('gameMakeWeapon/gameState', ['updateResult']),
    playCorrect(){
        this.$store.commit("sounds/play", {"name": "makeWeaponCorrect", "volume": .05});
    },
    playIncorrect(){
        this.$store.commit("sounds/play", {"name": "makeWeaponIncorrect", "volume": .05});
    },
    playBonus(){
        this.$store.commit("sounds/play", {"name": "makeWeaponBonus", "volume": .1});
    },
    checkAnswer($event){
      if(this.helpVisibility || !this.isGameBegine) return;
      
      this.updateResult(this.gunNum == this.currentGun);
      if ( this.gunNum == this.currentGun) {
        this.compleeted = true;
        $event.target.classList.add("correct");
        this.message = "correct!";
        this.updateGameProgress(this.currentDetail - 1);
        if (Date.now() < this.dateTime) {
          this.updateGameBonus();
          this.playBonus()
        }else 
          this.playCorrect();
      } 
      else 
      {
        $event.target.classList.add("incorrect");
        this.message = "wrong!";        
        this.playIncorrect();
      }
      setTimeout(()=>{
        $event.target.classList.remove("correct");
        $event.target.classList.remove("incorrect");
        this.updateCurrentDetail();
      }, 800) 
    }
  },
};
</script>
<style scoped lang="scss">
.game-detail {
  width: 14.9rem;
  height: 9.4rem;
  margin: 0 0.3rem;
  background: url("/img/miniGames/makeWeapon/detail-container.png") center center no-repeat;
  background-size: contain;
  text-align: center;
  line-height: 9.4rem;
  position: relative;
  cursor: pointer;

  &.correct {
    background: url("/img/miniGames/makeWeapon/detail-container-true.png") no-repeat center;
  }
  &.incorrect {
    background: url("/img/miniGames/makeWeapon/detail-container-false.png") no-repeat center;
  }

  &.correct,
  &.incorrect {
    background-size: cover;
    &:before {
      content: "";
      display: block;
      width: 100%;
      height: 100%;
      position: absolute;
      top: 0;
      left: 0;
      background: url("/img/miniGames/makeWeapon/detail-container-candies.png") no-repeat center;
      background-size: cover;
    }
  }

  &:hover {
    &:before,
    &:after {
      content: "";
      display: block;
      width: 100%;
      height: 100%;
      position: absolute;
      top: 0;
      left: 0;
    }
    &:before {
      background: radial-gradient(
        farthest-corner at -5rem -5rem,
        rgba(#566172, 0.7) 0%,
        rgba(39, 45, 54, 0) 100%
      );
      background-position: -50% -50%;
    }
    // &:after {
    //   background: url("/img/miniGames/makeWeapon/detail-container-candies.png") center
    //     center no-repeat;
    //   background-size: contain;
    // }
  }

  &__image {
    display: inline;
    vertical-align: middle;
  }
  &__message {
    display: none;
    position: absolute;
    top: -2.2rem;
    left: 0;
    width: 100%;
    
    font-size: 1.5rem;
    //font-weight: 700;
    line-height: 1.7rem;
    text-transform: uppercase;

    .correct & {
      display: block;
      color: #dcff00;
    }
    .incorrect & {
      display: block;
      color: #f00;
    }
  }
}
</style>
