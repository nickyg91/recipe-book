<script setup lang="ts">
import { resetPassword } from '@/core/api/user.api';
import ResetPasswordForm from './components/ResetPasswordForm.vue';
import { ref, watch } from 'vue';
import { useRouter } from 'vue-router';

const props = defineProps<{ token: string }>();
const router = useRouter();
const isLoading = ref(false);
const isSuccessful = ref(false);
const redirectTime = ref(5);

let timer: ReturnType<typeof setInterval> | null = null;
watch(
  () => redirectTime.value,
  (val) => {
    if (val === 0) {
      clearInterval(timer!);
      router.push({ name: 'LogIn' });
    }
  },
);

async function onSubmit(data: { password: string }) {
  isLoading.value = true;
  try {
    await resetPassword(props.token, data.password, data.password);
    isSuccessful.value = true;
    timer = setInterval(() => redirectTime.value--, 1000);
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
      <UCard class="w-full" v-if="!isSuccessful">
        <div class="flex flex-col gap-y-5">
          <h2 class="text-xl">Reset Password</h2>
          <p class="text-sm text-gray-500">Enter your new password below.</p>
          <ResetPasswordForm :is-loading="isLoading" @submit="onSubmit" />
        </div>
      </UCard>
      <UCard class="w-full" v-else>
        <div class="flex flex-col gap-y-5 items-center text-center">
          <UIcon name="i-lucide-check-circle" class="text-green-500 text-6xl"></UIcon>
          <h2 class="text-xl">Password Reset Successful</h2>
          <p class="text-sm text-gray-500">
            You will be redirected to the login page in {{ redirectTime }} seconds.
          </p>
        </div>
      </UCard>
    </div>
  </section>
</template>
