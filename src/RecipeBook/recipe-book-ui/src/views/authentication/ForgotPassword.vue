<script setup lang="ts">
import { requestPasswordReset } from '@/core/api/user.api';
import ForgotPasswordForm from './components/ForgotPasswordForm.vue';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { RouteNames } from '@/router/route-names';

const router = useRouter();
const isLoading = ref(false);

async function onSubmit(email: string) {
  isLoading.value = true;
  try {
    await requestPasswordReset({ email });
    router.push({ name: RouteNames.LogIn });
  } finally {
    isLoading.value = false;
  }
}
</script>

<template>
  <section class="flex flex-col lg:flex-row min-h-screen">
    <div
      class="flex w-full lg:w-1/2 bg-linear-to-br from-emerald-500 via-teal-400 to-cyan-500 items-center justify-center p-12"
    >
      <div class="text-white text-center">
        <div class="text-6xl mb-4">&#x1F373;</div>
        <h1 class="text-5xl font-bold mb-4">Recipe Book</h1>
        <p class="text-lg opacity-90">Discover. Cook. Share.</p>
      </div>
    </div>
    <div class="flex items-center justify-center w-1/2 p-8">
      <UCard class="w-full">
        <div class="flex flex-col gap-y-5">
          <h2 class="text-xl">Forgot Password</h2>
          <p class="text-sm text-gray-500">
            Enter your email and we'll send you a link to reset your password.
          </p>
          <ForgotPasswordForm :is-loading="isLoading" @submit="onSubmit" />
          <ULink :to="{ name: RouteNames.LogIn }" class="text-sm">Back to login</ULink>
        </div>
      </UCard>
    </div>
  </section>
</template>
