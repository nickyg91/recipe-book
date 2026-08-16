<script setup lang="ts">
import { useRegle } from '@regle/core';
import { email, required, withMessage } from '@regle/rules';
import { computed, ref } from 'vue';

const props = defineProps<{ isLoading: boolean }>();
const emits = defineEmits<{ submit: [email: string] }>();

const formData = ref({ email: '' });

const { r$ } = useRegle(formData, {
  email: {
    required: withMessage(required, 'Email is required.'),
    email: withMessage(email, 'A valid email is required.'),
  },
});

const emailError = computed(() => (!r$.email.$dirty ? '' : (r$.$errors?.email?.[0] ?? '')));

function onSubmitClicked() {
  if (r$.$invalid || props.isLoading) return;
  emits('submit', formData.value.email);
}
</script>

<template>
  <div class="flex flex-col gap-y-5 w-full">
    <UFormField label="Email" required :error="emailError || undefined">
      <UInput class="w-full" type="text" v-model="formData.email"></UInput>
    </UFormField>
    <UButton
      @click="onSubmitClicked"
      :loading="props.isLoading"
      :disabled="r$.$invalid || props.isLoading"
      class="justify-center"
      size="xl"
      icon="i-lucide-mail"
      label="Send Reset Link"
    ></UButton>
  </div>
</template>

<style lang="css" scoped></style>
