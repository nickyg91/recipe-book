<script setup lang="ts">
import type { ILogInRequest } from '@/core/models/ILogInRequest';
import { useRegle } from '@regle/core';
import { required, withMessage } from '@regle/rules';
import { ref, computed } from 'vue';

const isLoading = ref(false);

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

const onSubmitClicked = async () => {};
</script>

<template>
  <UCard class="h-fit w-full max-w-md mx-auto">
    <div class="flex flex-col gap-y-5 w-full">
      <div class="text-xl">Log In</div>
      <UFormField label="Email" required :error="emailError || undefined">
        <UInput class="w-full" type="text" v-model="credentials.email"></UInput>
      </UFormField>
      <UFormField label="Password" required :error="passwordError || undefined">
        <UInput class="w-full" type="password" v-model="credentials.password"></UInput>
      </UFormField>
      <UButton
        :loading="isLoading"
        class="justify-center"
        size="xl"
        icon="i-lucide-rocket"
        label="Submit"
      ></UButton>
      <div class="text-center">Don't have an account?</div>
      <ULink> Sign up. </ULink>
    </div>
  </UCard>
</template>

<style lang="css" scoped></style>
