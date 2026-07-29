<script lang="ts" setup>
import { ref } from 'vue';
import LoginForm from './components/LoginForm.vue';
import SignUpForm from './components/SignUpForm.vue';
const showCreateDialog = ref<boolean>(false);
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
    <div class="flex items-center justify-center w-1/2 p-8">
      <UCard class="w-full">
        <div class="flex flex-col gap-y-5">
          <LoginForm />
          <ULink @click="showCreateDialog = true">Sign up</ULink>
        </div>
        <UModal title="Create Account" v-model:open="showCreateDialog">
          <template #body>
            <SignUpForm @cancel="showCreateDialog = false"></SignUpForm>
          </template>
        </UModal>
      </UCard>
    </div>
  </section>
</template>
