<script setup lang="ts">
import { AxiosError } from 'axios';
import { onErrorCaptured } from 'vue';
import { RouterView } from 'vue-router';

const toast = useToast();
// import { useUserStore } from './stores/userStore';

//const userStore = useUserStore();

onErrorCaptured((err) => {
  if (err instanceof AxiosError) {
    if (err.code !== 'ERR_CANCELED') {
      toast.add({
        title: 'An error occurred',
        description: err.message,
        icon: 'i-lucide-bug',
      });
    }
  } else {
    toast.add({
      title: 'An error occurred',
      description: err.message,
      icon: 'i-lucide-bug',
    });
  }
});
</script>

<template>
  <UApp>
    <UMain>
      <RouterView />
    </UMain>
  </UApp>
</template>
