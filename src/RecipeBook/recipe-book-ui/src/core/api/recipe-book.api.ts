import { useHttpClient } from '../http-client';
import type { IRecipeBook } from '../models/IRecipeBook';

export const createRecipeBook = async (recipeBook: IRecipeBook): Promise<IRecipeBook> => {
  const { post } = useHttpClient();
  const { data } = await post<IRecipeBook>('recipe-books/create', recipeBook);
  return data;
};

export const getRecipeBooks = async (abortSignal?: AbortSignal): Promise<IRecipeBook[]> => {
  const { get } = useHttpClient();
  const { data } = await get<IRecipeBook[]>('recipe-books', {
    signal: abortSignal,
  });
  return data;
};
