<script setup lang="ts">
import { checkUsernameAvailability, signUp } from '@/core/api/user-api';
import type { ISignUpRequest } from '@/core/models/ISignUpRequest';
import { useRegle } from '@regle/core';
import { email, maxLength, minLength, regex, required, sameAs, withMessage } from '@regle/rules';
import { useDebounceFn } from '@vueuse/core';
import { AxiosError } from 'axios';
import { computed, ref, watch } from 'vue';

const toast = useToast();

let usernameCheckAbortController: AbortController | undefined;
let signUpAbortController: AbortController | undefined;
const isUsernameCheckLoading = ref<boolean>(false);
const isUsernameAvailable = ref<boolean>(false);
const isSignUpLoading = ref<boolean>(false);

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
      regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$/),
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
    isUsernameAvailable.value = !(await checkUsernameAvailability(
      signUpRequest.value.username,
      usernameCheckAbortController?.signal,
    ));
  }
}, 500);

const createAccount = async () => {
  if (signUpForm$.$invalid) {
    return;
  }
  try {
    isSignUpLoading.value = true;
    signUpAbortController?.abort();
    if (!signUpAbortController) {
      signUpAbortController = new AbortController();
    }
    await signUp(signUpRequest.value, signUpAbortController?.signal);
    toast.add({
      color: 'success',
      duration: 5000,
      description: 'Sign-up successful! Look for an email to confirm your account.',
      title: 'You did it! 🚀',
    });
    emits('cancel');
  } catch (err) {
    isSignUpLoading.value = false;
    if (err instanceof AxiosError && err.code !== 'ERR_CANCELED') {
      throw err;
    }
  } finally {
    isSignUpLoading.value = false;
  }
};
const cancel = () => {
  emits('cancel');
};

watch(
  () => signUpRequest.value.username,
  async (newUsername) => {
    if (!newUsername || newUsername.length <= 3) {
      isUsernameAvailable.value = false;
      return;
    }
    try {
      isUsernameCheckLoading.value = true;
      await debouncedUsernameChanged();
    } catch (err) {
      isUsernameCheckLoading.value = false;
      if (err instanceof AxiosError && err.code !== 'ERR_CANCELED') {
        throw err;
      }
    } finally {
      isUsernameCheckLoading.value = false;
    }
  },
);
</script>

<template>
  <section class="flex flex-col gap-y-5">
    <UFormField class="w-full" label="Username" required :error="usernameError || undefined">
      <UInput class="w-full" v-model="signUpRequest.username">
        <template #trailing>
          <UIcon
            v-if="isUsernameCheckLoading"
            class="animate-spin text-blue-500"
            name="i-lucide-loader-circle"
          />
          <UIcon v-else-if="!signUpRequest.username" class="text-gray-400" name="i-lucide-user" />
          <UIcon v-else-if="!isUsernameAvailable" class="text-red-500" name="i-lucide-circle-x" />
          <UIcon
            v-else-if="isUsernameAvailable"
            class="text-green-500"
            name="i-lucide-badge-check"
          />
        </template>
      </UInput>
    </UFormField>
    <UFormField class="w-full" label="Email" required :error="emailError || undefined">
      <UInput class="w-full" v-model="signUpRequest.email"></UInput>
    </UFormField>
    <UFormField class="w-full" label="Password" required :error="passwordError || undefined">
      <UInput class="w-full" type="password" v-model="signUpRequest.password"></UInput>
    </UFormField>
    <UFormField
      class="w-full"
      label="Confirm Password"
      required
      :error="confirmPasswordError || undefined"
    >
      <UInput class="w-full" type="password" v-model="signUpRequest.confirmPassword"></UInput>
    </UFormField>
    <section class="flex justify-between gap-x-5">
      <UButton
        :loading="isSignUpLoading"
        class="w-full justify-center"
        @click="createAccount"
        icon="i-lucide-user-plus"
        >Create Account</UButton
      >
      <UButton color="error" class="w-full justify-center" @click="cancel" icon="i-lucide-x"
        >Cancel</UButton
      >
    </section>
  </section>
</template>

<style lang="css" scoped></style>
