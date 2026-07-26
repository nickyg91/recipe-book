<script lang="ts" setup>
import LoginForm from './LoginForm.vue';
</script>
<template>
  <section class="flex flex-col lg:flex-row min-h-screen">
    <div
      class="flex w-full lg:w-1/2 bg-linear-to-br from-emerald-500 via-teal-400 to-cyan-500 items-center justify-center p-12"
    >
      <div class="text-white text-center">
        <div class="text-6xl mb-4">🍳</div>
        <h1 class="text-5xl font-bold mb-4">Recipe Book</h1>
        <p class="text-lg opacity-90">Discover. Cook. Share.</p>
      </div>
    </div>
    <div class="flex items-center justify-center w-full p-8">
      <LoginForm />
    </div>
  </section>
</template>
