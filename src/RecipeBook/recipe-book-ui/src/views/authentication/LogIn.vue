<script setup lang="ts">
import type { ILogInRequest } from '@/core/models/ILogInRequest';
import { useRegle } from '@regle/core';
import { required, withMessage } from '@regle/rules';
import { ref } from 'vue';

const credentials = ref<ILogInRequest>();
const form = useRegle(credentials, {
  email: {
    required: withMessage(required, 'Email is required.'),
  },
  password: {
    required: withMessage(required, 'Password is required.'),
  },
});
</script>

<template>
  <UCard></UCard>
</template>

<style lang="css" scoped></style>
