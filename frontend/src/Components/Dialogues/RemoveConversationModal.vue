<script setup lang="ts">
import { ref, computed } from 'vue';
import { Notify } from 'quasar';
import type { ISMSMessage } from '@/_Compostables/GraphQL/Messages/ISMSMessage';
import MDIDelete from '../SVG/Icons/MDIDelete.vue';
import { useModelValue } from '@/_Compostables/Utility/useModelValue';
import { useRemoteE164 } from '@/_Compostables/GraphQL/Messages/useRemoteE164';
import { useMutationRemoveFromTo } from '@/_Compostables/GraphQL/Messages/useMutationRemoveFromTo';
import { useRouter } from 'vue-router';
import { SRouteNameConversationE164, SRouteNameConversations } from '@/Symbols';
const { remove, removeLoading, removeError, removeDone } = useMutationRemoveFromTo();

const router = useRouter();

const props = defineProps<{
	modelValue?: ISMSMessage | null,
	value?: ISMSMessage | null,
}>()

const emit = defineEmits<{
	(e: 'update:modelValue', payload: ISMSMessage | null): void,
	(e: 'on-value-changed', payload: ISMSMessage | null): void,
}>();

const model = useModelValue(emit, props, null as ISMSMessage | null);
const e164 = useRemoteE164(model);

const isOpen = ref<boolean>(false);

const resetAndOpen = () => {
	reset();
	open();
};

const resetAndClose = () => {
	reset();
	close();
};


const confirm = () => {
	remove(e164.value);
};


const open = () => {
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	//
};

removeDone(() => {
	Notify.create({
		type: 'positive',
		message: 'Removed'
	});
	
	
	requestAnimationFrame(() => {
		
		if (
			router.currentRoute.value.name === SRouteNameConversationE164 &&
			router.currentRoute.value.params?.number === e164.value
		) {
			router.push({ name: SRouteNameConversations, params: {  } })
		}
		
	});
	
	reset();
	close();
});

removeError(() => {
	Notify.create({
		type: 'negative',
		message: 'Removing Number Failed'
	});
});

const isAnyLoading = computed(() => {
	return removeLoading.value;
});






defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});







</script>
<template>
	<q-dialog v-model="isOpen" persistent>
		<q-card>
			<div class="flex flex-row p-4 gap-2 items-center">
				<q-avatar color="primary" text-color="white">
					<MDIDelete class="m-2.5" />
				</q-avatar>
				<div class="flex flex-col p-2">
					<div class="font-bold">Are you sure you want to all messages from/to {{ e164 }}?</div>
				</div>
			</div>

			<q-card-actions align="right">
				<q-btn flat label="Cancel" color="green" @click="resetAndClose" :disable="isAnyLoading" />
				<q-btn flat label="Remove" color="red" @click="confirm" :loading="isAnyLoading" :disable="isAnyLoading" />
			</q-card-actions>
		</q-card>
	</q-dialog>
</template>