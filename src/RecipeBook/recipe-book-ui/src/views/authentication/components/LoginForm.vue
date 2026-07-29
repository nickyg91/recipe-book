<script setup lang="ts">
import type { ILogInRequest } from '@/core/models/ILogInRequest';
import { useUserStore } from '@/stores/userStore';
import { useRegle } from '@regle/core';
import { required, withMessage } from '@regle/rules';
import { AxiosError } from 'axios';
import { ref, computed } from 'vue';

const userStore = useUserStore();
const isLoading = ref(false);

const errorMessage = ref<string | undefined>();
const credentials = ref<ILogInRequest>({
  email: '',
  password: '',
});
const { r$ } = useRegle(credentials, {
  email: {
    required: withMessage(required, 'Email is required.'),
  },
  password: {
    required: withMessage(required, 'Password is required.'),
  },
});

const emailError = computed(() => {
  if (!r$.email.$dirty) return '';
  return r$.$errors?.email?.[0] || '';
});

const passwordError = computed(() => {
  if (!r$.password.$dirty) return '';
  return r$.$errors?.password?.[0] || '';
});

const onSubmitClicked = async () => {
  if (r$.$invalid) {
    return;
  }
  errorMessage.value = undefined;
  if (isLoading.value) {
    return;
  }
  isLoading.value = true;
  try {
    await userStore.logIn(credentials.value);
  } catch (err) {
    isLoading.value = false;
    if (err instanceof AxiosError) {
      errorMessage.value = err.response?.data;
    } else {
      throw err;
    }
  } finally {
    isLoading.value = false;
  }
};
</script>
<template>
  <div class="flex flex-col gap-y-5 w-full">
    <div class="text-xl">Log In</div>
    <UFormField label="Email" required :error="emailError || undefined">
      <UInput class="w-full" type="text" v-model="credentials.email"></UInput>
    </UFormField>
    <UFormField label="Password" required :error="passwordError || undefined">
      <UInput class="w-full" type="password" v-model="credentials.password"></UInput>
    </UFormField>
    <UButton
      @click="onSubmitClicked"
      :loading="isLoading"
      :disabled="r$.$invalid"
      class="justify-center"
      size="xl"
      icon="i-lucide-rocket"
      label="Submit"
    ></UButton>
    <div v-if="errorMessage">
      <UAlert :description="errorMessage" color="error"></UAlert>
    </div>
  </div>
</template>

<style lang="css" scoped></style>
