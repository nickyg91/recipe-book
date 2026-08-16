<script setup lang="ts">
import { getRecipeBooks } from '@/core/api/recipe-book.api';
import type { IRecipeBook } from '@/core/models/IRecipeBook';
import { onMounted, ref } from 'vue';
import NoRecipes from './components/NoRecipes.vue';

const isLoading = ref(true);
const recipeBooks = ref<IRecipeBook[]>([]);

onMounted(async () => {
  try {
    recipeBooks.value = await getRecipeBooks();
  } catch (err) {
    isLoading.value = false;
    throw err;
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <main class="flex justify-center items-center h-lvh">
    <Transition name="fade" mode="out-in">
      <UIcon
        v-if="isLoading"
        class="size-15 text-emerald-500 animate-spin"
        name="i-lucide-loader-circle"
      ></UIcon>
      <Transition v-else name="fade" mode="out-in">
        <NoRecipes v-if="recipeBooks.length === 0"></NoRecipes>
      </Transition>
    </Transition>
  </main>
</template>

<style lang="css" scoped></style>
