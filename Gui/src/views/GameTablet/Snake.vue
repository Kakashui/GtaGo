<template>
    <div id="mario">
      <iframe ref="iframe" src="http://slither.io/" width="100%" height="100%"></iframe>
    </div> 
</template>
<script>
export default {
    created(){
        setTimeout(()=>{
            this.$refs.iframe.focus();
        }, 250)
        
    }
}
</script>

<style lang="scss" scoped>
    #mario{
        position: fixed;
        width: 100%;
        height: 100%;
        left: 0;
        top: 0;
    }
</style>