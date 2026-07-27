<script setup lang="ts">
import { checkUsernameAvailability } from '@/core/api/user-api';
import type { ISignUpRequest } from '@/core/models/ISignUpRequest';
import { useRegle } from '@regle/core';
import { email, maxLength, minLength, regex, required, sameAs, withMessage } from '@regle/rules';
import { useDebounceFn } from '@vueuse/core';
import { computed, ref } from 'vue';

let usernameCheckAbortController: AbortController | undefined;
const isUsernameCheckLoading = ref<boolean>(false);
const isUsernameAvailable = ref<boolean>(false);

const emits = defineEmits<{ (e: 'cancel'): void }>();

const signUpRequest = ref<ISignUpRequest>({
  confirmPassword: '',
  email: '',
  password: '',
  username: '',
});

const { r$: signUpForm$ } = useRegle(signUpRequest, {
  email: {
    email: withMessage(email, 'A valid email is required.'),
  },
  password: {
    regex: withMessage(
      regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/),
      'Password must be at least 8 characters long, have one capital letter, lowercase letter, number, and symbol.',
    ),
  },
  confirmPassword: {
    required: withMessage(
      sameAs(() => signUpRequest.value.password),
      'Confirm password must match password.',
    ),
  },
  username: {
    required: withMessage(required, 'Username is required.'),
    minLength: withMessage(minLength(8), 'Minimum length must be 8 characters.'),
    maxLength: withMessage(maxLength(128), 'Maximum length is 128 characters.'),
  },
});

const emailError = computed(() => {
  if (!signUpForm$.email.$dirty) return '';
  return signUpForm$.email?.$errors[0] || '';
});

const passwordError = computed(() => {
  if (!signUpForm$.password.$dirty) return '';
  return signUpForm$.password?.$errors?.[0] || '';
});

const usernameError = computed(() => {
  if (!signUpForm$.username.$dirty) return '';
  return signUpForm$.username?.$errors[0] || '';
});

const confirmPasswordError = computed(() => {
  if (!signUpForm$.confirmPassword.$dirty) return '';
  return signUpForm$.confirmPassword?.$errors[0] || '';
});

const debouncedUsernameChanged = useDebounceFn(async () => {
  if (signUpRequest.value.username.length > 3) {
    usernameCheckAbortController?.abort();
    if (!usernameCheckAbortController) {
      usernameCheckAbortController = new AbortController();
    }
    isUsernameAvailable.value = await checkUsernameAvailability(
      signUpRequest.value.username,
      usernameCheckAbortController?.signal,
    );
  }
}, 500);

const createAccount = () => {};
const cancel = () => {
  emits('cancel');
};
</script>

<template>
  <UCard class="w-full">
    <section class="flex flex-col gap-y-5 w-full">
      <div class="text-xl">Create Account</div>
      <div class="flex items-center gap-x-5 w-full">
        <UFormField class="" label="Username" required :error="usernameError || undefined">
          <UInput
            class="w-full"
            @change="debouncedUsernameChanged"
            v-model="signUpRequest.username"
          ></UInput>
        </UFormField>
        <UIcon
          v-if="!isUsernameAvailable && signUpRequest.username && !isUsernameCheckLoading"
          class="text-red"
          name="i-lucide-circle-x"
        ></UIcon>
        <UIcon
          v-if="isUsernameCheckLoading"
          class="animate-spin"
          name="i-lucide-loader-circle"
        ></UIcon>
        <UIcon
          v-if="isUsernameAvailable && signUpRequest.username && !isUsernameCheckLoading"
          class="text-green-500"
          name="i-lucide-badge-check"
        ></UIcon>
      </div>
      <UFormField label="Email" required :error="emailError || undefined">
        <UInput v-model="signUpRequest.email"></UInput>
      </UFormField>
      <UFormField label="Password" required :error="passwordError || undefined">
        <UInput v-model="signUpRequest.password"></UInput>
      </UFormField>
      <UFormField label="Confirm Password" required :error="confirmPasswordError || undefined">
        <UInput v-model="signUpRequest.confirmPassword"></UInput>
      </UFormField>
      <section class="flex justify-between gap-x-5">
        <UButton class="w-full justify-center" @click="createAccount" icon="i-lucide-user-plus"
          >Create Account</UButton
        >
        <UButton color="error" class="w-full justify-center" @click="cancel" icon="i-lucide-x"
          >Cancel</UButton
        >
      </section>
    </section>
  </UCard>
</template>

<style lang="css" scoped></style>
