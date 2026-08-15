<script setup lang="ts">
import { useRegle } from '@regle/core';
import { required, sameAs, withMessage } from '@regle/rules';
import { computed, ref } from 'vue';

const props = defineProps<{ isLoading: boolean }>();
const emits = defineEmits<{ submit: [data: { password: string }] }>();

const formData = ref({ password: '', confirmPassword: '' });

const { r$ } = useRegle(formData, {
  password: {
    required: withMessage(required, 'Password is required.'),
  },
  confirmPassword: {
    required: withMessage(sameAs(() => formData.value.password), 'Passwords do not match.'),
  },
});

const passwordError = computed(() => (!r$.password.$dirty ? '' : r$.$errors?.password?.[0] ?? ''));
const confirmPasswordError = computed(() => (!r$.confirmPassword.$dirty ? '' : r$.$errors?.confirmPassword?.[0] ?? ''));

function onSubmitClicked() {
  if (r$.$invalid || props.isLoading) return;
  emits('submit', { password: formData.value.password });
}
</script>

<template>
  <div class="flex flex-col gap-y-5 w-full">
    <UFormField label="New Password" required :error="passwordError || undefined">
      <UInput class="w-full" type="password" v-model="formData.password"></UInput>
    </UFormField>
    <UFormField label="Confirm Password" required :error="confirmPasswordError || undefined">
      <UInput class="w-full" type="password" v-model="formData.confirmPassword"></UInput>
    </UFormField>
    <UButton
      @click="onSubmitClicked"
      :loading="props.isLoading"
      :disabled="r$.$invalid || props.isLoading"
      class="justify-center"
      size="xl"
      icon="i-lucide-key-round"
      label="Reset Password"
    ></UButton>
  </div>
</template>

<style lang="css" scoped></style>
