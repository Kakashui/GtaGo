<template>
  <div class="lds-ring"><div></div><div></div><div></div><div></div></div>
</template>

<script>
export default {
  name: 'PreloaderCircle'
}
</script>

<style lang="scss" scoped>
.lds-ring {
  display: inline-block;
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  margin: auto;
  width: 4rem;
  height: 4rem;
}
.lds-ring div {
  box-sizing: border-box;
  display: block;
  position: absolute;
  width: 3.2rem;
  height: 3.2rem;
  margin: 0.4rem;
  border: 0.4rem solid #CFD8DC;
  border-radius: 50%;
  animation: lds-ring 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
  border-color: #CFD8DC transparent transparent transparent;
}
.lds-ring div:nth-child(1) {
  animation-delay: -0.45s;
}
.lds-ring div:nth-child(2) {
  animation-delay: -0.3s;
}
.lds-ring div:nth-child(3) {
  animation-delay: -0.15s;
}
@keyframes lds-ring {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
