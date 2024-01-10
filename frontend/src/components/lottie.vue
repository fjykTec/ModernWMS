<template>
  <div ref="lottieBox"></div>
</template>

<script lang="ts" setup>
import lottie from 'lottie-web'
import { onMounted, ref } from 'vue'

const props = defineProps({
  src: {
    type: Object,
    default: () => {}
  },
  path: {
    type: String,
    default: ''
  },
  loop: {
    type: Boolean,
    default: true
  },
  autoplay: {
    type: Boolean,
    default: true
  }
})
const lottieBox = ref(null)
onMounted(() => {
  if (lottieBox.value) {
    lottie.loadAnimation({
      container: lottieBox.value,
      renderer: 'svg',
      loop: props.loop,
      autoplay: props.autoplay,
      path: props.path,
      animationData: Object.keys(props.src).length == 0 ? '' : props.src
    })
  }
})
</script>
