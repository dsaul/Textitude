<script setup lang="ts">
import MessageSummary from '@/Components/Rows/MessageSummary.vue';
import { useQueryLatestMessages } from '@/_Compostables/GraphQL/Messages/useQueryLatestMessages';
const { latest } = useQueryLatestMessages();

</script>
<template>
	<q-list>
		<q-item-label header>By Number</q-item-label>
		<MessageSummary
			v-for="(row, rowIdx) in latest"
			:key="row.id || `idx${rowIdx}`" 
			:value="row || undefined"
			/>
	</q-list>
</template>
