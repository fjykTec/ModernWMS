import { ref } from 'vue'

export default function windowResize() {
  // * Point to the outermost container
  const screenRef = ref()
  const screenContainerRef = ref()
  // * Timing function
  const timer = ref(0)
  // * Default scaling value
  const scale = {
    width: '1',
    height: '1',
  }
  // * Design draft size（px）
  const baseWidth = 1920
  const baseHeight = 1080

  const calcRate = () => {
    let width = screenContainerRef.value.offsetWidth
    let height = screenContainerRef.value.offsetHeight
    const fullScreen = screenContainerRef.value.dataset.screen

    if (fullScreen === 'true' || fullScreen === true) {
      width = window.innerWidth
      height = window.innerHeight
    }
    
    const widthPre = (width / baseWidth).toFixed(5)
    const heightPre = (height / baseHeight).toFixed(5)
    let pre = widthPre
    if (heightPre < widthPre) {
      pre = heightPre
    }

    if (screenRef.value) {
      screenRef.value.style.transform = `scale(${ pre }, ${ pre })`
    }
  }

  const resize = () => {
    clearTimeout(timer.value)
    timer.value = window.setTimeout(() => {
      calcRate()
    }, 100)
  }

  // Change window size and redraw
  const windowDraw = () => {
    window.addEventListener('resize', resize)
  }

  // Change window size and redraw
  const unWindowDraw = () => {
    window.removeEventListener('resize', resize)
  }

  return {
    screenContainerRef,
    screenRef,
    calcRate,
    windowDraw,
    unWindowDraw,
  }
}
