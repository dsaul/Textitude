<script setup lang="ts">
import { onMounted } from 'vue';
import { SRouteNameConversations } from '@/Symbols';
import { useRouter } from 'vue-router';

const router = useRouter()

onMounted(() => {
	router.push({ name: SRouteNameConversations, params: {  } })
});

</script>
<template>
	<div></div>
</template>