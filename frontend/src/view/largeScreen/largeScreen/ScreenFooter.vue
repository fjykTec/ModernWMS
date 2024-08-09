<template>
  <div class="screen-footer">
    <div class="screen-footer-left">
      <dv-decoration-10 :color="['#45a1f1', '#000000']" class="dv-dec-10" :dur="10" />
    </div>
    <div class="screen-footer-middle">
      <dv-decoration-5 :color="['#45a1f1', '#000000']" :dur="10" class="dv-dec-5" />
    </div>
    <div class="screen-footer-right">
      <dv-decoration-10 :color="['#45a1f1', '#000000']" class="dv-dec-10 dev-reverse" :dur="10" />
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style lang="less" scoped>
.screen-footer {
  display: flex;
  // align-items: center;

  .screen-footer-left {
    width: 35%;
  }

  .screen-footer-middle {
    width: 30%;
  }

  .screen-footer-right {
    width: 35%;
  }
  .dv-dec-5 {
    height: 40px;
  }
  .dv-dec-10 {
    margin-top: -2px;
    height: 20px;
  }
  .dev-reverse {
    transform: rotateY(180deg);
  }
}
</style>
