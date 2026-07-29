<script setup lang="ts">
import { confirmAccount } from '@/core/api/user-api';
import { ref, watch } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const props = defineProps<{ emailConfirmationToken: string }>();
const isLoading = ref(false);
const isSuccessful = ref(false);
const redirectTime = ref<number>(5);
const startCountdown = () => {
  const countdownTimeout = setInterval(() => {
    if (redirectTime.value === 0) {
      clearInterval(countdownTimeout);
      router.push({ name: 'LogIn' });
    }
    redirectTime.value--;
  }, 1000);
};

watch(
  () => props.emailConfirmationToken,
  async (newVal) => {
    if (newVal) {
      try {
        isLoading.value = true;
        await confirmAccount(props.emailConfirmationToken, undefined);
        startCountdown();
        isSuccessful.value = true;
      } catch (err) {
        isLoading.value = false;
        throw err;
      } finally {
        isLoading.value = false;
      }
    }
  },
  {
    immediate: true,
  },
);
</script>

<template>
  <section class="flex flex-col items-center gap-y-5 min-h-screen">
    <p class="text-5xl">Thanks for confirming your account!</p>
    <div v-if="isLoading">
      <UIcon name="i-lucide-loader-circle" class="text-blue-600 text-8xl animate-spin"></UIcon>
    </div>
    <p class="text-3xl" v-if="isSuccessful">
      You will be redirected to the login page in {{ redirectTime }} seconds.
    </p>
  </section>
</template>

<style lang="css" scoped></style>
