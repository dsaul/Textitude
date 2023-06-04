import { ref } from 'vue';

const global = ref<boolean>(false);

const useGlobalLeftDrawerOpen = () => {
	return global;
};

export { useGlobalLeftDrawerOpen }